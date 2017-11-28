using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameZone.VIEWMODEL
{
    public class RequestStatus
    {
        public virtual int code { get; set; }
    }
    public class GameGenre
    {
        public virtual string url_friendly_id { get; set; }
        public virtual string title { get; set; }
    }

    public class Game
    {
        public virtual int plays { get; set; }
        public virtual string language { get; set; }
        public virtual string title { get; set; }
        public virtual string date_added { get; set; }
        public virtual string icon_large { get; set; }
        public virtual string icon_medium { get; set; }
        public virtual string url { get; set; }
        public virtual string banner_large { get; set; }
        public virtual string banner_medium { get; set; }
        public virtual string short_description { get; set; }
        public virtual string icon_small { get; set; }
        public virtual string banner_small { get; set; }
        public virtual string long_description { get; set; }
    }
    public class GameData
    {
        public virtual GameGenre genre { get; set; }
        public virtual object games { get; set; }
    }

    public class CategoryGames
    {
        public virtual RequestStatus status { get; set; }
        public virtual GameData data { get; set; }
    }    
}
