using System;
using System.ComponentModel;
using AlarmBase.DomainModel.Entities;
using OnlineMonitoringLog.Core.Logger.Generics;

namespace OnlineMonitoringLog.UI_WPF.model
{

    public interface IVariable : ILoggableVariable<int>
    {
        int UnitId { get; } //relate to Config
        string name { get; set; }
      
        DateTime timeStamp { get; set; }
        void RecievedData(int val, DateTime dt);
      

    }
    //public interface IVariable : ILoggableVariable<int>, INotifyPropertyChanged
    //{
    //}

}