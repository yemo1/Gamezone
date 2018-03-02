using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameData;

namespace GameZone.Repositories
{
    public interface IServiceHeaderRepository
    {
        IEnumerable<ServiceHeaders> ServiceHeaders { get; }

        void SaveServiceHeader(ServiceHeaders serviceheader);
        ServiceHeaders DeleteServiceHeader(int headerId);

        ServiceHeaders GetServiceHeader(int headerId);
        ServiceHeaders GetServiceHeader(string servicename);

        ServiceHeaders GetServiceHeader(string servicename, string productname);

        ServiceHeaders GetServiceHeader(string servicename, string productname, string productcode);
        IList<ServiceHeaders> GetServiceHeaderByServiceName(string servicename);
    }
}
