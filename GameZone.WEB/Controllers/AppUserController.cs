using GameZone.Repositories;
using GameZone.TOOLS;
using GameZone.VIEWMODEL;
using GameZone.WEB.Mappings;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace GameZone.WEB.Controllers
{
    public class AppUserController : ApiController
    {
        Entities.GameContext _context;
        GameData.NGSubscriptionsEntities _NGSubscriptionsEntities;
        IAppUserRepository _IAppUserRepository;
        Notification _Notification;
        RegexUtilities _RegexUtilities;
        public AppUserController(IAppUserRepository iAppUserRepository, GameData.NGSubscriptionsEntities nGSubscriptionsEntities, Entities.GameContext context, Notification notification, RegexUtilities regexUtilities)
        {
            _context = context;
            _NGSubscriptionsEntities = nGSubscriptionsEntities;
            _IAppUserRepository = iAppUserRepository;
            _Notification = notification;
            _RegexUtilities = regexUtilities;
        }

        public ReturnMessage Post(AppUserVM appUserVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new ReturnMessage
                    {
                        ID = 0,
                        Success = false,
                        Message = "Please enter valid entries."
                    };
                }
                var passwordSalt = Guid.NewGuid().ToString();
                var originalPassword = appUserVM.szPassword.Trim();
                appUserVM.szPassword = Encryption.SaltEncrypt(originalPassword, passwordSalt);
                appUserVM.szPasswordSalt = passwordSalt.ToString();
                appUserVM.dCreatedOn = DateTime.Now;
                appUserVM.szUsername = appUserVM.szUsername.Trim();
                appUserVM.iChangePW = false;
                appUserVM.iStatus = 0;

                var retVal = _IAppUserRepository.AddAppUser(appUserVM.ToEntity());
                //Successful
                if (retVal.isSuccess)
                {
                    //Check if username is email address
                    if (_RegexUtilities.ContainsAlphabet(appUserVM.szUsername.Trim()))
                    {
                        //Send Email Notification
                        _Notification.SendEMail(appUserVM.szUsername.Trim(), "Welcome to GameZone.", "Thank you for joining <b>GameZone</b>. <br/> Your registration was successful.<br/> <br/><a style='background:#ff6a00; color: #ffffff; font-family:bitsumishi !important; padding:6px 12px; font-weight:400;text-align:center; vertical-align: middle; cursor: pointer; border:1px solid transparent; border-radius:4px; text-decoration: none;' href='http://www.gamezone.ng/'> Login </a>");
                    }

                    return new ReturnMessage
                    {
                        ID = long.Parse(retVal.id),
                        Success = true,
                        Message = retVal.message
                    };
                }
                else
                {
                    return new ReturnMessage
                    {
                        ID = (bool)appUserVM.isMobile ? 101 : 0,//MTN Number indicator (IsMobile)
                        Data = GetAuthenticateUser(appUserVM.szUsername,"password").Data,
                        Success = false,
                        Message = "Sorry, " + appUserVM.szUsername + " already exists. Please try a different UserID."
                    };
                }
            }
            catch (Exception ex)
            {
                return new ReturnMessage
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        [ResponseType(typeof(ReturnMessage)), Route("api/AppUser/ResetPw")]
        public ReturnMessage PostResetPw(AppUserVM appUserVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new ReturnMessage
                    {
                        ID = 0,
                        Success = false,
                        Message = "Please enter valid entries."
                    };
                }
                var passwordSalt = Guid.NewGuid().ToString();
                var originalPassword = passwordSalt.Substring(0, 6).Trim();
                var encriptedPassword = Encryption.SaltEncrypt(originalPassword, passwordSalt);

                var retVal = _IAppUserRepository.ResetAppUserPassword(appUserVM.szUsername.Trim(), encriptedPassword, passwordSalt, 0, true);
                //Successful
                if (retVal.isSuccess)
                {
                    //Check if username is email address
                    var ContainsAlphabet = _RegexUtilities.ContainsAlphabet(appUserVM.szUsername.Trim());
                    if (ContainsAlphabet)
                    {
                        //"C:\\Users\\Systems\\Documents\\GitHub\\Gamezone\\GameZone.WEB\\Content\\images\\logo.png"
                        //Send Email Notification
                        _Notification.SendEMail(appUserVM.szUsername.Trim(), "GameZone Password Reset.", $"Your password reset was successful. <br/> Your new password is: <b>{originalPassword}</b>. You will be prompted to change this password once you login. <br/><br/> <a style='background:#ff6a00; color: #ffffff; font-family:bitsumishi !important; padding:6px 12px; font-weight:400;text-align:center; vertical-align: middle; cursor: pointer; border:1px solid transparent; border-radius:4px; text-decoration: none;' href='http://www.gamezone.ng/'> Login </a>");
                    }

                    return new ReturnMessage
                    {
                        ID = long.Parse(retVal.id),
                        Success = true,
                        Message = ContainsAlphabet ? retVal.message + " Your new password has been sent to your registered email address."
                        : retVal.message + " Your new password has been sent to your registered phone no."
                    };
                }
                else
                {
                    return new ReturnMessage
                    {
                        Success = false,
                        Message = retVal.message
                    };
                }
            }
            catch (Exception ex)
            {
                return new ReturnMessage
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        [ResponseType(typeof(ReturnMessage)), Route("api/AppUser/ChangePw")]
        public ReturnMessage PostChangePw(AppUserVM appUserVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new ReturnMessage
                    {
                        ID = 0,
                        Success = false,
                        Message = "Please enter valid entries."
                    };
                }
                var passwordSalt = Guid.NewGuid().ToString();
                var originalPassword = appUserVM.szPassword.Trim();
                var encriptedPassword = Encryption.SaltEncrypt(originalPassword, passwordSalt);

                var retVal = _IAppUserRepository.ResetAppUserPassword(appUserVM.szUsername.Trim(), encriptedPassword, passwordSalt, 0, false);
                //Successful
                if (retVal.isSuccess)
                {
                    //Check if username is email address
                    if (_RegexUtilities.ContainsAlphabet(appUserVM.szUsername.Trim()))
                    {
                        //"C:\\Users\\Systems\\Documents\\GitHub\\Gamezone\\GameZone.WEB\\Content\\images\\logo.png"
                        //Send Email Notification
                        _Notification.SendEMail(appUserVM.szUsername.Trim(), "GameZone Password Change.", $"Your password change was successful. <br/><br/> <a style='background:#ff6a00; color: #ffffff; font-family:bitsumishi !important; padding:6px 12px; font-weight:400;text-align:center; vertical-align: middle; cursor: pointer; border:1px solid transparent; border-radius:4px; text-decoration: none;' href='http://www.gamezone.ng/'> Login </a>");
                    }

                    return new ReturnMessage
                    {
                        ID = long.Parse(retVal.id),
                        Success = true,
                        Message = retVal.message
                    };
                }
                else
                {
                    return new ReturnMessage
                    {
                        Success = false,
                        Message = retVal.message
                    };
                }
            }
            catch (Exception ex)
            {
                return new ReturnMessage
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public ReturnMessage GetAuthenticateUser(string szUsername, string szPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(szUsername.Trim()) || string.IsNullOrEmpty(szPassword.Trim()))
                {
                    return new ReturnMessage
                    {
                        Success = false,
                        Message = "Please enter valid credentials."
                    };
                }
                var retVal = _IAppUserRepository.GetAppUser(null, szUsername);
                //If User Found
                if (retVal != null && retVal.AppUserId > 0)
                {
                    //Check if User has been Deleted
                    if (retVal.isDeleted)
                    {
                        return new ReturnMessage
                        {
                            Success = false,
                            Message = $"Sorry, {szUsername} has been Deleted. Please contact Admin."
                        };
                    }
                    //Check if User has been Disabled
                    if (retVal.iStatus > 0)
                    {
                        return new ReturnMessage
                        {
                            Success = false,
                            Message = $"Sorry, {szUsername} has been Disabled. Please contact Admin."
                        };
                    }

                    var passwordSalt = retVal.szPasswordSalt.ToString();
                    var originalPassword = retVal.szPassword.Trim();
                    var decryptedPassword = Encryption.SaltDecrypt(originalPassword, passwordSalt);

                    //password correct
                    if (decryptedPassword.Trim() == szPassword.Trim())
                    {
                        //Login User
                        string loginToken = Guid.NewGuid().ToString();
                        _IAppUserRepository.LoginAppUser(retVal.szUsername, true, loginToken);

                        LoginAppUserVM loginAppUserVM = new LoginAppUserVM
                        {
                            AppUserId = retVal.AppUserId,
                            szImgURL = retVal.szImgURL,
                            szUsername = retVal.szUsername,
                            iStatus = retVal.iStatus,
                            iChangePW = retVal.iChangePW,
                            userLoginToken = loginToken
                        };
                        return new ReturnMessage
                        {
                            ID = retVal.AppUserId,
                            Success = true,
                            Message = "Login Successful",
                            Data = loginAppUserVM
                        };
                    }
                    else
                    {
                        return new ReturnMessage
                        {
                            Success = false,
                            Message = "Password Incorrect. Forgotten your password?. You can reset it."
                        };
                    }
                }
                else
                {
                    return new ReturnMessage
                    {
                        Success = false,
                        Message = "Invalid Credentials. Check your entry and try again."
                    };
                }
            }
            catch (Exception ex)
            {
                return new ReturnMessage
                {
                    Success = false,
                    Message = "Error Encountered: " + ex.Message
                };
            }
        }

        [ResponseType(typeof(ReturnMessage)), Route("api/AppUser/SubscriptionDetails")]
        public ReturnMessage GetSubscriptionDetails(long UID, string svcName)
        {
            try
            {
                if (UID < 1 || string.IsNullOrEmpty(svcName.Trim()))
                {
                    return new ReturnMessage
                    {
                        ID = 0,
                        Success = false,
                        Message = "Please enter valid entries."
                    };
                }

                var retVal = _IAppUserRepository.GetUserSubscriptionDetails(UID, svcName.Trim());
                //Successful
                if (retVal != null)
                {
                    return new ReturnMessage
                    {
                        ID = retVal.AppUserId,
                        Success = true,
                        Data = retVal
                    };
                }
                else
                {
                    return new ReturnMessage
                    {
                        Success = false,
                        Message = "Record Not Found"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ReturnMessage
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        [ResponseType(typeof(ReturnMessage)), Route("api/AppUser/MTNSubscriptionDetails")]
        public ReturnMessage GetMTNSubscriptionDetails(string MSISDN, string Shortcode)
        {
            try
            {
                if (string.IsNullOrEmpty(MSISDN.Trim()) || string.IsNullOrEmpty(Shortcode.Trim()))
                {
                    return new ReturnMessage
                    {
                        ID = 0,
                        Success = false,
                        Message = "Please enter valid entries."
                    };
                }

                var retVal = _IAppUserRepository.GetMTNUserSubscriptionDetails(MSISDN.Trim(), Shortcode.Trim());
                //Successful
                if (retVal != null)
                {
                    return new ReturnMessage
                    {
                        Success = true,
                        Data = retVal
                    };
                }
                else
                {
                    return new ReturnMessage
                    {
                        Success = false,
                        Message = "Record Not Found"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ReturnMessage
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        [Route("api/AppUser/StartValidUserSession")]
        public void PostStartValidUserSession(LoginAppUserVM loginAppUserVM)
        {
            if (loginAppUserVM != null)
            {
                //Put Valid Login User Data in Session
                GameUserIdentity.LoggedInUser = loginAppUserVM;
            }
        }

        [ResponseType(typeof(string)), Route("api/AppUser/AuthUserToken")]
        public string GetAuthUserToken(string redirectURL = null)
        {
            //System.Net.Http.HttpRequestMessage request = new System.Net.Http.HttpRequestMessage();
            string headerToken = "";
            System.Collections.Generic.IEnumerable<string> keys = null;

            //if (request.Headers.TryGetValues("authToken", out keys))
            //    headerToken = keys.First();

            if (Request.Headers.TryGetValues("authToken", out keys))
                headerToken = keys.First();

            //System.Collections.Specialized.NameValueCollection nvc = new System.Collections.Specialized.NameValueCollection();
            //nvc = Request.Headers;
            //System.Collections.Generic.Dictionary<string, string> ss = new System.Collections.Generic.Dictionary<string, string>();
            //foreach (var item in nvc.AllKeys)
            //{
            //    ss.Add(item, nvc[item]);
            //}

            //Check for Authentication Token in Request Header
            if (!string.IsNullOrEmpty(headerToken))
            {
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

        [ResponseType(typeof(bool)), Route("api/AppUser/ValidSubscription")]
        public bool GetValidSubscription(long UID, string MSISDN, string svcName, string Shortcode, bool IsMtn, string Productcode = null)
        {
            bool retVal = false;
            string conString = System.Configuration.ConfigurationManager.ConnectionStrings["NGSubscriptionsConnectionString"].ConnectionString;
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(conString))
            {
                try
                {
                    if (connection.State == System.Data.ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    System.Data.DataTable dt = new System.Data.DataTable();
                    using (var cmd = new System.Data.SqlClient.SqlCommand("ConfirmAppUserSubscription", connection))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@AppUserId", UID);
                        cmd.Parameters.AddWithValue("@MSISDN", MSISDN);
                        cmd.Parameters.AddWithValue("@ServiceName", svcName);
                        cmd.Parameters.AddWithValue("@Shortcode", Shortcode);
                        cmd.Parameters.AddWithValue("@CCode", Productcode);
                        cmd.Parameters.AddWithValue("@IsMtn", IsMtn);
                        using (var da = new System.Data.SqlClient.SqlDataAdapter(cmd))
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

        //POST: /Account/LogOff
       [ResponseType(typeof(string)), Route("api/AppUser/LogOff")]
        public string GetLogOff(string UID)
        {
            if (string.IsNullOrEmpty(UID))
            {
                return "";
            }
            _IAppUserRepository.LoginAppUser(UID, false, null);
            return "/Home";
        }
    }
}