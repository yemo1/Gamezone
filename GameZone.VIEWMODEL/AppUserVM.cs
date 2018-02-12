using System;

namespace GameZone.VIEWMODEL
{
    public class AppUserVM
    {
        public long AppUserId { get; set; }
        public string szImgURL { get; set; }
        public string szUsername { get; set; }
        public string szPassword { get; set; }
        public string szPasswordSalt { get; set; }
        public int iStatus { get; set; }
        public System.DateTime dCreatedOn { get; set; }
        public bool iChangePW { get; set; }
        public bool isDeleted { get; set; }
    }
}
