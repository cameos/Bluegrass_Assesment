using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI.Models;
using System.Text;
using DataAccess.Entities;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Security;

namespace UI.Controllers
{
    [RoutePrefix("login")]
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }


        [Route("register")]
        [HttpGet]
        public ActionResult register()
        {
            return View();
        }

        [Route("new")]
        [HttpPost]
        public ActionResult register_new(adminModel adminModel)
        {

            var error_message = new object();
            if(string.IsNullOrWhiteSpace(adminModel.adminName) || string.IsNullOrWhiteSpace(adminModel.adminLastName) || string.IsNullOrWhiteSpace(adminModel.adminPassword) || string.IsNullOrWhiteSpace(adminModel.adminEmail))
            {
                error_message = "error, something went wrong with the request";
                return Json(error_message, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }

            var salt = EncryptionVerifiers.generate_salt();
            var real_password = EncryptionVerifiers.encrypt_value(salt, adminModel.adminPassword);

            Admin admin = new Admin
            {
                Email = adminModel.adminEmail,
                FirstName = adminModel.adminName,
                LastName = adminModel.adminLastName,
                Password = adminModel.adminPassword,
                Salt = salt
            };

            var flag = false;
            using(var api = new HttpClient())
            {
                api.BaseAddress = new Uri("https://localhost:44343/api/admin/");
                api.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var post = api.PostAsJsonAsync<Admin>("new", admin);
                post.Wait();
                var result = post.Result;
                if(result.StatusCode == HttpStatusCode.Created)
                {
                    var s = result.Content.ReadAsAsync<bool>();
                    s.Wait();
                    flag = s.Result;
                }else if(result.StatusCode == HttpStatusCode.NotFound)
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
                error_message = "error, could not regsiter at this moment, please try again later";
                return Json(error_message, "application/json; charset=utf-8", Encoding.UTF8, JsonRequestBehavior.DenyGet);
            }
            else
            {
                //var data = Url.Action("index", "home");
                FormsAuthentication.SetAuthCookie(admin.Email, false);
                return RedirectToActionPermanent("Login", "home");
            }
        }

    }
}