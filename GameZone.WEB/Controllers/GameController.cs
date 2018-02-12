using GameZone.Repositories;
using GameZone.TOOLS;
using GameZone.VIEWMODEL;
using GameZone.WEB.Mappings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace GameZone.WEB.Controllers
{
    public class GameController : ApiController
    {
        //Hosted web API REST Service base url  
        string Baseurl = "http://funmobilelive.html5games.net/";

        Entities.GameContext _context;
        GameData.NGSubscriptionsEntities _NGSubscriptionsEntities;
        ISubscriberRepository _ISubscriberRepository;
        IAppUserRepository _IAppUserRepository;
        public GameController(ISubscriberRepository iSubscriberRepository, IAppUserRepository iAppUserRepository, GameData.NGSubscriptionsEntities nGSubscriptionsEntities, Entities.GameContext context)
        {
            _context = context;
            _NGSubscriptionsEntities = nGSubscriptionsEntities;
            _ISubscriberRepository = iSubscriberRepository;
            _IAppUserRepository = iAppUserRepository;
        }
        /// <summary>
        /// Method to get games regardless of category
        /// </summary>
        /// <param name="gameCount"></param>
        /// <returns></returns>
        public ReturnMessage Get(int gameCount)
        {
            ReturnMessage retVal;
            try
            {
                retVal = new ReturnMessage()
                {
                    ID = 1,
                    Success = true,
                    Data = GetGames($"api/portal_game/list/json/{gameCount}"),
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                retVal = new ReturnMessage()
                {
                    Success = false,
                    Message = ex.Message
                };
            }
            return retVal;
        }

        /// <summary>
        /// Method to get games based on category
        /// </summary>
        /// <param name="gameCategory"></param>
        /// <param name="gameCount"></param>
        /// <returns></returns>
        public JsonResult<ReturnMessage> Get(string gameCategory, int gameCount)
        {
            try
            {
                return Json(new ReturnMessage()
                {
                    ID = 1,
                    Success = true,
                    Data = GetGames($"api/portal_game/list_by_genre/{gameCategory}/json/{gameCount}"),
                    Message = ""
                });
            }
            catch (Exception ex)
            {
                return Json(new ReturnMessage()
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        public IList<VIEWMODEL.Game> GetGames(string URL)
        {
            CategoryGames CategoryGame = new CategoryGames();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var json = client.GetAsync(URL).Result;
                CategoryGame = JsonConvert.DeserializeObject<CategoryGames>(json.Content.ReadAsStringAsync().Result);

                string gameString = CategoryGame.data.games.ToString();
                var gameDict = JsonConvert.DeserializeObject<IDictionary<string, VIEWMODEL.Game>>(gameString);
                var gameList = gameDict.Values.ToList();

                //returning the game list to view  
                return gameList;
            }
        }

        public class NewSubscriber
        {
            public string nO { get; set; }
            public string t { get; set; }
            public int sT { get; set; }
        }
        public ReturnMessage PostNewSubscriber(NewSubscriber newSubscriber)
        {
            GameData.Game gameVW;
            try
            {
                //var subscriber = new SubscriberRepository(_context, _NGSubscriptionsEntities);

                var mobileUser = _ISubscriberRepository.GetUserByPhoneNoWithoutExpDateCheck(newSubscriber.t);
                //If user subscription is expired
                if (mobileUser == null)
                {
                    gameVW = new GameData.Game()
                    {
                        NetworkOperator = newSubscriber.nO,
                        MSISDN = newSubscriber.t,
                        SubDate = DateTime.Now,
                        ExpDate = (newSubscriber.sT == 0) ? DateTime.Today.AddDays(7) : DateTime.Today.AddDays(1),
                        Timestamped = DateTime.Now,
                        Token = Guid.NewGuid().ToString().Substring(0, 7).ToUpper()
                    };
                    return new ReturnMessage()
                    {
                        ID = _ISubscriberRepository.PostNewSubscriber(gameVW),
                        Message = $"Subscription Successful. Valid till: {gameVW.ExpDate.Value.ToShortDateString()}",
                        Success = true
                    };
                }
                else
                {
                    mobileUser.SubDate = DateTime.Now;
                    mobileUser.ExpDate = (newSubscriber.sT == 0) ?
                       (mobileUser.ExpDate < DateTime.Now) ?//if subscription is already expired
                       DateTime.Now.AddDays(7) : // New Expiry Date is 7 days from today
                       mobileUser.ExpDate.Value.AddDays(7) : //else New Expiry Date is 7 days from Old Expiry Date
                       (mobileUser.ExpDate < DateTime.Now) ? //if subscription is already expired
                       DateTime.Now.AddDays(1) :// New Expiry Date is 1 day from today
                       mobileUser.ExpDate.Value.AddDays(1);//else New Expiry Date is 1 day from Old Expiry Date

                    return new ReturnMessage()
                    {
                        ID = _ISubscriberRepository.PostNewSubscriber(mobileUser),
                        Message = $"Subscription Successful. Valid till: {mobileUser.ExpDate.Value.ToShortDateString()}",
                        Success = true
                    };
                }
            }
            catch (Exception ex)
            {
                return new ReturnMessage()
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public ReturnMessage PostNewAppUser(AppUserVM appUserVM)
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
        
        public ReturnMessage PostSubResponse(string resp = null)
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc = HttpContext.Current.Request.Headers;
            Dictionary<string, string> ss = new Dictionary<string, string>();
            foreach (var item in nvc.AllKeys)
            {
                ss.Add(item, nvc[item]);
            }

            return null;
        }
    }
}