using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JehovaJireh.Web.UI.Controllers
{
    public class ErrorsController : BaseController
    {
		// GET: Errors
		public ActionResult E500()
		{
			Response.StatusCode = 500;
			return View();
		}

		public ActionResult Error(System.Exception ex)
		{
			ModelState.AddModelError("", ex);
			return View();
		}

		public ActionResult E401()
		{
			Response.StatusCode = 401;
			return View();
		}

		public ActionResult E403()
		{
			Response.StatusCode = 403;
			return View();
		}

		public ActionResult E404()
		{
			Response.StatusCode = 404;
			Response.TrySkipIisCustomErrors = true; // 
			return View("E404");
		}
	}
}