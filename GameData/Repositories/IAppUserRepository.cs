using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameData;
using GameZone.VIEWMODEL;

namespace GameZone.Repositories
{
    public interface IAppUserRepository
    {
        IList<GetAppUser_Result> GetAppUsers();
        GetAppUser_Result GetAppUser(long? AppUserID, string Username);
        AddAppUser_Result AddAppUser(GameData.AppUser appUser);
        AppUserRepository ChangeAppUserPassword(AppUserRepository appUser);
    }
}
