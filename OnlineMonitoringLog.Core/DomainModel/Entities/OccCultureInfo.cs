using AlarmBase.DomainModel;
using AlarmBase.DomainModel.generics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;
using IwnTagType = System.Int32;
namespace AlarmBase.DomainModel.Entities
{

    public class OccCultureInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler CultureChangeSaved;
        internal void OnCultureChangeSaved()
        {
            PropertyChangedEventHandler handler = CultureChangeSaved;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(this.CultureInfoId.ToString()));
            }
        }

        [Key]
        public int CultureInfoId { get; set; }
        [ForeignKey("RegisteredOccConfig")]
        public int FK_occConfigId { get; set; }

        public string Culture { get; set; }
        public string Template { get; set; }

        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public virtual RegisteredOccConfig RegisteredOccConfig { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

    }
    

  
    public class cultureType
    {
        private cultureType(string value) { Value = value; }

        public string Value { get; set; }

        public static cultureType Default { get { return new cultureType("Default"); } }
        public static cultureType fa_IR { get { return new cultureType("fa_IR"); } }
        public static cultureType en_US { get { return new cultureType("en_US"); } }
       
    }

}