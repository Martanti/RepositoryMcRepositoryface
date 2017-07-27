using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProjectyMcProjectface
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "LoginPage",
                url: "Login",
                defaults: new { controller = "Login", action = "Index"});

            routes.MapRoute(
               name: "LogingIn",
               url: "Login/LogInAction",
               defaults: new { controller = "Login", action = "LogInAction" });
        }
    }
}
