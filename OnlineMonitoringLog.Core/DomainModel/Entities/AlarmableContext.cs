using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AlarmBase.DomainModel;
using System.Xml;
using System.Xml.Serialization;
using IwnTagType = System.Int32;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace AlarmBase.DomainModel.Entities
{

    public abstract class AlarmableContext : DbContext
    {
        /// <summary>
        /// Constructs a new context instance with the default connection string.
        /// </summary>
        public AlarmableContext(string nameOrStringConnection)
            : base(nameOrStringConnection)
        {
            // this.Configuration.LazyLoadingEnabled = false;
        }
        public AlarmableContext()
           : base()
        {

        }

        /// <summary>
        /// تنظیمات مربوط به رخدادهای ممکن اشیا در این لیست آورده میشود.
        /// </summary>
        public DbSet<RegisteredOccConfig> occConfig { get; set; }
        /// <summary>
        /// اطلاعات مربوط به یک رخداد آلارمی شامل زمان وقوع، دیده شدن، زمان برطرف شدن و ...
        /// </summary>
        public DbSet<OccurenceLog> occLog { get; set; }

        /// <summary>
        ///چند زبانه بودن سامانه با استفاده از این جدول انجام میشود.
        /// </summary>
        public DbSet<OccCultureInfo> occCulture { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OccurenceLog>()
                .Property(f => f.ClearTime)
                .HasColumnType("datetime2");

            modelBuilder.Entity<OccurenceLog>()
               .Property(f => f.SetTime)
               .HasColumnType("datetime2");

           // this.Database.Log = Console.Write;

            // configures one-to-many relationship
            //modelBuilder.Entity<OccurenceLog>()
            //    .HasRequired<RegisteredOccConfig>(s => s.RegisteredOccConfig)
            //    .WithMany(g => g.OccurenceLogs)
            //    .HasForeignKey<int>(s => s.FK_occConfigID);
        }


        internal async Task<int> AddOccLogAsync( OccurenceLog newlog)
        {
            Console.Write(newlog.FK_occConfigID.ToString() + "   " + "Enter Adding...\n");


            var res = await this.Database.ExecuteSqlCommandAsync(
                "Insert into OccurenceLogs Values(@OccLogId, @FK_occConfigID,@SetValuesAsJson,@state,@SetTime,@ClearTime,@Acknowledge,@AckUser,@Comment)",
                new SqlParameter("OccLogId", newlog.OccLogId),
                new SqlParameter("FK_occConfigID", newlog.FK_occConfigID),
                new SqlParameter("SetValuesAsJson", " "),
                new SqlParameter("state", newlog.state),
                new SqlParameter("ClearTime", (object)newlog.ClearTime ?? DBNull.Value),
                new SqlParameter("SetTime", newlog.SetTime),
                new SqlParameter("Acknowledge", " "),
                new SqlParameter("AckUser", " "),
                new SqlParameter("Comment", " ")
                );
            this.occLog.Attach(newlog);
            if (newlog == null ? false : (newlog.Acknowledge == true))
                this.Entry(newlog).State = EntityState.Detached;

            Console.Write(newlog.FK_occConfigID.ToString()+ "   "+"Added    "+ newlog.OccLogId.ToString()+ "\n");
            return res;   
        }
        internal int AddOccLog(OccurenceLog newlog)
        {
            Console.Write(newlog.FK_occConfigID.ToString() + "   " + "Enter Adding...\n");

            var tt = System.Environment.TickCount;
            //var res =  this.Database.ExecuteSqlCommand(
            //    "Insert into OccurenceLogs Values(@FK_occConfigID,@SetValuesAsJson,@state,@SetTime,@ClearTime,@Acknowledge,@AckUser,@Comment)",
            //    //"Insert into OccurenceLogs Values(@OccLogId, @FK_occConfigID,@SetValuesAsJson,@state,@SetTime,@ClearTime,@Acknowledge,@AckUser,@Comment)",
            //    //new SqlParameter("OccLogId", newlog.OccLogId),
            //    new SqlParameter("FK_occConfigID", newlog.FK_occConfigID),
            //    new SqlParameter("SetValuesAsJson", " "),
            //    new SqlParameter("state", newlog.state),
            //    new SqlParameter("ClearTime", (object)newlog.ClearTime ?? DBNull.Value),
            //    new SqlParameter("SetTime", newlog.SetTime),
            //    new SqlParameter("Acknowledge", " "),
            //    new SqlParameter("AckUser", " "),
            //    new SqlParameter("Comment", " ")
            //    );


          
            var res= this.SaveChanges();
            var ttt = System.Environment.TickCount - tt;
            Console.Write(newlog.FK_occConfigID.ToString() + "Insert into OccurenceLogs:" + ttt.ToString() + "\n");


            this.occLog.Attach(newlog);
            if (newlog == null ? false : (newlog.Acknowledge == true))
                this.Entry(newlog).State = EntityState.Detached;

            Console.Write(newlog.FK_occConfigID.ToString() + "   " + "Added    " + newlog.OccLogId.ToString() + "\n");
            return res;
        }

        internal async Task<int> SetClearedOccLogAsync(OccurenceLog newlog)
        {
            Console.Write(newlog.FK_occConfigID.ToString() + "   " + "Enter Clearing...\n");


            var res =await  this.Database.ExecuteSqlCommandAsync(
                "UPDATE [dbo].[OccurenceLogs] SET  [state] = @state,[ClearTime] = @ClearTime where [OccLogId] =@OccLogId",
                new SqlParameter("OccLogId", newlog.OccLogId),

                new SqlParameter("state", newlog.state),
                new SqlParameter("ClearTime", (object)newlog.ClearTime ?? DBNull.Value)
                     );
            if (newlog.state == generics.AlarmState.clear & newlog.Acknowledge == true)
                this.Entry(newlog).State = EntityState.Detached;
            else
                this.occLog.Attach(newlog);
            Console.Write(newlog.FK_occConfigID.ToString() + "   Cleared\n");

            return res;
        }
        internal  int SetClearedOccLog(OccurenceLog newlog)
        {
            Console.Write(newlog.FK_occConfigID.ToString() + "   " + "Enter Clearing...\n");


            var res =  this.Database.ExecuteSqlCommand(
                "UPDATE [dbo].[OccurenceLogs] SET  [state] = @state,[ClearTime] = @ClearTime where [OccLogId] =@OccLogId",
                new SqlParameter("OccLogId", newlog.OccLogId),

                new SqlParameter("state", newlog.state),
                new SqlParameter("ClearTime", (object)newlog.ClearTime ?? DBNull.Value)
                     );
            if (newlog.state == generics.AlarmState.clear & newlog.Acknowledge == true)
                this.Entry(newlog).State = EntityState.Detached;
            else
                this.occLog.Attach(newlog);
            Console.Write(newlog.FK_occConfigID.ToString() + "   Cleared\n");

            return res;
        }
        public async Task<int> UpdateOccLogAckAsync(OccurenceLog newlog)
        {
            Console.Write(newlog.OccLogId.ToString() + "   " + "Enter Acnowledging...\n");

            var res = await this.Database.ExecuteSqlCommandAsync(
                 "UPDATE [dbo].[OccurenceLogs] SET  [Acknowledge] = @Acknowledge where [OccLogId] =@OccLogId",
                 new SqlParameter("OccLogId", newlog.OccLogId),

                 new SqlParameter("Acknowledge", newlog.Acknowledge)
                      );
            if (newlog.state == generics.AlarmState.clear & newlog.Acknowledge == true)
                this.Entry(newlog).State = EntityState.Detached;
            else
                this.occLog.Attach(newlog);
            Console.Write(newlog.FK_occConfigID.ToString() + "   Acnowleged\n");


            return res;
        }
        public  int UpdateOccLogAck(OccurenceLog newlog)
        {
            //Console.Write(newlog.OccLogId.ToString() + "   " + "Enter Acnowledging...\n");

            //var res =  this.Database.ExecuteSqlCommand(
            //     "UPDATE [dbo].[OccurenceLogs] SET  [Acknowledge] = @Acknowledge where [OccLogId] =@OccLogId",
            //     new SqlParameter("OccLogId", newlog.OccLogId),

            //     new SqlParameter("Acknowledge", newlog.Acknowledge)
            //          );
            if (newlog.state == generics.AlarmState.clear & newlog.Acknowledge == true)
                this.Entry(newlog).State = EntityState.Detached;
            else
                this.occLog.Attach(newlog);
            //Console.Write(newlog.FK_occConfigID.ToString() + "   Acnowleged\n");


            return 1;
        }




        //public async override Task<int> SaveChangesAsync()
        //{
        //    // var res = base.SaveChangesAsync();

        //    var original = this.ChangeTracker.Entries()
        //             .Where(x => x.State != EntityState.Unchanged)
        //             .ToList();

        //    var res =await base.SaveChangesAsync();
        //    foreach (var cng in original)
        //    {
        //        if ((cng.Entity.GetType() == typeof(RegisteredOccConfig)))
        //        {
        //            RegisteredOccConfig modifiedRegisteredOccConfig = cng.Entity as RegisteredOccConfig;

        //            modifiedRegisteredOccConfig.OnConfigChangeSaved();
        //        }
        //    }

        //    return res;

        //}

        public async  Task<int> SaveOcclogChangesAsync()
        {
            int res = -1;
            // var res = base.SaveChangesAsync();

            var original = this.ChangeTracker.Entries()
                    .ToList();
            try
            {
                 res = await base.SaveChangesAsync();
                foreach (var cng in original)
                {
                    if ((cng.Entity.GetType() == typeof(RegisteredOccConfig)))
                    {
                        RegisteredOccConfig modifiedRegisteredOccConfig = cng.Entity as RegisteredOccConfig;

                        modifiedRegisteredOccConfig.OnConfigChangeSaved();
                    }

                    if ((cng.Entity.GetType() == typeof(OccurenceLog)))
                    {
                        OccurenceLog modifiedOccurenceLog = cng.Entity as OccurenceLog;
                        if (modifiedOccurenceLog.state == generics.AlarmState.clear & modifiedOccurenceLog.Acknowledge == true)
                            this.Entry(modifiedOccurenceLog).State = EntityState.Detached;
                    }
                }
            }
            catch(Exception c)

            {
                Console.Write("Error At:SaveOcclogChangesAsync:"+c.ToString()+"\n");

            }

            return res;

        }
        //public override int SaveChanges()
        //{

        //    var manager = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager;

        //    var res = base.SaveChanges();

        //    foreach (ObjectStateEntry entry in
        //          manager.GetObjectStateEntries(
        //          EntityState.Modified))
        //    {
        //        if (!entry.IsRelationship && (entry.Entity.GetType() == typeof(RegisteredOccConfig)))
        //        {

        //            RegisteredOccConfig modifiedRegisteredOccConfig = entry.Entity as RegisteredOccConfig;

        //            modifiedRegisteredOccConfig.OnConfigChangeSaved();
        //        }
             
        //    }
        //    return res;

        //}


        private readonly ObservableCollection<OccurenceLog> _posts =
       new ObservableCollection<OccurenceLog>();
        
        public virtual ObservableCollection<OccurenceLog> Posts
        {
            get { return _posts; }
        }

    }


}
