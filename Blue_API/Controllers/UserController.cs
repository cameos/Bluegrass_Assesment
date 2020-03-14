﻿using System;
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
    [RoutePrefix("api/user")]
    [EnableCors(origins: "*", headers: "*", methods: "*", PreflightMaxAge = int.MaxValue)]
    public class UserController : ApiController
    {
        private readonly IUser _usr;
        public UserController(IUser usr)
        {
            _usr = usr;
        }


        [Route("new")]
        [HttpPost]
        public HttpResponseMessage new_user(User user, Address address)
        {
            HttpResponseMessage message = null;
            var error_message = string.Empty;

            if (string.IsNullOrWhiteSpace(user.FirstName) || string.IsNullOrWhiteSpace(user.LastName) || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(address.AddressNumber))
            {
                error_message = "error, bad request could not be processed further";
                message = Request.CreateResponse(HttpStatusCode.BadRequest, error_message);
                message.Headers.Date = DateTime.Now;
                return message;
            }
            bool flag = _usr.insert(user, address);
            if (!flag)
            {
                message = Request.CreateResponse(HttpStatusCode.BadRequest, flag);
                message.Headers.Date = DateTime.Now;
            }
            else if (flag)
            {
                message = Request.CreateResponse(HttpStatusCode.OK, flag);
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
        public HttpResponseMessage update_user(User user)
        {
            HttpResponseMessage message = null;
            var error_message = string.Empty;
            if (user.UserId == null)
            {
                error_message = "error, bad request could not be processed futher please check format";
                message = Request.CreateResponse(HttpStatusCode.BadRequest, error_message); message.Headers.Date = DateTime.Now;
                return message;
            }

            bool flag = _usr.update(user);

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

        [Route("all")]
        [HttpGet]
        public HttpResponseMessage get_all()
        {
            HttpResponseMessage message = null;
            var error_message = string.Empty;
            List<User> users = new List<User>();

            users = _usr.all().ToList<User>();
            if(users.Count() == 0)
            {
                message = Request.CreateResponse(HttpStatusCode.NotFound, users);
                message.Headers.Date = DateTime.Now;
            }else if(users.Count() > 0)
            {
                message = Request.CreateResponse(HttpStatusCode.OK, users);
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
