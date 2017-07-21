using System.Web;
using System.Web.Optimization;

namespace ProjectyMcProjectface
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.10.2.min.js",
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css"));

            bundles.Add(new ScriptBundle("~/bundles/plugins-flot").Include(
                    "~/Scripts/excanvas.min.js",
                    "~/Scripts/flot-data.js",
                    "~/Scripts/jquery.flot.js",
                    "~/Scripts/jquery.flot.pie.js",
                    "~/Scripts/jquery.flot.resize.js",
                    "~/Scripts/jquery.flot.tooltip.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/plugins-morris").Include(
                "~/Scripts/morris.js",
                "~/Scripts/morris.min.js",
                "~/Scripts/morris-data.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/plugin-raphael").Include(
                "~/Scripts/raphael.min.js"));

            bundles.Add(new StyleBundle("~/Content/pluginCss").Include(
                "~/Content/morris.css",
                "~/Content/bootstrap.min.css",
                "~/Content/sb-admin.css"));

        }
    }
}
