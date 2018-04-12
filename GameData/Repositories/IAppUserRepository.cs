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
        ChangeAppUserPassword_Result ResetAppUserPassword(string szUsername, string szPassword, string szPasswordSalt, int iStatus, bool iChangePW);
        LoginAppUser_Result LoginAppUser(string szUsername, bool isLogin, string loginToken);
        ConfirmAppUserLoginToken_Result ConfirmAppUserToken(string loginToken, long AppUserID = 0, string szUsername = null);
        ConfirmAppUserSubscription_Result ConfirmUserSubscription(long AppUserID, string MSISDN, string ServiceName, string Shortcode, string Productcode, bool IsMtn);
        GetAppUserSubscriptionDetails_Result GetUserSubscriptionDetails(long AppUserID, string ServiceName, string shortCode = null);
        GetMTNUserSubscriptionDetails_Result GetMTNUserSubscriptionDetails(string MSISDN, string CCode);
        void UpdateAppUserMobilePayer(long AppUserId, string szMobilePayer);
    }
}
