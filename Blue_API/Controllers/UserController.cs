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
        public HttpResponseMessage new_user(ContactInformation information)
        {
            HttpResponseMessage message = null;
            var error_message = string.Empty;

            if (string.IsNullOrWhiteSpace(information.User.FirstName) || string.IsNullOrWhiteSpace(information.User.LastName) || string.IsNullOrWhiteSpace(information.User.Email) || string.IsNullOrWhiteSpace(information.Address.AddressNumber))
            {
                error_message = "error, bad request could not be processed further";
                message = Request.CreateResponse(HttpStatusCode.BadRequest, error_message);
                message.Headers.Date = DateTime.Now;
                return message;
            }

            User user = new User
            {
                Email = information.User.Email,
                FirstName = information.User.FirstName,
                Gender = information.User.Gender,
                Avatar = information.User.Avatar,
                ID = information.User.ID,
                LastName = information.User.LastName,
                Phone = information.User.Phone,
                Status = information.User.Status,
                MimeType = information.User.MimeType

            };
            Address address = new Address
            {
                AddressNumber = information.Address.AddressNumber,
                CityId = information.Address.CityId,
                CountryId = information.Address.CountryId,
                ProvinceId = information.Address.ProvinceId,
                PostalCode = information.Address.PostalCode,
                StreetName = information.Address.StreetName,
                Surburb = information.Address.Surburb
            };

            bool flag = _usr.insert(user, address);
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
            else if (flag)
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
            if (users.Count() == 0)
            {
                message = Request.CreateResponse(HttpStatusCode.NotFound, users);
                message.Headers.Date = DateTime.Now;
            }
            else if (users.Count() > 0)
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

        [Route("show/id")]
        [HttpPost]
        public HttpResponseMessage full_info([FromBody]Guid id)
        {
            HttpResponseMessage message = null;
            var error_message = string.Empty;
            User user = new User();

            if (id == null)
            {
                error_message = "error, could not be processed further bad request";
                message = Request.CreateResponse(HttpStatusCode.BadRequest, error_message);
                message.Headers.Date = DateTime.Now;
                return message;
            }

            user = _usr.show_by_id(id);
            if (user == null)
            {
                message = Request.CreateResponse(HttpStatusCode.NotFound, user);
                message.Headers.Date = DateTime.Now;
            }
            else if (user != null)
            {
                message = Request.CreateResponse(HttpStatusCode.OK, user);
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


        [Route("search/name")]
        [HttpPost]
        public HttpResponseMessage search_name(PredictiveFilter filter)
        {
            List<User> users = new List<User>();
            HttpResponseMessage message = null;
            var error_message = string.Empty;

            if (string.IsNullOrWhiteSpace(filter.First))
            {
                message = Request.CreateResponse(HttpStatusCode.NotFound, users);
                message.Headers.Date = DateTime.Now;
                return message;
            }

            users = _usr.search_first(filter);
            if (users.Count() == 0)
            {
                message = Request.CreateResponse(HttpStatusCode.NotFound, users);
                message.Headers.Date = DateTime.Now;
            }
            else if (users.Count() > 0)
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

        [Route("search/surname")]
        [HttpPost]
        public HttpResponseMessage search_surname([FromBody]string surname)
        {
            List<User> users = new List<User>();
            HttpResponseMessage message = null;
            var error_message = string.Empty;

            if (string.IsNullOrWhiteSpace(surname))
            {
                error_message = "error, bad request could not be processed further";
                message = Request.CreateResponse(HttpStatusCode.BadRequest, error_message);
                message.Headers.Date = DateTime.Now;
                return message;
            }

            users = _usr.search_surname(surname);
            if (users.Count() == 0)
            {
                message = Request.CreateResponse(HttpStatusCode.NotFound, users);
                message.Headers.Date = DateTime.Now;
            }
            else if (users.Count() > 0)
            {
                message = Request.CreateResponse(HttpStatusCode.OK, users);
                message.Headers.Date = DateTime.Now;
            }
            else
            {
                error_message = "error, internal server error could not be processed further";
                message = Request.CreateResponse(HttpStatusCode.InternalServerError, error_message);
                message.Headers.Date = DateTime.Now;
            }
            return message;
        }

        [Route("remove/id")]
        [HttpPost]
        public HttpResponseMessage remove_by_id([FromBody]Guid id)
        {
            var error_message = string.Empty;
            HttpResponseMessage message = null;

            if (id == null)
            {
                error_message = "error, bad request could not bee processed further";
                message = Request.CreateResponse(HttpStatusCode.BadRequest, error_message);
                message.Headers.Date = DateTime.Now;
                return message;
            }

            bool flag = _usr.remove_by_id(id);
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

        [Route("contact/page")]
        [HttpGet]
        public HttpResponseMessage contact_page()
        {
            HttpResponseMessage message = null;
            UserCities userCities = new UserCities();
            var error_message = string.Empty;

            userCities = _usr.contact_home();

            if (userCities == null)
            {
                message = Request.CreateResponse(HttpStatusCode.BadRequest, userCities);
                message.Headers.Date = DateTime.Now;
            }
            else if (userCities != null)
            {
                message = Request.CreateResponse(HttpStatusCode.OK, userCities);
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

        [Route("user/information")]
        [HttpPost]
        public HttpResponseMessage full_information([FromBody]Guid id)
        {
            HttpResponseMessage message = null;
            var error_message = string.Empty;
            FullUserInformation fullUserInformation = new FullUserInformation();

            if (id == null || id== Guid.Empty)
            {
                error_message = "error, could not be processed further bad request";
                message = Request.CreateResponse(HttpStatusCode.BadRequest, error_message);
                message.Headers.Date = DateTime.Now;
                return message;
            }

            fullUserInformation = _usr.full_info(id);
            if (fullUserInformation.User == null)
            {
                message = Request.CreateResponse(HttpStatusCode.NotFound, fullUserInformation);
                message.Headers.Date = DateTime.Now;
            }
            else if (fullUserInformation.User != null)
            {
                message = Request.CreateResponse(HttpStatusCode.OK, fullUserInformation);
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
