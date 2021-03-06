﻿using System.Linq;
using System.Web.Mvc;
using GameZone.VIEWMODEL;
using GameZone.TOOLS;
using GameZone.Repositories;

namespace GameZone.WEB.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        IAppUserRepository _IAppUserRepository;
        
        public AccountController(IAppUserRepository iAppUserRepository)
        {
            _IAppUserRepository = iAppUserRepository;
        }

        //public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        //{
        //    UserManager = userManager;
        //    SignInManager = signInManager;
        //}

        [HttpPost]
        [AllowAnonymous]
        public void StartValidUserSession(LoginAppUserVM loginAppUserVM)
        {
            if (loginAppUserVM != null)
            {
                //Put Valid Login User Data in Session
                GameUserIdentity.LoggedInUser = loginAppUserVM;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public string AuthUserToken(string redirectURL = null)
        {
            System.Collections.Specialized.NameValueCollection nvc = new System.Collections.Specialized.NameValueCollection();
            nvc = Request.Headers;
            System.Collections.Generic.Dictionary<string, string> ss = new System.Collections.Generic.Dictionary<string, string>();
            foreach (var item in nvc.AllKeys)
            {
                ss.Add(item, nvc[item]);
            }

            //Check for Authentication Token in Request Header
            if (ss.ContainsKey("authToken") && !string.IsNullOrEmpty(ss["authToken"].ToString()))
            {
                string headerToken = ss["authToken"].ToString();
                //Validate User and Token
                string appUserID = headerToken.Split(';')[0], userToken = headerToken.Split(';')[1];
                var retVal = _IAppUserRepository.ConfirmAppUserToken(userToken, long.Parse(appUserID));
                //User and Token Match
                if (retVal.isSuccess)
                {
                    return redirectURL ?? "/Home/Index";
                }
                else
                {
                    return "/Home/Index;Access Denied";
                }
            }
            else
            {
                return "/Home/Index;Access Denied";
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public bool ValidSubscription(long UID, string MSISDN, string svcName, string Shortcode, bool IsMtn, string Productcode = null)
        {
            string serviceShortcode = System.Configuration.ConfigurationManager.AppSettings["SERVICE_SHORTCODE"].ToString();
            return _IAppUserRepository.ConfirmUserSubscription(UID, MSISDN, svcName, serviceShortcode, Productcode, IsMtn).isSuccess;
        }

        // POST: /Account/LogOff
        [HttpPost]
        [AllowAnonymous]
        public string LogOff(string UID)
        {
            if (string.IsNullOrEmpty(UID))
            {
                return "";
            }
            _IAppUserRepository.LoginAppUser(UID, false, null);
            return "/Home";
        }
        
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            bool isMobi = false;
            if (Request.UserAgent.Contains("Mobi") == true)
            {
                isMobi = true;
            }
            ViewBag.IsMobile = isMobi;
            var mtnNumberSession = System.Web.HttpContext.Current.Session["mtnNumber"];
            var hedaData = mtnNumberSession != null ? (MSISDNRepository)mtnNumberSession : null;
            string mtnNumber = null;
            if (hedaData != null)
            {
                mtnNumber = (hedaData.Lines.FirstOrDefault().Phone.Trim() == "XXX-XXXXXXXX") ? null : hedaData.Lines.FirstOrDefault().Phone.Trim();
            }

            ViewBag.mtnNumber = mtnNumber;
            return View(returnUrl);
        }
        
        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            bool isMobi = false;
            if (Request.UserAgent.Contains("Mobi") == true)
            {
                isMobi = true;
            }
            ViewBag.IsMobile = isMobi;
            var mtnNumberSession = System.Web.HttpContext.Current.Session["mtnNumber"];
            var hedaData = mtnNumberSession != null ? (MSISDNRepository)mtnNumberSession : null;
            string mtnNumber = null;
            if (hedaData != null)
            {
                mtnNumber = (hedaData.Lines.FirstOrDefault().Phone.Trim() == "XXX-XXXXXXXX") ? null : hedaData.Lines.FirstOrDefault().Phone.Trim();
            }

            ViewBag.mtnNumber = mtnNumber;
            return View();
        }
        
        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            bool isMobi = false;
            if (Request.UserAgent.Contains("Mobi") == true)
            {
                isMobi = true;
            }
            ViewBag.IsMobile = isMobi;
            var mtnNumberSession = System.Web.HttpContext.Current.Session["mtnNumber"];
            var hedaData = mtnNumberSession != null ? (MSISDNRepository)mtnNumberSession : null;
            string mtnNumber = null;
            if (hedaData != null)
            {
                mtnNumber = (hedaData.Lines.FirstOrDefault().Phone.Trim() == "XXX-XXXXXXXX") ? null : hedaData.Lines.FirstOrDefault().Phone.Trim();
            }

            ViewBag.mtnNumber = mtnNumber;
            return View();
        }
        
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            bool isMobi = false;
            if (Request.UserAgent.Contains("Mobi") == true)
            {
                isMobi = true;
            }
            ViewBag.IsMobile = isMobi;
            var mtnNumberSession = System.Web.HttpContext.Current.Session["mtnNumber"];
            var hedaData = mtnNumberSession != null ? (MSISDNRepository)mtnNumberSession : null;
            string mtnNumber = null;
            if (hedaData != null)
            {
                mtnNumber = (hedaData.Lines.FirstOrDefault().Phone.Trim() == "XXX-XXXXXXXX") ? null : hedaData.Lines.FirstOrDefault().Phone.Trim();
            }

            ViewBag.mtnNumber = mtnNumber;
            return View();
        }        
    }
}