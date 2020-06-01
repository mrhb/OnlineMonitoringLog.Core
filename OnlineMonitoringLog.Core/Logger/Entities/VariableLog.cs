


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

    public class VariableLog : INotifyPropertyChanged
    {
        

           [Key]
        public Guid VariableLogId { get; set; }
        [ForeignKey("RegisteredVarConfig")]
        public int FK_varaiableConfigID { get; set; }

        public int Value { get; set; }

        DateTime _timeStamp;
        public DateTime _imeStamp
        {
            get { return _timeStamp; }
            set
            {
                _timeStamp = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged("SetTime");
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

        public virtual RegisteredVarConfig RegisteredVarConfig { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
       
    }



}