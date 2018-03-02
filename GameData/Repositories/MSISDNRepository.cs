using GameZone.VIEWMODEL;
using System.Collections.Generic;

namespace GameZone.Repositories
{
    public class MSISDNRepository : IMSISDN
    {
        private List<MSISDNLine> lineCollection = new List<MSISDNLine>();

        public void AddItem(string msisdn, string ipaddress, bool isheader = true)
        {
            this.Clear();
            lineCollection.Add(new MSISDNLine { Phone = msisdn, IpAddress = ipaddress, IsHeader = isheader });
        }

        public void RemoveLine(string msisdn)
        {
            lineCollection.RemoveAll(p => p.Phone == msisdn);
            lineCollection.Clear();
        }


        public IEnumerable<MSISDNLine> Lines
        {
            get { return lineCollection; }
        }

        public void Clear()
        {
            lineCollection.Clear();
        }
    }

}