using GameZone.VIEWMODEL;
using System.Web;

namespace GameZone.TOOLS
{
    public class GameUserIdentity
    {
        public static GameVM LoggedInUser
        {
            get { return ((GameVM)HttpContext.Current.Session["GameUser"]) ?? null; }
            set { HttpContext.Current.Session["GameUser"] = value; }
        }
    }
}
