
using AlarmBase.DomainModel.Entities;
using System.Collections.Generic;
using System.Data.Entity;


namespace OnlineMonitoringLog.UI_WPF.model
{
    public class LoggingContext : AlarmableContext
    {
        public LoggingContext() : base("name = Default") { }
        public virtual DbSet<UnitEntity> Units { get; set; }

        /// <summary>
        /// تنظیمات مربوط به متغییرهای هر واحد(ولتاژ، توان و ...) در این لیست آورده میشود.
        /// </summary>
        public DbSet<RegisteredVarConfig> varConfig { get; set; }

        /// <summary>
        /// اطلاعات مربوط به یک رخداد آلارمی شامل زمان وقوع، دیده شدن، زمان برطرف شدن و ...
        /// </summary>
        public DbSet<VariableLog> varLog { get; set; }


    }
}
