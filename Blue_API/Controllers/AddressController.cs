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

    [RoutePrefix("api/address")]
    [EnableCors(origins:"*", headers:"*", methods:"*",PreflightMaxAge =int.MaxValue)]
    public class AddressController : ApiController
    {
        private readonly IAddress _add;
        public AddressController(IAddress add)
        {
            _add = add;
        }

        [Route("update")]
        [HttpPut]
        public HttpResponseMessage update_address(Address ad)
        {
            HttpResponseMessage message = null;
            var error_message = string.Empty; 
            if (ad.AddressNumber == null)
            { error_message = "error, bad request could not be processed futher please check format"; 
                message = Request.CreateResponse(HttpStatusCode.BadRequest, error_message); message.Headers.Date = DateTime.Now; 
                return message;
            }
            bool flag = _add.update(ad);
            if (!flag) 
            { message = Request.CreateResponse(HttpStatusCode.NotFound, flag); message.Headers.Date = DateTime.Now; 
            } 
            else if 
                (flag) 
            { message = Request.CreateResponse(HttpStatusCode.OK, flag); message.Headers.Date = DateTime.Now; return message; 
            }
            else
            { error_message = "error, internal server error could not be processed further"; message = Request.CreateResponse(HttpStatusCode.InternalServerError, error_message); message.Headers.Date = DateTime.Now; return message; 
            }
            return message;
        }

       

    }
}
