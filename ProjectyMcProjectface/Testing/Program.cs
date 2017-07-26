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
            //IUserManager manager = new UserManager(); //Karolis_to do - dependency managers
            UserManager manager = new UserManager();
            manager.RegisterUser("Martynas", "123456", "martis@gmail.com");

            Console.WriteLine("Viskas veikia");
            Console.ReadKey();
        }
    }
}
