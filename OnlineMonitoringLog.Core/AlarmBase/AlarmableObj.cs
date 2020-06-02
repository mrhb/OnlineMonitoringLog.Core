using AlarmBase.DomainModel.Entities;
using AlarmBase.DomainModel.generics;
using AlarmBase.DomainModel.repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IwnTagType = System.Int32;

namespace AlarmBase.DomainModel
{
    public abstract class AlarmableObj<StateType> : INotifyPropertyChanged, IAlarmableObj
        where StateType : IComparable<StateType>
    {

        protected IAlarmRepository _Repo;
        public event PropertyChangedEventHandler PropertyChanged;
        IwnTagType _ObjId { get; set; }
        StateType _CurrentState { get; set; }
        public StateType State
        {
            get { return _CurrentState; }
            set
            {
                OnPropertyChanged("State", value, _CurrentState);

                _CurrentState = value;
            }
        }
        protected void OnPropertyChanged(string name, StateType newState, StateType prestate)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new StatePropertyChangedEventArgs(name, newState, prestate));
            }
        }
        public class StatePropertyChangedEventArgs : PropertyChangedEventArgs
        {
            public StateType _newState;
            public StateType _prestate;
            public StatePropertyChangedEventArgs(string propertyName, StateType newState, StateType prestate)
                : base(propertyName)
            {
                _newState = newState;
                _prestate = prestate;
            }
        }
        public IwnTagType ObjId
        {
            get { return _ObjId; }

        }
        private List<Occurence<StateType>> _Occurences = new List<Occurence<StateType>>();
        public List<Occurence<StateType>> Occurences
        {
            get
            {
                return _Occurences;
            }
            private set
            {
                _Occurences = value;
            }
        }
        string _ObjName = "noName";
        public string ObjName
        {
            get
            {
                return _ObjName;
            }
        }
        public AlarmableObj(IwnTagType objId, IAlarmRepository Repo)
        {
            this.PropertyChanged += StateChangeEvent;
            _ObjId = objId;
            _Repo = Repo;
            foreach (var occ in ObjOccurences())
            {
                occ.OccConfig.ObjName = this.ObjName;
                if (!Occurences.Contains(occ))
                {
                    Occurences.Add(occ);

                }
                else
                    throw new System.ArgumentException("this Occurence is duplicate");
            }

            ResetConfig();
        }
        private async void StateChangeEvent(object sender, PropertyChangedEventArgs e)
        {
            await checkStateAsync(((StatePropertyChangedEventArgs)e)._newState, ((StatePropertyChangedEventArgs)e)._prestate);
        }
        public abstract List<Occurence<StateType>> ObjOccurences();
        protected async Task<Int32> checkStateAsync(StateType newState, StateType preState)
        {
            Int32 res = 0;
            BeforCheckState(newState, preState);
            foreach (var occ in Occurences)
            {
                res = 0;
                var hasChanged = occ.Checker(newState, preState);
                if (hasChanged)
                {
                    var tt = System.Environment.TickCount;
                    try
                    {
                        occ.tokenSource?.Cancel();
                        occ.tokenSource = new System.Threading.CancellationTokenSource();

                        res = await _Repo.LogOccerence(occ);//LogOccerence(occ.OccConfig, occ.state, msg, delay, occ.tokenSource.Token);
                    }
                    catch (TaskCanceledException)
                    {
                        Console.WriteLine(occ.ConfigId.ToString() + "  checkStateAsync: task Canceled");
                        res = -1;
                    }
                    catch (Exception c)
                    {
                        Console.WriteLine("Error at checkStateAsync:   " + c.ToString());
                        res = -1;
                    }
                    //tt = System.Environment.TickCount - tt;
                    //Console.WriteLine(occ.ConfigId.ToString() + "  checkStateAsync Done.  tick: "+ tt.ToString()+ "\n");
                }
                else
                    Console.WriteLine(occ.ConfigId.ToString() + "  No Action at checkStateAsync:   ");
            }
            return res;
        }

        public abstract Task<Int32> BeforCheckState(StateType newState, StateType preState);

        public void ResetConfig()
        {
            foreach (var occ in Occurences)
            {
                var OccConfig = _Repo.ReadConfigInfo(occ);
                //   occ.Initialization(OccConfig, occCulture);
                occ.SetConfig(OccConfig);
                // var occCulture= _Repo.SetDefaultCultureInfo(OccConfig.OccConfigID, string.Join("|", occ.AvailableParams));
                var occCulture = _Repo.ReadCultureInfo(occ);

                occ.Initialization(OccConfig, occCulture);
            }
            _Repo.SaveConfigs();
        }
    }

}
