using AlarmBase.DomainModel.generics;
using OnlineMonitoringLog.Core.DomainModel;
using OnlineMonitoringLog.Core.DomainModel.generics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IwnTagType = System.Int32;
namespace AlarmBase.DomainModel.Entities
{

    public class OccurenceLog : INotifyPropertyChanged
    {

        [Key]
        public Guid OccLogId { get; set; }
        [ForeignKey("RegisteredOccConfig")]
        public int FK_occConfigID { get; set; }

        
        public string SetValuesAsJson { get; set; }
        AlarmState _state { get; set; }

        public AlarmState state
        {
            get { return _state; }
            set
            {
                _state = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("state");
            }
        }


        public DateTime SetTime { get; set; }

        DateTime? _ClearTime;
        public DateTime? ClearTime
        {
            get { return _ClearTime; }
            set
            {
                _ClearTime = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("ClearTime");
            }
        }

        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }




        bool _Acknowledge;
        public bool Acknowledge
        {
            get { return _Acknowledge; }
            set
            {
                _Acknowledge = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("Acknowledge");
            }
        }
        public string AckUser { get; set; }
         string _Comment { get; set; }

        public string Comment
        {
            get { return _Comment; }
            set
            {
                _Comment = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("Comment");
            }
        }
        public virtual RegisteredOccConfig RegisteredOccConfig { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        [NotMapped]
        string _AlarmMessage="Not Set";
        public string AlarmMessage
        {
            get {
                //return RegisteredOccConfig.Config+RegisteredOccConfig.ObjName;
                return _AlarmMessage;
            }
            set
            { _AlarmMessage = value; }
        }
    }



}