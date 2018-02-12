using System;
using System.Linq;
using GameData;
using GameZone.Entities;
using GameZone.VIEWMODEL;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;

namespace GameZone.Repositories
{
    public class AppUserRepository : IAppUserRepository
    {
        private GameContext _context;
        private NGSubscriptionsEntities _NGSubscriptionsEntities;
        public AppUserRepository(GameContext context, NGSubscriptionsEntities nGSubscriptionsEntities)
        {
            _context = context;
            _NGSubscriptionsEntities = nGSubscriptionsEntities;
        }

        public AppUserRepository()
        {
            _context = new GameContext();
        }

        /// <summary>
        /// Get Application User Data from Database
        /// </summary>
        /// <param name="AppUserID"></param>
        /// <param name="Username"></param>
        /// <returns></returns>
        public GetAppUser_Result GetAppUser(long? AppUserID, string Username)
        {
            try
            {
                var retVal = _NGSubscriptionsEntities.GetAppUser(AppUserID, Username).FirstOrDefault();
                return retVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method returns records gotten from DB matching users phone number
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        public IList<GetAppUser_Result> GetAppUsers()
        {
            return null;
            //try
            //{
            //    //return _context.Games.Where(s => s.MSISDN == phoneNo && s.ExpDate > DateTime.Now).FirstOrDefault();
            //    var retVal = _NGSubscriptionsEntities.Games.Where(s => s.MSISDN == phoneNo && s.ExpDate > DateTime.Now).FirstOrDefault();
            //    return retVal;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        /// <summary>
        /// This method returns records gotten from DB matching users phone number
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        public GameData.Game GetUserByPhoneNoWithoutExpDateCheck(string phoneNo)
        {
            try
            {
                //return _context.Games.Where(s => s.MSISDN == phoneNo && s.ExpDate > DateTime.Now).FirstOrDefault();
                var retVal = _NGSubscriptionsEntities.Games.Where(s => s.MSISDN == phoneNo).FirstOrDefault();
                return retVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method returns records gotten from DB matching users phone number
        /// </summary>
        /// <param name="tell"></param>
        /// <returns></returns>
        public ReturnMessage UpdateGameUserLastAccess(string tell, GameData.Game game)
        {
            ReturnMessage returnMessage;

            if (tell.Trim() != game.MSISDN.Trim())
            {
                return returnMessage = new ReturnMessage()
                {
                    Message = "Record Mismatch",
                    Success = false
                };
            }

            try
            {
                //Update Last Access DateTime to Today
                game.LastAccess = DateTime.Today;

                _NGSubscriptionsEntities.Entry(game).State = EntityState.Modified;
                var retVal = _NGSubscriptionsEntities.SaveChanges();
                return returnMessage = new ReturnMessage()
                {
                    ID = retVal,
                    Message = "Operation Successful",
                    Success = true,
                    Data = game
                };
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubscriberExists(tell))
                {
                    return returnMessage = new ReturnMessage()
                    {
                        Message = "Record Not Found",
                        Success = false
                    };
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        public AddAppUser_Result AddAppUser(GameData.AppUser appUser)
        {
            try
            {      
                var retVal = _NGSubscriptionsEntities.AddAppUser(appUser.szImgURL, appUser.szUsername, appUser.szPassword, appUser.szPasswordSalt, appUser.iStatus, appUser.dCreatedOn, appUser.iChangePW, appUser.isLogin, appUser.isMobile, appUser.isDeleted).FirstOrDefault();
                return retVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ChangeAppUserPassword_Result ResetAppUserPassword(string szUsername, string szPassword, string szPasswordSalt, int iStatus,bool iChangePW)
        {
            try
            {
                var retVal = _NGSubscriptionsEntities.ChangeAppUserPassword(szUsername, szPassword, szPasswordSalt, iStatus, iChangePW).FirstOrDefault();
                return retVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LoginAppUser_Result LoginAppUser(string szUsername, bool isLogin, string loginToken)
        {
            try
            {
                var retVal = _NGSubscriptionsEntities.LoginAppUser(szUsername, isLogin, loginToken).FirstOrDefault();
                return retVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ConfirmAppUserLoginToken_Result ConfirmAppUserToken(string loginToken, long AppUserID = 0,string szUsername = null)
        {
            try
            {
                var retVal = _NGSubscriptionsEntities.ConfirmAppUserLoginToken(AppUserID, szUsername, loginToken).FirstOrDefault();
                return retVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ConfirmAppUserSubscription_Result ConfirmUserSubscription(long AppUserID, string ServiceName)
        {
            try
            {
                var retVal = _NGSubscriptionsEntities.ConfirmAppUserSubscription(AppUserID, ServiceName).FirstOrDefault();
                return retVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SubscriberExists(string tell)
        {
            return _NGSubscriptionsEntities.Games.Count(e => e.MSISDN == tell) > 0;
        }        
    }
}