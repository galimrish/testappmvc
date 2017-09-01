using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NLog;

namespace TestWebApplication.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public static Logger Logger { get { return _logger; } }

        protected void Application_Start()
        {
            Logger.Info("Application started");
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        
        protected void Application_Error()
        {
            Exception ex = Server.GetLastError();
            Server.ClearError();
            Logger.Error(ex);            
            int errorCode = ex is HttpException ?
                ((HttpException)ex).GetHttpCode() : -1;
            string viewName = errorCode == 404 ?
                "/404" : "/Error";
            Response.Redirect(viewName);
        }
    }
}
