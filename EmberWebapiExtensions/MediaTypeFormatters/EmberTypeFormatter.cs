using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Net.Http.Headers;
using EmberWebapiExtensions.Attributes;
using System.Dynamic;
namespace EmberWebapiExtensions
{
    public class EmberTypeFormatter : JsonMediaTypeFormatter
    {

        private readonly ConcurrentDictionary<Type, Type> envelopeTypeCache =
            new ConcurrentDictionary<Type, Type>();

        private readonly ConcurrentDictionary<Type, bool> shouldEnvelopeCache =
            new ConcurrentDictionary<Type, bool>();

        public EmberTypeFormatter()
        {
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
        }
        public override bool CanReadType(Type type)
        {
            if(type.isEmberModel())
                return true;

            return base.CanReadType(type);
        }
        public override bool CanWriteType(Type type)
        {
            if (type.isEmberModel())
                return true;

            return base.CanReadType(type);
        }
        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            if(type.isCollection()){
                    if(type.getInnerType().isEmberModel() == false){
                        return base.WriteToStreamAsync(type, value, writeStream, content, transportContext);
                    }
            } else if(type.isEmberModel() == false){
                return base.WriteToStreamAsync(type, value, writeStream, content, transportContext);
            }

            return Task.Factory.StartNew(() =>
            {
                Dictionary<string, object> returnObject = new Dictionary<string, object>();

                if (type.isCollection())
                {
                    var emberAttr = type.getInnerType().GetCustomAttribute<EmberModelAttribute>();
                    returnObject.Add(emberAttr.pluralName ?? emberAttr.name, value);
                }
                else {
                    var emberAttr = type.GetCustomAttribute<EmberModelAttribute>();
                    returnObject.Add(emberAttr.name ?? type.Name, extractInnerRelations(returnObject, value, type));
                }

                var x = JsonConvert.SerializeObject(returnObject);
                var y = 0;
                y++;
                var sr = new StreamWriter(writeStream);
                sr.WriteLine(x);
                sr.Flush();
            });
        }

        //ToDo The BelongsTo method has to be pulled out for reuse and the hasmany must be implemented. 
        //ToDo Also sideloading attribute property must be added.
        private IDictionary<string, object> extractInnerRelations(IDictionary<string, object> returnObj, object baseObj, Type baseType){
            Dictionary<string, object> returnClassObject = new Dictionary<string, object>();

            foreach (var prop in baseType.GetProperties())
            {
                var propAttr = prop.GetCustomAttribute<EmberPropertyAttribute>();
                var name = propAttr == null ? prop.Name : (propAttr.name ?? prop.Name);
                                
                if (prop.hasAttribute<HasManyAttribute>()) {
                    
                }
                else if (prop.hasAttribute<BelongsToAttribute>()) {
                    
                    var propValue = prop.GetValue(baseObj);
                    if (propValue == null) { continue; }

                    var propValueType = propValue.GetType();
                    var modelAttr = propValueType.GetCustomAttribute<EmberModelAttribute>();

                    if (modelAttr == null) { throw new Exception("Your BelongsTo on " + prop.Name + " in " + baseType.Name + " is not a Ember model. Relations must be between ember models."); }
                    var primayK = propValueType.GetProperty(modelAttr.primaryKey).GetValue(propValue);
                    returnClassObject.Add(name, primayK); 
                }
                else {
                    returnClassObject.Add(name, prop.GetValue(baseObj)); 
                }
            }

            return returnClassObject;

        }

     }
}
