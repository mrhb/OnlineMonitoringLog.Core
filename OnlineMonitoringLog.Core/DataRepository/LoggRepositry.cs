using System;
using System.Linq;
using System.Threading.Tasks;
using AlarmBase.DomainModel.repository;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Threading;

using OnlineMonitoringLog.Core.Interfaces;
using OnlineMonitoringLog.Core.DataRepository.Entities;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace OnlineMonitoringLog.Core.DataRepository
{
    public class LoggRepositry : AlarmRepository, ILoggRepository
    {
        protected LoggingContext _VarConfigContex;
        protected LoggingContext _LogContex;
        static Thread t;
        static public ConcurrentQueue<VariableLog> OPCDataQueue = new ConcurrentQueue<VariableLog>();
        public LoggRepositry(LoggingContext Contex) : base(Contex)
        {
            _VarConfigContex = Contex;

            _LogContex = (LoggingContext)Activator.CreateInstance(_VarConfigContex.GetType());

            _VarConfigContex.varConfig.ToList();
            if (t == null)
            {
                t = new Thread(() => ProcessUa());
                t.Name = "simplyLogRepo";
                t.IsBackground = true;
                t.Start();

                Console.WriteLine($"theared {t.Name} with id={t.ManagedThreadId} is started");
            }
        }

        ~LoggRepositry()
        {        }

        private void ProcessUa()
        {
            var Contex = (LoggingContext)Activator.CreateInstance(_VarConfigContex.GetType());
            do
            {
                try
                {
                    int a = 0;
                    List<VariableLog> list = new List<VariableLog>();
                    VariableLog item;



             //       Metrics.Collector = new CollectorConfiguration()
             //.Tag.With("Company", "TetaPower")
             //.Tag.With("UnitName", "TestUnit")
             //.Batch.AtInterval(TimeSpan.FromSeconds(2))
             //.WriteTo.InfluxDB("http://localhost:8086", "chronograf")
             //// .WriteTo.InfluxDB("udp://localhost:8089", "data")
             //.CreateCollector();



                    while (OPCDataQueue.TryDequeue(out item))
                    {
                        a++;



                        //***********Save SQL**********
                        Contex.varLog.Add(new VariableLog()
                        {
                            FK_varaiableConfigID = item.FK_varaiableConfigID,
                            TimeStamp = DateTime.Now,
                            Value = item.Value,
                            VariableLogId = Guid.NewGuid()
                        });

                        Contex.varLog.Add(item);
                        //list.Add(item);


                        //Contex.varLog.AddRange(list);

                        //***********/Save SQL**********



                        int? i = a;

                        //    var watch = System.Diagnostics.Stopwatch.StartNew();
                        //    Metrics.Increment("mrhb_iterations");
                        //    var datas = new Dictionary<string, object>
                        //{

                        //     {item.FK_varaiableConfigID.ToString(), item.Value}
                        //};
                        //    Metrics.Write("UIWPFtime", datas);

                        //    Console.WriteLine($"{item.Value} is added to varlog Table and buffer has {OPCDataQueue.Count()} numbers      Execution Time: {watch.ElapsedMilliseconds} ms");



                        if (a > 100)
                        {
                            var watch = System.Diagnostics.Stopwatch.StartNew();
                            Contex.SaveChanges();
                            Console.WriteLine($"{a} is added to varlog Table and buffer has {OPCDataQueue.Count()} numbers      Execution Time: {watch.ElapsedMilliseconds} ms");
                            a = 0;
                        }
                        Thread.Sleep(1);
                    }//while

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

            }
            while (true);

        }

        public int logVlaueChange(VariableLog varlog)
        {
            //  varlog.RegisteredVarConfig = _VarConfigContex.varConfig.FirstOrDefault();
          //  OPCDataQueue.Enqueue(varlog);
            return 1;
        }

        public async Task<int> logVlaueChanges(VariableLog vari)
        {
            throw new NotImplementedException();
        }

        public RegisteredVarConfig ReadVarConfigInfo(ILoggableVariable<int> vari)
        {
            RegisteredVarConfig varConfig = _VarConfigContex.varConfig
                .Where(p => p.Fk_UnitEntityId == vari.UnitId && p.resourceName == vari.name)
                .Select(a => a).FirstOrDefault();

            if (varConfig == null)
            {

                var NewOccConfig = new RegisteredVarConfig()
                {
                    Fk_UnitEntityId = vari.UnitId,
                    resourceName = vari.name,
                };
                varConfig = NewOccConfig;
                _VarConfigContex.varConfig.Add(varConfig);

            }
            return varConfig;

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
                    Fk_UnitEntityId = vari.UnitId,
                    resourceName = vari.name,

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

        public void SaveVarConfigs()
        {
            var original = _VarConfigContex.ChangeTracker.Entries()
                     .Where(x => x.State != EntityState.Unchanged)
                     .ToList();

            var res = _VarConfigContex.SaveChanges();
            foreach (var cng in original)
            {
                var EnType = ObjectContext.GetObjectType(cng.Entity.GetType());
                if (EnType == typeof(RegisteredVarConfig))
                {
                    RegisteredVarConfig modifiedRegisteredOccConfig = cng.Entity as RegisteredVarConfig;
                    modifiedRegisteredOccConfig.OnConfigChangeSaved();
                    var ConfigInLogg = _VarConfigContex.varConfig.Where(p => p.Fk_UnitEntityId == modifiedRegisteredOccConfig.Fk_UnitEntityId).FirstOrDefault();
                    ConfigInLogg = modifiedRegisteredOccConfig;
                }


            }

        }


    }
}
