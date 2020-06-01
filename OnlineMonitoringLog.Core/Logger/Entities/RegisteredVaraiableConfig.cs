using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
namespace AlarmBase.DomainModel.Entities
{
    public class RegisteredVarConfig : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler ConfigChangeSaved;
        // Create the OnConfigChanged method to raise the event
        internal void OnConfigChangeSaved()
        {
            PropertyChangedEventHandler handler = ConfigChangeSaved;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(this.VarConfigID.ToString()));
            }
        }

        [Key]
        public int VarConfigID { get; set; }

        public int Fk_UnitEntityId { get; set; }

       //نوع متغییر را تعیین میکند که ولتاژ است یا جریان یا ...
        public string resourceName { get; set; }

        public ICollection<VariableLog> OccurenceLogs { get; set; }
    

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