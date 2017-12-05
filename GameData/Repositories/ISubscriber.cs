using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameData;

namespace GameZone.Repositories
{
    public interface ISubscriber
    {
        Game GetUser(string tk);
    }
}
