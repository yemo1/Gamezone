using System.Web.Mvc;

namespace GameZone.WEB.Controllers
{
    public class HomeController : Controller
    {        
        public ActionResult Index()
        {
            var request = ControllerContext.HttpContext.Request;

            //if (request.Browser.IsMobileDevice)
            if(Request.UserAgent.Contains("Mobi") == true)
            {
                //mobile
                ViewBag.IsMobile = true;
            }
            else
            {
                //laptop or desktop
                ViewBag.IsMobile = false;
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Games()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}