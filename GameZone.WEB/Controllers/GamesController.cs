using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameZone.WEB.Controllers
{
    public class GamesController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}