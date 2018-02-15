using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameData;

namespace GameZone.Repositories
{
    public interface IServiceResponse
    {
        IEnumerable<ServiceResponses> ServiceResponses { get; }

        void SaveServiceResponse(ServiceResponses serviceresponse);
        ServiceResponses DeleteServiceResponse(int responseId);

        ServiceResponses GetServiceResponse(int responseId);

        ServiceResponses GetServiceResponse(int responseId, int requestId);
    }
}
