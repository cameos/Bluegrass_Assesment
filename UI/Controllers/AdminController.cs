using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}