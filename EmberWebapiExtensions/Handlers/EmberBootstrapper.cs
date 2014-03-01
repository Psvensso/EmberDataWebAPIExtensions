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

namespace EmberWebapiExtensions
{
	/// <summary>
	/// Bootstrapper that starts up the model generator.
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
	}
}