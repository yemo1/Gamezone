using System;
using System.Linq;
using GameData;
using GameZone.Entities;
using GameZone.VIEWMODEL;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace GameZone.Repositories
{
    public class SubscriberRepository : ISubscriberRepository
    {
        private GameContext _context;
        private NGSubscriptionsEntities _NGSubscriptionsEntities;
        public SubscriberRepository(GameContext context, NGSubscriptionsEntities nGSubscriptionsEntities)
        {
            _context = context;
            _NGSubscriptionsEntities = nGSubscriptionsEntities;
        }

        public SubscriberRepository()
        {
            _context = new GameContext();
        }

        public GameData.Game GetUser(string tk)
        {
            return _context.Games.Where(s => s.Token == tk && s.ExpDate > DateTime.Now).FirstOrDefault();
        }

        /// <summary>
        /// This method returns records gotten from DB matching users phone number
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        public GameData.Game GetUserByPhoneNo(string phoneNo)
        {
            try
            {
                //return _context.Games.Where(s => s.MSISDN == phoneNo && s.ExpDate > DateTime.Now).FirstOrDefault();
                var retVal = _NGSubscriptionsEntities.Games.Where(s => s.MSISDN == phoneNo && s.ExpDate > DateTime.Now).FirstOrDefault();
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
        /// Method to add new Subscriber to DB
        /// </summary>
        /// <param name="t">User's Telephone Number</param>
        /// <param name="sT">Users selected Subscription Type</param>
        /// <returns></returns>
        public int PostNewSubscriber(GameData.Game game)
        {
            try
            {
                //Check if User already exists in DB
                if (SubscriberExists(game.MSISDN))
                {
                    _NGSubscriptionsEntities.Entry(game).State = EntityState.Modified;
                }
                else
                {
                    _NGSubscriptionsEntities.Games.Add(game);

                }
                var retVal = _NGSubscriptionsEntities.SaveChanges();
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