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
            ViewBag.Chapters = !string.IsNullOrEmpty(id) ? await GetChapters(id) : null;
            ViewBag.Books = !string.IsNullOrEmpty(id) ? await GetBooks(id) : null;
            return View();
        }

        public ActionResult Seach(string query, string dam_id)
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetParams(string id)
        {
            var verses = !string.IsNullOrEmpty(id) ? await GetVerses(id) : null;
            var chapters = !string.IsNullOrEmpty(id) ? await GetChapters(id) : null;
            var books = !string.IsNullOrEmpty(id) ? await GetBooks(id) : null;
            var data = new Dictionary<string, object>();
            var dic = new Dictionary<string, object>();
            dic.Add("verses", verses);
            dic.Add("chapters", chapters);
            dic.Add("books", books);
            data.Add("data", dic);
            return Json(data, JsonRequestBehavior.AllowGet);
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

        public async Task<List<ChapterViewModels>> GetChapters(string id)
        {
            string html = string.Empty;
            var response = new HttpResponseMessage();
            var endPoint = string.Format("{0}bible/chapters?id={1}", GetBaseUrl(), id);

            using (var client = new System.Net.Http.HttpClient())
            {
                response = await client.GetAsync(endPoint);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var j = await response.Content.ReadAsStringAsync();
                var o = JObject.Parse(j);
                var jo = o["response"]["chapters"];
                var list = JsonConvert.DeserializeObject<List<ChapterViewModels>>(jo.ToString());
                return list;
            }
        }

        public async Task<List<BookViewModels>> GetBooks(string version)
        {
            version = version.Substring(0, version.IndexOf(":"));
            string html = string.Empty;
            var response = new HttpResponseMessage();
            var endPoint = string.Format("{0}bible/book?version={1}", GetBaseUrl(), version);

            using (var client = new System.Net.Http.HttpClient())
            {
                response = await client.GetAsync(endPoint);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var j = await response.Content.ReadAsStringAsync();
                var o = JObject.Parse(j);
                var jo = o["response"]["books"];
                try
                {
                    var list = JsonConvert.DeserializeObject<List<BookViewModels>>(jo.ToString());
                    return list;
                }
                catch (System.Exception ex)
                {

                    throw;
                }
               
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