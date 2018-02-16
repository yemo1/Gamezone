using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameZone.VIEWMODEL
{
    public class ServiceHeaderVM
    {
        public int HeaderId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceLabel { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string MenuCategory { get; set; }
        public string MenuCategoryLabel { get; set; }
        public string Category { get; set; }
        public string CategoryLabel { get; set; }
        public string ProductCode { get; set; }
        public bool IsActive { get; set; }
        public string ServiceUrl { get; set; }
        public string ServiceParams { get; set; }
        public Nullable<int> ParamsType { get; set; }
        public string ImageUrl { get; set; }
        public string HomeCategory { get; set; }
        public string HomeCategoryLabel { get; set; }
        public string TimeFormat { get; set; }
    }
}
