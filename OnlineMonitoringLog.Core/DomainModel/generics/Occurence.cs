using AlarmBase.DomainModel.Entities;
using OnlineMonitoringLog.Core.DomainModel.generics;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using IwnTagType = System.Int32;

namespace AlarmBase.DomainModel.generics
{
    public abstract class Occurence<StateType> : OccurenceConfige, IOccurence
    {
        internal CancellationTokenSource tokenSource = new CancellationTokenSource();

        public CancellationToken Token => tokenSource.Token;
        StateType _setValue { get; set; }
        StateType _clearValue { get; set; }
        public StateType setpoint
        {
            get
            {
                if (IsAlarm)
                    return (StateType)setpointObj;
                else
                    return default(StateType);
            }
            set
            {
                setpointObj = (object)value;
            }
        }
        public AlarmState State => state;
        internal AlarmState state;
        [MessageParam]
        public int delayTime => state == AlarmState.set ? OccConfig.OnDelay : OccConfig.OffDelay;
        public string Message => OccSerialization.ReplaceParamWithValues(CultureTemplate, this);
        DateTime _setTime;
        DateTime _clearTime;
        public Occurence(IwnTagType _objId)
        {
            Config.Fk_AlarmableObjId = _objId;
            Config.SetPointType = GetSetPointType;
        }

        /// <summary>
        /// در این تابع باید وضعیت آلارمی بررسی شود
        /// 
        /// </summary>
        /// <param name="NewState"></param>
        /// <param name="PreState"></param>
        /// <returns></returns>
        /// 
        public abstract AlarmState Check(StateType NewState, StateType PreState);

        public bool Checker(StateType newState, StateType preState)
        {
            bool haschanged = false;
            AlarmState _state;
            _state = Check(newState, preState);
            if (IsAlarm)
            {
                if (_state != state)
                {
                    haschanged = true;

                    if (_state == AlarmState.set)
                    { _setValue = newState; _setTime = DateTime.Now; }
                    else
                    { _clearValue = newState; _clearTime = DateTime.Now; }
                }

                state = _state;
            }
            else if (_state == AlarmState.set)
            {
                haschanged = true;
                _setValue = newState;
                _setTime = DateTime.Now;
            }

            return haschanged;
        }

        public bool Equals(Occurence<StateType> other)
        {
            if (other == null) return false;
            if (other.GetType().ToString() != this.GetType().ToString()) return false;

            return true;
        }
        public abstract string StateTypeToString(StateType st);
        [MessageParam]
        public string SetPoint
        {
            get
            {
                return StateTypeToString(setpoint);
            }
        }

        [MessageParam]
        public string SetValue
        {
            get
            {
                return StateTypeToString(_setValue);
            }
        }

        [MessageParam]
        public string ClearValue
        {
            get
            {
                return StateTypeToString(_clearValue);
            }
        }

        public string GetSetPointType
        {
            get
            {
                return typeof(StateType).ToString();
            }
        }

        public virtual string DefaultMessage => "ObjName|SetPoint|SetValue|ClearValue|CurrentValue|HysterisisOffset|OnDelay|OffDelay|OccSeverity|OccCulture|";
    }
}
