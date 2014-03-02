using EmberWebapiExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmberWebapiExtensions
{
    public sealed class ModelExtractor
    {
        
        private static volatile ModelExtractor instance;
        private static object syncRoot = new Object();

        private ModelExtractor() { }

        public static ModelExtractor Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ModelExtractor();
                    }
                }

                return instance;
            }
        }
        
        /// <summary>
        /// Gets all models from a given assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public IEnumerable<Type> GetTypesWithEmberModelAttribute(Assembly assembly)
        {
            return FindTypes(assembly);
        }

        /// <summary>
        /// Loads upp and assembly based on a name and finds types within it.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public IEnumerable<Type> GetTypesWithEmberModelAttribute(string assemblyName)
        {
            return FindTypes(Assembly.Load(assemblyName));
        }

        /// <summary>
        /// Gets types from the executing assembly
        /// </summary>
        /// <returns>IEnumerable<Type></returns>
        public IEnumerable<Type> GetTypesWithEmberModelAttribute()
        {
            return FindTypes(Assembly.GetExecutingAssembly());
        }

        private IEnumerable<Type> FindTypes(Assembly assembly) {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeof(EmberModelAttribute), true).Length > 0)
                {
                    yield return type;
                }
            }
        }
    }
}
