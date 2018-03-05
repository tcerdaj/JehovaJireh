using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using JehovaJireh.Core.Entities;
using JehovaJireh.Core.IRepositories;
using JehovaJireh.Data.Repositories;
using JehovaJireh.Logging;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using NHibernate;

namespace JehovaJireh.Web.UI.Helpers
{
	public static class GravatarHelper
	{
		public static HtmlString GravatarImage(this HtmlHelper htmlHelper, string userName, string action = null, string controllerName = null, object routeValues = null, GravatarOptions options = null)
		{
			if (options == null)
				options = GravatarOptions.GetDefaults();

			var imgTag = new TagBuilder("img");
			User user = null;

			if (HttpContext.Current.Session["UserSettings"] != null)
			{
				user = (new User()).ToObject(HttpContext.Current.Session["UserSettings"].ToString());
			}

			if (user == null && HttpContext.Current.User.Identity.IsAuthenticated)
			{
				var container = MvcApplication.BootstrapContainer();
				var userRepository = container.Resolve<IUserRepository>();
				user = userRepository.GetByUserName(HttpContext.Current.User.Identity.Name);
				HttpContext.Current.Session["UserSettings"] = user.ToJson();
			}

			if (!string.IsNullOrEmpty(options.CssClass))
			{
				imgTag.AddCssClass(options.CssClass);
			}

			var imageUrl = user != null && !string.IsNullOrEmpty(user.ImageUrl) ? user.ImageUrl : "/img/no-photo.png";
			imgTag.Attributes.Add("src", string.Format("{0}?width={1}&height={2}",
				imageUrl,
				50,
				50
				)
			);

			imgTag.MergeAttribute("width", "50px");
			imgTag.MergeAttribute("height", "50px");
			imgTag.MergeAttribute("alt", "Manage");
			// build the <a> tag
			if (!string.IsNullOrEmpty(action) && !string.IsNullOrEmpty(controllerName))
			{
				var url = new UrlHelper(htmlHelper.ViewContext.RequestContext);
				string imgHtml = imgTag.ToString(TagRenderMode.SelfClosing);
				var anchorBuilder = new TagBuilder("a");
				anchorBuilder.MergeAttribute("href", action.Contains("#") && controllerName.Contains("#")? "#" : url.Action(action,controllerName, routeValues));
				anchorBuilder.MergeAttribute("class", "avatar-image-container");
				anchorBuilder.MergeAttribute("title", "Click to Manage");
				anchorBuilder.InnerHtml = imgHtml; // include the <img> tag inside
				string anchorHtml = anchorBuilder.ToString(TagRenderMode.Normal);
				return new HtmlString(anchorHtml);
			}
			return new HtmlString(imgTag.ToString(TagRenderMode.SelfClosing));
		}

		// Source: http://msdn.microsoft.com/en-us/library/system.security.cryptography.md5.aspx
		private static string GetMd5Hash(string input)
		{
			byte[] data = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input));
			var sBuilder = new StringBuilder();
			for (int i = 0; i < data.Length; i++)
			{
				sBuilder.Append(data[i].ToString("x2"));
			}
			return sBuilder.ToString();
		}
	}
}