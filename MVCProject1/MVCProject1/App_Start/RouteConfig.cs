/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using System.Web.Routing;
using Microsoft.AspNetCore.Routing;

namespace MVCProject1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

  
            routes.MapRoute(
               name: "test",
              url: "test/{action}",
               defaults: new { controller = "test", action = "TestView"}
          );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Payment", id = UrlParameter.Optional }
            );
        }

    }
}
*/
