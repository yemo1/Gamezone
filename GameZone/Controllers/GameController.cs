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

        // GET: api/Game
        [Route("SingleGame")]
        public IEnumerable<string> GetSingleGame()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Game/5
        
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
    }
}
