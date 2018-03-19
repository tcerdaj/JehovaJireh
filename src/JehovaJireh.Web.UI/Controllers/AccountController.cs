using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using JehovaJireh.Web.UI.Models;
using Microsoft.AspNet.Identity.Owin;
using JehovaJireh.Core.Entities;
using Omu.ValueInjecter;
using JehovaJireh.Data.Mappings;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using JehovaJireh.Data.Repositories;
using JehovaJireh.Logging;
using Castle.Windsor;
using System.Diagnostics;
using Facebook;
using System.Security.Claims;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace JehovaJireh.Web.UI.Controllers
{
	[Authorize]
	public class AccountController : BaseController
	{

        #region Ctr
        public AccountController()
        {
            container = MvcApplication.BootstrapContainer();
            log = container.Resolve<ILogger>();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ILogger log)
        {
           
            _UserManager = userManager;
            _SignInManager = signInManager;
            container = MvcApplication.BootstrapContainer();
            log = container.Resolve<ILogger>();
        }
        #endregion
        #region Authorize
        [HttpGet]
        public ActionResult Authorize()
        {
            var claims = new ClaimsPrincipal(User).Claims.ToArray();
            var identity = new ClaimsIdentity(claims, "Bearer");
            AuthenticationManager.SignIn(identity);
            return new EmptyResult();
        }
        #endregion

        #region Variables
        private ApplicationSignInManager _signInManager;
        private static ApplicationUserManager _userManager;
        private static ApplicationRoleManager _roleManager;
        private static IWindsorContainer container;
        private ILogger log;
        private const string USERSETTINGS = "UserSettings";
        public static string OEmail { get; set; }
        public static string OBirthday { get; set; }
        public static string OFname { get; set; }
        public static string OLname { get; set; }
        public static string OProfilePhoto { get; set; }
        public static string OGender { get; set; }
        public ApplicationSignInManager _SignInManager
        {
            get
            {
                return _signInManager?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); 
            }
            private set
            {
                _signInManager = value;
            }
        }
        public static ApplicationUserManager _UserManager
        {
            get
            {
                return _userManager ?? System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>(); 
            }
            private set
            {
                _userManager = value;
            }
        }
        public static ApplicationRoleManager _RoleManager
        {
            get
            {
                return _roleManager ?? System.Web.HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>(); 
            }
            private set
            {
                _roleManager = value;
            }
        }
        #endregion

        #region Login
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Stopwatch timespan = Stopwatch.StartNew();
            log.LoginStarted(model.UserName);
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await _SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            timespan.Stop();
            log.LoginFinished(model.UserName, result.ToString(), timespan.Elapsed);
            switch (result)
            {
                case SignInStatus.Success:
                    var user = _UserManager.FindByName(model.UserName);
                    Session[USERSETTINGS] = user.ToJson();
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }
        #endregion

        #region VerifyCode
        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await _SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await _SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }
        #endregion

        #region Register
        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register(string culture = null)
        {
            var culture1 = Request.Cookies["culture"];
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Stopwatch timespan = Stopwatch.StartNew();
                    var confirmationToken = await _UserManager.CreateConfirmationTokenAsync();
                    ImageService imageService = new ImageService(log);

                    var user = (User)new User().InjectFrom<DeepCloneInjection>(model);
                    log.RegisterStarted<User>(user);

                    user.ImageUrl = await imageService.CreateUploadedImageAsync(model.FileData, Guid.NewGuid().ToString(), true, 100, 100);
                    user.CreatedOn = DateTime.Now;
                    user.Active = true;
                    user.ModifiedOn = null;
                    user.LastLogin = DateTime.Now;

                    var result = await _UserManager.CreateAsync(user, model.PasswordHash);
                    timespan.Stop();
                    log.SaveFinished(user, timespan.Elapsed);

                    if (result.Succeeded)
                    {
                        await _SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                        //Send email to user to complete the proccess.
                        //var callbackUrl = Url.Action("RegisterConfirmationCallBack", "Account", new { token = confirmationToken }, protocol: Request.Url.Scheme);
                        //var emailResult = UserManager.SendEmailAsync(user.Id.ToString(), "Register Confirmation", "Please complete your registration by clicking <a href=\"" + callbackUrl + "\">here</a>");
                        Session[USERSETTINGS] = user.ToJson();
                        return RedirectToAction("Index", "Home");
                    }

                    AddErrors(result);


                }
                catch (System.Exception ex)
                {
                    string[] errors = new string[] { ex.Message };
                    AddErrors(new IdentityResult(errors));
                }

            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }
        #endregion

        #region ConfirmEmail
        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await _UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }
        #endregion

        #region ForgotPassword
        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await _UserManager.IsEmailConfirmedAsync(user.Id.ToString())))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        #endregion

        #region ResetPassword
        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await _UserManager.ResetPasswordAsync(user.Id.ToString(), model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        #endregion

        #region ExternalLogin

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            string userId = null; //get userid;
            //string confirmationUserName;
            //bool custEmailConf = false; //to check if email is confirmed;
            string custUserName = null; //to check if username exist in the database
            var info = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction("Index", "Home");
            }
            string userprokey = info.Login.ProviderKey;
            User user = null;

            try
            {
                if (_userManager == null)
                    _userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                user = await _UserManager.FindAsync(info.Login);
                userId = user != null ? user.Id.ToString() : userId; //get userId
            }
            catch (System.Exception ex)
            {
                throw;
            }
          

            //if (userId != null)
            //{
            //    //check if email is confirmed
            //    custEmailConf = true;
            //    //get username
            //    custUserName = user.UserName;
            //}

            //Email confirmation handling
            //if (custEmailConf == false && custUserName != null)
            //{
            //    confirmationUserName = custUserName;
            //    return RedirectToAction("EmailConfirmationFailed", "Account");
            //}

            // Sign in the user with this external login provider if the user already has a login
            var result = await _SignInManager.ExternalSignInAsync(info, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    if (user != null)
                        UpdateUserAuditValues(user);
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = info.Login.LoginProvider;

                    switch (info.Login.LoginProvider)
                    {
                        case "Facebook":
                            var identity = AuthenticationManager.GetExternalIdentity
                           (DefaultAuthenticationTypes.ExternalCookie);
                            var access_token = identity.FindFirstValue("FacebookAccessToken");
                            var fb = new FacebookClient(access_token);
                            dynamic uEmail = fb.Get("/me?fields=email");
                            dynamic uBirtDate = fb.Get("/me?fields=birthday");
                            dynamic uFname = fb.Get("/me?fields=first_name");
                            dynamic uLname = fb.Get("/me?fields=last_name");
                            dynamic uLimage = fb.Get("/me?fields=image");
                            dynamic uGender = fb.Get("/me?fields=gender");
                            OEmail = uEmail.email;
                            OBirthday = uBirtDate.birthday;
                            OProfilePhoto = uLimage.image;
                            OFname = uFname.first_name;
                            OLname = uLname.last_name;
                            OGender = uGender;
                            break;
                        case "Google":
                            OEmail = info.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
                            //OBirthday = info.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dateofbirth").Value;
                            dynamic image =  JsonConvert.DeserializeObject(info.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == "urn:google:image").Value);
                            OProfilePhoto = image.url;
                            OFname = info.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname").Value;
                            OLname = info.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname").Value;
                            OGender = info.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == "urn:google:gender").Value;
                            break;
                        case "Microsoft":
                            OEmail = info.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;

                            string bday = info.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == "urn:microsoft:birth_day").Value,
                                   bmonth = info.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == "urn:microsoft:birth_month").Value, 
                                   byear = info.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == "urn:microsoft:birth_year").Value;

                            OBirthday = string.Format("{0}/{1}/{2}", bday, bmonth, byear);
                            OFname = info.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == "urn:microsoft:first_name").Value;
                            OLname = info.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == "urn:microsoft:last_name").Value;
                            OProfilePhoto = info.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == "urn:microsoft:image").Value;
                            OGender = info.ExternalIdentity.Claims.FirstOrDefault(c => c.Type == "urn:google:gender").Value;
                            break;
                        default:
                            OEmail = null;
                            OBirthday = null;
                            OFname = null;
                            OLname = null;
                            OProfilePhoto = null;
                            OGender = null;
                            break;
                    }
                  

                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = OEmail, ExtFirstName = OFname, ExtLastName = OLname, ExtBirtDate = OBirthday, ExtProfilePhoto = OProfilePhoto, ExtGender = OGender});
            }
        }

        private async void UpdateUserAuditValues(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            user.LastLogin = DateTime.Now;
            await _UserManager.UpdateAsync(user);
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var userByEmail = await _UserManager.FindByEmailAsync(model.Email);
                var userByUsername = await _UserManager.FindByNameAsync(model.Email);
                DateTime.TryParse(model.ExtBirtDate, out var birthDate);
                var identity = (ClaimsIdentity)User.Identity;
                if (model.ExtUsername != null)
                    identity.AddClaim(new System.Security.Claims.Claim(nameof(model.ExtUsername), model.ExtUsername));
                if (model.Email != null)
                    identity.AddClaim(new System.Security.Claims.Claim(nameof(model.Email), model.Email));
                if (model.ExtFirstName != null)
                    identity.AddClaim(new System.Security.Claims.Claim(nameof(model.ExtFirstName), model.ExtFirstName));
                if (model.ExtLastName != null)
                    identity.AddClaim(new System.Security.Claims.Claim(nameof(model.ExtLastName), model.ExtLastName));
                if (model.ExtCountry != null)
                    identity.AddClaim(new System.Security.Claims.Claim(nameof(model.ExtCountry), model.ExtCountry));
                if (model.ExtBirtDate != null)
                    identity.AddClaim(new System.Security.Claims.Claim(nameof(model.ExtBirtDate), birthDate.ToString()));
                if (model.ExtProfilePhoto != null)
                    identity.AddClaim(new System.Security.Claims.Claim(nameof(model.ExtProfilePhoto), model.ExtProfilePhoto));
                if (model.ExtGender != null)
                    identity.AddClaim(new System.Security.Claims.Claim(nameof(model.ExtGender), model.ExtGender));

                IEnumerable<System.Security.Claims.Claim> claims = identity.Claims;
                //var defaulRole = await RoleManager.FindByNameAsync("externallogin");

                var user = new User
                {
                    UserName = model.ExtUsername,
                    Email = model.Email,
                    FirstName = model.ExtFirstName,
                    LastName = model.ExtLastName,
                    Country = model.ExtCountry,
                    BirthDate = birthDate,
                    ImageUrl = model.ExtProfilePhoto,
                    Gender = model.ExtGender,
                    LastLogin = DateTime.Now,
                    CreatedOn = DateTime.Now
                };

                user.AddRole(new Role { Id = "externallogin", Name = "ExternalLogin" });
                user.AddLogin(new Login { ProviderKey = info.Login.ProviderKey, LoginProvider = info.Login.LoginProvider });
                user.AddClaim(claims.Select(x => new Core.Entities.Claim { ClaimType = x.Type, ClaimValue = x.Value }).ToList<JehovaJireh.Core.Entities.Claim>());

                if (userByEmail == null && userByUsername == null)
                {
                    var result = await _UserManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        await _SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        Session[USERSETTINGS] = user.ToJson();
                        return RedirectToAction("Index", "Home");
                    }
                    AddErrors(result);
                }
                else
                {
                    if (userByEmail != null)
                    {
                        ModelState.AddModelError("", "Email is already registered.");
                    }
                    if(userByUsername != null)
                    {
                        ModelState.AddModelError("", string.Format("Username {0} is already taken.", model.ExtUsername));
                    }
                }
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        #endregion

        #region Send Code/Email
        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await _SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await _UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await _SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }
        #endregion

        #region LogOff
        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            log.LogOff(User.Identity.Name);
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session[USERSETTINGS] = string.Empty;
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Helpers
        public static Task<List<SelectListItem>> GetContries()
        {
            System.Globalization.CultureInfo[] cinfo = System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.SpecificCultures & ~System.Globalization.CultureTypes.NeutralCultures);

           List<SelectListItem> response = new List<SelectListItem>();

            foreach (System.Globalization.CultureInfo cul in cinfo)
            {
                var region = new System.Globalization.RegionInfo(cul.Name);
                if (!cul.IsNeutralCulture && !response.Any(x => x.Text == region.DisplayName))
                {
                    response.Add(new SelectListItem { Text = region.DisplayName, Value = region.DisplayName });
                }
            }

            response = new List<SelectListItem>(response.OrderBy(x=>x.Text));
            return Task<List<SelectListItem>>.FromResult(response);

        }
        public static string FindEmail(string oEmail)
        {
            var user = _UserManager.FindByEmail(oEmail);
            return user?.Email;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

		private IAuthenticationManager AuthenticationManager
		{
			get
			{
				return HttpContext.GetOwinContext().Authentication;
			}
		}

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error);
			}
		}

		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			return RedirectToAction("Index", "Home");
		}

		public class ChallengeResult : HttpUnauthorizedResult
		{
			public ChallengeResult(string provider, string redirectUri)
				: this(provider, redirectUri, null)
			{
			}

			public ChallengeResult(string provider, string redirectUri, string userId)
			{
				LoginProvider = provider;
				RedirectUri = redirectUri;
				UserId = userId;
			}

			public string LoginProvider { get; set; }
			public string RedirectUri { get; set; }
			public string UserId { get; set; }

			public override void ExecuteResult(ControllerContext context)
			{
				var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
				if (UserId != null)
				{
					properties.Dictionary[XsrfKey] = UserId;
				}
				context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
			}
		}
		#endregion
	}
}