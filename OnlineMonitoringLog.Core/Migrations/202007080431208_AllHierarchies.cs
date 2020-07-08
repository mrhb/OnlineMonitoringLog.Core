namespace OnlineMonitoringLog.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllHierarchies : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UnitEntities", newName: "Unit");
            MoveTable(name: "dbo.Unit", newSchema: "hier");
            CreateTable(
                "hier.Area",
                c => new
                    {
                        AreaId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        DistributionID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AreaId)
                .ForeignKey("hier.Distribution", t => t.DistributionID)
                .Index(t => t.DistributionID);
            
            CreateTable(
                "hier.Distribution",
                c => new
                    {
                        DistributionId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        RegionalID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DistributionId)
                .ForeignKey("hier.Regional", t => t.RegionalID)
                .Index(t => t.RegionalID);
            
            CreateTable(
                "hier.Regional",
                c => new
                    {
                        RegionalId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.RegionalId);
            
            CreateTable(
                "hier.Province",
                c => new
                    {
                        ProvinceId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        RegionalID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProvinceId)
                .ForeignKey("hier.Regional", t => t.RegionalID)
                .Index(t => t.RegionalID);
            
            CreateTable(
                "hier.Station",
                c => new
                    {
                        StationId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        AreaID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StationId)
                .ForeignKey("hier.Area", t => t.AreaID)
                .Index(t => t.AreaID);
            
            AddColumn("hier.Unit", "UnitId", c => c.Int(nullable: false));
            AddColumn("hier.Unit", "Name", c => c.String(nullable: false, maxLength: 100));
            AddColumn("hier.Unit", "StationID", c => c.Int(nullable: false));
            AddColumn("hier.Unit", "Capacity", c => c.Int(nullable: false));
            AlterColumn("dbo.OccurenceLogs", "SetTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.OccurenceLogs", "ClearTime", c => c.DateTime());
            CreateIndex("hier.Unit", "StationID");
            AddForeignKey("hier.Unit", "StationID", "hier.Station", "StationId");
        
       
        }
        
        public override void Down()
        {
            DropForeignKey("hier.Station", "AreaID", "hier.Area");
            DropForeignKey("hier.Unit", "StationID", "hier.Station");
            DropForeignKey("hier.Province", "RegionalID", "hier.Regional");
            DropForeignKey("hier.Distribution", "RegionalID", "hier.Regional");
            DropForeignKey("hier.Area", "DistributionID", "hier.Distribution");
            DropIndex("hier.Station", new[] { "AreaID" });
            DropIndex("hier.Unit", new[] { "StationID" });
            DropIndex("hier.Province", new[] { "RegionalID" });
            DropIndex("hier.Distribution", new[] { "RegionalID" });
            DropIndex("hier.Area", new[] { "DistributionID" });
            AlterColumn("dbo.OccurenceLogs", "ClearTime", c => c.DateTime(precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.OccurenceLogs", "SetTime", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            DropColumn("hier.Unit", "Capacity");
            DropColumn("hier.Unit", "StationID");
            DropColumn("hier.Unit", "Name");
            DropColumn("hier.Unit", "UnitId");
            DropTable("hier.Station");
            DropTable("hier.Province");
            DropTable("hier.Regional");
            DropTable("hier.Distribution");
            DropTable("hier.Area");
            MoveTable(name: "hier.Unit", newSchema: "dbo");
            RenameTable(name: "dbo.Unit", newName: "UnitEntities");
        }
    }
}
