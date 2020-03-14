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
    [RoutePrefix("api/province")]
    [EnableCors(origins: "*", headers: "*", methods: "*", PreflightMaxAge = int.MaxValue)]
    public class ProvinceController : ApiController
    {

        private readonly IProvince _prov;
        public ProvinceController(IProvince prov)
        {
            _prov = prov;
        }


        [Route("new")]
        [HttpPost]
        public HttpResponseMessage add_province(Province prov)
        {
            HttpResponseMessage message = null;
            var error_message = string.Empty;

            if (string.IsNullOrWhiteSpace(prov.ProvinceName))
            {
                error_message = "error, please supply the country name";
                message = Request.CreateResponse(HttpStatusCode.BadRequest, error_message);
                message.Headers.Date = DateTime.Now;
                return message;
            }

            bool flag = _prov.insert(prov);


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
        [HttpDelete]
        public HttpResponseMessage delete_province(Province prov)
        {
            HttpResponseMessage message = null;
            var error_message = string.Empty;

            if (prov.ProvinceId == null)
            {
                error_message = "error, this province could could not be deleted because of bad format";
                message = Request.CreateResponse(HttpStatusCode.BadRequest, error_message);
                message.Headers.Date = DateTime.Now;
                return message;
            }

            bool flag = _prov.remove(prov);
            if (!flag)
            {
                message = Request.CreateResponse(HttpStatusCode.NotFound, flag);
                message.Headers.Date = DateTime.Now;
            }
            else if (flag) { message = Request.CreateResponse(HttpStatusCode.OK, message); message.Headers.Date = DateTime.Now; } else { error_message = "error, internal server error could not be processed further"; message = Request.CreateResponse(HttpStatusCode.InternalServerError, error_message); return message; }
            return message;
        }

        [Route("remove/id")]
        [HttpDelete]
        public HttpResponseMessage remove_by_id([FromBody]Guid id)
        {
            HttpResponseMessage message = null; var error_message = string.Empty;
            if (id == null)
            {
                error_message = "error, this province could could not be deleted because of bad format"; message = Request.CreateResponse(HttpStatusCode.BadRequest, error_message); message.Headers.Date = DateTime.Now; return message;
            }

            bool flag = _prov.remove_by_id(id);

            if (!flag)
            { message = Request.CreateResponse(HttpStatusCode.NotFound, flag); message.Headers.Date = DateTime.Now; }
            else if (flag)
            { message = Request.CreateResponse(HttpStatusCode.OK, flag); message.Headers.Date = DateTime.Now; }
            else
            { error_message = "error, internal server error could not be processed further"; message = Request.CreateResponse(HttpStatusCode.InternalServerError, error_message); return message; }
            return message;
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage update_province(Province prov)
        {
            HttpResponseMessage message = null; var error_message = string.Empty; if (prov.ProvinceId == null) { error_message = "error, bad request could not be processed futher please check format"; message = Request.CreateResponse(HttpStatusCode.BadRequest, error_message); message.Headers.Date = DateTime.Now; return message; }
            bool flag = _prov.update(prov);
            if (!flag) { message = Request.CreateResponse(HttpStatusCode.NotFound, flag); message.Headers.Date = DateTime.Now; } else if (flag) { message = Request.CreateResponse(HttpStatusCode.OK, flag); message.Headers.Date = DateTime.Now; return message; } else { error_message = "error, internal server error could not be processed further"; message = Request.CreateResponse(HttpStatusCode.InternalServerError, error_message); message.Headers.Date = DateTime.Now; return message; }
            return message;
        }


        [Route("all")]
        [HttpGet]
        public HttpResponseMessage all_provinces()
        {
            HttpResponseMessage message = null;
            var error_message = string.Empty;
            List<Province> provinces = new List<Province>();

            provinces = _prov.all().ToList<Province>();
            if (provinces.Count() == 0)
            {
                message = Request.CreateResponse(HttpStatusCode.NotFound, provinces);
                message.Headers.Date = DateTime.Now;
            }
            else if (provinces.Count() > 0)
            {
                message = Request.CreateResponse(HttpStatusCode.OK, provinces);
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

            Province prov = _prov.show_by_id(id);

            if (prov == null)
            {
                message = Request.CreateResponse(HttpStatusCode.NotFound, prov);
                message.Headers.Date = DateTime.Now;
            }
            else if (prov != null)
            {
                message = Request.CreateResponse(HttpStatusCode.OK, prov);
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
