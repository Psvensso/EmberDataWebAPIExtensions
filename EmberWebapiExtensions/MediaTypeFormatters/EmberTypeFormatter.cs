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
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            return hasEmberModelAttribute(type);
        }
        
        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
        {
            return Task.Factory.StartNew(() =>
            {
                var serializedType = value != null
                ? value.GetType()
                : type;

                var shouldEnvelope = shouldEnvelopeCache.GetOrAdd(serializedType, ShouldEnvelope);
                var isArr = serializedType.IsArray;
                var members = type.GetMembers();
                var innerValue = shouldEnvelope
                ? new EnvelopeWrite(value)
                : value;

                var val = value;
                var sw = new StreamWriter(writeStream, System.Text.Encoding.UTF8);

                var p = type.GetProperties();
                var f = type.GetFields();
                var h = type.CustomAttributes;
                var allEmberModels = GetTypesWithHelpAttribute(Assembly.GetExecutingAssembly());
                sw.WriteLine("{}");
                sw.Flush();

            });
            
            //return base.WriteToStreamAsync(type, innerValue, writeStream, content, transportContext);
        }
        public bool hasEmberModelAttribute(Type type)
        {
            return (type.GetCustomAttributes(typeof(EmberModelAttribute), true).Length > 0);
        }
        static IEnumerable<Type> GetTypesWithHelpAttribute(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeof(EmberModelAttribute), true).Length > 0)
                {
                    yield return type;
                }
            }
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

            var innerType = GetInnerType(type);

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
        private Type GetInnerType(Type type)
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
