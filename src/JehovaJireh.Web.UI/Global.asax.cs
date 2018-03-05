using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.Windsor;
using Castle.Windsor.Installer;
using JehovaJireh.Logging;
using JehovaJireh.Web.UI.CustomAttributes;
using JehovaJireh.Web.UI.Helpers;
using JehovaJireh.Web.UI.Plumbing;
using Microsoft.AspNet.SignalR;

namespace JehovaJireh.Web.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static IWindsorContainer container;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(6000);
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(6000);
            GlobalHost.Configuration.KeepAlive = TimeSpan.FromSeconds(100);

            container = BootstrapContainer();
            var log = container.Resolve<ILogger>();
            log.WebApplicationStarting();

            //initialize controller situation
            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

            //ControllerBuilder.Current.SetControllerFactory(new DefaultControllerFactory(new CultureAwareControllerActivator()));
        }

        public static IWindsorContainer BootstrapContainer()
        {
            if (container != null)
            {
                return container;
            }

            return container = new WindsorContainer()
                .Install(FromAssembly.This(), FromAssembly.Containing<Infrastructure.Installers.RepositoryInstaller>());
        }

        protected void Application_End()
        {
            container.Dispose();
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            if (Request.Cookies["Culture"] != null && !string.IsNullOrEmpty(Request.Cookies["Culture"].Value))
            {
                string culture = Request.Cookies["culture"].Value;
                culture = CultureHelper.GetImplementedCulture(culture); // This is safe
                CultureInfo ci = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = ci;
            }
        }
    }
}
