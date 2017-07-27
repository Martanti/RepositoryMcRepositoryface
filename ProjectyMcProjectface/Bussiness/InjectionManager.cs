using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness
{
    public class InjectionManager<T>
    {
        private readonly Dictionary<string, Type> _factoryDictionary = new Dictionary<string, Type>();

        public InjectionManager()
        {
            Console.WriteLine("Factory Constructor");

            Type[] types = Assembly.GetAssembly(typeof(T)).GetTypes();

            foreach (Type type in types)
            {
                if (!typeof(T).IsAssignableFrom(type) || type == typeof(T))
                {
                    // Incorrect type
                    continue;
                }

                Console.WriteLine(string.Format("Factory adding: {0}", type.Name));

                // Add the type
                _factoryDictionary.Add(type.Name, type);
            }
        }

        public T Create<V>(params object[] args)
        {
            return (T)Activator.CreateInstance(_factoryDictionary[typeof(V).Name], args);
        }
    }
}
