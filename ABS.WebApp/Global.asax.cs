using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace ABS.WebApp
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest()
        {
            HttpContext context = HttpContext.Current;

            /*// Allow cross-origin requests from http://localhost:3000
            context.Response.AddHeader("Access-Control-Allow-Origin", "http://localhost:3000");

            // Allowed HTTP methods for cross-origin requests
            context.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS");

            // Allowed headers for cross-origin requests
            context.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");*/

            // Handle preflight (OPTIONS) requests
            if (context.Request.HttpMethod == "OPTIONS")
            {
                // Return OK status for OPTIONS requests
                context.Response.StatusCode = 200;
                context.Response.End();
            }

        }

    }
}