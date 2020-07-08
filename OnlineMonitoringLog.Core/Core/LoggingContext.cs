
using AlarmBase.DomainModel.Entities;
using OnlineMonitoringLog.Core.DataRepository.Entities;
using System.Data.Entity;


namespace OnlineMonitoringLog.Core
{

    public class LoggingContext : AlarmableContext
    {
        public LoggingContext() : base("name = Default") { }
        public LoggingContext(string nameOrConnectionString) : base(nameOrConnectionString) { }
     //   public virtual DbSet<UnitEntity> Units { get; set; }


        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<Distribution> Distributions { get; set; }
        public virtual DbSet<Province> Provinces { get; set; }
        public virtual DbSet<Regional> Regionals { get; set; }
        public virtual DbSet<Station> Stations { get; set; }
        public virtual DbSet<UnitEntity> UnitEntity { get; set; }



        /// <summary>
        /// تنظیمات مربوط به متغیرهای هر واحد(ولتاژ، توان و ...) در این لیست آورده میشود.
        /// </summary>
        public DbSet<RegisteredVarConfig> varConfig { get; set; }

        /// <summary>
        /// اطلاعات مربوط به یک رخداد آلارمی شامل زمان وقوع، دیده شدن، زمان برطرف شدن و ...
        /// </summary>
        public DbSet<VariableLog> varLog { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Area>()
         .HasMany(e => e.Stations)
         .WithRequired(e => e.Area)
         .WillCascadeOnDelete(false);

            modelBuilder.Entity<Distribution>()
                .HasMany(e => e.Areas)
                .WithRequired(e => e.Distribution)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Regional>()
                .HasMany(e => e.Distributions)
                .WithRequired(e => e.Regional)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Regional>()
                .HasMany(e => e.Provinces)
                .WithRequired(e => e.Regional)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Station>()
                .HasMany(e => e.Units)
                .WithRequired(e => e.Station)
                .WillCascadeOnDelete(false);
        }


        }
}
