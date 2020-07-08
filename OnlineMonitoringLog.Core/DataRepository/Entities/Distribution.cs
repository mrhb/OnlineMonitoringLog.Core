namespace OnlineMonitoringLog.Core.DataRepository.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("hier.Distribution")]
    public partial class Distribution
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Distribution()
        {
            Areas = new HashSet<Area>();
        }

        public int DistributionId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public int RegionalID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Area> Areas { get; set; }

        public virtual Regional Regional { get; set; }

    
    }
}
