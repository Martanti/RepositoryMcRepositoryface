using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;

namespace InstallAdventureWorksDb
{
    class Program
    {
        static void Main(string[] args)
        {
            string executionType;
            Console.Write("Would you like to enter DB backup data manually? (y/n) : ");
            executionType = Console.ReadLine();

            if(executionType.Trim().ToLower() == "y")
            {

            }
            else if(executionType.Trim().ToLower() == "n")
            {
                ServerConnection srvConn = new ServerConnection();
                /*srvConn.ServerInstance = @".\SQLExpress";
                srvConn.LoginSecure = true;

                Server server = new Server(srvConn);*/
                
                /*foreach(Database db in server.Databases)
                {
                    Console.WriteLine(db);
                }*/
               //Console.WriteLine(server.Name);
            }
            Console.ReadKey();
        }
    }
}
