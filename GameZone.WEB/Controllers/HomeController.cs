using GameData;
using GameZone.VIEWMODEL;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;

namespace GameZone.WEB.Controllers
{
    public class HomeController : Controller
    {
        GameData.NGSubscriptionsEntities _NGSubscriptionsEntities;

        public HomeController()
        {
            _NGSubscriptionsEntities = new GameData.NGSubscriptionsEntities();
        }
        public ActionResult Index(string retVal = "")
        {
            //#region WEB DOI Implementation
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
            //#endregion

            var request = ControllerContext.HttpContext.Request;

            //if (request.Browser.IsMobileDevice)
            if (Request.UserAgent.Contains("Mobi") == true)
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
    }
}