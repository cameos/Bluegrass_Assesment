using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using DataAccess.Entities;
using Abstraction.Abstract;

namespace Blue_API.Controllers
{
    [RoutePrefix("api/admin")]
    [EnableCors(origins: "*", headers: "*", methods: "*", PreflightMaxAge = int.MaxValue)]
    public class AdminController : ApiController
    {
        private readonly IAdmin _admin;
        public AdminController(IAdmin ad)
        {
            _admin = ad;
        }

        [Route("new")]
        [HttpPost]
        public HttpResponseMessage new_admin(Admin admin)
        {
            HttpResponseMessage message = null;
            var error_message = string.Empty;

            if (string.IsNullOrWhiteSpace(admin.Salt) || string.IsNullOrWhiteSpace(admin.Password) || string.IsNullOrWhiteSpace(admin.Email) || string.IsNullOrWhiteSpace(admin.FirstName))
            {
                error_message = "error, bad request could not be processed further";
                message = Request.CreateResponse(HttpStatusCode.BadRequest, error_message);
                message.Headers.Date = DateTime.Now;
                return message;
            }
            bool flag = _admin.new_admin(admin);

            if (!flag)
            {
                message = Request.CreateResponse(HttpStatusCode.BadRequest, flag);
                message.Headers.Date = DateTime.Now;
            }
            else if (flag)
            {
                message = Request.CreateResponse(HttpStatusCode.Created, flag);
                message.Headers.Date = DateTime.Now;
            }
            else
            {
                error_message = "error, internal server error could not be processed further";
                message = Request.CreateResponse(HttpStatusCode.InternalServerError, error_message);
                message.Headers.Date = DateTime.Now;
                return message;
            }
            return message;
        }


        [Route("all")]
        [HttpGet]
        public HttpResponseMessage all_admins()
        {
            HttpResponseMessage message = null;
            var error_message = string.Empty;
            List<Admin> admins = new List<Admin>();

            admins = _admin.all_admin().ToList<Admin>();

            if (admins.Count() == 0)
            {
                message = Request.CreateResponse(HttpStatusCode.NotFound, admins);
                message.Headers.Date = DateTime.Now;
            }
            else if (admins.Count() > 0)
            {
                message = Request.CreateResponse(HttpStatusCode.OK, admins);
                message.Headers.Date = DateTime.Now;
            }
            else
            {
                error_message = "error, internal server error could not be processed further";
                message = Request.CreateResponse(HttpStatusCode.InternalServerError, error_message);
                message.Headers.Date = DateTime.Now;
                return message;
            }
            return message;
        }


        [Route("show")]
        [HttpPost]
        public HttpResponseMessage show_one([FromBody]Guid id)
        {
            HttpResponseMessage message = null;
            var error_message = string.Empty;


            if (id == null)
            {
                error_message = "error, could not be processed further bad request";
                message = Request.CreateResponse(HttpStatusCode.BadRequest, error_message);
                message.Headers.Date = DateTime.Now;
                return message;
            }

            Admin admin = _admin.show_admin(id);
            if (admin == null)
            {
                message = Request.CreateResponse(HttpStatusCode.NotFound, admin);
                message.Headers.Date = DateTime.Now;
            }
            else if (admin != null)
            {
                message = Request.CreateResponse(HttpStatusCode.OK, admin);
                message.Headers.Date = DateTime.Now;
            }
            else
            {
                error_message = "error, internal server error could not be processed further";
                message = Request.CreateResponse(HttpStatusCode.InternalServerError, error_message);
                message.Headers.Date = DateTime.Now;
                return message;
            }
            return message;
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage update_admin(Admin admin)
        {
            HttpResponseMessage message = null;
            var error_message = string.Empty;
            if (admin.AdminId == null)
            {
                error_message = "error, bad request could not be processed futher please check format";
                message = Request.CreateResponse(HttpStatusCode.BadRequest, error_message); message.Headers.Date = DateTime.Now;
                return message;
            }

            bool flag = _admin.update_admin(admin);

            if (!flag)
            {
                message = Request.CreateResponse(HttpStatusCode.NotFound, flag);
                message.Headers.Date = DateTime.Now;
            }
            else if
                (flag)
            {
                message = Request.CreateResponse(HttpStatusCode.OK, flag);
                message.Headers.Date = DateTime.Now; return message;
            }
            else
            {
                error_message = "error, internal server error could not be processed further"; message = Request.CreateResponse(HttpStatusCode.InternalServerError, error_message); message.Headers.Date = DateTime.Now; return message;
            }
            return message;
        }

        [Route("search/email")]
        [HttpPost]
        public HttpResponseMessage email_search([FromBody]string email)
        {
            HttpResponseMessage message = null;
            var error_message = string.Empty;


            if (string.IsNullOrWhiteSpace(email)|| !(email.Contains("@")))
            {
                error_message = "error, could not be processed further bad request";
                message = Request.CreateResponse(HttpStatusCode.BadRequest, error_message);
                message.Headers.Date = DateTime.Now;
                return message;
            }

            Admin admin = _admin.show_by_email(email);
            if (admin == null)
            {
                message = Request.CreateResponse(HttpStatusCode.NotFound, admin);
                message.Headers.Date = DateTime.Now;
            }
            else if (admin != null)
            {
                message = Request.CreateResponse(HttpStatusCode.OK, admin);
                message.Headers.Date = DateTime.Now;
            }
            else
            {
                error_message = "error, internal server error could not be processed further";
                message = Request.CreateResponse(HttpStatusCode.InternalServerError, error_message);
                message.Headers.Date = DateTime.Now;
                return message;
            }
            return message;
        }

    }
}
