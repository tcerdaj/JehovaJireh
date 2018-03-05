using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JehovaJireh.Web.Services.Controllers
{
    public class ErorsController : Controller
    {
        // GET: Erors
        public ActionResult Index()
        {
            return View();
        }

		public ActionResult Error()
		{
			return View();
		}
	}
}