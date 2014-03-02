using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmberWebapiExtensions.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class HasManyAttribute : Attribute
    {
        public string embedded { get; set; }
        public string inverse { get; set; }
    }
}