namespace OnlineMonitoringLog.Core.Migrations
{
    using DataRepository.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<OnlineMonitoringLog.Core.LoggingContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(OnlineMonitoringLog.Core.LoggingContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.Units.AddOrUpdate(
              p => p.StringIp,
              new UnitEntity {Type= ProtocolType.IEC104, StringIp = "192.168.0.19" },
              new UnitEntity { Type =ProtocolType.CoAp, StringIp = "127.0.0.1" }
            );

        }
    }
}
