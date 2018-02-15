using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameData;

namespace GameZone.Repositories
{
    public interface ISubscriberRepository
    {
        GameData.Game GetUser(string tk);

        GameData.Game GetUserByPhoneNoWithoutExpDateCheck(string phoneNo);

        int PostNewSubscriber(GameData.Game game);
    }
}
