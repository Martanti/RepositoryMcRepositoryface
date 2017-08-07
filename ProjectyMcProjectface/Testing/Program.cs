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

            SmoManager manager = new SmoManager();
            Dto.Database db = manager.GetDatabaseByInternalConnString(@"Data Source=.\SQLEXPRESS;Initial Catalog=3_Northwind;Integrated Security=True", "");
            Dto.Table table = manager.GetCompleteTableData(@"Data Source=.\SQLEXPRESS;Database=3_Northwind;Integrated Security=True", "dbo", "Categories");
            Console.WriteLine(":)");
            Console.ReadKey();
        }
    }
}
