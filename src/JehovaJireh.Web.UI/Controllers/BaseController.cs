using System;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using JehovaJireh.Web.UI.Helpers;
using System.Linq;

namespace JehovaJireh.Web.UI.Controllers
{
	public class BaseController : Controller
    {
		//protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
		//{
		//	string cultureName = RouteData.Values["culture"] as string;

		//	// Attempt to read the culture cookie from Request
		//	if (cultureName == null)
		//		cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ? Request.UserLanguages[0] : null; // obtain it from HTTP header AcceptLanguages

		//	// Validate culture name
		//	cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe


		//	if (RouteData.Values["culture"] as string != cultureName)
		//	{

		//		// Force a valid culture in the URL
		//		RouteData.Values["culture"] = cultureName.ToLowerInvariant(); // lower case too

		//		// Redirect user
		//		Response.RedirectToRoute(RouteData.Values);
		//	}

            //test
		//	// Modify current thread's cultures            
		//	Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
		//	Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;


		//	return base.BeginExecuteCore(callback, state);
		//}

		protected override void Initialize(RequestContext requestContext)
		{
			string culture = null;
			var request = requestContext.HttpContext.Request;
			string cultureName = null;

			// Attempt to read the culture cookie from Request
			HttpCookie cultureCookie = request.Cookies["culture"];
			if (cultureCookie != null)
				cultureName = cultureCookie.Value;
			else
				cultureName = request.UserLanguages[0]; // obtain it from HTTP header AcceptLanguages

			// Validate culture name
			cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

			if (request.QueryString.AllKeys.Contains("culture"))
			{
				culture = request.QueryString["culture"];
			}
			else
			{
				culture = cultureName;
			}

			
			Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
			Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

			base.Initialize(requestContext);
		}

		public void SetCulture(string culture)
		{
			culture = CultureHelper.GetImplementedCulture(culture);

			if (!string.IsNullOrEmpty(culture))
				RouteData.Values["culture"] = culture;  // set culture
			else
				RouteData.Values["culture"] = CultureHelper.GetDefaultCulture();  // set culture
		}
	}
}