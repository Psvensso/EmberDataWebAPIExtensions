using EmberWebapiExtensions.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmberWebapiExtensions
{
    public static class ExtensionMethods
    {
        #region Type
        public static bool hasAttribute<T>(this Type type){
            return (type.GetCustomAttributes(typeof(T), true).Length > 0);
        }
        public static bool isEmberModel(this Type t)
        {
            return t.hasAttribute<EmberModelAttribute>();
        }
        public static Type getInnerType(this Type type)
        {
            if (type.IsArray)
            {
                return type.GetElementType();
            }

            var underlying = Nullable.GetUnderlyingType(type);
            if (underlying != null)
            {
                return underlying;
            }

            if (type.IsGenericType
                && typeof(IEnumerable<>).IsAssignableFrom(type.GetGenericTypeDefinition()))
            {
                return type.GetGenericArguments()[0];
            }

            return type;
        }
        public static bool isCollection(this Type type) {
            return typeof(IEnumerable).IsAssignableFrom(type);
        }
        #endregion
        #region PropertyInfo
        public static bool hasAttribute<T>(this PropertyInfo type)
        {
            return (type.GetCustomAttributes(typeof(T), true).Length > 0);
        }
        public static string getEmberPropertyType(this PropertyInfo prop)
        {
            var p = prop;

            switch (p.PropertyType.Name)
            {
                case "String":
                    return "string";
                case "Int32":
                    return "number";
                case "Boolean":
                    return "boolean";
                case "DateTime":
                    return "date";

            }
            return String.Empty;
        }
        #endregion

        public static string uppercaseFirst(this String s){
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}
