using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmberWebapiExtensions.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EmberPropertyAttribute : Attribute
    {
        public string defaultValue { get; set; }
        public string emberType { get; set; }
        public string name { get; set; }

        public EmberPropertyAttribute(string Name, string EmberType, string DefaultValue)
        {
                defaultValue = DefaultValue; name = Name; emberType = EmberType;
        }
        public EmberPropertyAttribute(string Name, string EmberType)
        {
            name = Name; emberType = EmberType;
        }
        public EmberPropertyAttribute(string Name)
        {
            name = Name;
        }
        public EmberPropertyAttribute(){}
    }
}