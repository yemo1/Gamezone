using System.Linq;
using GameData;
using System.Collections.Generic;

namespace GameZone.Repositories
{
    public class ServiceHeaderRepository : IServiceHeaderRepository
    {
        private readonly NGSubscriptionsEntities context = new NGSubscriptionsEntities();

        public IEnumerable<ServiceHeaders> ServiceHeaders
        {
            get
            {
                return context.ServiceHeaders;
            }
        }

        public ServiceHeaders DeleteServiceHeader(int headerId)
        {
            ServiceHeaders dbentry = context.ServiceHeaders.Find(headerId);
            if (dbentry != null)
            {
                context.ServiceHeaders.Remove(dbentry);
                context.SaveChanges();
            }
            return dbentry;
        }

        public ServiceHeaders GetServiceHeader(string servicename)
        {
            return context.ServiceHeaders.FirstOrDefault(s => s.ProductName == servicename | s.ServiceName == servicename);
        }

        public ServiceHeaders GetServiceHeader(int headerId)
        {
            return context.ServiceHeaders.FirstOrDefault(s => s.HeaderId == headerId);
        }

        public ServiceHeaders GetServiceHeader(string servicename, string productname)
        {
            return context.ServiceHeaders.FirstOrDefault(s => s.ProductName == productname & s.ServiceName == servicename);
        }

        public ServiceHeaders GetServiceHeader(string servicename, string productname, string productcode)
        {
            return context.ServiceHeaders.FirstOrDefault(s => s.ProductName == productname & s.ServiceName == servicename & s.ProductCode == productcode);
        }

        public void SaveServiceHeader(ServiceHeaders serviceheader)
        {
            if (serviceheader.HeaderId == 0)
            {
                context.ServiceHeaders.Add(serviceheader);
            }
            else
            {
                ServiceHeaders dbentry = context.ServiceHeaders.Find(serviceheader.HeaderId);
                if (dbentry != null)
                {
                    dbentry.Category = serviceheader.Category;
                    dbentry.CategoryLabel = serviceheader.CategoryLabel;
                    dbentry.Description = serviceheader.Description;
                    dbentry.HomeCategory = serviceheader.HomeCategory;
                    dbentry.HomeCategoryLabel = serviceheader.HomeCategoryLabel;
                    dbentry.ImageUrl = serviceheader.ImageUrl;
                    dbentry.IsActive = serviceheader.IsActive;
                    dbentry.MenuCategory = serviceheader.MenuCategory;
                    dbentry.MenuCategoryLabel = serviceheader.MenuCategoryLabel;
                    dbentry.ProductCode = serviceheader.ProductCode;
                    dbentry.ProductName = serviceheader.ProductName;
                    dbentry.ServiceName = serviceheader.ServiceName;
                    dbentry.ServiceLabel = serviceheader.ServiceLabel;
                    dbentry.ServiceUrl = serviceheader.ServiceUrl;
                    dbentry.ServiceParams = serviceheader.ServiceParams;
                    dbentry.ParamsType = serviceheader.ParamsType;
                    dbentry.TimeFormat = serviceheader.TimeFormat;
                }
            }
            context.SaveChanges();
        }

    }
}