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
using UI.Models;

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
            UserCities userCities = new UserCities();
            using (var api = new HttpClient())
            {
                api.BaseAddress = new Uri("https://localhost:44343/api/user/");
                api.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var get_user = api.GetAsync("contact/page");
                get_user.Wait();
                var result = get_user.Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var s = result.Content.ReadAsAsync<UserCities>();
                    s.Wait();
                    userCities = s.Result;
                }
                else if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    var s = result.Content.ReadAsAsync<UserCities>();
                    s.Wait();
                    userCities = s.Result;
                }
            }
            return View(userCities);
        }


        [Route("profile/ajax")]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult contact_profile(string contactId)
        {
            FullUserInformation fullUserInformation = new FullUserInformation();
            var userID = Guid.Parse(contactId);
            using (var api = new HttpClient())
            {
                api.BaseAddress = new Uri("https://localhost:44343/api/user/");
                api.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var get_user = api.PostAsJsonAsync<Guid>("user/information", userID);
                get_user.Wait();
                var result = get_user.Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var s = result.Content.ReadAsAsync<FullUserInformation>();
                    s.Wait();
                    fullUserInformation = s.Result;
                }
                else if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    var s = result.Content.ReadAsAsync<FullUserInformation>();
                    s.Wait();
                    fullUserInformation = s.Result;
                }
            }
            return View(fullUserInformation);
        }

        [Route("profile/normal")]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult contact_profile_normal(Guid contactId)
        {
            FullUserInformation fullUserInformation = new FullUserInformation();
            
            using (var api = new HttpClient())
            {
                api.BaseAddress = new Uri("https://localhost:44343/api/user/");
                api.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var get_user = api.PostAsJsonAsync<Guid>("user/information", contactId);
                get_user.Wait();
                var result = get_user.Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var s = result.Content.ReadAsAsync<FullUserInformation>();
                    s.Wait();
                    fullUserInformation = s.Result;
                }
                else if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    var s = result.Content.ReadAsAsync<FullUserInformation>();
                    s.Wait();
                    fullUserInformation = s.Result;
                }
            }
            return View("contact_profile", fullUserInformation);
        }



        [Route("predictive/first")]
        [HttpPost]
        public ActionResult PredictiveFirst(ContactFilter filt)
        {
            List<User> users = new List<User>();
            if (!string.IsNullOrWhiteSpace(filt.First))
            {
                PredictiveFilter filter = new PredictiveFilter
                {
                    First = filt.First
                };
                if (filt.CountryId != null)
                {
                    var couId = Guid.Parse(filt.CountryId);
                    filter.CountryId = couId;
                }
                if (filt.ProvinceId != null)
                {
                    var prId = Guid.Parse(filt.ProvinceId);
                    filter.ProvinceId = prId;
                }
                if (filt.CityId != null)
                {
                    var ciId = Guid.Parse(filt.CityId);
                    filter.CityId = ciId;
                }


                using (var api = new HttpClient())
                {
                    api.BaseAddress = new Uri("https://localhost:44343/api/user/");
                    api.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var get_user = api.PostAsJsonAsync<PredictiveFilter>("search/name", filter);
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

        [Route("filter/provinces")]
        [HttpPost]
        public ActionResult get_provinces(string CountryId)
        {
            List<Province> provinces = new List<Province>();
            var id = Guid.Parse(CountryId);
            if (CountryId == null)
            {
                return Json(provinces, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }

            using (var api = new HttpClient())
            {
                api.BaseAddress = new Uri("https://localhost:44343/api/province/");
                api.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var get_provinces = api.PostAsJsonAsync<Guid>("search/country", id);
                get_provinces.Wait();
                var result = get_provinces.Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var s = result.Content.ReadAsAsync<List<Province>>();
                    s.Wait();
                    provinces = s.Result;
                }
                else if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    var s = result.Content.ReadAsAsync<List<Province>>();
                    s.Wait();
                    provinces = s.Result;
                }
            }
            return Json(provinces, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.AllowGet);
        }


        [Route("filter/cities")]
        [HttpPost]
        public ActionResult get_cities(string ProvinceId)
        {
            List<City> cities = new List<City>();
            var id = Guid.Parse(ProvinceId);
            if (ProvinceId == null)
            {
                return Json(cities, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.AllowGet);
            }

            using (var api = new HttpClient())
            {
                api.BaseAddress = new Uri("https://localhost:44343/api/city/");
                api.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var get_cities = api.PostAsJsonAsync<Guid>("seach/province", id);
                get_cities.Wait();
                var result = get_cities.Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var s = result.Content.ReadAsAsync<List<City>>();
                    s.Wait();
                    cities = s.Result;
                }
                else if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    var s = result.Content.ReadAsAsync<List<City>>();
                    s.Wait();
                    cities = s.Result;
                }
            }
            return Json(cities, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.AllowGet);
        }

        [Route("information/user")]
        [HttpPost]
        public ActionResult get_full_user(ContactFilterFound filterFound)
        {
            User user = new User();
            if (!string.IsNullOrWhiteSpace(filterFound.contactUserHiddenSearch))
            {
                var id = Guid.Parse(filterFound.contactUserHiddenSearch);
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