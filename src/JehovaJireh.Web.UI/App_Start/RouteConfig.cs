using System.Web.Mvc;
using System.Web.Routing;
using JehovaJireh.Web.UI.Helpers;
namespace JehovaJireh.Web.UI
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
				name: "DefaultCulture",
				url: "{culture}/{controller}/{action}/{id}",
				constraints: new { culture = new CultureConstraint(defaultCulture: "nl", pattern: "[a-z]{2}") },
				defaults: new { culture = CultureHelper.GetDefaultCulture(), controller = "Home", action = "Index", id = UrlParameter.Optional }
			);

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { culture = CultureHelper.GetDefaultCulture(),controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
        }
	}
}
