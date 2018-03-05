using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JehovaJireh.Web.UI.App_GlobalResources;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Web.Mvc.Filters;
using JehovaJireh.Core.IRepositories;
using JehovaJireh.Web.UI.Models;

namespace JehovaJireh.Web.UI.Controllers
{

	public class HomeController : BaseController
	{
        public ActionResult Index()
		{
            IndexViewModel m = new IndexViewModel();
            return View(m);
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
        [Authorize]
        public ActionResult Chat()
        {
            var container = MvcApplication.BootstrapContainer();
            IUserRepository userRepository = container.Resolve<IUserRepository>();
            var user = userRepository.GetByUserName(User.Identity.Name);
            dynamic me = new System.Dynamic.ExpandoObject();
            me.UserName = user.UserName;
            me.Name = user.FirstName;
            me.LastName = user.LastName;
            me.Avatar = user.ImageUrl;
            me.ConnectedDateTime = DateTime.Now;
            ViewBag.Me = me;
            return View();
        }
    }
}