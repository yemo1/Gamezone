using GameZone.VIEWMODEL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace GameZone.Controllers
{
    [RoutePrefix("api/Game")]
    public class GameController : ApiController
    {
        //Hosted web API REST Service base url  
        string Baseurl = " http://funmobilelive.html5games.net/";
        
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
        public ReturnMessage Get(string gameCategory, int gameCount)
        {
            ReturnMessage retVal;
            try
            {
                retVal = new ReturnMessage()
                {
                    ID = 1,
                    Success = true,
                    Data = GetGames($"api/portal_game/list_by_genre/{gameCategory}/json/{gameCount}"),
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


        public IList<Game> GetGames(string URL)
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
                var gameDict = JsonConvert.DeserializeObject<IDictionary<string, Game>>(gameString);
                var gameList = gameDict.Values.ToList();

                //returning the game list to view  
                return gameList;
            }
        }

        public class NewSubscriber
        {
            public string t { get; set; }
            public int sT { get; set; }
        }
        public ReturnMessage PostNewSubscriber(NewSubscriber newSubscriber)
        {
            GameData.Game gameVW;
            try
            {
                Entities.GameContext _context = new Entities.GameContext();
                GameData.NGSubscriptionsEntities _NGSubscriptionsEntities = new GameData.NGSubscriptionsEntities();
                var subscriber = new Repositories.Subscriber(_context, _NGSubscriptionsEntities);

                var mobileUser = subscriber.GetUserByPhoneNo(newSubscriber.t);
                //If user subscription is expired
                if (mobileUser == null)
                {
                    gameVW = new GameData.Game()
                    {
                        MSISDN = newSubscriber.t,
                        SubDate = DateTime.Now,
                        ExpDate = (newSubscriber.sT == 0) ? DateTime.Today.AddDays(7) : DateTime.Today.AddDays(1),
                        Timestamped = DateTime.Now,
                        Token = Guid.NewGuid().ToString().Substring(0, 7).ToUpper()
                    };
                    return subscriber.PostNewSubscriber(gameVW);
                }
                else
                {
                    gameVW = new GameData.Game()
                    {
                        MSISDN = newSubscriber.t,
                        SubDate = DateTime.Now,
                        ExpDate = (newSubscriber.sT == 0) ? mobileUser.ExpDate.Value.AddDays(7) : mobileUser.ExpDate.Value.AddDays(1),
                        Timestamped = DateTime.Now,
                        Token = mobileUser.Token
                    };
                    return subscriber.PostNewSubscriber(gameVW);
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
    }
}
