using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlarmBase.DomainModel.repository;
using AlarmBase.DomainModel;
using OnlineMonitoringLog.UI_WPF.model;
using AlarmBase.DomainModel.generics;
using System.ComponentModel;
using AlarmBase.DomainModel.Entities;

namespace AlarmBase.DomainModel
{

    // public abstract class AlarmableObj<StateType> : INotifyPropertyChanged, IAlarmableObj
    public class LoggableObj : AlarmableObj<int>
    {
        ILoggRepository _loggRepo;
        public LoggableObj(int objId, ILoggRepository Repo) : base(objId, Repo)
        {
            _loggRepo = Repo;
        }

        public override int BeforCheckState(int newState, int preState)
        {
            var varlog = new VariableLog() {
                VariableLogId= Guid.NewGuid(),
                FK_varaiableConfigID =ObjId,
                Value =newState,
                TimeStamp =DateTime.Now
            };
            
            return  _loggRepo.logVlaueChange(varlog);           
        }

        public override List<Occurence<int>> ObjOcucrences()
        {
            return new List<Occurence<int>>() { };// new hi(ObjId) { setpoint = 50 } };
        }
    }
    class hi : IntThreshold
    {
        public hi(int _objId) : base(_objId)
        {
        }
    }
}
