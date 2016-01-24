using System.Web.Mvc;
using System.Web.Routing;

namespace MockApi.Web
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Settings", "settings/{action}/{id}", new {controller = "Settings", action = "Index", id = UrlParameter.Optional});

            routes.MapRoute("CatchAll", "{*url}", new { controller = "WebApp", action = "Index" });
        }
    }
}
