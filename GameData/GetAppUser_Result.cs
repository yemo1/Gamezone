//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GameData
{
    using System;
    
    public partial class GetAppUser_Result
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
