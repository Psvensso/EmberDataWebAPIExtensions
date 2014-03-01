using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace EmberWebapiExtensions
{
    public class EmberHttpHandler : IHttpHandler
    {
        
            private static string _proxyJs;
            private static volatile object _syncRoot = new object();
            private IScriptGenerator _generator;

            public EmberHttpHandler()
            {
                _generator = new EmberScriptGenerator();
            }

            /// <summary>
            /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler" /> instance.
            /// </summary>
            /// <returns>true if the <see cref="T:System.Web.IHttpHandler" /> instance is reusable; otherwise, false.</returns>
            public bool IsReusable
            {
                get { return true; }
            }

            /// <summary>
            /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler" /> interface.
            /// </summary>
            /// <param name="context">An <see cref="T:System.Web.HttpContext" /> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
            public void ProcessRequest(HttpContext context)
            {
                if (context == null) throw new ArgumentNullException("context");

                context.Response.ContentType = "application/javascript";
                context.Response.Write(GetProxyJs(context));
            }

            private string GetProxyJs(HttpContext context)
            {
                if (_proxyJs == null)
                    lock (_syncRoot)
                        if (_proxyJs == null)
                            _proxyJs = _generator.GenerateModelScript();

                return _proxyJs;
            }
        }
    
}