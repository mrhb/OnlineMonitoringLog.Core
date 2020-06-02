using System;
using System.ComponentModel;
using AlarmBase.DomainModel.Entities;
using OnlineMonitoringLog.Core.Logger.Generics;

namespace OnlineMonitoringLog.UI_WPF.model
{

    public interface IVariable : ILoggableVariable<int>
    {
        new int UnitId { get; } //relate to Config
        new string name { get; set; }
        new string value { get; set; }
        new DateTime timeStamp { get; set; }      

    }
}