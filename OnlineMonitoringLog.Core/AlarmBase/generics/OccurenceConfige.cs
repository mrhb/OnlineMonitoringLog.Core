using AlarmBase.DomainModel.Entities;
using AlarmBase.DomainModel.generics;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using IwnTagType = System.Int32;

namespace OnlineMonitoringLog.Core.DomainModel.generics
{
    public abstract class OccurenceConfige : INotifyPropertyChanged, IOccurenceConfige
    {
        internal object setpointObj;
        private const string Params = "ObjName|SetPoint|SetValue|ClearValue|CurrentValue|HysterisisOffset|OnDelay|OffDelay|OccSeverity|OccCulture|";
    
        internal RegisteredOccConfig Config = new RegisteredOccConfig();
        private OccCultureInfo Culture = new OccCultureInfo();
        public IwnTagType ObjId
        {
            get { return Config.Fk_AlarmableObjId; }
        }
        public string ObjName { get { return Config.ObjName; } }

        public bool IsAlarm { get { return Config.IsAlarm; } }
        public string CultureTemplate
        {
            get
            {
                return Culture.Template;
            }
            set
            {
                Culture.Template = value;
                NotifyPropertyChanged();
            }
        }
        public string AvailableParams
        {
            get
            {
                return Params;
               // return OccSerialization.AvailableParam(Type.GetType(OccConfig.OccKindName));
            }

        }
        public int HysterisisOffset
        {
            get
            {
                return Config.HysterisisOffset;
            }
            set
            {
                Config.HysterisisOffset = value;
                NotifyPropertyChanged();
            }

        }
        public int OnDelay
        {
            get
            {
                return Config.OnDelay;
            }
            set
            {
                Config.OnDelay = value;
                NotifyPropertyChanged();
            }
        }
        public int OffDelay
        {
            get
            {
                return Config.OffDelay;
            }
            set
            {
                Config.OffDelay = value;
                NotifyPropertyChanged();
            }
        }
        public string SerializedSetPoint
        {
            get
            {
                return Config.SerializedSetPoint;
            }
            set
            {
                Config.SerializedSetPoint = value;
                NotifyPropertyChanged();
            }
        }

        public string SetPointType
        {
            get
            {
                return Config.SetPointType;
            }
            set
            {
                Config.SetPointType = value;
                NotifyPropertyChanged();
            }
        }

        //شدت آلارم
        public int OccSeverity
        {
            get
            {
                return Config.OccSeverity;
            }
            set
            {
                Config.OccSeverity = value;
                NotifyPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        internal RegisteredOccConfig OccConfig { get { return Config; } set { Config = value; } }
        internal OccCultureInfo OccCulture { get { return Culture; } set{ Culture = value; } }
        public int ConfigId { get { return OccConfig.OccConfigID; } }

      
        public OccurenceConfige()
        {

        }
        static OccurenceConfige()
        {

           // defultTemplate = "HysterisisOffset|OnDelay|OffDelay|OccSeverity|OccConfig|OccCulture|ConfigId";
        }
        public Boolean Initialization(RegisteredOccConfig OccConfig, OccCultureInfo cultureInfo)
        {
            //this.OccConfig = OccConfig;
            //this.OccConfig.ConfigChangeSaved += ChangedConfigEvent;

            CultureTemplate = cultureInfo.Template;
            OccCulture = cultureInfo;
            OccCulture.CultureChangeSaved += ChangedCutureEvent;

            return SetConfig(OccConfig);
        }
        public Boolean SetConfig(RegisteredOccConfig _OccConfig)
        {
            OccConfig = _OccConfig;
            OccConfig.ConfigChangeSaved += ChangedConfigEvent;
            Boolean result = true;
            try
            {
                Type t = Type.GetType(_OccConfig.SetPointType);
                var sfsdf = Activator.CreateInstance(t);

                if (t == typeof(int))
                {
                    setpointObj = Convert.ToInt32(_OccConfig.SerializedSetPoint);
                }
                else if (t == typeof(bool))
                {
                    setpointObj = Convert.ToBoolean(_OccConfig.SerializedSetPoint);
                }

                else
                    result = false;
            }
            catch (Exception c)
            {
                result = false;
            }
            return result;
            }
        private void ChangedConfigEvent(object sender, PropertyChangedEventArgs e)
        {
            SetConfig((RegisteredOccConfig)sender);
        }
        private void ChangedCutureEvent(object sender, PropertyChangedEventArgs e)
        {
            CultureTemplate = ((OccCultureInfo)sender).Template;
        }

        public  bool SetPointSet(object setpoint)
        {
            setpointObj = setpoint;
            return true;
        }

    }
}
