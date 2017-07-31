using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using log4net;
using ProjectyMcProjectface.Controllers;

namespace ProjectyMcProjectface
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            /*ILog log = log4net.LogManager.GetLogger(typeof(ErrorController));

            log.Error(filterContext.Exception.Message);*/

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
