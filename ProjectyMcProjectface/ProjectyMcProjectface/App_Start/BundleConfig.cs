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
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/pluginCss").Include(
                "~/Content/bootstrap.min.css",
                "~/Content/sb-admin.css",
                "~/Content/CustomStylesForMainPage.css"
                ));

            bundles.Add(new StyleBundle("~/Content/LoginVisuals").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap.min.css",
                "~/Content/LoginStyle.css"));

            bundles.Add(new ScriptBundle("~/bundles/ScriptsForLoginPage").Include(
                 "~/Scripts/loginPageJscript.js"));

            bundles.Add(new ScriptBundle("~/bundles/FormValidationsScripts").Include(
                "~/Scripts/jquery-{version}.js",
                 "~/Scripts/jquery.validate.js",
                  "~/Scripts/LoginFormValidation.js",
                  "~/Scripts/RegistrationFormValidation.js"
                 ));
            bundles.Add(new ScriptBundle("~/bundles/MainPageCommonScripts").Include(
                "~/Scripts/MainPageCommonScript.js"));
        }
    }
}
