using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using DataAccess.Entities;
using System.Text;

namespace UI.Controllers
{
    [RoutePrefix("contact")]
    public class ContactController : Controller
    {
        // GET: Contact
        [Route("home")]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [Route("predictive/first")]
        [HttpPost]
        public ActionResult PredictiveFirst(string first)
      {
            List<User> users = new List<User>();
            if(!string.IsNullOrWhiteSpace(first))
            {
                using (var api = new HttpClient())
                {
                    api.BaseAddress = new Uri("https://localhost:44343/api/user/");
                    api.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var get_user = api.PostAsJsonAsync<string>("search/name", first);
                    get_user.Wait();
                    var result = get_user.Result;
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        var s = result.Content.ReadAsAsync<List<User>>();
                        s.Wait();
                        users = s.Result;
                    }
                    else if (result.StatusCode == HttpStatusCode.NotFound)
                    {
                        var s = result.Content.ReadAsAsync<List<User>>();
                        s.Wait();
                        users = s.Result;
                    }
                }
            }

            return Json(users, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
        }

        [Route("get/user")]
        [HttpPost]
        public ActionResult getUser(string UserId)
        {
            User user = new User();
            if (!string.IsNullOrWhiteSpace(UserId))
            {
                var id = Guid.Parse(UserId);
                using (var api = new HttpClient())
                {
                    api.BaseAddress = new Uri("https://localhost:44343/api/user/");
                    api.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var get_user = api.PostAsJsonAsync<Guid>("show/id", id);
                    get_user.Wait();
                    var result = get_user.Result;
                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        var s = result.Content.ReadAsAsync<User>();
                        s.Wait();
                        user = s.Result;
                    }
                    else if (result.StatusCode == HttpStatusCode.NotFound)
                    {
                        var s = result.Content.ReadAsAsync<User>();
                        s.Wait();
                        user = s.Result;
                    }
                }
            }
            return Json(user, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
        }


    }
}