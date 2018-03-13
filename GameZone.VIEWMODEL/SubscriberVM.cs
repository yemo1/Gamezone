using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameZone.VIEWMODEL
{
    public class SubscriberVM
    {
        public string ServiceName { get; set; }
        public Nullable<System.DateTime> Sub { get; set; }
        public Nullable<System.DateTime> Exp { get; set; }
        public Nullable<int> Status { get; set; }
    }
}
