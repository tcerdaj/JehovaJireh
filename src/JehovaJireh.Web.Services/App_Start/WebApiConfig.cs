using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using JehovaJireh.Web.Services.Filters;
using Microsoft.Owin.Security.OAuth;
using WebApiContrib.Logging.Raygun;
using JehovaJireh.Web.Services.ErrorHandler;
using System.Web.Http.Cors;

namespace JehovaJireh.Web.Services
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services
			
			// Configure Web API to use only bearer token authentication.
			config.SuppressDefaultHostAuthentication();
			config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

			//Configure Raygun for WebApi
			var raygunSettings = (Mindscape.Raygun4Net.RaygunSettings)ConfigurationManager.GetSection("RaygunSettings");
			config.Services.Add(typeof(IExceptionLogger), new RaygunExceptionLogger(raygunSettings.ApiKey)); //Add raygun

			//Unhandled exceptions per controller
			config.Filters.Add(new ValidationExceptionFilterAttribute());

			//config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
			config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = 
				Newtonsoft.Json.ReferenceLoopHandling.Serialize;
			config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = 
				Newtonsoft.Json.PreserveReferencesHandling.Objects;

            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

            //Remove Existing Handler and add custom
            config.Services.Replace(typeof(IExceptionHandler), new OopsExceptionHandler());

            // Setting Cors
            var cors = new EnableCorsAttribute(origins: "http://localhost:53371, http://jehovajireh.web.ui", headers: "accept,accesstoken,authorization,cache-control,pragma,content-type,origin", methods: "GET,PUT,POST,DELETE,TRACE,HEAD,OPTIONS");
            //var cors = new EnableCorsAttribute("*","*","*");
            cors.SupportsCredentials = true;
            config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "IdActionApi",
                routeTemplate: "{controller}/{id}/{action}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "ActionIdApi",
                routeTemplate: "{controller}/{action}",
                defaults: new {action= "Get" }
            );
        }
	}
}
