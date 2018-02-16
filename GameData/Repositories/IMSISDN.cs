using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameData;
using GameZone.VIEWMODEL;

namespace GameZone.Repositories
{
    public interface IMSISDN
    {
        void AddItem(string msisdn, string ipaddress, bool isheader = true);
        IEnumerable<MSISDNLine> Lines { get; }

        void RemoveLine(string msisdn);

        void Clear();
    }
}
