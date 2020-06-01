using System;
using System.ComponentModel;
using AlarmBase.DomainModel.Entities;

namespace OnlineMonitoringLog.UI_WPF.model
{
    public interface IVariable : INotifyPropertyChanged
    {
        int UnitId { get;} //relate to Config
        string name { get; set; }
        string value { get; set; }
        DateTime timeStamp { get; set; }
        void RecievedData(int val, DateTime dt);
        string ToString();
        bool SetConfig(RegisteredVarConfig varConfig);
        bool Initialization(RegisteredVarConfig varConfig);
    }
}