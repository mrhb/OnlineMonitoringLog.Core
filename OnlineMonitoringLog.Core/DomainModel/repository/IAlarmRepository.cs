using System.Threading;
using System.Threading.Tasks;
using AlarmBase.DomainModel.Entities;
using AlarmBase.DomainModel.generics;
using System;

namespace AlarmBase.DomainModel.repository
{
    public interface IAlarmRepository
    {
        Task<Int32> LogOccerence(IOccurence occ);
        RegisteredOccConfig ReadConfig(int tag, string OccName);
        RegisteredOccConfig ReadConfigInfo(IOccurence occ);
        OccCultureInfo ReadCultureInfo(IOccurence occ);
        int WriteConfig(int tag, string config, string name);
        RegisteredOccConfig WriteNewConfig(int tag, string config, string name);
        RegisteredOccConfig ReadConfigInfo(RegisteredOccConfig  Defaultconfig);
        OccCultureInfo SetDefaultCultureInfo(int occConfigId, string template);
        void SaveConfigs();
    }
       
}