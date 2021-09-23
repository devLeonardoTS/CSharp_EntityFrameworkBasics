using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;

namespace TaskMasterTutorial
{
    static class Program
    {

        private static IConfiguration AppConfig { get; set; }

        public static IConfiguration GetAppConfig()
        {
            /*
             * Using "Microsoft.Extensions.Configuration.Json" and "Microsoft.Extensions.Configuration.Binder"
             * To read a configuration file since we don't have "App.config" in .NET Core 5.0.
             * 
             * The function acts as a singleton* access to the data in the configuration file, this allows the DbContext
             * to access the SQLiteDbFile Name from a config file instead of a hard-coded string when trying to use the
             * "Add-Migration" command through the Package Manager Console.
             * 
             * Singleton Pattern: Basically ensures that only one instance of an object is instantiated throughout the program.
             */

            // 
            if (AppConfig == null)
            {
                AppConfig = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();
            }
            return AppConfig;
        }


        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            GetAppConfig(); // Initializes/store the content within "appsettings.json" in the AppConfig property.

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
