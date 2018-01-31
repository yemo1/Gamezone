using GameZone.VIEWMODEL;
using System.Web;

namespace GameZone.TOOLS
{
    public class GameUserIdentity
    {
        public static LoginAppUserVM LoggedInUser
        {
            get { return ((LoginAppUserVM)HttpContext.Current.Session["GameUser"]) ?? null; }
            set { HttpContext.Current.Session["GameUser"] = value; }
        }
    }
}
