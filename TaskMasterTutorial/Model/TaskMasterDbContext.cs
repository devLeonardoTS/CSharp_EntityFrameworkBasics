using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TaskMasterTutorial.Model
{
    public class TaskMasterDbContext : DbContext
    {
  
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration appConfig = Program.GetAppConfig();

            //string connectionString = $"Data Source={AppDomain.CurrentDomain.BaseDirectory}TaskMaster.db";
            string sqliteDbFile = appConfig.GetSection("ConnectionStrings:SQLiteDbFile").Value;

            optionsBuilder.UseSqlite($"Data Source={AppDomain.CurrentDomain.BaseDirectory}{sqliteDbFile}");
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Status> Statuses { get; set; }


        /*
         * Remember, we are using a Code-First approach here. A Migration reflects your model class as an Entity in the DBMS.
         * With that said, when you have the DB Schema already defined, we can generate model classes based on the schema.
         * 
         * With the above configurations done, we can go ahead and use the Package Manager Console to
         * "add-migration MeaningfullNameToThisMigrationWithoutSpace" -- Sets up the Migration.
         * "update-database" -- Actually creates/updates the physical database schema.
         * 
         * Too long didn't read.:
         * 1. We created the Model Class for Tasks.
         * 2. We added it ot a DbContext wired to our SQLite DB file (Which got generated on the last step).
         * 3. We created a Migration (Since we are going Code-First here).
         * 4. We ran that Migration, creating/update the existing DB Data Source (In our case: A.k.a SQLite Db File).
         * 
         * Extra: We created a singleton property in our program entry-point to allow access to a "appsettings" file.
         * Through it we can assign the SQLite DB File name, instead of hard coding it here in this context's Data Source.
         * 
         * We can use "update-database MigrationName" to "revert" or "foward" to the specified migration state of our DB.
         * 
         * Next: We are adding an "AddStatusSeedDataToDb" migration in order to programatically add our base statuses into the "Statuses" table.
         * The migration will be created using "Add-Migration AddStatusSeedDataToDb" command and will have its Up() and Down() functions empty.
         */
    }
}
