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

            routes.MapRoute(
                name:"Registration",
                url:"Registration",
                defaults: new { controller = "Registration", action = "Register"});

            routes.MapRoute(

                name: "RegistrationSubmit",
                url: "Registration/RegistrationSubmint",
                defaults: new { controller = "Registration", action = "RegistrationSubmint" });
           
          routes.MapRoute(
                name: "ErrorPage",
                url: "Error/{action}",
                defaults: new {controller = "Error", action = "Handle404"});

            routes.MapRoute(
                name:"MainPageHome",
                url:"Home/Index",
                defaults: new { controller = "Home", action = "Index" });

            routes.MapRoute(
                name: "default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" }
                );

        }
    }
}
