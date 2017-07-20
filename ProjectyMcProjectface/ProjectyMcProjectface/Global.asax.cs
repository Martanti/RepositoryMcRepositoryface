using ProjectyMcProjectface.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ProjectyMcProjectface
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Add any pending Database migrations to the database
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<InternalDBModel, Configuration>());

            //For DB Testing purposes only =>
            using (var context = new InternalDBModel()) {
                context.ConnectionStrings.Add(new ConnectionString() { ConnectionId = 1, UserId = 1, String = "Lioler" });
                context.SaveChanges();
            }
                //<=

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
