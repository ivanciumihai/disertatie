using System.Web.Optimization;

namespace personal_pages
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-datepicker3.css",
                "~/Content/bootstrap-datepicker.css",
                "~/Content/site.css",
                "~/Content/themes/base/all.css",
                "~/Content/jquery-ui.css"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/bootstrap-datepicker.js",
                "~/Scripts/DatePickerReady.js",
                "~/Scripts/jquery-3.1.1.js",
                "~/Scripts/jquery-3.1.1.min.js",
                "~/Scripts/respond.js",
                "~/Scripts/regex.js",
                "~/Scripts/moment.js",
                "~/Scripts/modernizr-*",
                "~/Scripts/jquery-ui-{version}.js",
                "~/Scripts/jquery.validate"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = false;
        }
    }
}