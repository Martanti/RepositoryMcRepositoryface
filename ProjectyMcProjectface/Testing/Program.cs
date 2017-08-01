using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bussiness;
using EFDataModels;
using Resources;
using System.Reflection;

namespace Testing
{
    class Program
    {
        static void Main(string[] args)
        {

            IDatabaseManager manager = InjectionKernel.Instance.Get<IDatabaseManager>();
            /*bool IsAvailable = manager.IsDatabaseAvailable(System.Configuration.ConfigurationManager.AppSettings["InternalDBConnectionString"].ToString());
            if (IsAvailable)
            {
                Console.WriteLine("Connection succeeded");
            }
            else
            {
                Console.WriteLine("Connection failed");
            }*/
            Console.ReadKey();
        }
    }
}
