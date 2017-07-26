using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bussiness;

namespace Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseCopy.CopyDatabaseSMO(@"Data Source=.\SQLExpress;Database=Northwind; Integrated security=true", @"Data Source=.\SQLExpress; Integrated security=true", "10");
            Console.WriteLine("Valio");
            Console.ReadKey();
        }
    }
}
