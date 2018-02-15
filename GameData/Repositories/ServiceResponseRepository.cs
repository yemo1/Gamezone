using System;
using System.Linq;
using GameData;
using GameZone.Entities;
using GameZone.VIEWMODEL;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;

namespace GameZone.Repositories
{
    public class ServiceResponseRepository : IServiceResponse
    {
        private readonly NGSubscriptionsEntities context;
        //private readonly EFDbContext context = new EFDbContext();

        public IEnumerable<ServiceResponses> ServiceResponses
        {
            get
            {
                return context.ServiceResponses;
            }
        }

        public ServiceResponses DeleteServiceResponse(int responseId)
        {
            ServiceResponses dbentry = context.ServiceResponses.Find(responseId);
            if (dbentry != null)
            {
                context.ServiceResponses.Remove(dbentry);
                context.SaveChanges();
            }
            return dbentry;
        }

        public ServiceResponses GetServiceResponse(int responseId)
        {
            return context.ServiceResponses.FirstOrDefault(r => r.ResponseId == responseId | r.RequestId == responseId);
        }

        public ServiceResponses GetServiceResponse(int responseId, int requestId)
        {
            return context.ServiceResponses.FirstOrDefault(r => r.RequestId == requestId & r.ResponseId == responseId);
        }


        public void SaveServiceResponse(ServiceResponses serviceresponse)
        {
            if (serviceresponse.ResponseId == 0)
            {
                context.ServiceResponses.Add(serviceresponse);
            }
            else
            {
                ServiceResponses dbentry = context.ServiceResponses.Find(serviceresponse.ResponseId);
                if (dbentry != null)
                {
                    dbentry.Description = serviceresponse.Description;
                    dbentry.RequestId = serviceresponse.RequestId;
                    dbentry.StatusCode = serviceresponse.StatusCode;
                    dbentry.Timestamped = serviceresponse.Timestamped;
                }
            }
            context.SaveChanges();
        }

    }
}