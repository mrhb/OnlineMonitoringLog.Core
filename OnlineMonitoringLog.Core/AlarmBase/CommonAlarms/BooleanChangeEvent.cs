using AlarmBase.DomainModel.generics;
using System;

namespace AlarmBase.DomainModel
{
    public abstract class BooleanChangeEvent : Event<bool>
    {
        public BooleanChangeEvent(int _objId) : base(_objId)
        {
        }

        public bool Setpoint { get; set; }
       
        public override string StateTypeToString(bool st)
        {
            return st.ToString();
        }
        
    }

}