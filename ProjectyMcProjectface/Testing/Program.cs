using System;
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
            bool IsAvailable = manager.IsDatabaseAvailable(@"Data Source=.\SQLEXPRESS;Database=InternalDBd;Integrated Security=True");
            if (IsAvailable)
            {
                Console.WriteLine("Connection succeeded");
            }
            else
            {
                Console.WriteLine("Connection failed");
            }
            Console.ReadKey();
        }
    }
}
