using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess.Entities;
using System.Text;
using UI.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace UI.Controllers
{
    [RoutePrefix("admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        [Route("home")]
        [HttpGet]
        public ActionResult home()
        {
            return View();
        }



        [Route("country")]
        [HttpPost]
        public ActionResult add_country(CountryAdd AddCountry)
        {
            var error_message = new object();
            if (string.IsNullOrWhiteSpace(AddCountry.countryAdd))
            {
                error_message = "error, input empty basd request";
                return Json(error_message, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }

            Country country = new Country
            {
                CountryName = AddCountry.countryAdd,
                Description = AddCountry.adminCountryDesc
            };

            //first check if this country exists
            Country temp = new Country();
            using(var api = new HttpClient())
            {
                api.BaseAddress = new Uri("https://localhost:44343/api/country/");
                api.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var post = api.PostAsJsonAsync<string>("search/name", AddCountry.countryAdd);
                post.Wait();
                var result = post.Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var s = result.Content.ReadAsAsync<Country>();
                    s.Wait();
                    temp = s.Result;
                }
                else if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    var s = result.Content.ReadAsAsync<Country>();
                    s.Wait();
                    temp = s.Result;
                }
                else
                {
                    error_message = "error, internal server error please try again later";
                    return Json(error_message, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
                }
            }

            if(temp.CountryName == country.CountryName)
            {
                error_message = "error, country exists";
                return Json(error_message, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }

            //ad this country
            bool flag = false;
            using (var api = new HttpClient())
            {
                api.BaseAddress = new Uri("https://localhost:44343/api/country/");
                api.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var post = api.PostAsJsonAsync<Country>("new", country);
                post.Wait();
                var result = post.Result;
                if (result.StatusCode == HttpStatusCode.Created)
                {
                    var s = result.Content.ReadAsAsync<bool>();
                    s.Wait();
                    flag = s.Result;
                }
                else if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    var s = result.Content.ReadAsAsync<bool>();
                    s.Wait();
                    flag = s.Result;
                }
                else
                {
                    error_message = "error, internal server error please try again later";
                    return Json(error_message, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
                }
            }


            if (!flag)
            {
                error_message = "error, internal server error please try again later";
                return Json(error_message, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }
            else
            {
                error_message = Url.Action("home", "admin");
                return Json(error_message, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }


        }
    }
}