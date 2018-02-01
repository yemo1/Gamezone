using GameZone.Repositories;
using GameZone.TOOLS;
using GameZone.VIEWMODEL;
using GameZone.WEB.Mappings;
using System;
using System.Web.Http;

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
                        _Notification.SendEMail(appUserVM.szUsername.Trim(),"Welcome to GameZone.", "Thank you for joining <b>GameZone</b>. <br/> Your account has been successfully created.<br/> <br/><a style='background:#ff6a00; color: #ffffff; font-family:bitsumishi !important; padding:6px 12px; font-weight:400;text-align:center; vertical-align: middle; cursor: pointer; border:1px solid transparent; border-radius:4px; text-decoration: none;' href='http://www.gamezone.ng/'> Login </a>");
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
                        LoginAppUserVM loginAppUserVM = new LoginAppUserVM
                        {
                            AppUserId = retVal.AppUserId,
                            szImgURL = retVal.szImgURL,
                            szUsername = retVal.szUsername,
                            iStatus = retVal.iStatus,
                            iChangePW = retVal.iChangePW
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
    }
}