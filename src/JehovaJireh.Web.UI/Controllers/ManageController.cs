﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using JehovaJireh.Web.UI.Models;
using System.Diagnostics;
using JehovaJireh.Data.Repositories;
using JehovaJireh.Core.Entities;
using JehovaJireh.Data.Mappings;
using JehovaJireh.Logging;
using Omu.ValueInjecter;
using JehovaJireh.Web.UI.App_GlobalResources;
using System.Collections.Generic;
using Newtonsoft.Json;
using JehovaJireh.Web.UI.CustomAttributes;

namespace JehovaJireh.Web.UI.Controllers
{
	[Authorize]
	public class ManageController : BaseController
	{
		private ApplicationSignInManager _signInManager;
		private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private ILogger log;
        private const string USERSETTINGS = "UserSettings";
        private const string OPERATION_STATUS = "operationStatus";

        public ManageController()
		{
			var container = MvcApplication.BootstrapContainer();
			log = container.Resolve<ILogger>();
		}

		public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
		{
			UserManager = userManager;
			SignInManager = signInManager;
			var container = MvcApplication.BootstrapContainer();
			log = container.Resolve<ILogger>();
		}

		public ApplicationSignInManager SignInManager
		{
			get
			{
				return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
			}
			private set
			{
				_signInManager = value;
			}
		}

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public ApplicationUserManager UserManager
		{
			get
			{
				return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			private set
			{
				_userManager = value;
			}
		}

        public ActionResult Roles(AddNewRoleViewModel model, ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? Resources.YourPasswordHaschanged
                : message == ManageMessageId.SetPasswordSuccess ? Resources.YourPasswordHasSet
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? Resources.Error
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : message == ManageMessageId.UpdateAccountSuccess ? Resources.YourAccoutHasUpdated
                : message == ManageMessageId.OperationSuccess ? "Operation has been completed Successfully!!!"
                : "";
            //TODO:retrieve all roles here...
            if (message == ManageMessageId.OperationSuccess)
                ViewBag.Success = true;

            if (TempData[OPERATION_STATUS] != null)
                ViewBag.StatusMessage = TempData[OPERATION_STATUS];

            var roles = RoleManager.Roles;
            if (model.Roles == null)
                model.Roles = new List<RoleViewModel>();

            foreach (var r in roles)
            {
                var role = new RoleViewModel()
                {
                    Id = r.Id,
                    Name = r.Name
                };
                if (r.Users != null)
                {
                    foreach (var u in r.Users)
                    {
                        if (role.Users == null)
                            role.Users = new List<UserViewModel>();
                        role.Users.Add(new UserViewModel
                        {
                            Id = u.Id,
                            UserName = u.UserName,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            Email = u.Email
                        });
                    }
                }
            
                model.Roles.Add(role);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Roles(AddNewRoleViewModel model)
        {
            ViewBag.StatusMessage = "An error ocurred adding Role";
            IEnumerable<string> errors = new List<string>();
            
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await RoleManager.CreateAsync(new Role() { Id = model.Id, Name = model.Name });
                    if (result.Succeeded)
                    {
                        ViewBag.Success = true;
                        return RedirectToAction("Roles", new { message = ManageMessageId.OperationSuccess });
                    }
                    else
                    {
                        ViewBag.Success = false;
                        errors = result.Errors;
                    }
                }
                catch (System.Exception ex)
                {
                    ViewBag.Success = false;
                    errors = new List<string>(new string[] { string.Format("{0} Please check your entry and try again.", ex.Message) });
                }

                ViewBag.StatusMessage = string.Join(",", errors);
            }

            return View(model);
        }

        public ActionResult Users(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                 message == ManageMessageId.ChangePasswordSuccess ? Resources.YourPasswordHaschanged
                 : message == ManageMessageId.SetPasswordSuccess ? Resources.YourPasswordHasSet
                 : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                 : message == ManageMessageId.Error ? Resources.Error
                 : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                 : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                 : message == ManageMessageId.UpdateAccountSuccess ? Resources.YourAccoutHasUpdated
                 : message == ManageMessageId.OperationSuccess ? "Operation has been completed Successfully!!!"
                 : "";

            var users = UserManager.Users.ToList();
            var model = new List<UpdateAccountViewModel>();

            foreach (var user in users)
            {
                var _user = (UpdateAccountViewModel)new UpdateAccountViewModel().InjectFrom<DeepCloneInjection>(user);
                model.Add(_user);
            }
           
            return View(model);
        }

        public async Task<ActionResult> DeleteRole(string roleId)
        {
            ViewBag.StatusMessage = "An error ocurred deleting Role";
            IEnumerable<string> errors = new List<string>();
            try
            {
                var role = await RoleManager.FindByIdAsync(roleId);
                var result = await RoleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    ViewBag.Success = true;
                    return RedirectToAction("Roles", new { message = ManageMessageId.OperationSuccess });
                }
                else
                {
                    ViewBag.Success = false;
                    errors = result.Errors;
                }
            }
            catch (System.Exception ex)
            {
                ViewBag.Success = false;
                errors = new List<string>(new string[] { string.Format("{0} Please check your entry and try again.", ex.Message) });
            }

            TempData[OPERATION_STATUS]  = string.Join(",", errors);
            return RedirectToAction("Roles", new { message = ManageMessageId.Error });
        }

        public async Task<ActionResult> GoToRole(string roleId, ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? Resources.YourPasswordHaschanged
                : message == ManageMessageId.SetPasswordSuccess ? Resources.YourPasswordHasSet
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? Resources.Error
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : message == ManageMessageId.UpdateAccountSuccess ? Resources.YourAccoutHasUpdated
                : message == ManageMessageId.OperationSuccess ? "Operation has been completed Successfully!!!"
                : "";

            if (string.IsNullOrEmpty(roleId))
                return RedirectToAction("Roles", new { message = ManageMessageId.Error });
            //TODO:retrieve all roles here...
            if (message == ManageMessageId.OperationSuccess)
                ViewBag.Success = true;

            if (TempData[OPERATION_STATUS] != null)
                ViewBag.StatusMessage = TempData[OPERATION_STATUS];

            var role = await RoleManager.FindByIdAsync(roleId);

            if (string.IsNullOrEmpty(roleId) || role == null)
                RedirectToAction("Roles", new { message = ManageMessageId.Error });


            var model = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name,
                Users = role.Users != null ? role.Users.Select(u => new UserViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email
                }).ToList<UserViewModel>() : null
            };

            var users = UserManager.Users.ToList();

            var allUsers = users.Select(u=> new UserViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email
            }).ToList<UserViewModel>();

            model.AllUsers = allUsers.Where(p => !role.Users.Any(p2 => p2.Id == p.Id)).ToList();

            return View(model);
        }

        public async Task<ActionResult> EditRole(string roleId, ManageMessageId? message)
        {
            ViewBag.StatusMessage =
               message == ManageMessageId.ChangePasswordSuccess ? Resources.YourPasswordHaschanged
               : message == ManageMessageId.SetPasswordSuccess ? Resources.YourPasswordHasSet
               : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
               : message == ManageMessageId.Error ? Resources.Error
               : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
               : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
               : message == ManageMessageId.UpdateAccountSuccess ? Resources.YourAccoutHasUpdated
               : message == ManageMessageId.OperationSuccess ? "Operation has been completed Successfully!!!"
               : "";

            //TODO:retrieve all roles here...
            if (message == ManageMessageId.OperationSuccess)
                ViewBag.Success = true;

            if (TempData[OPERATION_STATUS] != null)
                ViewBag.StatusMessage = TempData[OPERATION_STATUS];

            var role = await RoleManager.FindByIdAsync(roleId);

            if (string.IsNullOrEmpty(roleId) || role == null)
                RedirectToAction("Roles", new { message = ManageMessageId.Error });

            var model = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            };

            if (model.Users == null)
                model.Users = new List<UserViewModel>();

            foreach (var u in role.Users)
            {
                model.Users.Add(new UserViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email
                });
            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> EditRole(RoleViewModel model)
        {
            ViewBag.StatusMessage = "An error ocurred adding Role";
            IEnumerable<string> errors = new List<string>();

            if (ModelState.IsValid)
            {
                try
                {
                    var role = await RoleManager.FindByIdAsync(model.Id);
                    role.Name = model.Name;
                    var result = await RoleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        ViewBag.Success = true;
                        return RedirectToAction("Roles", new { message = ManageMessageId.OperationSuccess });
                    }
                    else
                    {
                        errors = result.Errors;
                    }
                }
                catch (System.Exception ex)
                {
                    ViewBag.Success = false;
                    errors = new List<string>(new string[] { string.Format("{0} Please check your entry and try again.", ex.Message) });
                }

                ViewBag.StatusMessage = string.Join(",", errors);
            }

            return View(model);
        }

        public async Task<ActionResult> AddUsersToRole(string roleId,string users)
        {
            IDictionary<string, string[]> errors = 
                new Dictionary<string, string[]>();

            var userArray = users.Split(',');

            try
            {
                if (!string.IsNullOrEmpty(roleId))
                {
                    foreach (var userId in userArray)
                    {
                        var result = await UserManager.AddToRoleAsync(userId, roleId);
                        if (!result.Succeeded)
                        {
                            errors.Add("UserId_" + userId, result.Errors.ToArray());
                        }
                    }

                    if (errors.Count() == 0)
                    {
                        return RedirectToAction("GoToRole", new { roleId = roleId, message = ManageMessageId.OperationSuccess });
                    }
                }
            }
            catch (System.Exception ex)
            {
                ViewBag.Success = false;
                errors.Add("GeneralError", new string[] { ex.Message });
            }

            TempData[OPERATION_STATUS] = JsonConvert.SerializeObject(errors);
            return RedirectToAction("GoToRole", new { roleId = roleId, message = ManageMessageId.Error });
        }

        public async Task<ActionResult> RemoveUsersFromRole(string roleId, string users)
        {
            IDictionary<string, string[]> errors =
                new Dictionary<string, string[]>();
            var userArray = users.Split(',');

            try
            {
                foreach (var userId in userArray)
                {
                    var result = await UserManager.RemoveFromRoleAsync(userId, roleId);
                    if (!result.Succeeded)
                    {
                        errors.Add("userId_"+userId, result.Errors.ToArray<string>());
                    }
                }

                if (errors.Count() == 0)
                {
                    return RedirectToAction("GoToRole", new { roleId = roleId, message = ManageMessageId.OperationSuccess });
                }
            }
            catch (System.Exception ex)
            {
                ViewBag.Success = false;
                errors.Add("GeneralError", new string[] { ex.Message });
            }

            TempData[OPERATION_STATUS] = JsonConvert.SerializeObject(errors);
            return RedirectToAction("GoToRole", new {roleId = roleId, message = ManageMessageId.Error });
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
		{
			//if (!string.IsNullOrEmpty(culture))
				//SetCulture(culture);

			ViewBag.StatusMessage =
				message == ManageMessageId.ChangePasswordSuccess ? Resources.YourPasswordHaschanged
				: message == ManageMessageId.SetPasswordSuccess ? Resources.YourPasswordHasSet
				: message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
				: message == ManageMessageId.Error ? Resources.Error
				: message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
				: message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
				: message == ManageMessageId.UpdateAccountSuccess? Resources.YourAccoutHasUpdated
				: "";

			var userId = User.Identity.GetUserId();
			var model = new IndexViewModel
			{
				HasPassword = HasPassword(),
				PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
				TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
				Logins = await UserManager.GetLoginsAsync(userId),
				BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
			};
			return View(model);
		}

		//
		// POST: /Manage/RemoveLogin
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
		{
			ManageMessageId? message;
			var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
			if (result.Succeeded)
			{
				var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
				if (user != null)
				{
					await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
				}
				message = ManageMessageId.RemoveLoginSuccess;
			}
			else
			{
				message = ManageMessageId.Error;
			}
			return RedirectToAction("ManageLogins", new { Message = message });
		}

        // POST: /Account/LogOff
        public ActionResult LogOff()
        {
            log.LogOff(User.Identity.Name);
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session[USERSETTINGS] = string.Empty;
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
		{
			return View();
		}

		//
		// POST: /Manage/AddPhoneNumber
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			// Generate the token and send it
			var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
			if (UserManager.SmsService != null)
			{
				var message = new IdentityMessage
				{
					Destination = model.Number,
					Body = "Your security code is: " + code
				};
				await UserManager.SmsService.SendAsync(message);
			}
			return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
		}

		//
		// POST: /Manage/EnableTwoFactorAuthentication
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> EnableTwoFactorAuthentication()
		{
			await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
			var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
			if (user != null)
			{
				await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
			}
			return RedirectToAction("Index", "Manage");
		}

		//
		// POST: /Manage/DisableTwoFactorAuthentication
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> DisableTwoFactorAuthentication()
		{
			await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
			var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
			if (user != null)
			{
				await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
			}
			return RedirectToAction("Index", "Manage");
		}

		//
		// GET: /Manage/VerifyPhoneNumber
		public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
		{
			var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
			// Send an SMS through the SMS provider to verify the phone number
			return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
		}

		//
		// POST: /Manage/VerifyPhoneNumber
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
			if (result.Succeeded)
			{
				var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
				if (user != null)
				{
					await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
				}
				return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
			}
			// If we got this far, something failed, redisplay form
			ModelState.AddModelError("", "Failed to verify phone");
			return View(model);
		}

		//
		// POST: /Manage/RemovePhoneNumber
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> RemovePhoneNumber()
		{
			var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
			if (!result.Succeeded)
			{
				return RedirectToAction("Index", new { Message = ManageMessageId.Error });
			}
			var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
			if (user != null)
			{
				await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
			}
			return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
		}
		
		// GET: /Manage/UpdateAccount
		public ActionResult UpdateAccount(string userId = null, string returnUrl = null)
		{
            var user = UserManager.FindById(User.Identity.GetUserId());
            ViewBag.returnUrl = returnUrl;
            if (!string.IsNullOrEmpty(userId))
            {
                if (!(user.Roles != null && user.Roles.Any(x => x.Name.ToLower() == "administrators")))
                {
                    return RedirectToAction("E401", "Errors");
                }

                user = UserManager.FindById(userId);
            }

            var model = (UpdateAccountViewModel)new UpdateAccountViewModel().InjectFrom<DeepCloneInjection>(user);
            model.AllRoles = RoleManager.Roles.ToList<Role>().Select(x => new SelectListItem { Text = x.Name, Value = x.Id, Selected = model.Roles.Any(r => r.Id == x.Id) }).ToList<SelectListItem>();
			return View(model);
		}

        //
        // POST: /Manage/UpdateAccount
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> UpdateAccount(UpdateAccountViewModel model, string returnUrl=null)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			Stopwatch timespan = Stopwatch.StartNew();
			var user = await  UserManager.GetUserById(model.Id);
			user.Gender = model.Gender;
			user.PhoneNumber = model.PhoneNumber;
			user.Address = model.Address;
			user.City = model.City;
			user.State = model.State;
			user.Zip = model.Zip;
			user.IsChurchMember = model.IsChurchMember;
			user.ChurchName = model.ChurchName;
			user.ChurchAddress = model.ChurchAddress;
			user.ChurchPhone = model.ChurchPhone;
			user.Comments = model.Comments;
			user.ModifiedOn = DateTime.UtcNow;
            user.ModifiedBy = new Core.Entities.User { Id = model.Id };
            

            if (UserManager.IsInRole(user.Id.ToString(), "administrators"))
            {
                user.Roles = model.AllRoles.Where(x => x.Selected == true).Select(r => new Role { Id = r.Value, Name = r.Text }).ToList<Role>();
            }

			if (model.FileData != null)
			{
				ImageService imageService = new ImageService(log);
				var fileName = model.FileData.FileName;
				user.ImageUrl = await imageService.CreateUploadedImageAsync(model.FileData, fileName, true, 100, 100);
			}
			
			var result = await UserManager.UpdateAsync(user);
			timespan.Stop();

			if (result.Succeeded)
			{
				Session["UserSettings"] = user.ToJson();
                if (string.IsNullOrEmpty(returnUrl))
                    return RedirectToAction("Index", new { Message = ManageMessageId.UpdateAccountSuccess });
                else
                    return Redirect(returnUrl);
            }

            AddErrors(result);
			return View(model);
		}

		//
		// GET: /Manage/ChangePassword
		public ActionResult ChangePassword()
		{
			return View();
		}

		//
		// POST: /Manage/ChangePassword
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
			if (result.Succeeded)
			{
				var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
				if (user != null)
				{
					await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
				}
				return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
			}
			AddErrors(result);
			return View(model);
		}

		//
		// GET: /Manage/SetPassword
		public ActionResult SetPassword()
		{
			return View();
		}

		//
		// POST: /Manage/SetPassword
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
				if (result.Succeeded)
				{
					var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
					if (user != null)
					{
						await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
					}
					return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
				}
				AddErrors(result);
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		//
		// GET: /Manage/ManageLogins
		public async Task<ActionResult> ManageLogins(ManageMessageId? message)
		{
			ViewBag.StatusMessage =
				message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
				: message == ManageMessageId.Error ? "An error has occurred."
				: "";
			var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
			if (user == null)
			{
				return View("Error");
			}
			var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
			var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
			ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
			return View(new ManageLoginsViewModel
			{
				CurrentLogins = userLogins,
				OtherLogins = otherLogins
			});
		}

		//
		// POST: /Manage/LinkLogin
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LinkLogin(string provider)
		{
			// Request a redirect to the external login provider to link a login for the current user
			return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
		}

		//
		// GET: /Manage/LinkLoginCallback
		public async Task<ActionResult> LinkLoginCallback()
		{
			var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
			if (loginInfo == null)
			{
				return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
			}
			var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
			return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && _userManager != null)
			{
				_userManager.Dispose();
				_userManager = null;
			}

			base.Dispose(disposing);
		}

		#region Helpers
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

		private bool HasPassword()
		{
			var user = UserManager.FindById(User.Identity.GetUserId());
			if (user != null)
			{
				return user.PasswordHash != null;
			}
			return false;
		}

		private bool HasPhoneNumber()
		{
			var user = UserManager.FindById(User.Identity.GetUserId());
			if (user != null)
			{
				return user.PhoneNumber != null;
			}
			return false;
		}

		public enum ManageMessageId
		{
			AddPhoneSuccess,
			ChangePasswordSuccess,
			SetTwoFactorSuccess,
			SetPasswordSuccess,
			RemoveLoginSuccess,
			RemovePhoneSuccess,
			Error,
			UpdateAccountSuccess,
            OperationSuccess
		}

		#endregion
	}
}