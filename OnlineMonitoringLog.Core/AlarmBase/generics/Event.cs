using AlarmBase.DomainModel.Entities;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using IwnTagType = System.Int32;

namespace AlarmBase.DomainModel.generics
{
    public abstract class Event<StateType> : Occurence<StateType>
    {
        public Event(IwnTagType _objId) : base(_objId)
        {
            OccConfig.IsAlarm = false;
        }
        public override string DefaultMessage => "ObjName|SetPoint|SetValue|ClearValue|CurrentValue|HysterisisOffset|OnDelay|OffDelay|OccSeverity|OccCulture| رویداد";

        public override AlarmState Check(StateType NewState, StateType PreState)
        {                     
                return AlarmState.clear;
        }


    }
}