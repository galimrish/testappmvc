using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TestWebApplication.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: null,
            //    url: "{category}",
            //    defaults: new { controller = "Product", action = "Catalog", page = 1 }
            //    );

            //routes.MapRoute(
            //    name: null,
            //    url: "page={page}",
            //    defaults: new { controller = "Product", action = "Catalog" },
            //    constraints: new { page = @"\d+" }
            //    );

            //routes.MapRoute(
            //    name: "null",
            //    url: "{controller}/{action}",
            //    defaults: new { controller = "Home", action = "Index" }
            //);

            routes.MapRoute(
                name: null,
                url: "Error",
                defaults: new { controller = "Error", action = "Index" }
            );

            routes.MapRoute(
                name: null,
                url: "404",
                defaults: new { controller = "Error", action = "NotFound" }
            );            
            
            routes.MapRoute(
                name: null,
                url: "{controller}/{action}/{category}",
                defaults: new { controller = "Home", action = "Index", category = UrlParameter.Optional }
                //defaults: new { controller = "Product", action = "Catalog", category = "MobilePhones" }
                //defaults: new { controller = "Product", action = "FillBrandsCodes", category = ""}
            );
        }
    }
}
