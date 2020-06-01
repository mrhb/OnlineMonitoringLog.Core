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

namespace AlarmBase.DomainModel
{

// public abstract class AlarmableObj<StateType> : INotifyPropertyChanged, IAlarmableObj
public  class LoggableObj : AlarmableObj<int>
    {
      

        public LoggableObj(int objId, ILoggRepository Repo) : base(objId, Repo)
        {
           
        }

        public override async Task<Int32> checkStateAsync(int newState, int preState)
        {

            Int32 res = 0;

            foreach (var occ in Occurences)
            {
                res = 0;
                var hasChanged = occ.Checker(newState, preState);
                if (hasChanged)
                {
                    var tt = System.Environment.TickCount;
                    try
                    {
                        occ.tokenSource?.Cancel();
                        occ.tokenSource = new System.Threading.CancellationTokenSource();

                        res = await base._Repo.LogOccerence(occ);//LogOccerence(occ.OccConfig, occ.state, msg, delay, occ.tokenSource.Token);
                    }
                    catch (TaskCanceledException)
                    {
                        Console.WriteLine(occ.ConfigId.ToString() + "  checkStateAsync: task Canceled");
                        res = -1;
                    }
                    catch (Exception c)
                    {
                        Console.WriteLine("Error at checkStateAsync:   " + c.ToString());
                        res = -1;
                    }
                    //tt = System.Environment.TickCount - tt;
                    //Console.WriteLine(occ.ConfigId.ToString() + "  checkStateAsync Done.  tick: "+ tt.ToString()+ "\n");
                }
                else
                    Console.WriteLine(occ.ConfigId.ToString() + "  No Action at checkStateAsync:   ");
            }
            return res;
        }

        public override List<Occurence<int>> ObjOccurences()
        {
            return new List<Occurence<int>>() { new hi(ObjId) { setpoint = 50 } };
        }
    }
    class hi : IntThreshold
    {
        public hi(int _objId) : base(_objId)
        {
        }
    }
}
