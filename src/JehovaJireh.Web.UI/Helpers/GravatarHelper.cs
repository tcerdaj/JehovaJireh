using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
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
    public enum Position
      {
          Horizontal,
          Vertical
      }
public static class CheckBoxListExtensions
    {
        #region -- CheckBoxList (Horizontal) --
        /// <summary>
        /// CheckBoxList.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="listInfo">SelectListItem.</param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper,
            string name,
            IEnumerable<SelectListItem> listInfo)
        {
            return htmlHelper.CheckBoxList(name, listInfo, (IDictionary<string, object>)null, 0);
        }
 
        /// <summary>
        /// CheckBoxList.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="listInfo">SelectListItem.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper,
            string name,
            IEnumerable<SelectListItem> listInfo,
            object htmlAttributes)
        {
            return htmlHelper.CheckBoxList(name, listInfo, (IDictionary<string, object>)new RouteValueDictionary(htmlAttributes), 0);
        }
 
        /// <summary>
        /// CheckBoxList.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="listInfo">The list info.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="number">每個Row的顯示個數.</param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper,
            string name,
            IEnumerable<SelectListItem> listInfo,
            IDictionary<string, object> htmlAttributes,
            int number)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentException("You musth give this CheckBoxList Tag Name", "name");
            }
            if (listInfo == null)
            {
                throw new ArgumentNullException("listInfo", "Must give List<SelectListItem> listInfo");
            }
            if (listInfo.Count() < 1)
            {
                throw new ArgumentException("List<SelectListItem> listInfo 至少要有一組資料", "listInfo");
            }
 
            StringBuilder sb = new StringBuilder();
            int lineNumber = 0;
 
            foreach (SelectListItem info in listInfo)
            {
                lineNumber++;
 
                TagBuilder builder = new TagBuilder("input");
                if (info.Selected)
                {
                    builder.MergeAttribute("checked", "checked");
                }
                builder.MergeAttributes<string, object>(htmlAttributes);
                builder.MergeAttribute("type", "checkbox");
                builder.MergeAttribute("value", info.Value);
                builder.MergeAttribute("name", name);
                sb.Append(builder.ToString(TagRenderMode.Normal));
 
                TagBuilder labelBuilder = new TagBuilder("label");
                labelBuilder.MergeAttribute("for", name);
                labelBuilder.InnerHtml = info.Text;
                sb.Append(labelBuilder.ToString(TagRenderMode.Normal));
 
                if (number == 0 || (lineNumber % number == 0))
                {
                    sb.Append("<br />");
                }
            }
            return MvcHtmlString.Create(sb.ToString());
        }
        #endregion
 
        #region -- CheckBoxListVertical --
        /// <summary>
        /// Checks the box list vertical.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="listInfo">The list info.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="columnNumber">The column number.</param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxListVertical(this HtmlHelper htmlHelper,
            string name,
            IEnumerable<SelectListItem> listInfo,
            IDictionary<string, object> htmlAttributes,
            int columnNumber = 1)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentException("必須給這些CheckBoxList一個Tag Name", "name");
            }
            if (listInfo == null)
            {
                throw new ArgumentNullException("listInfo", "必須要給List<CheckBoxListInfo> listInfo");
            }
            if (listInfo.Count() < 1)
            {
                throw new ArgumentException("List<CheckBoxListInfo> listInfo 至少要有一組資料", "listInfo");
            }
 
            int dataCount = listInfo.Count();
 
            // calculate number of rows
            int rows = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(dataCount) / Convert.ToDecimal(columnNumber)));
            if (dataCount <= columnNumber || dataCount - columnNumber == 1)
            {
                rows = dataCount;
            }
 
            TagBuilder wrapBuilder = new TagBuilder("div");
            wrapBuilder.MergeAttribute("style", "float: left; light-height: 25px; padding-right: 5px;");
 
            string wrapStart = wrapBuilder.ToString(TagRenderMode.StartTag);
            string wrapClose = string.Concat(wrapBuilder.ToString(TagRenderMode.EndTag), " <div style=\"clear:both;\"></div>");
            string wrapBreak = string.Concat("</div>", wrapBuilder.ToString(TagRenderMode.StartTag));
 
            StringBuilder sb = new StringBuilder();
            sb.Append(wrapStart);
 
            int lineNumber = 0;
 
            foreach (var info in listInfo)
            {
                TagBuilder builder = new TagBuilder("input");
                if (info.Selected)
                {
                    builder.MergeAttribute("checked", "checked");
                }
                builder.MergeAttributes<string, object>(htmlAttributes);
                builder.MergeAttribute("type", "checkbox");
                builder.MergeAttribute("value", info.Value);
                builder.MergeAttribute("name", name);
                sb.Append(builder.ToString(TagRenderMode.Normal));
 
                TagBuilder labelBuilder = new TagBuilder("label");
                labelBuilder.MergeAttribute("for", name);
                labelBuilder.InnerHtml = info.Text;
                sb.Append(labelBuilder.ToString(TagRenderMode.Normal));
 
                lineNumber++;
 
                if (lineNumber.Equals(rows))
                {
                    sb.Append(wrapBreak);
                    lineNumber = 0;
                }
                else
                {
                    sb.Append("<br/>");
                }
            }
            sb.Append(wrapClose);
            return MvcHtmlString.Create(sb.ToString());
        }
        #endregion
 
        #region -- CheckBoxList (Horizonal, Vertical) --
        /// <summary>
        /// Checks the box list.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="listInfo">The list info.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="position">The position.</param>
        /// <param name="number">Position.Horizontal則表示每個Row的顯示個數, Position.Vertical則表示要顯示幾個Column</param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper,
            string name,
            IEnumerable<SelectListItem> listInfo,
            IDictionary<string, object> htmlAttributes,
            Position position = Position.Horizontal,
            int number = 0)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentException("必須給這些CheckBoxList一個Tag Name", "name");
            }
            if (listInfo == null)
            {
                throw new ArgumentNullException("listInfo", "必須要給List<SelectListItem> listInfo");
            }
            if (listInfo.Count() < 1)
            {
                throw new ArgumentException("List<SelectListItem> listInfo 至少要有一組資料", "listInfo");
            }
 
            StringBuilder sb = new StringBuilder();
            int lineNumber = 0;
 
            switch (position)
            {
                case Position.Horizontal:
 
                    foreach (SelectListItem info in listInfo)
                    {
                        lineNumber++;
                        sb.Append(CreateCheckBoxItem(info, name, htmlAttributes));
 
                        if (number == 0 || (lineNumber % number == 0))
                        {
                            sb.Append("<br />");
                        }
                    }
                    break;
 
                case Position.Vertical:
 
                    int dataCount = listInfo.Count();
 
                    // 計算最大顯示的列數(rows)
                    int rows = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(dataCount) / Convert.ToDecimal(number)));
                    if (dataCount <= number || dataCount - number == 1)
                    {
                        rows = dataCount;
                    }
 
                    TagBuilder wrapBuilder = new TagBuilder("div");
                    wrapBuilder.MergeAttribute("style", "float: left; light-height: 25px; padding-right: 5px;");
 
                    string wrapStart = wrapBuilder.ToString(TagRenderMode.StartTag);
                    string wrapClose = string.Concat(wrapBuilder.ToString(TagRenderMode.EndTag), " <div style=\"clear:both;\"></div>");
                    string wrapBreak = string.Concat("</div>", wrapBuilder.ToString(TagRenderMode.StartTag));
 
                    sb.Append(wrapStart);
 
                    foreach (SelectListItem info in listInfo)
                    {
                        lineNumber++;
                        sb.Append(CreateCheckBoxItem(info, name, htmlAttributes));
 
                        if (lineNumber.Equals(rows))
                        {
                            sb.Append(wrapBreak);
                            lineNumber = 0;
                        }
                        else
                        {
                            sb.Append("<br/>");
                        }
                    }
                    sb.Append(wrapClose);
                    break;
            }
 
            return MvcHtmlString.Create(sb.ToString());
        }
 
        internal static string CreateCheckBoxItem(SelectListItem info, string name, IDictionary<string, object> htmlAttributes)
        {
            StringBuilder sb = new StringBuilder();
 
            TagBuilder builder = new TagBuilder("input");
            if (info.Selected)
            {
                builder.MergeAttribute("checked", "checked");
            }
            builder.MergeAttributes<string, object>(htmlAttributes);
            builder.MergeAttribute("type", "checkbox");
            builder.MergeAttribute("value", info.Value);
            builder.MergeAttribute("name", name);
            sb.Append(builder.ToString(TagRenderMode.Normal));
 
            TagBuilder labelBuilder = new TagBuilder("label");
            labelBuilder.MergeAttribute("for", name);
            labelBuilder.InnerHtml = info.Text;
            sb.Append(labelBuilder.ToString(TagRenderMode.Normal));
 
            return sb.ToString();
        }
        #endregion
    }
}