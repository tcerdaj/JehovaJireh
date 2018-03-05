using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using JehovaJireh.Web.UI.App_GlobalResources;

namespace JehovaJireh.Web.UI.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {

		[Display(Name = "Email", ResourceType = typeof(Resources))]
		[EmailAddress]
        public string Email { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "UserNameRequired")]
		[Display(Name = "UserName", ResourceType = typeof(Resources))]
		public string UserName { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "PasswordRequired")]
		[StringLength(100, ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "PasswordLong", MinimumLength = 6)]
		[DataType(DataType.Password, ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "PasswordInvalid")]
		[Display(Name = "PasswordHash", ResourceType = typeof(Resources))]
		public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

	public class RegisterViewModel
	{
		[Required(ErrorMessage =null, ErrorMessageResourceName = "EmailRequired",  ErrorMessageResourceType = typeof(Resources))]
		[Display(Name = "Email", ResourceType = typeof(Resources))]
		[EmailAddress(ErrorMessage = null, ErrorMessageResourceName = "EmailInvalid",ErrorMessageResourceType = typeof(Resources))]
		public string Email { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "UserNameRequired")]
		[Display(Name = "UserName", ResourceType = typeof(Resources))]
		[StringLength(50, ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "UserNameLong")]
		public string UserName { get; set; }
		
		public object ImageUrl { get; set; }

		public HttpPostedFileBase FileData { get; set; }
		public bool FileDataChange { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "FirstNameRequired")]
		[Display(Name = "FirstName", ResourceType = typeof(Resources))]
		[StringLength(50, ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "FirstNameLong")]
		public string FirstName { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "LastNameRequired")]
		[Display(Name = "LastName", ResourceType = typeof(Resources))]
		[StringLength(50, ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "LastNameLong")]
		public string LastName { get; set; }

		[Display(Name = "Gender", ResourceType = typeof(Resources))]
		[StringLength(15, ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "GenderLong")]
		public string Gender { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "PasswordRequired")]
		[StringLength(100, ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "PasswordLong", MinimumLength = 6)]
		[DataType(DataType.Password, ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "PasswordInvalid")]
		[Display(Name = "PasswordHash", ResourceType = typeof(Resources))]
		public string PasswordHash { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "ConfirmPassword", ResourceType = typeof(Resources))]
		[Compare("PasswordHash", ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "PasswordCompare")]
		public string ConfirmPassword { get; set; }

		[Display(Name = "Address", ResourceType = typeof(Resources))]
		[StringLength(80, ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "AddressLong")]
		public string Address { get; set; }

		[Display(Name = "City", ResourceType = typeof(Resources))]
		[StringLength(50, ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "CityLong")]
		public string City { get; set; }

		[Display(Name = "State", ResourceType = typeof(Resources))]
		[StringLength(50, ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "StateLong")]
		public string State { get; set; }

		[Display(Name = "Zip", ResourceType = typeof(Resources))]
		[StringLength(15, ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "ZipLong")]
		public string Zip { get; set; }

		[Display(Name = "Phone", ResourceType = typeof(Resources))]
		[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", 
			ErrorMessageResourceType = typeof(Resources),
			ErrorMessageResourceName = "PhoneInvalid")]
		[StringLength(15, ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "PhoneLong")]
		public string PhoneNumber { get; set; }

		[Display(Name = "IsChurchMember", ResourceType = typeof(Resources))]
		public bool IsChurchMember { get; set; }

		[Display(Name = "ChurchName", ResourceType = typeof(Resources))]
		[StringLength(50, ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "ChurchNameLong")]
		public string ChurchName { get; set; }

		[Display(Name = "ChurchAddress", ResourceType = typeof(Resources))]
		[StringLength(80, ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "ChurchAddressLong")]
		public string ChurchAddress { get; set; }

		[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
			ErrorMessageResourceType = typeof(Resources),
			ErrorMessageResourceName = "PhoneInvalid")]
		[Display(Name = "ChurchPhone", ResourceType = typeof(Resources))]
		[StringLength(15, ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "PhoneLong")]
		public string ChurchPhone { get; set; }

		[Display(Name = "ChurchPastor", ResourceType = typeof(Resources))]
		[StringLength(20, ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "ChurchPastorLong")]
		public string ChurchPastor { get; set; }

		[Display(Name = "NeedToBeVisited", ResourceType = typeof(Resources))]
		public bool NeedToBeVisited { get; set; }

		[Display(Name = "Comments", ResourceType = typeof(Resources))]
		public string Comments { get; set; }

		
	}

	public class UpdateAccountViewModel
	{
		public int Id { get; set; }
		[Display(Name = "Email", ResourceType = typeof(Resources))]
		public string Email { get; set; }

		
		[Display(Name = "UserName", ResourceType = typeof(Resources))]
		public string UserName { get; set; }

		public string ImageUrl { get; set; }

		public HttpPostedFileBase FileData { get; set; }
		public bool FileDataChange { get; set; }

		[Display(Name = "FirstName", ResourceType = typeof(Resources))]
		public string FirstName { get; set; }

		[Display(Name = "LastName", ResourceType = typeof(Resources))]
		public string LastName { get; set; }

		[Display(Name = "Gender", ResourceType = typeof(Resources))]
		[StringLength(15, ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "GenderLong")]
		public string Gender { get; set; }

        public bool ShareMyAddress { get; set; }

		[Display(Name = "Address", ResourceType = typeof(Resources))]
		[StringLength(80, ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "AddressLong")]
		public string Address { get; set; }

		[Display(Name = "City", ResourceType = typeof(Resources))]
		[StringLength(50, ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "CityLong")]
		public string City { get; set; }

		[Display(Name = "State", ResourceType = typeof(Resources))]
		[StringLength(50, ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "StateLong")]
		public string State { get; set; }

		[Display(Name = "Zip", ResourceType = typeof(Resources))]
		[StringLength(15, ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "ZipLong")]
		public string Zip { get; set; }

		[Display(Name = "Phone", ResourceType = typeof(Resources))]
		[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
			ErrorMessageResourceType = typeof(Resources),
			ErrorMessageResourceName = "PhoneInvalid")]
		[StringLength(15, ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "PhoneLong")]
		public string PhoneNumber { get; set; }

		[Display(Name = "IsChurchMember", ResourceType = typeof(Resources))]
		public bool IsChurchMember { get; set; }

		[Display(Name = "ChurchName", ResourceType = typeof(Resources))]
		[StringLength(50, ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "ChurchNameLong")]
		public string ChurchName { get; set; }

		[Display(Name = "ChurchAddress", ResourceType = typeof(Resources))]
		[StringLength(80, ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "ChurchAddressLong")]
		public string ChurchAddress { get; set; }

		[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
			ErrorMessageResourceType = typeof(Resources),
			ErrorMessageResourceName = "PhoneInvalid")]
		[Display(Name = "ChurchPhone", ResourceType = typeof(Resources))]
		[StringLength(15, ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "PhoneLong")]
		public string ChurchPhone { get; set; }

		[Display(Name = "ChurchPastor", ResourceType = typeof(Resources))]
		[StringLength(20, ErrorMessageResourceType = typeof(Resources),
			  ErrorMessageResourceName = "ChurchPastorLong")]
		public string ChurchPastor { get; set; }

		[Display(Name = "NeedToBeVisited", ResourceType = typeof(Resources))]
		public bool NeedToBeVisited { get; set; }

		[Display(Name = "Comments", ResourceType = typeof(Resources))]
		public string Comments { get; set; }


	}

	public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
