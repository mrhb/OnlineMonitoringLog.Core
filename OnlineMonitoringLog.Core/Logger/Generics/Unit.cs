using AlarmBase.DomainModel.repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMonitoringLog.UI_WPF.model.Generics
{
    public abstract class  Unit:IUnit, INotifyPropertyChanged
    {
        static protected ILoggRepository repo = new LoggRepositry(new LoggingContext());
        private ObservableCollection<IVariable> _Variables = new ObservableCollection<IVariable>();
        private string _LastUpdateTime;
        protected IPAddress _Ip;
        public Unit(int unitId,IPAddress ip)
        {
            Ip = ip;
            ID = unitId;
            Initialize();
        }
        public void Initialize()
        {
            var resources = UnitVariables();

            foreach (var res in resources)
            {  
                if (!_Variables.Contains(res))
                {
                    res.PropertyChanged += valuChange;
                 
                    _Variables.Add(res);
                }
                else
                    throw new System.ArgumentException("this Occurence is duplicate");
            }           

            ResetConfig();
        }

        public void ResetConfig()
        {
            foreach (var vari in _Variables)
            {
                var OccConfig = repo.ReadVarConfigInfo(vari);
                             
                vari.SetConfig(OccConfig);

                vari.Initialization(OccConfig);
            }
            repo.SaveConfigs();
        }

        private void valuChange(object sender, PropertyChangedEventArgs e)
        {
            LastUpdateTime = DateTime.Now.ToString();
        }

        public string LastUpdateTime
        {
            get { return _LastUpdateTime; }
            private set
            {
                _LastUpdateTime = value;
                NotifyPropertyChanged("LastUpdateTime");
            }
        }

        public ObservableCollection<IVariable> Variables
        {
            get { return _Variables; }
            set
            {
                _Variables = value;
                NotifyPropertyChanged("Units");
            }
        }
        public int ID { get; set; }

        public IPAddress Ip
        {
            get { return _Ip; }
            private set
            {
                _Ip = value;
                NotifyPropertyChanged("ip");
            }
        }
        public string StringIp
        {
            get
            {
                return _Ip.ToString();
            }
            set
            {
                Ip = IPAddress.Parse(value);
            }
        }
        public abstract override string ToString();
        public event PropertyChangedEventHandler PropertyChanged;
        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public abstract List<IVariable> UnitVariables();
    }
}
