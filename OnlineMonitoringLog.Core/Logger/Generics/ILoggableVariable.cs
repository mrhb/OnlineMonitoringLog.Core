using AlarmBase.DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace OnlineMonitoringLog.UI_WPF.model
{
    public interface ILoggableVariable<StateType> : INotifyPropertyChanged
    {
        int UnitId { get; } //relate to Config
        string name { get; set; }
        string value { get; set; }
        DateTime timeStamp { get; set; }
        void RecievedData(StateType val, DateTime dt);
        string ToString();
        bool SetConfig(RegisteredVarConfig varConfig);
        bool Initialization(RegisteredVarConfig varConfig);
    }
}
