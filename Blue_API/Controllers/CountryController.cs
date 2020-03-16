using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Abstraction.Abstract;
using DataAccess.Entities;

namespace Blue_API.Controllers
{
    [RoutePrefix("api/country")]
    [EnableCors(origins: "*", headers: "*", methods: "*", PreflightMaxAge = int.MaxValue)]
    public class CountryController : ApiController
    {

        private readonly ICountry _country;
        public CountryController(ICountry country)
        {
            _country = country;
        }


        [Route("new")]
        [HttpPost]
        public HttpResponseMessage add_country(Country cou)
        {
            HttpResponseMessage message = null;
            var error_message = string.Empty;

            if (string.IsNullOrWhiteSpace(cou.CountryName))
            {
                error_message = "error, please supply the country name";
                message = Request.CreateResponse(HttpStatusCode.BadRequest, error_message);
                message.Headers.Date = DateTime.Now;
                return message;
            }

            bool flag = _country.insert(cou);


            if (!flag)
            {
                message = Request.CreateResponse(HttpStatusCode.NotFound, flag);
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

        [Route("remove")]
        [HttpPost]
        public HttpResponseMessage remove_country(Country cou)
        {
            HttpResponseMessage message = null;
            var error_message = string.Empty;

            if (cou.CountryId == null)
            {
                error_message = "error, this coutry could could not be deleted because of bad format";
                message = Request.CreateResponse(HttpStatusCode.BadRequest, error_message);
                message.Headers.Date = DateTime.Now;
                return message;
            }

            bool flag = _country.remove(cou);
            if (!flag)
            {
                message = Request.CreateResponse(HttpStatusCode.NotFound, flag);
                message.Headers.Date = DateTime.Now;
            }
            else if (flag) { message = Request.CreateResponse(HttpStatusCode.OK, message); message.Headers.Date = DateTime.Now; } else { error_message = "error, internal server error could not be processed further"; message = Request.CreateResponse(HttpStatusCode.InternalServerError, error_message); return message; }
            return message;
        }

        [Route("remove/id")]
        [HttpPost]
        public HttpResponseMessage remove_by_id([FromBody] Guid id)
        {
            HttpResponseMessage message = null; var error_message = string.Empty; if (id == null) { error_message = "error, this coutry could could not be deleted because of bad format"; message = Request.CreateResponse(HttpStatusCode.BadRequest, error_message); message.Headers.Date = DateTime.Now; return message; }
            bool flag = _country.remove_by_id(id);
            if (!flag) { message = Request.CreateResponse(HttpStatusCode.NotFound, flag); message.Headers.Date = DateTime.Now; } else if (flag) { message = Request.CreateResponse(HttpStatusCode.OK, flag); message.Headers.Date = DateTime.Now; } else { error_message = "error, internal server error could not be processed further"; message = Request.CreateResponse(HttpStatusCode.InternalServerError, error_message); return message; }
            return message;
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage update_country(Country cou)
        {
            HttpResponseMessage message = null; var error_message = string.Empty; if (cou.CountryId == null) { error_message = "error, bad request could not be processed futher please check format"; message = Request.CreateResponse(HttpStatusCode.BadRequest, error_message); message.Headers.Date = DateTime.Now; return message; }
            bool flag = _country.update(cou); if (!flag) { message = Request.CreateResponse(HttpStatusCode.NotFound, flag); message.Headers.Date = DateTime.Now; } else if (flag) { message = Request.CreateResponse(HttpStatusCode.OK, flag); message.Headers.Date = DateTime.Now; return message; } else { error_message = "error, internal server error could not be processed further"; message = Request.CreateResponse(HttpStatusCode.InternalServerError, error_message); message.Headers.Date = DateTime.Now; return message; }
            return message;
        }

        [Route("all")]
        [HttpGet]
        public HttpResponseMessage all_countries()
        {
            HttpResponseMessage message = null;
            var error_message = string.Empty;
            List<Country> countries = new List<Country>();

            countries = _country.all().ToList<Country>();
            if(countries.Count() == 0)
            {
                message = Request.CreateResponse(HttpStatusCode.NotFound, countries);
                message.Headers.Date = DateTime.Now;
            }else if(countries.Count() > 0)
            {
                message = Request.CreateResponse(HttpStatusCode.OK, countries);
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
                error_message = "error, could not be processed further";
                message = Request.CreateResponse(HttpStatusCode.BadRequest, error_message);
                message.Headers.Date = DateTime.Now;
                return message;
            }

            Country cou = _country.show_by_id(id);

            if(cou == null)
            {
                message = Request.CreateResponse(HttpStatusCode.NotFound, cou);
                message.Headers.Date = DateTime.Now;
            }else if(cou != null)
            {
                message = Request.CreateResponse(HttpStatusCode.OK, cou);
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
        public HttpResponseMessage search_one([FromBody] string name)
        {
            HttpResponseMessage message = null;
            var error_message = string.Empty;


            if (string.IsNullOrWhiteSpace(name))
            {
                error_message = "error, could not be processed further";
                message = Request.CreateResponse(HttpStatusCode.BadRequest, error_message);
                message.Headers.Date = DateTime.Now;
                return message;
            }

            Country cou = _country.search_by_name(name);

            if (cou == null)
            {
                message = Request.CreateResponse(HttpStatusCode.NotFound, cou);
                message.Headers.Date = DateTime.Now;
            }
            else if (cou != null)
            {
                message = Request.CreateResponse(HttpStatusCode.OK, cou);
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
