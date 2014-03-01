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

        public EmberPropertyAttribute(string _defaultValue, string _type, string _name) {
            defaultValue = _defaultValue; name = _name; emberType = _type;
        }
        
        public EmberPropertyAttribute(){}
    }
}