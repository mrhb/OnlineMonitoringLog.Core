using AlarmBase.DomainModel.generics;
using System;

namespace AlarmBase.DomainModel
{
    public abstract class BoleanThreshold : Alarm<bool>
    {
        public BoleanThreshold(int _objId) : base(_objId)
        {

        }

        public bool Setpoint { get; set; }
        public BoleanTresholdKind tresholdKind { get; set; }
        public override AlarmState  Check(bool NewState, bool PreState)
        {
           AlarmState _state = state;
            //DO ALL THE HEAVY LIFTING!!!
            if (NewState != PreState)
            {
                _state = NewState== Setpoint ? AlarmState.set : AlarmState.clear ;                
            }           

            return _state;
        }

              
        public override string StateTypeToString(bool st)
        {
            return st.ToString();
        }
        
    }
    public enum BoleanTresholdKind
    { OnAlarm, OffAlarm }
}