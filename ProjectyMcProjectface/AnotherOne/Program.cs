using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Data;

namespace AnotherOne
{
    class Program
    {
        static void Main(string[] args)
        {
            string executionType;
            Console.Write("Would you like to enter DB backup data manually? (y/n) : ");
            executionType = Console.ReadLine();

            if (executionType.Trim().ToLower() == "y")
            {
                Console.WriteLine("Warning: entering invalid data will cause the application to crash!");
                Console.Write("Enter the database name: ");
                string DBName = Console.ReadLine();
                Console.Write(@"Enter complete server name, for example '.\SQLExpress': ");
                string serverName = Console.ReadLine();
                Console.Write(@"Enter the path of the .bak file: ");
                string backupPath = Console.ReadLine();
                Console.Write(@"Enter the path of the .mdf file (point to .bak file and change data type): ");
                string mdfPath = Console.ReadLine();
                Console.Write(@"Enter the path of the .ldf file (point to .bak file and change data type): ");
                string ldfPath = Console.ReadLine();

                RestoreDatabase(DBName, serverName, backupPath, mdfPath, ldfPath);


            }
            else if (executionType.Trim().ToLower() == "n")
            {
                RestoreDatabase("NorthWind", @".\SQLExpress", @"C:\DBBackup\Northwind.bak", @"C:\DBBackup\Northwind.mdf", @"C:\DBBackup\Northwind.ldf");
            }
            else if(executionType.Trim().ToLower() == "l")
            {
                Console.Write(@"Enter complete server name, for example '.\SQLExpress': ");
                string serverName = Console.ReadLine();
                Console.Write(@"Enter the path of the .bak file: ");
                string backupPath = Console.ReadLine();

            }

            Console.ReadKey();
        }
        static void GetListOfLogicalFiles(string serverInstance, string backupPath)
        {
            ServerConnection srvConn = new ServerConnection();
            srvConn.ServerInstance = serverInstance;
            srvConn.LoginSecure = true;

            Server server = new Server(srvConn);

            Restore res = new Restore();
            DataTable dt;
            DataRow[] foundrows;

            res.Devices.AddDevice(backupPath, DeviceType.File);
            dt = res.ReadFileList(server);

            foundrows = dt.Select();

            foreach (DataRow r in foundrows)
            {
                Console.WriteLine(r["LogicalName"].ToString());
            }
        }
        static void RestoreDatabase(string DBName, string serverInstance, string backupFilePath, string mdfPath, string ldfPath)
        {
            Console.WriteLine("Restoring "+DBName+" DB");

            ServerConnection srvConn = new ServerConnection();
            srvConn.ServerInstance = serverInstance;
            srvConn.LoginSecure = true;

            Server server = new Server(srvConn);

            Restore res = new Restore();
            res.Database = DBName;
            res.Action = RestoreActionType.Database;
            res.Devices.AddDevice(backupFilePath, DeviceType.File);
            res.PercentCompleteNotification = 10;

            res.ReplaceDatabase = true;
            res.PercentComplete += new PercentCompleteEventHandler(ProgressEventHandler);

            res.RelocateFiles.Add(new RelocateFile(DBName, mdfPath));
            res.RelocateFiles.Add(new RelocateFile(DBName+"_Log", ldfPath));

            res.SqlRestore(server);

            Console.WriteLine("Restoration Successfull !");
        }
        static void ProgressEventHandler(object sender, PercentCompleteEventArgs e)
        {
            Console.WriteLine(e.Percent.ToString() + "% restored");
        }
    }
}
