using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlarmBase.DomainModel.Entities;
using AlarmBase.DomainModel.repository;

namespace OnlineMonitoringLog.UI_WPF.model
{
   public class LoggRepositry : AlarmRepository,ILoggRepository
    {
        protected LoggingContext _VarConfigContex;
        public LoggRepositry(LoggingContext Contex) : base(Contex)
        {
            _VarConfigContex = Contex;
        }

        public async Task<int> logVlaueChanges(VariableLog vari)
        {
            throw new NotImplementedException();
        }

        public RegisteredVarConfig ReadVarConfigInfo(RegisteredVarConfig Defaultconfig)
        {
            RegisteredVarConfig varConfig = _VarConfigContex.varConfig
               .Where(p => p.Fk_UnitEntityId == Defaultconfig.Fk_UnitEntityId &&
               p.resourceName == Defaultconfig.resourceName)
               .Select(a => a).FirstOrDefault();
            if (varConfig == null)
            {
                varConfig = Defaultconfig;
                _VarConfigContex.varConfig.Add(varConfig);
                var res = ConfigContex.SaveChanges();
            }
            return varConfig;

        }
        public RegisteredVarConfig ReadVarConfigInfo(IVariable vari)
        {
            var typ = vari.name;
            RegisteredVarConfig OccConfig = _VarConfigContex.varConfig
                .Where(p => p.Fk_UnitEntityId == vari.UnitId && p.resourceName == typ)
                .Select(a => a).FirstOrDefault();

            if (OccConfig == null)
            {

                var NewOccConfig = new RegisteredVarConfig()
                {
                    Fk_UnitEntityId=vari.UnitId,
                    resourceName=vari.name,
                    
                    //Fk_AlarmableObjId = occ.ObjId,
                    //SerializedSetPoint = occ.SetPoint,
                    //OccKindName = typ,
                    //SetPointType = occ.GetSetPointType,
                    //HysterisisOffset = occ.HysterisisOffset,
                    //OnDelay = occ.OnDelay,
                    //OffDelay = occ.OffDelay,
                    //OccSeverity = occ.OccSeverity,
                    //IsAlarm = occ.IsAlarm
                };
                OccConfig = NewOccConfig;
                _VarConfigContex.varConfig.Add(OccConfig);
                var res = _VarConfigContex.SaveChanges();
            }
            return OccConfig;
        }



    }
}
