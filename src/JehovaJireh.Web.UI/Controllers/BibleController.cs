using JehovaJireh.Web.UI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JehovaJireh.Web.UI.Controllers
{
    public class BibleController : Controller
    {
        // GET: Bible
        public async Task<ActionResult> Index(string id = null)
        {
            ViewBag.Id = id;
            ViewBag.Verses = !string.IsNullOrEmpty(id) ? await GetVerses(id): null;
            return View();
        }

        public ActionResult Seach(string query, string dam_id)
        {
            return View();
        }

        public async Task<List<VerseViewModels>> GetVerses(string id)
        {
            string html = string.Empty;
            var response = new HttpResponseMessage();
            var endPoint = string.Format("{0}bible/verses?id={1}", GetBaseUrl(), id);

            using (var client = new System.Net.Http.HttpClient())
            {
                response = await client.GetAsync(endPoint);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var j = await response.Content.ReadAsStringAsync();
                var o = JObject.Parse(j);
                var jo = o["response"]["verses"];
                var list = JsonConvert.DeserializeObject<List<VerseViewModels>>(jo.ToString());
                return list;
            }
        }
        private string GetBaseUrl()
        {
            var request = HttpContext.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;
            var authority = "jehovajireh.web.service";

            if (appUrl != "/")
                appUrl = "/" + appUrl;

            if (request.Url.Authority.Contains("localhost"))
                authority = "localhost:58095";

            var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, authority, appUrl);

            return baseUrl;
        }
    }
}