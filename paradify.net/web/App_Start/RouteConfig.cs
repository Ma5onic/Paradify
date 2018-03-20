using System.Web.Mvc;
using System.Web.Routing;

namespace web.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: "Search",
              url: "SearchP",
              defaults: new { controller = "SearchP", action = "Index" }

          );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Authorize",
                url: "Authorize/Index",
                defaults: new { controller = "Authorize", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
