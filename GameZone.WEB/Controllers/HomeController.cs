﻿using GameData;
using GameZone.Repositories;
using GameZone.TOOLS;
using GameZone.TOOLS.Enums;
using GameZone.VIEWMODEL;
using GameZone.WEB.Mappings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Web.Mvc;

namespace GameZone.WEB.Controllers
{
    public class HomeController : Controller
    {
        GameData.NGSubscriptionsEntities _NGSubscriptionsEntities;
        MSISDNRepository _MSISDNRepository;
        HeaderController _HeaderController;
        private readonly IServiceRequestRepository _IServiceRequestRepository;
        private readonly IServiceHeaderRepository _IServiceHeaderRpository;
        static TOOLS.WapHeaderUtil _WapHeaderUtil;
        public static readonly log4net.ILog _Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string svcName = System.Configuration.ConfigurationManager.AppSettings["SERVICE_NAME"].ToString();
        static string serviceShortcode = System.Configuration.ConfigurationManager.AppSettings["SERVICE_SHORTCODE"].ToString();
        public HomeController(IServiceHeaderRepository ServiceHeader, MSISDNRepository mSISDNRepository, HeaderController headerController, IServiceRequestRepository serviceRequestRepository, TOOLS.WapHeaderUtil wapHeaderUtil)
        {
            _IServiceHeaderRpository = ServiceHeader;
            _IServiceRequestRepository = serviceRequestRepository;
            _HeaderController = headerController;
            _MSISDNRepository = mSISDNRepository;
            _NGSubscriptionsEntities = new GameData.NGSubscriptionsEntities();
            _WapHeaderUtil = wapHeaderUtil;
        }
        public ActionResult Index(string retVal = null)
        {
            #region WEB DOI Implementation
            //ReturnMessage returnMessage;
            //NameValueCollection nvc = new NameValueCollection();
            //nvc = Request.Headers;
            //Dictionary<string, string> ss = new Dictionary<string, string>();
            //foreach (var item in nvc.AllKeys)
            //{
            //    ss.Add(item, nvc[item]);
            //}
            //#region Successful Web DOI Subscription

            ////Increment IDSeries
            //var updateResult_CCCC = _NGSubscriptionsEntities.UpdateIDSeries("WebDOITransactionID-CCCC").FirstOrDefault();
            //var updateResult_DDD = _NGSubscriptionsEntities.UpdateIDSeries("WebDOITransactionID-DDD").FirstOrDefault();
            //if (!updateResult_CCCC.isSuccess)
            //{
            //    returnMessage = new ReturnMessage
            //    {
            //        Success = false,
            //        Message = updateResult_CCCC.message
            //    };
            //    ViewBag.ReturnMessage = returnMessage;
            //    return View();
            //}
            //else if (!updateResult_DDD.isSuccess)
            //{
            //    returnMessage = new ReturnMessage
            //    {
            //        Success = false,
            //        Message = updateResult_DDD.message
            //    };
            //    ViewBag.ReturnMessage = returnMessage;
            //    return View();
            //}
            //#endregion
            //if (ss.ContainsKey("MSISDN"))
            //{
            //    //Not Mtn
            //    ViewBag.IsMTN = false;
            //}
            #endregion

            //if (request.Browser.IsMobileDevice)
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
            ViewBag.mtnNumber = "2348147911707";
            return View();
        }

        public ActionResult Subscription(string msisdn, bool go, bool mobi, string heda, bool frmGame)
        {
            //AppUserController AUC = new AppUserController();
            //AppUserVM AUVM = new AppUserVM();
            //AUC.Post();

            ViewBag.IsMobile = false;
            ViewBag.mtnNumber = null;
            bool isMobi = false;
            ViewBag.Go = go;
            ViewBag.frmGame = frmGame; 
            if (Request.UserAgent.Contains("Mobi") == true)
            {
                isMobi = true;
            }
            if (go && !isMobi)
            {
                ViewBag.responseMSG = "Sorry. Unrecognised Mobile Device.";
                ViewBag.IsMobile = isMobi;
                ViewBag.mtnNumber = null;
                return View();
            }
            isMobi = true;
            if (go && string.IsNullOrEmpty(msisdn))
            {
                ViewBag.mtnNumber = null;
                ViewBag.IsMobile = isMobi;
                ViewBag.responseMSG = "Sorry. Could not verify your phone number.";
                return View();
            }
            if (go && !string.IsNullOrEmpty(msisdn) && isMobi)
            {
                //mobile
                //ViewBag.IsMobile = true;
                //var headerData = _HeaderController.FillMSISDN();

                //if (headerData == null)
                //{
                //    //Not Mtn
                //    ViewBag.mtnNumber = null;
                //}
                //else
                //{
                //Not Mtn
                //var mtnNumber = headerData.Lines.FirstOrDefault().Phone;
                //ViewBag.mtnNumber = (mtnNumber.Trim() == "XXX-XXXXXXXX") ? null : mtnNumber.Trim();

                //Valid MTN Number
                //Initiate USSD Subscription
                new Thread(() =>
                {
                    LocalLogger.LogFileWrite(
                        JsonConvert.SerializeObject(new LogVM()
                        {
                            Message = "Initiating USSD Subscription",
                            LogData = msisdn
                        }));
                }).Start();
                ViewBag.mtnNumber = msisdn;
                ViewBag.IsMobile = isMobi;
                ViewBag.responseMSG = MTNUSSDSubscription(msisdn, true, serviceShortcode, null, heda);
            }
            //Just for test of Auto Registration
            //ViewBag.IsMobile = true;
            //ViewBag.mtnNumber = "2348168423222";

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

        /// <summary>
        /// Function to Execute WEB DOI
        /// </summary>
        /// <returns></returns>
        public ActionResult MobileSubscription()
        {
            ReturnMessage returnMessage;
            //Confirm Mobile
            if (Request.UserAgent.Contains("Mobi") == true)
            {
                //mobile
                ViewBag.IsMobile = true;

                //Get User MSISDN
                NameValueCollection nvc = new NameValueCollection();
                nvc = Request.Headers;
                Dictionary<string, string> ss = new Dictionary<string, string>();
                foreach (var item in nvc.AllKeys)
                {
                    ss.Add(item, nvc[item]);
                }
                //Check if MSISDN (Phone No) Exists in the Header
                if (ss.ContainsKey("MSISDN"))
                {
                    //Ensure the Key Holding the Phone No isn't empty
                    if (!string.IsNullOrEmpty(ss["MSISDN"]))
                    {
                        Random ran = new Random(1000000); ;
                        string Nonce = "";
                        for (int i = 0; i < 3; i++)
                        {
                            Nonce += ran.Next(900000000).ToString();
                        }

                        #region Transaction ID Generation
                        //The transaction ID is in the following format:
                        //AAAAAAAXXXXBBBBBBBBBBBBCCCCDDD
                        //In the format:
                        //AAAAAAA: The fixed value is 0110000.
                        //XXXX: sum of the four octets of a node IP address if a
                        //component is deployed as a cluster.Assume that the
                        //IP address of a node is 192.168.10.2.The value of
                        //XXXX is 0372.
                        //Gamezone IP (72.3.183.215) = 0473
                        //BBBBBBBBBBBB: time stamp. The format is as
                        //follows: YYMMDDHHMMSS
                        //YY indicates the year(for example, 09), MM indicates
                        //the month (for example, 08), DD indicates the day (for
                        //example, 31), HH indicates the hour (for example, 13),
                        //MM indicates the minute (for example, 31), and SS
                        //indicates the second(for example, 31).
                        //CCCC: sequence number. The value ranges from
                        //0001 to 9999.
                        //DDD: message sequence number.The value ranges
                        //from 001 to 999.
                        //[Example]
                        //100001200101110623021721000011
                        #endregion


                        #region CCCC Formatting
                        var CCCC = _NGSubscriptionsEntities.GetIDSeriesNextValue("WebDOITransactionID-CCCC").FirstOrDefault();
                        int ccSequence = 0;
                        if (!CCCC.isSuccess)
                        {
                            returnMessage = new ReturnMessage
                            {
                                Success = false,
                                Message = CCCC.message
                            };
                            ViewBag.ReturnMessage = returnMessage;
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ccSequence = int.Parse(CCCC.message);
                        }
                        //Putting CCC format in 1000's
                        string completeCCCC = (ccSequence > 99 && ccSequence < 1000) ? $"0{ccSequence}" :
                            (ccSequence > 9 && ccSequence < 100) ? $"00{ccSequence}" :
                            (ccSequence > 0 && ccSequence < 10) ? $"000{ccSequence}" : ccSequence.ToString();
                        #endregion

                        #region DDD Formatting
                        var DDD = _NGSubscriptionsEntities.GetIDSeriesNextValue("WebDOITransactionID-DDD").FirstOrDefault();
                        int ddSequence = 0;
                        if (!DDD.isSuccess)
                        {
                            returnMessage = new ReturnMessage
                            {
                                Success = false,
                                Message = DDD.message
                            };
                            ViewBag.ReturnMessage = returnMessage;
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ddSequence = int.Parse(CCCC.message);
                        }
                        //Putting CCC format in 100's
                        string completeDDD = (ddSequence > 9 && ddSequence < 100) ? $"0{ddSequence}" :
                            (ddSequence > 0 && ddSequence < 10) ? $"00{ddSequence}" : ddSequence.ToString();
                        #endregion

                        string transactionId = $"{"0110000"}{"0473"}{DateTime.Now.ToString()}{completeCCCC}{completeDDD}";
                        //Redirect to MTN Double OPT In (DOI) URL
                        string parameterWithValues = $"X-MSISDN={ss["MSISDN"].ToString()}&spAccount=funmobile&spPasswd=bmeB500&Nonce={Nonce}&Created{DateTime.UtcNow}&endUserIdentifier={ss["MSISDN"].ToString()}&Scope=99&amount=20&transactionId={transactionId}&currency=N&contentId=null&frequency=0&description=null&productName=GameZone&totalAmount=0&notificationURL={"http://localhost:17683/"}&tokenValidity=null&tokenType=null&serviceInterval=24&serviceIntervalUnit=null";
                        Redirect($"{"http://41.206.4.159/portalone/otp/nigeria?"}{parameterWithValues}");

                        //"endUserIdentifier=2348147911707&amount=2000&currency=N&contentId=null&frequency=0&description=null&productName=SoccerPro.mobi&totalAmount=0&notificationUrl=http%3A%2F%2Fhyvesdp.com%2Fcallback%2Fmtnsdpng%2Fsubscription%2Fauthorization%2F185491171&tokenValidity=null&tokenType=null&serviceInterval=24&serviceIntervalUnit=1&transactionId=234011000589920180205165130185491171&productId=23401220000022404&nonce=201802051651308213&created=2018-02-05T16:51:30.7200Z&convertedFrequency=One-off&partnerId=2340110005899&partnerName=CELLFIND&carrierId=23401&country=nigeria&lang=en&discountedPrice=N20.0+%2F+24+hours&actualPrice=N20.0+%2F+24+hours&discountApplicability=-1&freePeriod=&pageExpireTime=6&SPACCOUNT=2340110005899&serviceID=234012000019291&SPPASSWD=B6uOeO7ITIlflH7dIhmBAIQIEGL5L1%2FLuff8uhIdd1c%3D"
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                //laptop or desktop
                ViewBag.IsMobile = false;
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public string MTNUSSDSubscription(string MSISDN, bool IsMtn, string Shortcode, string Productcode = null, string headerId = "")
        {
            if (string.IsNullOrEmpty(MSISDN))
            {
                return JsonConvert.SerializeObject(new ReturnMessage()
                {
                    Success = false,
                    Message = "Subscription failed. \n Could not validate your phone number."
                });
            }
            MSISDNRepository msisdn = new MSISDNRepository();
            try
            {
                if (MSISDN.StartsWith("0"))
                    MSISDN = "234" + MSISDN.TrimStart('0');

                var subscriptionConfirm = _NGSubscriptionsEntities.ConfirmAppUserSubscription(0, MSISDN, null, Shortcode, Productcode, IsMtn).FirstOrDefault();
                //Valid Subscription Exists
                if (subscriptionConfirm.isSuccess)
                {
                    return JsonConvert.SerializeObject(new ReturnMessage()
                    {
                        Success = false,
                        Message = "You already have an active Subscription."
                    });
                }

                msisdn = (MSISDNRepository)Session["XMSISDN"];

                if ((MSISDNRepository)Session["XMSISDN"] != null)
                {
                    msisdn = (MSISDNRepository)Session["XMSISDN"];
                }
                else
                {
                    msisdn = _HeaderController.FillMSISDN();
                }

                //For test
                //msisdn.Lines.FirstOrDefault().Phone = "2348147911707";
                //msisdn.Lines.FirstOrDefault().IsHeader = true;
                //msisdn.Lines.FirstOrDefault().IpAddress = "192.168.10.212";
                //For test

                var ipthis = msisdn.Lines.FirstOrDefault().IpAddress;
                msisdn.Clear();
                msisdn.AddItem(MSISDN, ipthis, true);
                var newmsisdn = (MSISDNRepository)Session["XMSISDN"];
                HttpContext.Session["XMSISDN"] = msisdn;

                if (msisdn != null && msisdn.Lines.Count() > 0 && msisdn.Lines.FirstOrDefault().Phone != "XXX-XXXXXXXX")
                {
                    var subResault = _IServiceRequestRepository.Subscribe(Convert.ToInt32(headerId), msisdn.Lines.FirstOrDefault().IpAddress, msisdn.Lines.FirstOrDefault().Phone, msisdn.Lines.FirstOrDefault().IsHeader);
                    //ServiceHeaders serviceHeaders = new ServiceHeaders();
                    if (subResault.Success)
                    {
                        //serviceHeaders = (ServiceHeaders)subResault.Data;
                        new Thread(() =>
                        {
                            LocalLogger.LogFileWrite(
                                JsonConvert.SerializeObject(new LogVM()
                                {
                                    LogData = subResault
                                }));
                        }).Start();

                        return JsonConvert.SerializeObject(subResault);
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(new ReturnMessage()
                        {
                            Success = true,
                            Message = $"Subscription failed. \n {subResault.Message}"
                        });
                    }
                }
                else
                {
                    //var header = _IServiceHeaderRpository.GetServiceHeader(Convert.ToInt32(headerId));
                    //header.Category = category;
                    //var model = new HeaderVM();
                    //model.header = header.ToModel();
                    //return View(model);
                    return JsonConvert.SerializeObject(new ReturnMessage()
                    {
                        Success = false,
                        Message = "Subscription failed. \n Could not validate your phone number."
                    });
                }
            }
            catch (Exception ex)
            {
                new Thread(() =>
                {
                    LocalLogger.LogFileWrite(ex.Message);
                }).Start();

                return JsonConvert.SerializeObject(new ReturnMessage()
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet]
        public string GetHeaderByServiceName(string serviceName)
        {
            var serviceHeda = _IServiceHeaderRpository.GetServiceHeaderByServiceName(serviceName).Select(x => x.ToModel()).ToList();
            return JsonConvert.SerializeObject((List<ServiceHeaderVM>)serviceHeda);
        }
        public ActionResult AddSubscription(string textPhone, string category, string headerId)
        {
            if (!string.IsNullOrEmpty(textPhone))
            {
                if (textPhone.StartsWith("0"))
                    textPhone = "234" + textPhone.TrimStart('0');
                var msisdn = new MSISDNRepository();
                msisdn = (MSISDNRepository)Session["XMSISDN"];
                //if (msisdn == null)
                //    msisdn = FillMSISDN();
                //13/02/2017

                if ((MSISDNRepository)Session["XMSISDN"] != null)
                {
                    msisdn = (MSISDNRepository)Session["XMSISDN"];
                }
                else
                {
                    msisdn = _HeaderController.FillMSISDN();
                }

                var ipthis = msisdn.Lines.FirstOrDefault().IpAddress;
                msisdn.Clear();
                msisdn.AddItem(textPhone, ipthis, false);
                var newmsisdn = (MSISDNRepository)Session["XMSISDN"];
                HttpContext.Session["XMSISDN"] = msisdn;

                try
                {
                    if (msisdn != null && msisdn.Lines.Count() > 0 && msisdn.Lines.FirstOrDefault().Phone != "XXX-XXXXXXXX")

                        _IServiceRequestRepository.Subscribe(Convert.ToInt32(headerId), msisdn.Lines.FirstOrDefault().IpAddress, msisdn.Lines.FirstOrDefault().Phone, msisdn.Lines.FirstOrDefault().IsHeader);

                    //to return manual numbers here
                    else
                        return Redirect(Url.Action("Fill", new { category = category, headerId = headerId }));

                    var newmsisdn1 = (MSISDNRepository)Session["XMSISDN"];

                }
                catch (Exception ex)
                {
                    LocalLogger.LogFileWrite(ex.Message);
                }
                if (string.IsNullOrEmpty(category)) category = "Home";
                return Redirect(Url.Action("Index", new { controller = category, action = "Index" }));
            }
            return Redirect(Request.Url.PathAndQuery);
        }
    }
}