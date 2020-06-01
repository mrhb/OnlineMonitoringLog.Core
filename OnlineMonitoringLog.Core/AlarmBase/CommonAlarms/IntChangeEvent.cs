using AlarmBase.DomainModel.generics;
using System;

namespace AlarmBase.DomainModel
{
    public abstract class IntChangeEvent : Event<int>
    {
        public IntChangeEvent(int _objId) : base(_objId)
        {
        }
        public override string DefaultMessage => "ObjName|SetPoint|SetValue|ClearValue|CurrentValue|HysterisisOffset|OnDelay|OffDelay|OccSeverity|OccCulture| رویداد تغییر";

        public override AlarmState Check(int NewState, int PreState)
        {

            if (NewState != PreState)
            {
                return AlarmState.set;
            }
            else
                return AlarmState.clear;
        }
        public override string StateTypeToString(int st)
        {
            return st.ToString();
        }

    }

}