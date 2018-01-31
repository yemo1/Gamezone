using System;

namespace GameZone.VIEWMODEL
{
    public class LoginAppUserVM
    {
        public long AppUserId { get; set; }
        public string szImgURL { get; set; }
        public string szUsername { get; set; }
        public int iStatus { get; set; }
        public bool iChangePW { get; set; }
    }
}
