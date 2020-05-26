using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using IwnTagType = System.Int32;

namespace AlarmBase.DomainModel.Entities
{
    public class RegisteredOccConfig : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler ConfigChangeSaved;
        // Create the OnConfigChanged method to raise the event
        internal void OnConfigChangeSaved()
        {
            PropertyChangedEventHandler handler = ConfigChangeSaved;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(this.OccConfigID.ToString()));
            }
        }       

        [Key]
        public int OccConfigID { get; set; }

        public IwnTagType Fk_AlarmableObjId { get; set; }

        public string SetPointType { get; set; }

        string _Config;
        public string SerializedSetPoint
        {
            get { return _Config; }
            set
            {             
                _Config = value;
                NotifyPropertyChanged();
            }
        }

        bool _IsAlarm;
        public bool IsAlarm
        {
            get { return _IsAlarm; }
            set
            {
                _IsAlarm = value;
                NotifyPropertyChanged();
            }
        }

        //نوع آلارم مانند Hi , HiHi
        public string OccKindName { get; set; }      

        [NotMapped]
        int _hysterisisOffset { get; set; }
        [DefaultValue(0)]
        public int HysterisisOffset
        {
            get
            {
                return _hysterisisOffset;
            }
            set
            {
                _hysterisisOffset = value;
                NotifyPropertyChanged();
            }
        }

        [DefaultValue(0)]
        public int OnDelay { get; set; }
        [DefaultValue(0)]
        public int OffDelay { get; set; }
        //شدت آلارم
        public int  OccSeverity { get; set; }
        public ICollection<OccurenceLog> OccurenceLogs { get; set; }
        public ICollection<OccCultureInfo> OccCultureInfoes { get; set; }

        //public virtual IAlarmableObj AlarmableObj { get; set; }
        [NotMapped]
        public virtual string ObjName
        {
            get { return _ObjName; }
            set { _ObjName = value; }
        }
        [NotMapped]
        public string _ObjName="NotSet";

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}