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
            DataRow[] logicalfiles;

            Console.WriteLine("Warning: entering invalid data will cause the application to crash!");
            Console.Write(@"Enter complete server name, for example '.\SQLExpress': ");
            string serverName = Console.ReadLine();

            Console.Write("Would you like to create an InternalDB database on the specified server? (y/n): ");
            string createInternalDB = Console.ReadLine();
            if(createInternalDB.Trim().ToLower() == "y")
            {
                Console.Write("Enter the database name: ");
                string DatabaseName = Console.ReadLine();
                CreateDatabase(serverName, DatabaseName);
            }
            else if(createInternalDB.Trim().ToLower() != "n" && createInternalDB.Trim().ToLower() != "y")
            {
                Console.WriteLine("Invalid command! ");
                Console.WriteLine("");
            }


            

            Console.Write("Enter the database name: ");
            string DBName = Console.ReadLine();
            
            Console.Write(@"Enter the path of the .bak file: ");
            string backupPath = Console.ReadLine();
            Console.Write(@"Enter the path where the .mdf file will be created: ");
            string mdfPath = Console.ReadLine();
            Console.Write(@"Enter the path where the .ldf file will be created: ");
            string ldfPath = Console.ReadLine();

            logicalfiles = GetListOfLogicalFiles(serverName, backupPath);
            RestoreDatabase(DBName, serverName, backupPath, logicalfiles[0]["LogicalName"].ToString(), logicalfiles[1]["LogicalName"].ToString(), mdfPath, ldfPath);
            

            Console.ReadKey();
        }
        static void CreateDatabase(string serverInstance, string databaseName)
        {
            ServerConnection srvConn = new ServerConnection();
            srvConn.ServerInstance = serverInstance;
            srvConn.LoginSecure = true;

            Server server = new Server(srvConn);

            Database db = new Database(server, databaseName);
            db.Create();
            Console.WriteLine("Database created successfully!");
            Console.WriteLine(" ");
        }
        static DataRow[] GetListOfLogicalFiles(string serverInstance, string backupPath)
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
            Console.WriteLine("List of logicalfiles: ");
            foreach (DataRow r in foundrows)
            {
                Console.WriteLine(r["LogicalName"].ToString());
            }
            Console.WriteLine();
            return foundrows;
        }
        static void RestoreDatabase(string DBName, string serverInstance, string backupFilePath,string mdfLogicalName, string ldfLogicalName, string mdfPath, string ldfPath)
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

            res.RelocateFiles.Add(new RelocateFile(mdfLogicalName, mdfPath));
            res.RelocateFiles.Add(new RelocateFile(ldfLogicalName, ldfPath));

            res.SqlRestore(server);

            Console.WriteLine("Restoration Successfull !");
        }
        static void ProgressEventHandler(object sender, PercentCompleteEventArgs e)
        {
            Console.WriteLine(e.Percent.ToString() + "% restored");
        }
    }
}
