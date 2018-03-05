using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JehovaJireh.Web.UI.Models;
using JehovaJireh.Web.UI.Extentions;

namespace JehovaJireh.Web.UI.Controllers
{
    public class ServicesController : Controller
    {
        // GET: Services
        public JsonResult DonationStatus()
        {
			var results =  Enum.GetValues(typeof(DonationStatus))
					.Cast<DonationStatus>()
					.Select(
						 enu => new SelectListItem() {
							 Text = enu.GetDescription(),
							 Value = ((int)enu).ToString() }
					 )
					.ToList();

			return Json(results, JsonRequestBehavior.AllowGet);
        }

		public JsonResult DonationTypes()
		{
			var results = Enum.GetValues(typeof(DonationType))
					.Cast<DonationType>()
					.Select(
						 enu => new SelectListItem() {
							 Text = enu.GetDescription().ToString(),
							 Value = ((int)enu).ToString() }
					 )
					.ToList();

			return Json(results, JsonRequestBehavior.AllowGet);
		}
	}
}