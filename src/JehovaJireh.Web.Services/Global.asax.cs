using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.Windsor;
using Castle.Windsor.Installer;
using JehovaJireh.Infrastructure.Installers;
using JehovaJireh.Logging;
using JehovaJireh.Web.Services.Plumbing;
using Mindscape.Raygun4Net;

namespace JehovaJireh.Web.Services
{
	public class WebApiApplication : System.Web.HttpApplication
	{

		private static IWindsorContainer container;

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

			//set the IoC container
			container = BootstrapContainer();
			var log = container.Resolve<ILogger>();
			log.APIApplicationStarting();

			//initialize controller situation
			var controllerFactory = new WindsorControllerFactory(container.Kernel);
			ControllerBuilder.Current.SetControllerFactory(controllerFactory);

			//WebApi
			GlobalConfiguration.Configuration.Services.Replace(
						typeof(IHttpControllerActivator),
						new WindsorCompositionRoot(container));
		}

		public static IWindsorContainer BootstrapContainer()
		{
			if (container != null)
			{
				return container;
			}

			return container = new WindsorContainer()
				.Install(FromAssembly.This(),
				FromAssembly.Containing<RepositoryInstaller>());
		}

        //protected void Application_BeginRequest()
        //{
        //    string[] allowedOrigin = new string[] { "http://localhost:53371", "http://jehovajireh.web.ui" };
        //    var origin = HttpContext.Current.Request.Headers["Origin"];
        //    if (origin != null && allowedOrigin.Contains(origin))
        //    {
        //        HttpContext.Current.Request.Headers.Add("Access-Control-Allow-Origin", "*");
        //        HttpContext.Current.Request.Headers.Add("Access-Control-Allow-Methods", "GET,POST,PUT,DELETE");
        //    }
        //}

        protected void Application_End()
		{
			container.Dispose();
		}

		protected void Application_Error()
		{
			var exception = Server.GetLastError();
			new RaygunClient().Send(exception);
		}
	}
}
