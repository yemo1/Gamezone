using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;
using System.Data.Entity;
using GameData;
using GameZone.Repositories;
using GameZone.Classes;

namespace GameZone.Controllers
{
    
    public class AuthController : ApiController
    {
        public void Post([FromBody]string value)
        {
        }

        [Route("Auth/{token}")]
        public IHttpActionResult Get(string token)
        {
            var subscriber = new Subscriber();
            var s = subscriber.GetUser(token);
            if (s == null)
            {
                //return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, "You do not have a valid subscription"));
                ///return ResponseMessage(GetMessage("You do not have a valid subscription", HttpStatusCode.NotFound, 400));
                return ResponseMessage(Request.CreateResponse(new ResponseStatus() { status = HttpStatusCode.NotFound, message = "You do not have a valid subscription"}));
            }

            //return ResponseMessage(GetMessage("Valid User", HttpStatusCode.OK, 200));
            return Ok(new ResponseStatus() { status = HttpStatusCode.OK, message = "Valid User" });
        }

        public HttpResponseMessage GetMessage(string message, HttpStatusCode code,int val)
        {
            HttpError custom = new HttpError(message) { { "status", val } };
            return Request.CreateResponse(code, custom);
        }
    }
}