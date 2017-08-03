using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Ninject.Syntax;
using EFDataModels;

namespace Bussiness
{
    public class InjectionKernel : StandardKernel
    {
        private static InjectionKernel instance;
        private InjectionKernel()
        {
            Bind<IUserManager>().ToConstructor(x => new UserManager(x.Inject<IEncryptionManager>(), x.Inject<IInternalDBModel>()));
            Bind<IEncryptionManager>().ToConstructor(x => new EncryptionManager());
            Bind<IDatabaseCopy>().ToConstructor(x => new DatabaseCopy());
            Bind<IInternalDBModel>().ToConstructor(x => new InternalDBContext());
            Bind<IDatabaseManager>().ToConstructor(x => new DatabaseManager(x.Inject<IInternalDBModel>()));
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
