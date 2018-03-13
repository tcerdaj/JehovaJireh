﻿using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using JehovaJireh.Web.UI.Models;
using System.Web;
using JehovaJireh.Core.Entities;
using System.Configuration;
using JehovaJireh.Data.Repositories;
using NHibernate;
using JehovaJireh.Logging;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Owin.Security.OAuth;
using JehovaJireh.Web.UI.Providers;

namespace JehovaJireh.Web.UI
{
    public partial class Startup
    {
        public Startup()
        {
            PublicClientId = "self";
            var container = MvcApplication.BootstrapContainer();
            var session = container.Resolve<ISession>();
            var exManager = container.Resolve<ExceptionManager>();
            var log = container.Resolve<ILogger>();

            UserManagerFactory = new UserManager<User>(new UserRepository(session, exManager, log));
            RoleManagerFactory = new RoleManager<Role>(new RoleRepository(session, exManager, log));

            CookieOptions = new CookieAuthenticationOptions();

            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId, ()=> UserManagerFactory),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = true
            };
        }
		// For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
		public void ConfigureAuth(IAppBuilder app)
		{
            // Configure the db context, user manager and signin manager to use a single instance per request
            //app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
			app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
            app.MapSignalR();

            // Check to see if we are running local and if we are set the cookie domain to nothing so authentication works correctly.
            string cookieDomain = ".jehovajireh.com";
			bool isLocal = HttpContext.Current.Request.IsLocal;
			if (isLocal)
			{
				cookieDomain = "";
			}

			// Enable the application to use a cookie to store information for the signed in user
			// and to use a cookie to temporarily store information about a user logging in with a third party login provider
			// Configure the sign in cookie
			app.UseCookieAuthentication(new CookieAuthenticationOptions
			{
				AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
				LoginPath = new PathString("/Account/Login"),
				CookieDomain = cookieDomain,
				Provider = new CookieAuthenticationProvider
				{
					// Enables the application to validate the security stamp when the user logs in.
					// This is a security feature which is used when you change a password or add an external login to your account.  
					OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, User>(
						validateInterval: TimeSpan.FromMinutes(30),
						regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
				}
			});
			app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

			// Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
			app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

			// Enables the application to remember the second login verification factor such as phone or email.
			// Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
			// This is similar to the RememberMe option when you log in.
			app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
            app.UseOAuthBearerTokens(OAuthOptions);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");
            #region Google
            var googleAuthenticationOptions = new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = ConfigurationManager.AppSettings["GglI"],
                ClientSecret = ConfigurationManager.AppSettings["GglS"],
                Provider = new GoogleOAuth2AuthenticationProvider()
                {
                    OnAuthenticated = async context =>
                    {
                        context.Identity.AddClaim(new System.Security.Claims.Claim("GoogleAccessToken",
                            context.AccessToken));
                        foreach (var claim in context.User)
                        {
                            var claimType = string.Format("urn:google:{0}", claim.Key);
                            string claimValue = claim.Value.ToString();
                            if (!context.Identity.HasClaim(claimType, claimValue))
                                context.Identity.AddClaim(new System.Security.Claims.Claim(claimType,
                                    claimValue, "XmlSchemaString", "Google"));
                        }
                    }
                }
            };
            app.UseGoogleAuthentication(googleAuthenticationOptions);
            #endregion
            //Default Values
            try
            {
                createDefaultRolesandUsers();
            }
            catch(System.Exception ex) 
            {
                Trace.WriteLine(ex);
            }
        }

        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static CookieAuthenticationOptions CookieOptions { get; private set; }

        public static UserManager<User> UserManagerFactory { get; set; }
        public static RoleManager<Role> RoleManagerFactory { get; set; }

        public static string PublicClientId { get; private set; }
        private void createDefaultRolesandUsers()
        {
            var userManager = UserManagerFactory;
            var roleManager = RoleManagerFactory;
            string defaultRole = "administrators";

            //Create default user
            var user = userManager.FindByName(ConfigurationManager.AppSettings["adminUserName"]);
            if (user == null)
            {
                user = new User()
                {
                    UserName = ConfigurationManager.AppSettings["adminUserName"],
                    FirstName = ConfigurationManager.AppSettings["adminFirstName"],
                    LastName = ConfigurationManager.AppSettings["adminLastName"],
                    Email = ConfigurationManager.AppSettings["adminEmail"],
                    PasswordHash = ConfigurationManager.AppSettings["adminPassword"],
                    CreatedOn = DateTime.Now,
                    ModifiedOn = null,
                    LastLogin = null
                };

               var result = userManager.CreateAsync(user, user.PasswordHash);
            }

            //Create admin role
            Role role = null;
            if (!roleManager.RoleExists(defaultRole))
            {
                role = new Role()
                {
                    Id = ConfigurationManager.AppSettings["adminUserName"],
                    Name = defaultRole
                };
                roleManager.CreateAsync(role);
            }
            else
            {
                role = roleManager.FindByName(defaultRole);
            }

            //Create user role
            if (!roleManager.RoleExists("users"))
            {
                var userRole = new Role()
                {
                    Id = "users",
                    Name = "users"
                };
                roleManager.CreateAsync(userRole);
            }

            //Add user to role
            if (user != null && !user.Roles.Any(x => x.Name == defaultRole))
            {
                if (user.Roles == null)
                {
                   user.Roles = new List<Role>();
                }
                user.Roles.Add(role);
                userManager.UpdateAsync(user);
            }
        }
    }
}