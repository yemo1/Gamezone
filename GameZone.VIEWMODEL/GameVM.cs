using System;

namespace GameZone.VIEWMODEL
{
    public class GameVM
    {
        public int Id { get; set; }
        public string MSISDN { get; set; }
        public Nullable<System.DateTime> SubDate { get; set; }
        public Nullable<System.DateTime> ExpDate { get; set; }
        public string Token { get; set; }
        public Nullable<System.DateTime> Timestamped { get; set; }
        public Nullable<System.DateTime> LastAccess { get; set; }
    }
}
