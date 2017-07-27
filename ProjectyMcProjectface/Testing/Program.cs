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
            
            using(var context = new InternalDBModel())
            {
                context.RegisteredUsers.Add(new RegisteredUser() { UserName = "Martynas" , PassWord = "Gaidys", Email="lioler@yourass.com"});
                context.SaveChanges();
            }
            Console.ReadKey();
        }
    }
}
