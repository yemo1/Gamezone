using GameData;
using GameZone.Repositories;
using System.Web.Mvc;

namespace GameZone.WEB.Controllers
{
    public class GamesController : Controller
    {
        public ActionResult Index(string phoneNo)
        {
            Entities.GameContext _context = new Entities.GameContext();
            NGSubscriptionsEntities _NGSubscriptionsEntities = new NGSubscriptionsEntities();
            var subscriber = new Subscriber(_context, _NGSubscriptionsEntities);
            var s = subscriber.GetUserByPhoneNo(phoneNo);
            if (s == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// This is the view to display complete list of games
        /// with menu to select categories
        /// </summary>
        /// <param name="t">This is the Telephone Number Passed</param>
        /// <returns></returns>
        public ActionResult List(string t)
        {
            //Validate User Session
            //if (GameUserIdentity.LoggedInUser == null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            //Entities.GameContext _context = new Entities.GameContext();
            //NGSubscriptionsEntities _NGSubscriptionsEntities = new NGSubscriptionsEntities();
            //var subscriber = new Subscriber(_context, _NGSubscriptionsEntities);

            ////Check for subscription Expiry

            ////Check for Wrong Date and Time

            //var s = subscriber.GetUserByPhoneNo(t);
            //if (s == null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            //else // Access Granted
            //{
            //    //Implement Caching of last game category selected
            //    GameUserIdentity.LoggedInUser = s.ToModel();
                //subscriber.UpdateGameUserLastAccess(t, s);
            return View();
            //}
        }
        public ActionResult GamePlay()
        {
            ////Validate User Session
            //if (GameUserIdentity.LoggedInUser == null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            //Entities.GameContext _context = new Entities.GameContext();
            //NGSubscriptionsEntities _NGSubscriptionsEntities = new NGSubscriptionsEntities();
            //var subscriber = new Subscriber(_context, _NGSubscriptionsEntities);

            ////Check for subscription Expiry

            ////Check for Wrong Date and Time
            //string userTel = GameUserIdentity.LoggedInUser.MSISDN;
            //var s = subscriber.GetUserByPhoneNo(userTel);
            //if (s == null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            //else
            //{
            //    //Keep Track of User Last Game Played
            //    //GameUserIdentity.LoggedInUser = s.ToModel();
            //    //subscriber.UpdateGameUserLastAccess(userTel, s);
                return View();
            //}
        }
    }
}