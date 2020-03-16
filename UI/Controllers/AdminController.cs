﻿using System;
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
    [Authorize]
    public class AdminController : Controller
    {
        // GET: Admin
        [Route("home")]
        [HttpGet]
        public ActionResult home()
        {
            return View();
        }

        [Route("countries")]
        [HttpGet]
        public ActionResult get_all_countries()
        {
            List<Country> countries = new List<Country>();

            using (var api = new HttpClient())
            {
                api.BaseAddress = new Uri("https://localhost:44343/api/country/");
                api.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var get_countries = api.GetAsync("all");
                get_countries.Wait();
                var result = get_countries.Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var s = result.Content.ReadAsAsync<List<Country>>();
                    s.Wait();
                    countries = s.Result;
                }
                else if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    var s = result.Content.ReadAsAsync<List<Country>>();
                    s.Wait();
                    countries = s.Result;
                }
            }
            return Json(countries, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.AllowGet);
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
            using (var api = new HttpClient())
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

            if (temp != null && (temp.CountryName != null) && (temp.CountryName == country.CountryName))
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

        [Route("province")]
        [HttpPost]
        public ActionResult add_province(AddProvince prov)
        {

            var error_message = new object();
            if (string.IsNullOrWhiteSpace(prov.adminCSelect) || string.IsNullOrWhiteSpace(prov.adminProvince))
            {
                error_message = "error, bad request please check inputs";
                return Json(error_message, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }

            var countryId = Guid.Parse(prov.adminCSelect);
            Province province = new Province
            {
                CountryId = countryId,
                ProvinceName = prov.adminProvince,
                Description = prov.adminProvinceDesc
            };

            Province temp = new Province();
            using (var api = new HttpClient())
            {
                api.BaseAddress = new Uri("https://localhost:44343/api/province/");
                api.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var post = api.PostAsJsonAsync<string>("search/name", prov.adminProvince);
                post.Wait();
                var result = post.Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var s = result.Content.ReadAsAsync<Province>();
                    s.Wait();
                    temp = s.Result;
                }
                else if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    var s = result.Content.ReadAsAsync<Province>();
                    s.Wait();
                    temp = s.Result;
                }
                else
                {
                    error_message = "error, internal server error please try again later";
                    return Json(error_message, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
                }
            }
            if (temp != null && (temp.ProvinceName != null) && (temp.ProvinceName == prov.adminProvince) && (temp.CountryId == province.CountryId))
            {
                error_message = "error, country exists";
                return Json(error_message, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }


            //add new province
            bool flag = false;
            using (var api = new HttpClient())
            {
                api.BaseAddress = new Uri("https://localhost:44343/api/province/");
                api.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var post = api.PostAsJsonAsync<Province>("new", province);
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


        [Route("provinces")]
        [HttpPost]
        public ActionResult get_provinces(string CountryId)
        {
            List<Province> provinces = new List<Province>();
            var id = Guid.Parse(CountryId);

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

        [Route("city")]
        [HttpPost]
        public ActionResult add_city(AddCity AddCity)
        {

            var error_message = new object();
            if(string.IsNullOrWhiteSpace(AddCity.adminCoSelect) || string.IsNullOrWhiteSpace(AddCity.adminPrSelect) || string.IsNullOrWhiteSpace(AddCity.adminCity))
            {
                error_message = "error, bad request please check inputs";
                return Json(error_message, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }

            var countryId = Guid.Parse(AddCity.adminCoSelect);
            var provinceId = Guid.Parse(AddCity.adminPrSelect);
            City city = new City
            {
                CityName = AddCity.adminCity,
                CountryId = countryId,
                ProvinceId = provinceId,
                Description = AddCity.adminCityDesc
            };

            //add new city
            bool flag = false;
            using (var api = new HttpClient())
            {
                api.BaseAddress = new Uri("https://localhost:44343/api/city/");
                api.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var post = api.PostAsJsonAsync<City>("new", city);
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