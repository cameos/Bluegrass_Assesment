using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

    }
}