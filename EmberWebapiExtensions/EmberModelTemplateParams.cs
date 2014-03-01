using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmberWebapiExtensions
{
    public partial class EmberModelTemplate
    {
        public IList<string> Definitions { get; private set; }
        public string name { get; private set; }
        public EmberModelTemplate()
        {
            this.name = "New name";
        }
    }
}