using AlarmBase.DomainModel.Entities;
using AlarmBase.DomainModel.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMonitoringLog.UI_WPF.model
{

    public interface ILoggRepository : IAlarmRepository
    {
      
    RegisteredVarConfig ReadVarConfigInfo(RegisteredVarConfig Defaultconfig);
    RegisteredVarConfig ReadVarConfigInfo(ILoggableVariable<int> vari);
    Task<Int32> logVlaueChanges(VariableLog vari);
        int logVlaueChange(VariableLog varlog);

        void SaveVarConfigs();
    }




}
