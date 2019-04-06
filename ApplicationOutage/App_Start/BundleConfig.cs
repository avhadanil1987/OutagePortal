using System.Web;
using System.Web.Optimization;

namespace ApplicationOutage
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-migrate-1.1.1.js",
                "~/Scripts/jquery.unobtrusive-ajax.js",
                "~/Scripts/jquery-ui.js",
                "~/Scripts/jquery-ui.min.js",
                "~/Scripts/jquery.datetimepicker.full.js",
                "~/Scripts/moment.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate-vsdoc.js",
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js",
                        "~/Scripts/jquery.validate.date.js"
                        ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            //Encrypt Decrypt scripts
            bundles.Add(new ScriptBundle("~/bundles/crytography").Include(
                        "~/Scripts/aes.js",
                        "~/Scripts/cryptography.min.js"
                        ));

            // Application Scripts
            bundles.Add(new ScriptBundle("~/bundles/AppScript").Include(
                "~/Scripts/Outage.js",
                "~/Scripts/totalAvailablity.js"
                ));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/jquery.datetimepicker.min.css",
                      "~/Content/jquery-ui.theme.css",
                       "~/Content/c3.min.css",
                      "~/Content/site.css"));
        }
    }
}
