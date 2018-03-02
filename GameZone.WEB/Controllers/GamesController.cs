using GameData;
using GameZone.Repositories;
using GameZone.TOOLS;
using GameZone.TOOLS.Enums;
using GameZone.VIEWMODEL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace GameZone.WEB.Controllers
{
    public class GamesController : Controller
    {
        SubscriberRepository subscriberRepository;
        string testFlutterwaveSecKey = "FLWSECK-62c555ca07f7a21adc144f757778a729-X";
        NGSubscriptionsEntities _NGSubscriptionsEntities;
        public static readonly log4net.ILog _Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static string svcName = System.Configuration.ConfigurationManager.AppSettings["SERVICE_NAME"].ToString();
        string logObj;

        HeaderController _HeaderController;
        public GamesController(HeaderController headerController)
        {
            _HeaderController = headerController;
            Entities.GameContext _context = new Entities.GameContext();
            _NGSubscriptionsEntities = new NGSubscriptionsEntities();
            subscriberRepository = new SubscriberRepository(_context, _NGSubscriptionsEntities);
        }
        public ActionResult Index(string phoneNo)
        {
            //Validate Login Credentials
            var loggedInUser = GameUserIdentity.LoggedInUser;
            if (loggedInUser == null)
            {
                return RedirectToAction("Index", "Home");
            }

            //Validate User Subscription
            var s = subscriberRepository.GetUserByPhoneNo(phoneNo);

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
        public ActionResult List(string t = null)
        {
            if (Request.UserAgent.Contains("Mobi") == true)
            {
                //mobile
                ViewBag.IsMobile = true;

                //Get Number from Header
                //NameValueCollection nvc = new NameValueCollection();
                //nvc = Request.Headers;
                //Dictionary<string, string> ss = new Dictionary<string, string>();
                //foreach (var item in nvc.AllKeys)
                //{
                //    ss.Add(item, nvc[item]);
                //}
                var headerData = _HeaderController.FillMSISDN();
                //if (!ss.ContainsKey("MSISDN"))
                if (headerData == null)
                {
                    //Not Mtn
                    ViewBag.mtnNumber = null;
                }
                else
                {
                    //Not Mtn
                    //ViewBag.mtnNumber = nvc.GetValues("MSISDN");
                    var mtnNumber = headerData.Lines.FirstOrDefault().Phone;
                    ViewBag.mtnNumber = (mtnNumber.Trim() == "XXX-XXXXXXXX") ? null : mtnNumber.Trim();
                    new Thread(() =>
                    {
                        LocalLogger.LogFileWrite(
                            JsonConvert.SerializeObject(new LogVM()
                            {
                                Message = "Recognised MTN Number",
                                LogData = mtnNumber
                            }));
                    }).Start();
                }
            }
            else
            {
                //laptop or desktop
                ViewBag.IsMobile = false;
                ViewBag.mtnNumber = null;
            }
            if (Session["fltwvSubscription"] != null)
            {
                ViewBag.fltwvSubscription = Session["fltwvSubscription"].ToString();
                //Response.Write($"<script language='javascript' type='text/javascript'>alert('{Session["fltwvSubscription"].ToString()}');</script>");
            }
            Session["fltwvSubscription"] = null;

            //Just for test of Auto Registration
            //ViewBag.IsMobile = true;
            ViewBag.mtnNumber = "2348168423222";
            return View();
        }

        [HttpGet]
        public ActionResult SubResponse(string resp = null)
        {
            try
            {
                //Get User ID
                //Put Valid Login User Data in Session
                var userData = (LoginAppUserVM)GameUserIdentity.LoggedInUser;
                if (userData == null)
                {
                    Session["fltwvSubscription"] = "Session timed out. Please try again.";
                    return RedirectToAction("Index", "Home");
                }
                //Check for valid Exisiting Subscription
                var subscriptionConfirm = _NGSubscriptionsEntities.ConfirmAppUserSubscription(userData.AppUserId, null, svcName, null,null, false).FirstOrDefault();
                //Valid Subscription Exists
                if (subscriptionConfirm.isSuccess)
                {
                    Session["fltwvSubscription"] = "You already have an active Subscription.";
                    return RedirectToAction("Index", "Home");
                }
                logObj = JsonConvert.SerializeObject(new LogVM()
                {
                    Message = "User Making Payment",
                    LogData = userData
                });
                new Thread(() =>
                {
                    _Log.Info(logObj);
                }).Start();

                var responseJSON = JsonConvert.DeserializeObject<FlutterWaveJSONVM>(resp);

                #region Confirm Payment
                var data = new { flw_ref = responseJSON.tx.flwRef, SECKEY = testFlutterwaveSecKey, normalize = "1" };
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var responseMessage = client.PostAsJsonAsync("http://flw-pms-dev.eu-west-1.elasticbeanstalk.com/flwv3-pug/getpaidx/api/verify", data).Result;
                var responseStr = responseMessage.Content.ReadAsStringAsync().Result;
                var verifyRspJSON = JsonConvert.DeserializeObject<FlutterWavePayVerifyJSONVM>(responseStr);

                #region Log
                logObj = JsonConvert.SerializeObject(new LogVM()
                {
                    Message = "Payment Verification Result",
                    LogData = verifyRspJSON.data
                });
                new Thread(() =>
                {
                    _Log.Info(logObj);
                }).Start();
                #endregion
                #region Verify Payment was really successful

                //Verify Success Status, Amount Paid and chargeResponse
                if (verifyRspJSON.data.status.ToLower() != "successful" && verifyRspJSON.data.flwMeta.chargeResponse != "00" || verifyRspJSON.data.status.ToLower() != "successful" && verifyRspJSON.data.flwMeta.chargeResponse != "0")
                {
                    //Transaction Failed
                    Session["fltwvSubscription"] = "Sorry. Your subscription failed. Please try again later.";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    int subscriptionPeriod = 0;
                    //Daily Subscription
                    if (verifyRspJSON.data.amount >= (int)GameZonePrice.Daily && verifyRspJSON.data.amount < (int)GameZonePrice.Weekly)
                    {
                        subscriptionPeriod = (int)GameZonePrice.Daily;
                    }
                    //Weekly Subscription
                    if (verifyRspJSON.data.amount >= (int)GameZonePrice.Weekly && verifyRspJSON.data.amount < (int)GameZonePrice.Monthly)
                    {
                        subscriptionPeriod = (int)GameZonePrice.Weekly;
                    }
                    //Weekly Subscription
                    if (verifyRspJSON.data.amount >= (int)GameZonePrice.Monthly)
                    {
                        subscriptionPeriod = (int)GameZonePrice.Monthly;
                    }
                    DateTime periodEnd = (subscriptionPeriod == (int)GameZonePrice.Daily) ? DateTime.Now.AddDays(1) :
                                            (subscriptionPeriod == (int)GameZonePrice.Weekly) ? DateTime.Now.AddDays(7) :
                                            DateTime.Now.AddDays(30);
                    
                    //Save Subscription Data in DB
                    var rezolt = _NGSubscriptionsEntities.AddServiceSubscription(userData.AppUserId, svcName, Enum.GetName(typeof(GameZonePrice), subscriptionPeriod), DateTime.Now, periodEnd, verifyRspJSON.data.amount, true, true, DateTime.Now).FirstOrDefault();
                    Session["fltwvSubscription"] = "Your subscription was successful.";
                    //ViewBag.subscriptionSuccessful = "Your subscription was successful.";
                    return RedirectToAction("Index", "Home");
                }
                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                #region Log
                logObj = JsonConvert.SerializeObject(new LogVM()
                {
                    Message = "Exception",
                    LogData = ex
                });
                new Thread(() =>
                {
                    _Log.Error(logObj);
                }).Start();
                #endregion
                throw ex;
            }
        }
        public ActionResult GamePlay()
        {
            //Validate Login Credentials
            //var loggedInUser = GameUserIdentity.LoggedInUser;
            //if (loggedInUser == null)
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