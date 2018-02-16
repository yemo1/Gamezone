using System.Collections.Generic;
using GameData;

namespace GameZone.Repositories
{
    public interface IServiceResponseRepository
    {
        IEnumerable<ServiceResponses> ServiceResponses { get; }

        void SaveServiceResponse(ServiceResponses serviceresponse);
        ServiceResponses DeleteServiceResponse(int responseId);

        ServiceResponses GetServiceResponse(int responseId);

        ServiceResponses GetServiceResponse(int responseId, int requestId);
    }
}
