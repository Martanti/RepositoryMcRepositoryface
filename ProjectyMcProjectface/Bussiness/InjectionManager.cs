using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Ninject.Syntax;

namespace Bussiness
{
    public class InjectionKernel : StandardKernel
    {
        private static InjectionKernel instance;
        private InjectionKernel()
        {
            Bind<IUserManager>().ToConstructor(x => new UserManager(x.Inject<IEncryptionManager>())); // Possible to write x.Inject<Type>();
            Bind<IEncryptionManager>().ToConstructor(x => new EncryptionManager());
            
        }
        
        public static InjectionKernel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InjectionKernel();
                }
                return instance;
            }
        }

        public T Get<T>()
        {
            return ((StandardKernel)this).Get<T>();
        }

    }
    
}
