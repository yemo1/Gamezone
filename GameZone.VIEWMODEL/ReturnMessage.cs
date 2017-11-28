using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameZone.VIEWMODEL
{
    public class ReturnMessage
    {
        public virtual long ID { get; set; }
        public virtual string Message { get; set; }
        public virtual bool Success { get; set; }
        public virtual dynamic Data { get; set; }
    }
}
