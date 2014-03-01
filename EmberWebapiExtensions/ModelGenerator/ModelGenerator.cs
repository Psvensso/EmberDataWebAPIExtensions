using EmberWebapiExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace EmberWebapiExtensions
{
    public sealed class ModelGenerator
    {
        private static volatile ModelGenerator instance;
        private static volatile ModelExtractor modelExtractor;

        private static object syncRoot = new Object();

        private ModelGenerator() {
            modelExtractor = ModelExtractor.Instance;
        }

        public static ModelGenerator Instance
        {
            get 
            {
                if (instance == null) 
                {
                lock (syncRoot) 
                {
                    if (instance == null)
                        instance = new ModelGenerator();
                }
                }

                return instance;
            }
        }

        public EmberModel GenerateModel() {
            EmberModel model = CreateModelFromTypeTree(GetClassTypes());
            return model;
        }

        static IEnumerable<Type> GetClassTypes() {
            var assemblyNameSetting = WebConfigurationManager.AppSettings["EmberModel:ModelAssemblyName"];
            if(assemblyNameSetting == null){
                return modelExtractor.GetTypesWithEmberModelAttribute();
            }
            return modelExtractor.GetTypesWithEmberModelAttribute(assemblyNameSetting.ToString());
        }
        private EmberModel CreateModelFromTypeTree(IEnumerable<Type> types) {
            var model = new EmberModel();
            foreach(var t in types){
                var modelAttr = t.GetCustomAttribute<EmberModelAttribute>();
                var c = new EmberClass();
                
                c.name = String.IsNullOrWhiteSpace(modelAttr.name) ? t.Name : modelAttr.name;
                c.pluralName = String.IsNullOrWhiteSpace(modelAttr.pluralName) ? c.name : modelAttr.pluralName;
                c.primaryKey = "id";
                var props = t.GetProperties();

                foreach (var prop in props)
                {
                    if (hasAttribute<EmberIgnoreAttribute>(prop))
                        continue;
                    
                    var classProp = new EmberClassProperty();
                    var attr = prop.GetCustomAttribute<EmberPropertyAttribute>();
                    if (attr != null)
                    {
                        classProp.Name = attr.name ?? prop.Name;
                        classProp.PropertyType = attr.emberType ?? getEmberPropertyType(prop);
                    }
                    else {
                        classProp.Name = prop.Name;
                        classProp.PropertyType = getEmberPropertyType(prop);
                    }
                    if (hasAttribute<EmberPrimaryKeyAttribute>(prop))
                    {
                        c.primaryKey = classProp.Name;
                    }

                    //Ember dont want us to defined a attribute called id. 
                    if (classProp.Name == "id") 
                        continue;                
                    
                    c.properties.Add(classProp);
                }


                model.Classes.Add(c);
            }
            return model;
        }

        private string getEmberPropertyType(PropertyInfo prop) {
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
        private bool hasAttribute<T>(Type type) {
            return (type.GetCustomAttributes(typeof(T), true).Length > 0);
        }
        private bool hasAttribute<T>(PropertyInfo type)
        {
            return (type.GetCustomAttributes(typeof(T), true).Length > 0);
        }

    }
}
