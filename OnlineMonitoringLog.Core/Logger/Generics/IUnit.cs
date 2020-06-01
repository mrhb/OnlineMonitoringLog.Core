using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;

namespace OnlineMonitoringLog.UI_WPF.model
{
    public interface IUnit
    {
        ObservableCollection<IVariable> Variables { get; set; }
        int ID { get; set; }
        IPAddress Ip { get; }
        string LastUpdateTime { get; }
        string StringIp { get; set; }
        event PropertyChangedEventHandler PropertyChanged;
        List<IVariable> UnitVariables();
        void Initialize();
        string ToString();
    }
}