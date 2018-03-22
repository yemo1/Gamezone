using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Web.Routing;
using System.Threading;
using GameZone.TOOLS;
using GameZone.Repositories;
using Newtonsoft.Json;

namespace GameZone.WEB.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// Method to pop uo notification on the browser
        /// </summary>
        /// <param name="notificationType"></param>
        /// <param name="message"></param>
        public void PopUp(int notificationType, string message)
        {
            HttpContext.Session.Add("DisplayMessage", message);
            HttpContext.Session.Add("DisplayType", notificationType);
        }

        //protected override void Initialize()
        //{

        //}

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            System.Web.HttpContext.Current.Session["mtnNumber"] = null;
            MSISDNRepository headerData = System.Web.HttpContext.Current.Session["mtnNumber"] != null ? (MSISDNRepository)System.Web.HttpContext.Current.Session["mtnNumber"] : null;
            if (headerData == null)
            {
                headerData = FillMSISDN();
                if (headerData != null)
                {
                    new Thread(() =>
                    {
                        LocalLogger.LogFileWrite(
                            JsonConvert.SerializeObject(new VIEWMODEL.LogVM()
                            {
                                Message = "Recognised Wap Header",
                                LogData = headerData
                            }));
                    }).Start();
                }
            }
            System.Web.HttpContext.Current.Session["mtnNumber"] = headerData;
            ////===========================
            //// If session exists 
            //if (filterContext.HttpContext.Session == null) return;
            ////if Not new session 
            //if (!filterContext.HttpContext.Session.IsNewSession) return;
            //var cookie = filterContext.HttpContext.Request.Headers["Cookie"];
            ////if not cookie exists or sessionid index is not greater than zero 
            //if ((cookie == null) || (cookie.IndexOf("ASP.NET_SessionId", StringComparison.Ordinal) < 0)) return;
            //if (filterContext.HttpContext.Request.IsAjaxRequest())
            //{
            //    filterContext.Result = RedirectToAction("SessionExpired", "Home");
            //}
            //else
            //{
            //    filterContext.HttpContext.Session.Clear(); filterContext.Result = RedirectToAction("SessionExpired", "Home");
            //}
        }

        private RedirectToRouteResult _NoAccessRights() { return RedirectToAction("NoAccessRight", "Home"); }

        public static RedirectToRouteResult NoAccessRights() { var app = new BaseController(); return app._NoAccessRights(); }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }

        private MSISDNRepository FillMSISDN()
        {
            MSISDNRepository msisdntory = new MSISDNRepository();
            WapHeaderUtil _WapHeaderUtil = new WapHeaderUtil();
            msisdntory.Clear();

            string msisdn = _WapHeaderUtil.MSISDN_HEADER();
            if (msisdn == "XXX-XXXXXXXX")
            {
                msisdntory.AddItem(msisdn, WapHeaderUtil.GetIPAddress(), false);
            }
            else
            {
                msisdntory.AddItem(msisdn, WapHeaderUtil.GetIPAddress());
            }
            return msisdntory;
        }
    }
}