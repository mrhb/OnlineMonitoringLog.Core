using OnlineMonitoringLog.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;

namespace OnlineMonitoringLog.Core.Interfaces
{
    public interface IUnit
    {
        ObservableCollection<ILoggableVariable<int>> Variables { get; set; }
        int ID { get; set; }
        IPAddress Ip { get; }
        string LastUpdateTime { get; }
        string StringIp { get; set; }
        event PropertyChangedEventHandler PropertyChanged;
        List<ILoggableVariable<int>> UnitVariables();
        void Initialize();
        string ToString();
    }
}