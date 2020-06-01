using AlarmBase.DomainModel.Entities;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using IwnTagType = System.Int32;

namespace AlarmBase.DomainModel.generics
{
    public abstract class Alarm<StateType> : Occurence<StateType>
    {
        public Alarm(IwnTagType _objId) : base(_objId)
        {
                OccConfig.IsAlarm = true;     
            
        }
        public override string DefaultMessage => "ObjName|SetPoint|SetValue|ClearValue|CurrentValue|HysterisisOffset|OnDelay|OffDelay|OccSeverity|OccCulture| آلارم";

    }

  
    public enum AlarmState
    {
        notChecked, set, clear
    }


}