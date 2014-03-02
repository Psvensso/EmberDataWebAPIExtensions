using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace EmberWebapiExtensions.Templates
{
    public partial class EmberModelTemplate
    {
        public EmberModel model { get; set; }
        public string appNamespace { get; private set; }
        public EmberModelTemplate(EmberModel Model)
        {
            model = Model;
            var appName = WebConfigurationManager.AppSettings["EmberModel:AppNamespace"];
            appNamespace = appName == null ? "App" : appName.ToString();
        }
    }
}