using DI_Water_Wash;
using log4net;
using log4net.Config;
using System.Reflection;

namespace DI_Water_Wash_NetCore
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            string password = "VietNam@P55";
            var itemDB = new Cls_DBMsSQL();
            itemDB.Initialize("10.102.4.20", "ApprovalDB", "readuser", "Boyd@2025");
            password = itemDB.GetPasswordByUser("ChangeMode");
            if (password == null)
            {
                MessageBox.Show("Can't connect to DataBase. Please contact PIE Technical or IT!!!",
                                "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new FrmMain(password));
        }
    }
}