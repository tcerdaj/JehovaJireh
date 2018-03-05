using System.Web;
using System.Web.Optimization;

namespace JehovaJireh.Web.UI
{
	public class BundleConfig
	{
		// For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery.cookie").Include(
                        "~/Scripts/jquery.cookie.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
						"~/Scripts/jquery.validate*"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
					  "~/Scripts/bootstrap.js",
					  "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/searchable").Include(
                      "~/Scripts/jquery.searchable-1.0.0.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/pagination").Include(
                     "~/Scripts/pagination.js"));


            bundles.Add(new ScriptBundle("~/bundles/common").Include(
					  "~/Scripts/common.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
					  "~/Content/bootstrap.css",
					  "~/Content/site.css",
					   "~/Content/knockout-file-bindings.css"));

            bundles.Add(new StyleBundle("~/Content/flag").Include(
                      "~/Content/flags/flags.css"));

            bundles.Add(new StyleBundle("~/Content/fontawesome").Include(
                      "~/Content/font-awesome.css"
                      ));

            bundles.Add(new StyleBundle("~/SSCT/css").Include(
                    "~/SSCT/css/reset.css",
                    "~/SSCT/css/style.css"
                    ));

            bundles.Add(new ScriptBundle("~/SSCT/main").Include(
                      "~/SSCT/js/main.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
				     "~/Scripts/knockout-{version}.js",
				     "~/Scripts/knockout-{version}.debug.js",
					 "~/Scripts/knockout.validation.debug.js",
					 "~/Scripts/knockout-file-bindings.js",
					 "~/Scripts/knockout.mapping-latest.debug.js",
					 "~/Scripts/knockout.mapping-latest.js"
					 ));

			bundles.Add(new ScriptBundle("~/bundles/maskedinput").Include(
					 "~/Scripts/jquery.maskedinput.min.js"));

			bundles.Add(new ScriptBundle("~/bundles/areYouSure").Include(
			   "~/Scripts/jquery.are-you-sure.js"
			   ));

			bundles.Add(new StyleBundle("~/bundles/kendo-css").Include(
					 "~/styles/kendo.common.min.css",
					 "~/styles/kendo.default.min.css",
					 "~/styles/kendo.mobile.all.min.css"
					 ));

			bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
						"~/js/kendo.all.min.js",
						"~/js/kendo.timezones.min.js",
						"~/js/pako_deflate.min.js"
						));
            bundles.Add(new ScriptBundle("~/bundles/fontawesome").Include(
                        "~/fontawesome/css/font-awesome.min.css"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
					  "~/Scripts/moment.js",
					  "~/Scripts/moment-timezone.min.js"
					  ));

			#region ViewModels
			
			//Account/Register
			bundles.Add(new ScriptBundle("~/bundles/account/registerviewmodel")
			   .Include(
					"~/Scripts/KOViewModel/AccountViewModel/RegisterViewModel.js",
					"~/Scripts/KOViewModel/AccountViewModel/RegisterInit.js"
			   ));
			
			//Donation/MakeDonation
			bundles.Add(new ScriptBundle("~/bundles/donation/donationviewmodel")
			   .Include(
					"~/Scripts/KOViewModel/DonationVIewModel/MakeDonationViewModel.js",
					"~/Scripts/KOViewModel/DonationVIewModel/MakeDonationInit.js"
			   ));
            #endregion

            //#if DEBUG
            //     BundleTable.EnableOptimizations = false;
            //#else
            //     BundleTable.EnableOptimizations = true;
            //#endif
        }
    }
}
