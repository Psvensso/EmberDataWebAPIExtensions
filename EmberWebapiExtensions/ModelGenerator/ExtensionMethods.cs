using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmberWebapiExtensions
{
    public static class ExtensionMethods
    {
        public static bool hasAttribute<T>(this Type type){
            return (type.GetCustomAttributes(typeof(T), true).Length > 0);
        }
        public static bool hasAttribute<T>(this PropertyInfo type)
        {
            return (type.GetCustomAttributes(typeof(T), true).Length > 0);
        }
        public static string uppercaseFirst(this String s){
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}
