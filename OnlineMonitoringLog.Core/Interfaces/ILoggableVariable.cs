using OnlineMonitoringLog.Core.DataRepository.Entities;
using System;
using System.ComponentModel;

namespace OnlineMonitoringLog.Core.Interfaces
{
    public interface ILoggableVariable<StateType> : INotifyPropertyChanged
    {
        int UnitId { get; } //relate to Config
        string name { get; set; }
        string value { get; set; }
        DateTime timeStamp { get; set; }
        Boolean RecievedData(StateType val, DateTime dt);
        string ToString();
        bool SetConfig(RegisteredVarConfig varConfig);
        bool Initialization(RegisteredVarConfig varConfig);
    }
}
