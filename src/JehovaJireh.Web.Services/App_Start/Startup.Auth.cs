using System;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using JehovaJireh.Web.Services.Providers;
using JehovaJireh.Data.Repositories;
using JehovaJireh.Core.Entities;
using Castle.Windsor;
using NHibernate;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using JehovaJireh.Logging;
using System.Web;
using System.Web.Http;
using Microsoft.Owin.Cors;
using System.Web.Cors;
using System.Linq;
using System.Threading.Tasks;
//using Microsoft.Owin.Cors;

namespace JehovaJireh.Web.Services
{
	public partial class Startup
	{
        
        static string[] allowedMethods = new string[] { "GET", "PUT", "POST", "DELETE" };

        static Startup()
		{
			container = WebApiApplication.BootstrapContainer();

			PublicClientId = "self";

			UserManagerFactory = () => new UserManager<User>(new UserRepository(container.Resolve<ISession>(), container.Resolve<ExceptionManager>(), container.Resolve<ILogger>()));

			OAuthOptions = new OAuthAuthorizationServerOptions
			{
				TokenEndpointPath = new PathString("/oauth/access_token"),
				Provider = new ApplicationOAuthProvider(PublicClientId, UserManagerFactory),
				AuthorizeEndpointPath = new PathString("/oauth/account/enternal_login"),
				AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
				AllowInsecureHttp = true
			};
		}

		public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

		public static Func<UserManager<User>> UserManagerFactory { get; set; }

		public static string PublicClientId { get; private set; }

		private static IWindsorContainer container;

		// For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
		public void ConfigureAuth(IAppBuilder app)
		{
            //Check to see if we are running local and if we are set the cookie domain to nothing so authentication works correctly.
            //app.UseCors(CorsOptions.AllowAll);

            string cookieDomain = ".jehovajireh.com";
			bool isLocal = HttpContext.Current.Request.IsLocal;
			if (isLocal)
			{
				cookieDomain = "";
			}


			// Enable the application to use a cookie to store information for the signed in user
			// and to use a cookie to temporarily store information about a user logging in with a third party login provider
			app.UseCookieAuthentication(new CookieAuthenticationOptions
			{
				CookieDomain = cookieDomain
			});

			app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enable the application to use bearer tokens to authenticate users
            //app.UseOAuthAuthorizationServer(OAuthOptions);
            app.UseOAuthAuthorizationServer(OAuthOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            //app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            var config = new HttpConfiguration();
            //config.MapHttpAttributeRoutes();
            //app.UseWebApi(config);
            app.UseCors(CorsOptions.AllowAll);
            
            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            //app.UseGoogleAuthentication();
        }
    }
}