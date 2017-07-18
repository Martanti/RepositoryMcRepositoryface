using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AdventureWorksConnection
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new AdventureWorksDBModel())
            {
                context.Departments.RemoveRange(context.Departments.Where(d => d.GroupName == "FuckYou"));
                context.SaveChanges();

                foreach(var dep in context.Departments)
                {
                    Console.WriteLine(dep.GroupName);
                }
            }
                Console.ReadLine();
        }
    }
}
