﻿using EmberWebapiExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace EmberWebapiExtensions
{
    public class ModelGenerator
    {
        private static Assembly modelAssembly;
        #region Ctor
        private static volatile ModelGenerator instance;
        private static volatile ModelExtractor modelExtractor;

        private static object syncRoot = new Object();
        private ModelGenerator() {
            modelExtractor = ModelExtractor.Instance;
        }
        public ModelGenerator(Assembly ModelAssembly)
        {
            modelAssembly = ModelAssembly;
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
        #endregion

        public EmberModel GenerateModel() {
            EmberModel model = CreateModelFromTypeTree(GetClassTypes());
            return model;
        }
        public IEnumerable<Type> GetClassTypes()
        {
            var assemblyNameSetting = WebConfigurationManager.AppSettings["EmberModel:ModelAssemblyName"];
            if (assemblyNameSetting == null)
            {
                return modelExtractor.GetTypesWithEmberModelAttribute();
            }
            return modelExtractor.GetTypesWithEmberModelAttribute(assemblyNameSetting.ToString());
        }
        public EmberModel CreateModelFromTypeTree(IEnumerable<Type> types)
        {
            var model = new EmberModel();
            foreach(var t in types){
                model.Classes.Add(CreateEmberClassFromType(t));
            }
            return model;
        }
        public EmberClass CreateEmberClassFromType(Type t) {
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

                var emProp = CreateEmberProperty(prop, c);
            }

            return c;
        }
        public EmberClass CreateEmberProperty(PropertyInfo prop, EmberClass c)
        {
            if(hasAttribute<HasManyAttribute>(prop) || hasAttribute<BelongsToAttribute>(prop)){
                return createEmberRelationProperty(prop, c);
            }

            return createEmberValueProperty(prop, c);
        }
        public EmberClass createEmberRelationProperty(PropertyInfo prop, EmberClass c)
        {
            var relationProp = new EmberClassRelationProperty();
            var isBelongsTo =  hasAttribute<BelongsToAttribute>(prop);

            relationProp.isBelongsTo = isBelongsTo;
            relationProp.Name = hasAttribute<EmberPropertyAttribute>(prop) ? prop.GetCustomAttribute<EmberPropertyAttribute>().name : prop.Name;

            if (isBelongsTo) {
                if (!hasAttribute<EmberModelAttribute>(prop.PropertyType))
                    throw new Exception("You said BelongsTo on " + c.name + "." + prop.Name + ". But thats not a EmberModel.");

                var relPropAttr = prop.PropertyType.GetCustomAttribute<EmberModelAttribute>();
                relationProp.relationTypeName = relPropAttr.name;
            }
            else {
                var relProp = prop.PropertyType;
                if (relProp.IsGenericType && relProp.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var baseType = relProp.GetGenericArguments().First();
                    
                    if (baseType == null || !hasAttribute<EmberModelAttribute>(baseType))
                        throw new Exception("You said HasMany on " + c.name + "." + prop.Name + ". But thats not a EmberModel.");

                    var baseAttr = baseType.GetCustomAttribute<EmberModelAttribute>();
                    relationProp.relationTypeName = baseAttr.pluralName ?? baseAttr.name;
                }
            }

            c.relationproperties.Add(relationProp);

            return c;
        }
        public EmberClass createEmberValueProperty(PropertyInfo prop, EmberClass c)
        {
            var classProp = new EmberClassProperty();
            var attr = prop.GetCustomAttribute<EmberPropertyAttribute>();
            if (attr != null)
            {
                classProp.Name = attr.name ?? prop.Name;
                classProp.PropertyType = attr.emberType ?? getEmberPropertyType(prop);
            }
            else
            {
                classProp.Name = prop.Name;
                classProp.PropertyType = getEmberPropertyType(prop);
            }

            if (hasAttribute<EmberPrimaryKeyAttribute>(prop))
            {
                c.primaryKey = classProp.Name;
            }

            //Ember dont want us to defined a attribute called id. 
            if (classProp.Name != "id")
                c.properties.Add(classProp);
                
            return c;
        }
        public string getEmberPropertyType(PropertyInfo prop)
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
        public bool hasAttribute<T>(Type type)
        {
            return (type.GetCustomAttributes(typeof(T), true).Length > 0);
        }
        public bool hasAttribute<T>(PropertyInfo type)
        {
            return (type.GetCustomAttributes(typeof(T), true).Length > 0);
        }
    }
}