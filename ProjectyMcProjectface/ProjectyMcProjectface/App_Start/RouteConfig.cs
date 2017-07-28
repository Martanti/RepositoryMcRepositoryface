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
            /*
            routes.MapRoute(
                name: "LoginPage",
                url: "Login",
                defaults: new { controller = "Login", action = "Index"});

            routes.MapRoute(
               name: "LogingIn",
               url: "Login/LogInAction",
               defaults: new { controller = "Login", action = "LogInAction" });

            routes.MapRoute(
               name: "RedirectToRegistration",
               url: "Login/RedirectToRegisterPage",
               defaults: new { controller = "Login", action = "RedirectToRegisterPage" });
               */

            routes.MapRoute(
                name: "LoginPage",
                url: "Login/{action}",
                defaults: new { controller = "Login", action = "Index" });

            routes.MapRoute(
                name: "ErrorPage",
                url: "Error/{action}",
                defaults: new {controller = "Error", action = "Handle404"}
                );
        }
    }
}
