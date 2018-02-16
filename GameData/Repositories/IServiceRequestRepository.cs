using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameData;
using GameZone.VIEWMODEL;

namespace GameZone.Repositories
{
    public interface IServiceRequestRepository
    {
        IEnumerable<ServiceRequests> ServiceRequests { get; }

        void SaveServiceRequest(ServiceRequests servicerequest);
        ServiceRequests DeleteServiceRequest(int requestId);

        ServiceRequests GetServiceRequest(int requestId);
        ServiceRequests GetServiceRequest(string transactionId);

        ServiceRequests GetServiceRequest(int requestId, string msisdn);

        ServiceRequests GetServiceRequest(int requestId, string msisdn, string transactionId);

        ReturnMessage Subscribe(int headerId, string ipAddress, string msisdn, bool IsHeaderEnabled = true, string sourceChannel = "Standard");
    }
}
