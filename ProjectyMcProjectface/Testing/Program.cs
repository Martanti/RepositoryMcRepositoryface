using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bussiness;
using EFDataModels;

namespace Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            using(var context = new InternalDBModel())
            {
                foreach(RegisteredUser user in context.RegisteredUsers)
                {
                    Console.WriteLine(user.UserName + "_ _" + user.PassWord);
                }
            }
            Console.WriteLine("Viskas veikia");
            Console.ReadKey();
        }
    }
}
