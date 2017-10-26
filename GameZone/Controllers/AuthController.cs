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
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, "You do not have a valid subscription"));
            }
                
            return Ok("Valid User");
        }
    }
}