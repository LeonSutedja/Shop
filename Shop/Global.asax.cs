using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Shop
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AreaRegistration.RegisterAllAreas();

            //// I want to REMOVE the ASP.NET ViewEngine...
            ViewEngines.Engines.Clear();
            //// and then add my own :)
            ViewEngines.Engines.Add(new CustomViewLocationRazorViewEngine());
        }

        public class CustomViewLocationRazorViewEngine : RazorViewEngine
        {
            public CustomViewLocationRazorViewEngine()
            {
                ViewLocationFormats = new[]
                {
                    "~/Pages/{1}/{0}.cshtml", "~/Pages/Management/{1}/{0}.cshtml", "~/Shared/Views/{0}.cshtml"
                };
                PartialViewLocationFormats = new[]
                {
                    "~/Pages/{1}/{0}.cshtml",
                    "~/Pages/Management/{1}/{0}.cshtml",
                    "~/Shared/Views/{0}.cshtml"
                };
                MasterLocationFormats = new[]
                {
                    "~/Pages/{1}/{0}.cshtml", "~/Pages/Management/{1}/{0}.cshtml", "~/Shared/Views/{0}.cshtml"
                };
            }
        }
    }
}