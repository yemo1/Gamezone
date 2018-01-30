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
        public AppUserController(IAppUserRepository iAppUserRepository, GameData.NGSubscriptionsEntities nGSubscriptionsEntities, Entities.GameContext context)
        {
            _context = context;
            _NGSubscriptionsEntities = nGSubscriptionsEntities;
            _IAppUserRepository = iAppUserRepository;
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

                if (retVal.isSuccess)
                {
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
                            Message = "Password Incorrect. Forgotten your password?. Reset it."
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