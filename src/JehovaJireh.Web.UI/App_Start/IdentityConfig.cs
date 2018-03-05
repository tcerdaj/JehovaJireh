using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CSharpVitamins;
using JehovaJireh.Core.Entities;
using JehovaJireh.Data.Repositories;
using JehovaJireh.Logging;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using NHibernate;
using NHibernate.Linq;

namespace JehovaJireh.Web.UI
{
	public class EmailService : IIdentityMessageService
	{
		public Task SendAsync(IdentityMessage message)
		{
			// Plug in your email service here to send an email.
			return Task.FromResult(0);
		}
	}

	public class SmsService : IIdentityMessageService
	{
		public Task SendAsync(IdentityMessage message)
		{
			// Plug in your SMS service here to send a text message.
			return Task.FromResult(0);
		}
	}

	// Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
	public class ApplicationUserManager : UserManager<User>
	{
		UserRepository userRepository;
		public ApplicationUserManager(UserRepository store)
			: base(store)
		{
			userRepository = store;
		}


		public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
		{
			var container = MvcApplication.BootstrapContainer();
			var log = container.Resolve<ILogger>();
			var session = container.Resolve<ISession>();
			var errorHandler = container.Resolve<ExceptionManager>();
			var manager = new ApplicationUserManager(new UserRepository(session, errorHandler, log));

			// Configure validation logic for usernames
			manager.UserValidator = new UserValidator<User>(manager)
			{
				AllowOnlyAlphanumericUserNames = false,
				RequireUniqueEmail = true
			};

			// Configure validation logic for passwords
			manager.PasswordValidator = new PasswordValidator
			{
				RequiredLength = 6,
				RequireNonLetterOrDigit = false,
				RequireDigit = false,
				RequireLowercase = true,
				RequireUppercase = true,
			};

			// Configure user lockout defaults
			manager.UserLockoutEnabledByDefault = true;
			manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
			manager.MaxFailedAccessAttemptsBeforeLockout = 5;

			// Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
			// You can write your own provider and plug it in here.
			manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<User>
			{
				MessageFormat = "Your security code is {0}"
			});
			manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<User>
			{
				Subject = "Security Code",
				BodyFormat = "Your security code is {0}"
			});
			manager.EmailService = new EmailService();
			manager.SmsService = new SmsService();
			var dataProtectionProvider = options.DataProtectionProvider;
			if (dataProtectionProvider != null)
			{
				manager.UserTokenProvider =
					new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));
			}
			return manager;
		}

		public Task<User> UpdatetUserSettingsAsync(User user)
		{
			if (user == null)
				throw new ArgumentNullException("user");

			this.Update(user);
			return Task.FromResult(user);
		}

		public Task<User> GetUserById(int id)
		{
			var user = userRepository.GetById(id);
			return Task.FromResult(user);
		}

		public Task<string> CreateConfirmationTokenAsync()
		{
			return Task.FromResult<string>(ShortGuid.NewGuid().ToString());
		}

		public Task<User> GetByConfirmationTokennAsync(string token)
		{
			var us = userRepository.Query();
			var user = (from u in userRepository.Query() where u.ConfirmationToken == token select u).SingleOrDefault();
			return Task.FromResult(user);
		}


		public override Task SendEmailAsync(string userId, string subject, string body)
		{
			EmailService email = new EmailService();
			var destination = this.FindById(userId).Email;
			IdentityMessage message = new IdentityMessage()
			{
				Body = body,
				Subject = subject,
				Destination = destination
			};

			var result = email.SendAsync(message);
			return Task.FromResult(result);
		}
	}

	// Configure the application sign-in manager which is used in this application.
	public class ApplicationSignInManager : SignInManager<User, string>
	{
		public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
			: base(userManager, authenticationManager)
		{

		}

		public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
		{
			return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
		}

		public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
		{
			return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
		}

		public override async Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
		{
			User user = this.UserManager.FindByName(userName);
			user = user== null? this.UserManager.FindByEmail(userName) : user;
			userName = user!=null? user.UserName : userName;

			if (null != user)
			{
				if (false == user.Active)
				{
					return (SignInStatus.Failure);
				}
			}

			var result = await base.PasswordSignInAsync(userName, password, isPersistent, shouldLockout);

			return (result);
		}
	}
}
