using System.Web;
using System.Web.Mvc;
using JehovaJireh.Web.UI.CustomFilters;
using JehovaJireh.Web.UI.Helpers;

namespace JehovaJireh.Web.UI
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
			//filters.Add(new CultureFilter(defaultCulture: CultureHelper.GetDefaultCulture()));
		}
	}
}
