namespace OnlineMonitoringLog.Core.DataRepository.Entities
{
    using OnlineMonitoringLog.Core.DataRepository.Entities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Net;

    [Table("hier.Unit")]
    public  class UnitEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UnitEntity()
        {
        }

        public int UnitId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public int StationID { get; set; }

        public int Capacity { get; set; }

        public virtual Station Station { get; set; }
       public int ID { get; set; }

        public ProtocolType Type { get; set; }

        private IPAddress _Ip;

        [NotMapped]
        public IPAddress Ip
        {
            get { return _Ip; }
            private set
            {
                _Ip = value;
              
            }
        }

        public string StringIp
        {
            get
            {

                return _Ip?.ToString();
            }
            set
            {
                if(value!=null)
                Ip = IPAddress.Parse(value);
            }
        }

    }

    public enum ProtocolType
    {
        CoAP,
        IEC104,
        ModbusTcp
    }
}
