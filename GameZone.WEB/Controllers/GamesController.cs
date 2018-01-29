using GameData;
using GameZone.Repositories;
using GameZone.VIEWMODEL;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace GameZone.WEB.Controllers
{
    public class GamesController : Controller
    {
        public ActionResult Index(string phoneNo)
        {
            Entities.GameContext _context = new Entities.GameContext();
            NGSubscriptionsEntities _NGSubscriptionsEntities = new NGSubscriptionsEntities();
            var subscriber = new Subscriber(_context, _NGSubscriptionsEntities);
            var s = subscriber.GetUserByPhoneNo(phoneNo);
            if (s == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// This is the view to display complete list of games
        /// with menu to select categories
        /// </summary>
        /// <param name="t">This is the Telephone Number Passed</param>
        /// <returns></returns>
        /// 
        public ActionResult List(string t = null)
        {

            NameValueCollection nvc = new NameValueCollection();
            nvc = Request.Headers;
            Dictionary<string, string> ss = new Dictionary<string, string>();
            foreach (var item in nvc.AllKeys)
            {
                ss.Add(item, nvc[item]);
            }

            //Validate User Session
            //if (GameUserIdentity.LoggedInUser == null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            //Entities.GameContext _context = new Entities.GameContext();
            //NGSubscriptionsEntities _NGSubscriptionsEntities = new NGSubscriptionsEntities();
            //var subscriber = new Subscriber(_context, _NGSubscriptionsEntities);

            ////Check for subscription Expiry

            ////Check for Wrong Date and Time

            //var s = subscriber.GetUserByPhoneNo(t);
            //if (s == null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            //else // Access Granted
            //{
            //    //Implement Caching of last game category selected
            //    GameUserIdentity.LoggedInUser = s.ToModel();
            //subscriber.UpdateGameUserLastAccess(t, s);
            return View();
            //}
        }
        public ActionResult GamePlay()
        {
            ////Validate User Session
            //if (GameUserIdentity.LoggedInUser == null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            //Entities.GameContext _context = new Entities.GameContext();
            //NGSubscriptionsEntities _NGSubscriptionsEntities = new NGSubscriptionsEntities();
            //var subscriber = new Subscriber(_context, _NGSubscriptionsEntities);

            ////Check for subscription Expiry

            ////Check for Wrong Date and Time
            //string userTel = GameUserIdentity.LoggedInUser.MSISDN;
            //var s = subscriber.GetUserByPhoneNo(userTel);
            //if (s == null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            //else
            //{
            //    //Keep Track of User Last Game Played
            //    //GameUserIdentity.LoggedInUser = s.ToModel();
            //    //subscriber.UpdateGameUserLastAccess(userTel, s);
            
            //Make Game Appear Full Screen on Mobile Devices
            if (Request.UserAgent.Contains("Mobi") == true)
            {
                //mobile
                return RedirectToAction("GamePlayFull", "Games");
            }
            return View();
            //}
        }

        public ActionResult GamePlayFull()
        {
            ////Validate User Session
            //if (GameUserIdentity.LoggedInUser == null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            //Entities.GameContext _context = new Entities.GameContext();
            //NGSubscriptionsEntities _NGSubscriptionsEntities = new NGSubscriptionsEntities();
            //var subscriber = new Subscriber(_context, _NGSubscriptionsEntities);

            ////Check for subscription Expiry

            ////Check for Wrong Date and Time
            //string userTel = GameUserIdentity.LoggedInUser.MSISDN;
            //var s = subscriber.GetUserByPhoneNo(userTel);
            //if (s == null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            //else
            //{
            //    //Keep Track of User Last Game Played
            //    //GameUserIdentity.LoggedInUser = s.ToModel();
            //    //subscriber.UpdateGameUserLastAccess(userTel, s);
            return View();
            //}
        }

        public ActionResult EchoTest()
        {
            var retVal = EchoTestResult();
            ViewBag.retVal = retVal;
            return View();
        }

        // POST: FileUploader/UploadUserImage
        [HttpPost, Route("Games/EchoTestResult")]
        public string EchoTestResult()
        {
            try
            {
                NameValueCollection nvc = new NameValueCollection();
                nvc = Request.Headers;
                Dictionary<string, string> ss = new Dictionary<string, string>();
                foreach (var item in nvc.AllKeys)
                {
                    ss.Add(item, nvc[item]);
                }             

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string jsonString = serializer.Serialize(ss);
                
                return jsonString;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}