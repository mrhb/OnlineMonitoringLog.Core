
using AlarmBase.DomainModel.Entities;
using AlarmBase.DomainModel.generics;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IwnTagType = System.Int32;

namespace AlarmBase.DomainModel.repository
{
    public class AlarmRepository : IAlarmRepository
    {
        int _delayTime = 500;
        public CancellationTokenSource saveTaskTokenSource = new CancellationTokenSource();
        public CentralConfigViewModel ConfigViewModel;
        Task<int> saveTask;
        AlarmableContext _ConfigContex;
        AlarmableContext _LogContex;
        cultureType _cultureType = cultureType.en_US;// cultureType.Default;

        public AlarmRepository(AlarmableContext Contex)
        {
            _ConfigContex = Contex;
            _LogContex = (AlarmableContext)Activator.CreateInstance(_ConfigContex.GetType());

            _ConfigContex.occConfig.ToList();
            _LogContex.occConfig.ToList();
            ConfigViewModel = new CentralConfigViewModel(_ConfigContex, _cultureType);
        }
        public AlarmableContext ConfigContex
        {
            get
            { return _ConfigContex; }
        }
        public AlarmableContext LogContex
        {
            get
            { return _LogContex; }
        }
        public object ChangeTracker { get; private set; }
        public RegisteredOccConfig ReadConfig(IwnTagType tag, string OccName)
        {
            RegisteredOccConfig Occconfig = _ConfigContex.occConfig
                .Where(p => p.Fk_AlarmableObjId == tag && p.OccKindName == OccName)
                 .Select(a => a).FirstOrDefault();
            return Occconfig;
        }
        public OccCultureInfo SetDefaultCultureInfo(int occConfigId, string defaultMessage)
        {
            OccCultureInfo RegisteredOccCulture;


            try
            {
                RegisteredOccCulture = _ConfigContex.occCulture
               .Where(p => p.FK_occConfigId == occConfigId && p.Culture == _cultureType.Value)
                .Select(a => a).First();
            }
            catch
            {
                RegisteredOccCulture = new OccCultureInfo()
                {
                    Culture = _cultureType.Value,
                    FK_occConfigId = occConfigId,
                    Template = defaultMessage
                };
                _ConfigContex.occCulture.Add(RegisteredOccCulture);
            }



            return RegisteredOccCulture;
        }
        public int WriteConfig(IwnTagType tag, string config, string name)
        {
            var stud = new RegisteredOccConfig() { SerializedSetPoint = config, Fk_AlarmableObjId = tag, OccKindName = name };
            _ConfigContex.occConfig.Add(stud);
            var res = _ConfigContex.SaveChanges();
            return stud.OccConfigID;
        }
        public RegisteredOccConfig WriteNewConfig(IwnTagType tag, string config, string name)
        {
            var OccConfig = new RegisteredOccConfig() { SerializedSetPoint = config, Fk_AlarmableObjId = tag, OccKindName = name };
            _ConfigContex.occConfig.Add(OccConfig);
            var res = _ConfigContex.SaveChanges();
            return OccConfig;
        }
        public RegisteredOccConfig ReadConfigInfo(RegisteredOccConfig Defaultconfig)
        {
            RegisteredOccConfig OccConfig = _ConfigContex.occConfig
                .Where(p => p.Fk_AlarmableObjId == Defaultconfig.Fk_AlarmableObjId &&
                p.OccKindName == Defaultconfig.OccKindName)
                .Select(a => a).FirstOrDefault();
            if (OccConfig == null)
            {
                OccConfig = Defaultconfig;
                _ConfigContex.occConfig.Add(OccConfig);
                var res = _ConfigContex.SaveChanges();
            }
            return OccConfig;
        }
        public RegisteredOccConfig ReadConfigInfo(IOccurence occ)
        {
            var typ = occ.GetType().ToString();
            RegisteredOccConfig OccConfig = _ConfigContex.occConfig
                .Where(p => p.Fk_AlarmableObjId == occ.ObjId && p.OccKindName == typ)
                .Select(a => a).FirstOrDefault();

            if (OccConfig == null)
            {

                var NewOccConfig = new RegisteredOccConfig()
                {
                    Fk_AlarmableObjId = occ.ObjId,
                    SerializedSetPoint = occ.SetPoint,
                    OccKindName = typ,
                    SetPointType = occ.GetSetPointType,
                    HysterisisOffset = occ.HysterisisOffset,
                    OnDelay = occ.OnDelay,
                    OffDelay = occ.OffDelay,
                    OccSeverity = occ.OccSeverity,
                    IsAlarm = occ.IsAlarm
                };
                OccConfig = NewOccConfig;
                _ConfigContex.occConfig.Add(OccConfig);
                var res = _ConfigContex.SaveChanges();
            }
            return OccConfig;
        }
        public OccCultureInfo ReadCultureInfo(IOccurence occ)
        {
            var typ = occ.GetType().ToString();
            OccCultureInfo cultureInfo = _ConfigContex.occCulture
                .Where(p => p.FK_occConfigId == occ.ConfigId && p.Culture == _cultureType.Value)
                .Select(a => a).FirstOrDefault();

            if (cultureInfo == null)
            {

                var NewOccConfig = new OccCultureInfo()
                {
                    FK_occConfigId = occ.ConfigId,
                    Template = occ.DefaultMessage,
                    Culture = _cultureType.Value
                    //SerializedSetPoint = occ.SetPoint,
                    //OccKindName = typ,
                    //SetPointType = occ.GetSetPointType,
                    //HysterisisOffset = occ.HysterisisOffset,
                    //OnDelay = occ.OnDelay,
                    //OffDelay = occ.OffDelay,
                    //OccSeverity = occ.OccSeverity,
                    //IsAlarm = occ.IsAlarm
                };
                cultureInfo = NewOccConfig;
                _ConfigContex.occCulture.Add(cultureInfo);
                var res = _ConfigContex.SaveChanges();
            }
            return cultureInfo;
        }
        /// <summary>
        /// اگر در لیست نباشد به لیست اضافه میشود.
        /// در صورتی که کارش تمام شده باشد(از حالت آلارمی خارج شده باشد و روئیت شده باشد)از 
        /// لیست حذف میشود و در غیر این صورت بروز میشود. 
        /// </summary>
        /// <param name="ObjId"></param>
        /// <param name="OccConfig"></param>
        /// <param name="state"></param>
        /// <returns></returns>       
        public async Task<Int32> LogOccerence(IOccurence occ)
        {

            Int32 res = 0;
            await Task.Delay(occ.delayTime, occ.Token); //OnDelay and Off Delay waiting
            if (saveTask == null ? false : (saveTask.Status==TaskStatus.Running))
            {
                saveTask?.Wait(); // wait to complete saving
            }

            if (!occ.Token.IsCancellationRequested)
            {
                if (occ.IsAlarm)
                {
                    var tt = System.Environment.TickCount;

                    OccurenceLog log = null;
                    try
                    {
                        log = _LogContex.occLog.Local
                            .Where(l => l.FK_occConfigID == occ.ConfigId
                         )
                   .OrderByDescending(o => o.SetTime)
                   .FirstOrDefault();
                        var ttt = System.Environment.TickCount - tt;
                    }
                    catch (Exception c)
                    {
                        Console.WriteLine(occ.ConfigId.ToString() + "  Error at ReadLog:   " + c.ToString());
                        res = -1;
                    }

                    if (log == null ? (occ.State == AlarmState.set) : (log.state == AlarmState.clear) && occ.State == AlarmState.set)
                    {
                        res = AddNewLog(occ);
                    }
                    else if (log == null ? false : ((occ.State == AlarmState.clear) && (log.state == AlarmState.set)))
                    {
                        UpdateLog(occ.State, log);
                    }
                }
                else //is Event
                {
                    res = AddNewLog(occ);
                }

                saveTaskTokenSource?.Cancel();
                saveTaskTokenSource = new System.Threading.CancellationTokenSource();
                try
                {
                    saveTask = SaveLogs();
                }
                catch (TaskCanceledException)
                {

                }

            }
            return res;
        }
        private static void UpdateLog(AlarmState state, OccurenceLog log)
        {
            log.ClearTime = DateTime.Now;
            log.state = state;
        }
        private int AddNewLog(IOccurence occ)
        {
            int res = 0;
            var newlog = new OccurenceLog()
            {
                OccLogId = Guid.NewGuid(),
                FK_occConfigID = occ.ConfigId,
                state = occ.State,
                ClearTime = occ.State == AlarmState.set ? (DateTime?)null : DateTime.Now,
                SetTime = DateTime.Now,
                AlarmMessage = occ.Message,
                SetValuesAsJson=occ.SetValue
            };

            try
            {
                _LogContex.occLog.Add(newlog);
            }
            catch (Exception)
            {
                Console.WriteLine("  " + occ.ConfigId.ToString() + "  Error at Task_Add!!!!!!!!!!!!!   ");
                res = -1;
            }

            return res;
        }
       
        public int AckOccerence(List<OccurenceLog> AckOccs, string user, string comment)
        {

            if (AckOccs.Count > 0)
            {
                if (saveTask == null ? false : saveTask.IsCompleted)
                {
                    saveTask?.Wait();
                }
                foreach (var lo in AckOccs)
                {
                    lo.Acknowledge = true;
                    lo.AckUser = user;
                    lo.Comment = comment;
                }

                saveTaskTokenSource?.Cancel();
                saveTaskTokenSource = new System.Threading.CancellationTokenSource();
                try
                {
                    saveTask = SaveLogs();
                }
                catch (TaskCanceledException)
                {

                }
            }
            return AckOccs.Count;
        }
        internal async Task<Int32> SaveLogs()
        {
            Int32 res = -1;
            await Task.Delay(_delayTime, saveTaskTokenSource.Token);
            if (!saveTaskTokenSource.IsCancellationRequested)
            {
                // res = await _LogContex.SaveChangesAsync();

                var original = _LogContex.ChangeTracker.Entries()
                        .ToList();
                try
                {
                    res = await _LogContex.SaveChangesAsync();
                }
                catch (Exception c)

                {
                    Console.Write("Error At:SaveOcclogChangesAsync:" + c.ToString() + "\n");
                }

                try
                {
                    foreach (var cng in original)
                    {
                        if ((cng.Entity.GetType() == typeof(OccurenceLog)))
                        {
                            OccurenceLog modifiedOccurenceLog = cng.Entity as OccurenceLog;
                            if ((modifiedOccurenceLog.state == generics.AlarmState.clear || modifiedOccurenceLog.RegisteredOccConfig.IsAlarm == false)
                                && modifiedOccurenceLog.Acknowledge == true)
                                _LogContex.Entry(modifiedOccurenceLog).State = EntityState.Detached;
                        }
                    }
                }
                catch (Exception c)

                {
                    Console.Write("Error At: updating Entities:" + c.ToString() + "\n");
                }


            }

            return res;
        }
        public void SaveConfigs()
        {
            var original = _ConfigContex.ChangeTracker.Entries()
                     .Where(x => x.State != EntityState.Unchanged)
                     .ToList();

            var res = _ConfigContex.SaveChanges();
            foreach (var cng in original)
            {
                var EnType = ObjectContext.GetObjectType(cng.Entity.GetType());
                if (EnType == typeof(RegisteredOccConfig))
                {
                    RegisteredOccConfig modifiedRegisteredOccConfig = cng.Entity as RegisteredOccConfig;
                    modifiedRegisteredOccConfig.OnConfigChangeSaved();
                    var ConfigInLogg = _LogContex.occConfig.Where(p => p.OccConfigID == modifiedRegisteredOccConfig.OccConfigID).FirstOrDefault();
                    ConfigInLogg = modifiedRegisteredOccConfig;
                }

                if (EnType == typeof(OccCultureInfo))
                {
                    OccCultureInfo modifiedCultureInfo = cng.Entity as OccCultureInfo;

                    modifiedCultureInfo.OnCultureChangeSaved();
                    var CultureInLogg = _LogContex.occCulture.Where(p => p.CultureInfoId == modifiedCultureInfo.CultureInfoId).FirstOrDefault();
                    CultureInLogg = modifiedCultureInfo;
                }

            }

        }
    }

}
