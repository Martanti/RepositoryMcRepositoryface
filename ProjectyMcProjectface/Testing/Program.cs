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
            manager.RegisterDatabase(@"Data Source=.\SQLEXPRESS;Database=Northwind;Integrated Security=True", @"Data Source=.\SQLEXPRESS;Database=InternalDB;Integrated Security=True", 3, "TestingTTTTTTTTT");
            Console.WriteLine(":)");
            Console.ReadKey();
        }
    }
}
