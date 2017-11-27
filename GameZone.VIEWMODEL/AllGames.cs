using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameZone.VIEWMODEL
{
    public class AllGames
    {
        public virtual string genres { get; set; }
        public virtual long plays { get; set; }
        public virtual string language { get; set; }
        public virtual string title { get; set; }
        public virtual string date_added { get; set; }
        public virtual string icon_large { get; set; }
        public virtual string icon_medium { get; set; }
        public virtual bool featured { get; set; }
        public virtual string url { get; set; }
        public virtual string banner_large { get; set; }
        public virtual string banner_medium { get; set; }
        public virtual string short_description { get; set; }
        public virtual string icon_small { get; set; }
        public virtual string banner_small { get; set; }
        public virtual string long_description { get; set; }        
    }
}
