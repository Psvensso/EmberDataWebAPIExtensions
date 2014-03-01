using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmberWebapiExtensions.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EmberModelAttribute : Attribute
    {
        public string name { get; set; }
        public string pluralName { get; set; }
        public EmberModelAttribute(string Name, string PluralName) {
            name = Name;
            pluralName = PluralName;
        }
    }
}