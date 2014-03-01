using EmberWebapiExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmberWebapiExtensions
{
    public static class ModelExtractor
    {
        /// <summary>
        /// Gets all models from a given assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetTypesWithEmberModelAttribute(Assembly assembly)
        {
            return FindTypes(assembly);
        }

        public static IEnumerable<Type> GetTypesWithEmberModelAttribute(string assemblyName)
        {
            return FindTypes(Assembly.Load(assemblyName));
        }

        /// <summary>
        /// Gets types from the executing assembly
        /// </summary>
        /// <returns>IEnumerable<Type></returns>
        public static IEnumerable<Type> GetTypesWithEmberModelAttribute()
        {
            return FindTypes(Assembly.GetExecutingAssembly());
        }

        static IEnumerable<Type> FindTypes(Assembly assembly) {
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
