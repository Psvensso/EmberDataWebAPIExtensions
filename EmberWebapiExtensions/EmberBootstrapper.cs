using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using System.Diagnostics.CodeAnalysis;

[assembly: PreApplicationStartMethod(typeof(EmberWebapiExtensions.EmberBootstrapper), "RegisterProxyRoutes")]
[assembly: PreApplicationStartMethod(typeof(EmberWebapiExtensions.EmberBootstrapper), "AddEmberJsonFormatter")]


namespace EmberWebapiExtensions
{
	/// <summary>
	/// Bootstrapper that starts up the model generator. (Inject the /embermodel route to the route pipeline)
	/// </summary>
	public static class EmberBootstrapper
	{
		public static void RegisterProxyRoutes()
		{
			var routeValues = new RouteValueDictionary();
			routeValues.Add("controller", null);
			routeValues.Add("action", null);

            RouteTable.Routes.Add("embermodels",
				new Route("embermodels", routeValues, new RouteHandler()));
		}

        public static void AddEmberJsonFormatter() {
            var conf = GlobalConfiguration.Configuration;


        
        }
	}
}