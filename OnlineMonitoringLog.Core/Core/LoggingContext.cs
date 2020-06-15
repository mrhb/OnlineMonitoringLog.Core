﻿
using AlarmBase.DomainModel.Entities;
using OnlineMonitoringLog.Core.DataRepository.Entities;
using System.Data.Entity;


namespace OnlineMonitoringLog.Core
{

    public class LoggingContext : AlarmableContext
    {
        public LoggingContext() : base("name = Default") { }
        public LoggingContext(string nameOrConnectionString) : base(nameOrConnectionString) { }
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