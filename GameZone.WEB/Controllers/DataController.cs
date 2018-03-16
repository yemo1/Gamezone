using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using GameZone.WEB.Models;
using GameZone.VIEWMODEL;
using GameZone.TOOLS;
using GameZone.Repositories;
using System.Data.SqlClient;

namespace GameZone.WEB.Controllers
{
    public class DataController : Controller
    {
        GameData.NGSubscriptionsEntities _NGSubscriptionsEntities = new GameData.NGSubscriptionsEntities();
        string conString = System.Configuration.ConfigurationManager.ConnectionStrings["NGSubscriptionsConnectionString"].ConnectionString;
        public DataController()
        {

        }
        [HttpPost]
        public void StartValidUserSession(LoginAppUserVM loginAppUserVM)
        {
            if (loginAppUserVM != null)
            {
                //Put Valid Login User Data in Session
                GameUserIdentity.LoggedInUser = loginAppUserVM;
            }
        }

        [HttpPost]
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
                var retVal = _NGSubscriptionsEntities.ConfirmAppUserLoginToken(long.Parse(appUserID), null, userToken).FirstOrDefault();
                //_IAppUserRepository.ConfirmAppUserToken(userToken, long.Parse(appUserID));
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

        [HttpGet]
        public bool ValidSubscription(long UID, string MSISDN, string svcName, string Shortcode, bool IsMtn, string Productcode = null)
        {
            bool retVal = false;
            using (SqlConnection connection = new SqlConnection(conString))
            {
                try
                {
                    if (connection.State == System.Data.ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    System.Data.DataTable dt = new System.Data.DataTable();
                    using (var cmd = new SqlCommand("ConfirmAppUserSubscription", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@AppUserId", UID);
                        cmd.Parameters.AddWithValue("@MSISDN", MSISDN);
                        cmd.Parameters.AddWithValue("@ServiceName", svcName);
                        cmd.Parameters.AddWithValue("@Shortcode", Shortcode);
                        cmd.Parameters.AddWithValue("@CCode", Productcode);
                        cmd.Parameters.AddWithValue("@IsMtn", IsMtn);
                        using (var da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                    retVal = bool.Parse(dt.Rows[0]["isSuccess"].ToString());
                }
                catch (Exception ex)
                {
                    new System.Threading.Thread(() =>
                    {
                        LocalLogger.LogFileWrite(Newtonsoft.Json.JsonConvert.SerializeObject(new LogVM()
                        {
                            Message = ex.Message,
                            LogData = $"AppUserID: {UID}"
                        }));
                    }).Start();
                }
            }
            return retVal;
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
            _NGSubscriptionsEntities.LoginAppUser(UID, false, null);
            return "/Home";
        }
    }
}
