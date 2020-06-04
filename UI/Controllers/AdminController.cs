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
using System.Net.Mail;
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
            List<User> users = new List<User>();
            using (var api = new HttpClient())
            {
                api.BaseAddress = new Uri("https://localhost:44343/api/user/");
                api.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var get_users = api.GetAsync("all");
                get_users.Wait();
                var result = get_users.Result;
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
            return View(users);
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

        [Route("city")]
        [HttpPost]
        public ActionResult add_city(AddCity AddCity)
        {

            var error_message = new object();
            if (string.IsNullOrWhiteSpace(AddCity.adminCoSelect) || string.IsNullOrWhiteSpace(AddCity.adminPrSelect) || string.IsNullOrWhiteSpace(AddCity.adminCity))
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

        [Route("cities")]
        [HttpPost]
        public ActionResult get_cities_by_province(string ProvinceId)
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

        [Route("contact")]
        [HttpPost]
        public ActionResult add_contact(NewContact contact)
        {
            var error_message = new object();
            if (string.IsNullOrWhiteSpace(contact.contactFirstName) || string.IsNullOrWhiteSpace(contact.contactLastName) || string.IsNullOrWhiteSpace(contact.contactIdNumber) || string.IsNullOrWhiteSpace(contact.contactGender) || string.IsNullOrWhiteSpace(contact.contactEmail) || string.IsNullOrWhiteSpace(contact.contactStatus) || string.IsNullOrWhiteSpace(contact.contactPhone) || string.IsNullOrWhiteSpace(contact.addressNumber) || string.IsNullOrWhiteSpace(contact.addressName) || string.IsNullOrWhiteSpace(contact.addressSuburb) || string.IsNullOrWhiteSpace(contact.addressPostalCode) || string.IsNullOrWhiteSpace(contact.contactCountry) || string.IsNullOrWhiteSpace(contact.contactProv) || string.IsNullOrWhiteSpace(contact.contactCiti))
            {
                error_message = "error, please enter all fields";
                return Json(error_message, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }


            var countryId = Guid.Parse(contact.contactCountry);
            var provinceId = Guid.Parse(contact.contactProv);
            var cityId = Guid.Parse(contact.contactCiti);

            User user = new User
            {
                FirstName = contact.contactFirstName,
                LastName = contact.contactLastName,
                Gender = contact.contactGender,
                ID = contact.contactIdNumber,
                Phone = contact.contactPhone,
                Email = contact.contactEmail,
                Status = contact.contactStatus,

            };
            if (contact.contactImage != null)
            {
                user.MimeType = contact.contactImage.ContentType;
                user.Avatar = new byte[contact.contactImage.ContentLength];
                contact.contactImage.InputStream.Read(user.Avatar, 0, contact.contactImage.ContentLength);
            }
            Address address = new Address
            {
                AddressNumber = contact.addressNumber,
                StreetName = contact.addressName,
                CountryId = countryId,
                ProvinceId = provinceId,
                CityId = cityId,
                Surburb = contact.addressSuburb,
                PostalCode = contact.addressPostalCode
            };


            ContactInformation information = new ContactInformation
            {
                Address = address,
                User = user
            };



            //add new city
            bool flag = false;
            using (var api = new HttpClient())
            {
                api.BaseAddress = new Uri("https://localhost:44343/api/user/");
                api.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var post = api.PostAsJsonAsync<ContactInformation>("new", information);
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

                //message to be sent to the email
                var email_message = "<br/><img src='https://cdn.onlinewebfonts.com/svg/img_508672.png' height='30' width='30' class='rounded' style='display: inline-block;'/> <span style='font-weight:bold;font-size:1.5em;'>Contact Added successfully with following fields</span><br/><br/>Name: " + user.FirstName + " " + user.LastName;
                email_message += "<br/>Phone: " + user.Phone;
                email_message += "<br/>Email: " + user.Email;
                email_message += "<br/>Gender: " + user.Gender;
                email_message += "<br/>Status: " + user.Status;


                //construct mailmessage
                MailMessage message = new MailMessage("bqunta79@gmail.com", "mark@bluegrassdigital.com", "New contact added", email_message);
                message.IsBodyHtml = true;


                NetworkCredential credential = new NetworkCredential("bqunta79", "delete+92");
                SmtpClient client = new SmtpClient();
                client.UseDefaultCredentials = false;
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.ServicePoint.MaxIdleTime = 2;
                client.Credentials = credential;


                //send email
                client.Send(message);


                error_message = Url.Action("home", "admin");
                return Json(error_message, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }


        }

        [Route("contacts")]
        [HttpPost]
        public ActionResult all_contacts()
        {
            List<User> users = new List<User>();
            using (var api = new HttpClient())
            {
                api.BaseAddress = new Uri("https://localhost:44343/api/user/");
                api.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var get_users = api.GetAsync("all");
                get_users.Wait();
                var result = get_users.Result;
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
            return Json(users, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.AllowGet);
        }

        [Route("file")]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public FileContentResult get_image(Guid userId)
        {
            //get content from the web service
            List<User> users = new List<User>();
            using (var api = new HttpClient())
            {
                api.BaseAddress = new Uri("https://localhost:44381/api/user/");
                api.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var all = api.GetAsync("all");
                all.Wait();
                var result = all.Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var s = result.Content.ReadAsAsync<List<User>>();
                    s.Wait();
                    users = s.Result;
                }
            }

            User user = users.FirstOrDefault(x => x.UserId == userId);

            if (user != null)
            {
                return File(user.Avatar, user.MimeType);
            }
            else
            {
                return null;
            }

        }

        [Route("remove")]
        [HttpPost]
        public ActionResult delete_user(ContactDelete del)
        {
            var error_message = new object();
            if (string.IsNullOrWhiteSpace(del.contactDeleteHidden))
            {
                error_message = "error, empty request";
                return Json(error_message, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }
            var id = Guid.Parse(del.contactDeleteHidden);

            bool flag = false;
            using (var api = new HttpClient())
            {
                api.BaseAddress = new Uri("https://localhost:44343/api/user/");
                api.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var post = api.PostAsJsonAsync<Guid>("remove/id", id);
                post.Wait();
                var result = post.Result;
                if (result.StatusCode == HttpStatusCode.OK)
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

        [Route("find/user")]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult get_user(Guid userID)
        {

            User user = new User();

            using (var api = new HttpClient())
            {
                api.BaseAddress = new Uri("https://localhost:44343/api/user/");
                api.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var get_user = api.PostAsJsonAsync<Guid>("show/id", userID);
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
            return View(user);

        }

        [Route("show/user")]
        [HttpPost]
        public ActionResult ShowUser(ContactUpdate update)
        {
            User user = new User();
            var error_message = new object();
            JsonResult json = new JsonResult();
            if (string.IsNullOrWhiteSpace(update.contactUpdateHidden))
            {
                error_message = "error, could not update click";
                return Json(error_message, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }

            var id = Guid.Parse(update.contactUpdateHidden);

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

            error_message = user;
            json.Data = error_message;
            json.ContentEncoding = Encoding.UTF8;
            json.ContentType = "application/json; charset=utf-8";
            json.MaxJsonLength = int.MaxValue;
            return json;
           
        }

        [Route("contact/update")]
        [HttpPost]
        public ActionResult updateContact(ContactUpdateFinal update)
        {
            var error_message = new object();
            if (string.IsNullOrWhiteSpace(update.updateContactFirst) || string.IsNullOrWhiteSpace(update.updateContactLast) || string.IsNullOrWhiteSpace(update.updateContactIDnumber) || string.IsNullOrWhiteSpace(update.updateContactGender) || string.IsNullOrWhiteSpace(update.updateContactEmail) || string.IsNullOrWhiteSpace(update.updateContactStatus) || string.IsNullOrWhiteSpace(update.updateContactPhone) || string.IsNullOrWhiteSpace(update.updateContactHiddenField))
            {
                error_message = "error, please enter all fields";
                return Json(error_message, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }
            var id = Guid.Parse(update.updateContactHiddenField);

            User temp = new User();
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
                    temp = s.Result;
                }
                else if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    var s = result.Content.ReadAsAsync<User>();
                    s.Wait();
                    temp = s.Result;
                }
            }


            User user = new User
            {
                UserId = id,
                Email = update.updateContactEmail,
                FirstName = update.updateContactFirst,
                Gender = update.updateContactGender,
                ID = update.updateContactGender,
                LastName = update.updateContactLast,
                Phone = update.updateContactPhone,
                Status = update.updateContactStatus,
                Avatar = temp.Avatar,
                MimeType = temp.MimeType,

            };

            bool flag = false;
            using (var api = new HttpClient())
            {
                api.BaseAddress = new Uri("https://localhost:44343/api/user/");
                api.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var post = api.PutAsJsonAsync<User>("update", user);
                post.Wait();
                var result = post.Result;
                if (result.StatusCode == HttpStatusCode.OK)
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

                //message to be sent to the email
                var email_message = "<br/><img src='https://cdn.onlinewebfonts.com/svg/img_508672.png' height='30' width='30' class='rounded' style='display: inline-block;'/> <span style='font-weight:bold;font-size:1.5em;'>Contact updated successfully with following fields</span><br/><br/>Name: " + user.FirstName + " " + user.LastName;
                email_message += "<br/>Phone: " + user.Phone;
                email_message += "<br/>Email: " + user.Email;
                email_message += "<br/>Gender: " + user.Gender;
                email_message += "<br/>Status: " + user.Status;



                //construct mailmessage
                MailMessage message = new MailMessage("bqunta79@gmail.com", "mark@bluegrassdigital.com", "Contact updated", email_message);
                message.IsBodyHtml = true;


                NetworkCredential credential = new NetworkCredential("bqunta79", "delete+92");
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = credential;


                //send email
                client.Send(message);

                error_message = Url.Action("home", "admin");
                return Json(error_message, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }
        }


    }
}