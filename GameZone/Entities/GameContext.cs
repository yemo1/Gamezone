using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using GameData;

namespace GameZone.Entities
{
    public class GameContext:DbContext
    {
        public DbSet<Game> Games { get; set; }
    }
}