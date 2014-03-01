using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmberWebapiExtensions.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EmberPrimaryKeyAttribute : Attribute
    {
        public string _keyName { get; set; }
        public EmberPrimaryKeyAttribute(string keyname)
        {
            _keyName = keyname;
        }
    }
}
