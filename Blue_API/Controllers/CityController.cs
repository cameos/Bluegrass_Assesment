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
    [RoutePrefix("api/city")]
    [EnableCors(origins: "*", headers: "*", methods: "*", PreflightMaxAge = int.MaxValue)]
    public class CityController : ApiController
    {
        private readonly ICity _city;
        public CityController(ICity ci)
        {
            _city = ci;
        }


        [Route("all")]
        [HttpGet]
        public HttpResponseMessage show_all()
        {
            HttpResponseMessage message = null;
            var error_message = string.Empty;
            List<City> cities = new List<City>();

            cities = _city.all().ToList<City>();
            if (cities.Count() == 0)
            {
                message = Request.CreateResponse(HttpStatusCode.NotFound, cities);
                message.Headers.Date = DateTime.Now;
            }
            else if (cities.Count() > 0)
            {
                message = Request.CreateResponse(HttpStatusCode.OK, cities);
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


        [Route("new")]
        [HttpPost]
        public HttpResponseMessage add_city(City city)
        {
            HttpResponseMessage message = null;
            var error_message = string.Empty;

            if (string.IsNullOrWhiteSpace(city.CityName)|| city.CountryId == null || city.ProvinceId == null)
            {
                error_message = "error, please supply the city name";
                message = Request.CreateResponse(HttpStatusCode.BadRequest, error_message);
                message.Headers.Date = DateTime.Now;
                return message;
            }
            bool flag = _city.insert(city);


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


        [Route("show")]
        [HttpPost]
        public HttpResponseMessage show_by_id([FromBody]Guid id)
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

            City city = _city.show_by_id(id);

            if (city == null)
            {
                message = Request.CreateResponse(HttpStatusCode.NotFound, city);
                message.Headers.Date = DateTime.Now;
            }
            else if (city != null)
            {
                message = Request.CreateResponse(HttpStatusCode.OK, city);
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
        public HttpResponseMessage update_city(City city)
        {
            HttpResponseMessage message = null; var error_message = string.Empty; if (city.CityId == null) { error_message = "error, bad request could not be processed futher please check format"; message = Request.CreateResponse(HttpStatusCode.BadRequest, error_message); message.Headers.Date = DateTime.Now; return message; }
            bool flag = _city.update(city);
            if (!flag) { message = Request.CreateResponse(HttpStatusCode.NotFound, flag); message.Headers.Date = DateTime.Now; } else if (flag) { message = Request.CreateResponse(HttpStatusCode.OK, flag); message.Headers.Date = DateTime.Now; return message; } else { error_message = "error, internal server error could not be processed further"; message = Request.CreateResponse(HttpStatusCode.InternalServerError, error_message); message.Headers.Date = DateTime.Now; return message; }
            return message;
        }


        [Route("remove/id")]
        [HttpDelete]
        public HttpResponseMessage remove_by_id([FromBody] Guid id)
        {
            HttpResponseMessage message = null; var error_message = string.Empty;
            if (id == null)
            {
                error_message = "error, this city could could not be deleted because of bad format"; message = Request.CreateResponse(HttpStatusCode.BadRequest, error_message); message.Headers.Date = DateTime.Now; return message;
            }

            bool flag = _city.remove_by_id(id);

            if (!flag)
            {
                message = Request.CreateResponse(HttpStatusCode.NotFound, flag); message.Headers.Date = DateTime.Now;
            }
            else if (flag)
            {
                message = Request.CreateResponse(HttpStatusCode.OK, flag); message.Headers.Date = DateTime.Now;
            }
            else
            {
                error_message = "error, internal server error could not be processed further"; message = Request.CreateResponse(HttpStatusCode.InternalServerError, error_message); return message;
            }
            return message;
        }

        [Route("remove")]
        [HttpDelete]
        public HttpResponseMessage remove_city(City city)
        {
            HttpResponseMessage message = null;
            var error_message = string.Empty;

            if (city.CityId == null)
            {
                error_message = "error, this city could could not be deleted because of bad format";
                message = Request.CreateResponse(HttpStatusCode.BadRequest, error_message);
                message.Headers.Date = DateTime.Now;
                return message;
            }

            bool flag = _city.remove(city);
            if (!flag)
            {
                message = Request.CreateResponse(HttpStatusCode.NotFound, flag);
                message.Headers.Date = DateTime.Now;
            }
            else if (flag) { message = Request.CreateResponse(HttpStatusCode.OK, message); message.Headers.Date = DateTime.Now; } else { error_message = "error, internal server error could not be processed further"; message = Request.CreateResponse(HttpStatusCode.InternalServerError, error_message); return message; }
            return message;
        }

        [Route("seach/province")]
        [HttpPost]
        public HttpResponseMessage get_cities_by_province([FromBody]Guid id)
        {
            List<City> cities = new List<City>();
            HttpResponseMessage message = null;
            var error_message = string.Empty;

            if (id == null)
            {
                error_message = "error, bad request could not be processed further";
                message = Request.CreateResponse(HttpStatusCode.BadRequest, error_message);
                message.Headers.Date = DateTime.Now;
                return message;
            }

            cities = _city.cities_by_province(id);
            if (cities.Count() == 0)
            {
                message = Request.CreateResponse(HttpStatusCode.NotFound, cities);
                message.Headers.Date = DateTime.Now;
            }
            else if (cities.Count() > 0)
            {
                message = Request.CreateResponse(HttpStatusCode.OK, cities);
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
    }
}
