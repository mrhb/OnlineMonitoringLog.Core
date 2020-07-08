using AlarmBase.DomainModel.repository;
using System;
using System.Threading.Tasks;

using OnlineMonitoringLog.Core.DataRepository.Entities;

namespace OnlineMonitoringLog.Core.Interfaces
{
    public interface ILoggRepository : IAlarmRepository
    {
      
    RegisteredVarConfig ReadVarConfigInfo(RegisteredVarConfig Defaultconfig);
    RegisteredVarConfig ReadVarConfigInfo(ILoggableVariable<int> vari);
    Task<Int32> logVlaueChanges(VariableLog vari);
        int logVlaueChange(VariableLog varlog);

        void SaveVarConfigs();

        UnitEntity ReadUnitEntity(int ID);
    }




}
