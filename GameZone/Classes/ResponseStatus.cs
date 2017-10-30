using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace GameZone.Classes
{
    public class ResponseStatus
    {
        public HttpStatusCode status { get; set; }
        public string message { get; set; }
    }
}