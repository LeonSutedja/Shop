using System.Web.Optimization;

namespace Shop
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Problem is currently with jquery-ui and bootstrap together.
            // In order to be able to use jquery dialog properly, we need the bootstrap javascript
            // to be added before the jquery-ui.
            // Some bootstrap javascript functionality may be broken because of the jquery-ui

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui-dialogextend").Include(
                        "~/Scripts/jquery.dialogextend.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/themes/base/core.css",
                      "~/Content/themes/base/resizable.css",
                      "~/Content/themes/base/selectable.css",
                      "~/Content/themes/base/dialog.css",
                      "~/Content/themes/base/theme.css",
                      "~/Content/site.css"
                      ));
        }
    }
}