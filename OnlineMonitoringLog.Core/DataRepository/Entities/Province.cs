namespace OnlineMonitoringLog.Core.DataRepository.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("hier.Province")]
    public partial class Province
    {
        public int ProvinceId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public int RegionalID { get; set; }

        public virtual Regional Regional { get; set; }
    }
}
