using System.Web.Mvc;
using System.Web.Routing;

namespace TestMVC.App_Start
{
    public class RouteConfig
    {
        public static void Configure(RouteCollection routes)
        {
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            //routes.MapRoute(
            //    name: "Helloss",
            //    url: "{controller}/{action}/{s}/{i}"
            //    );
        }
    }
}