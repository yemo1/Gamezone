using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameData;
using GameZone.Entities;

namespace GameZone.Repositories
{
    public class Subscriber : ISubscriber
    {
        private GameContext _context;

        public Subscriber(GameContext context)
        {
            _context = context;
        }

        public Subscriber()
        {
            _context = new GameContext();
        }

        public Game GetUser(string tk)
        {
            return _context.Games.Where(s => s.Token == tk && s.ExpDate > DateTime.Now).FirstOrDefault();
        }
    }
}