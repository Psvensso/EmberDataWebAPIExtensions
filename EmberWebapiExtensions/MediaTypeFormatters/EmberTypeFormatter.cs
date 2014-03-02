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
                    returnObject.Add(emberAttr.name ?? type.Name, value);
                }

                var x = JsonConvert.SerializeObject(returnObject);
                var y = 0;
                y++;
                var sr = new StreamWriter(writeStream);
                sr.WriteLine(x);
                sr.Flush();

            });
        }
        
        public bool ShouldEnvelope(Type type)
        {
            if (type == typeof(object))
            {
                return false;
            }

            if (type == typeof(IEnumerable))
            {
                return false;
            }

            var innerType = type.getInnerType();

            if (innerType == typeof(string))
            {
                return false;
            }

            if (innerType == typeof(DateTime))
            {
                return false;
            }

            if (innerType == typeof(decimal))
            {
                return false;
            }

            if (innerType.IsPrimitive)
            {
                return false;
            }

            if (IsAnonymousType(innerType))
            {
                return false;
            }

            return true;
        }
        private static bool IsAnonymousType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                   && type.IsGenericType && type.Name.Contains("AnonymousType")
                   && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
                   && (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
        }
        
        internal class EnvelopeWrite : IEnvelope
        {
            public EnvelopeWrite(object value)
            {
                Value = value;
            }

            public object Value { get; set; }
        }
        internal interface IEnvelope
        {
        }
    }
}
