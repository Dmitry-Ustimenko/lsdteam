//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LeagueSoldierDeathTeam.DataBaseLayer.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserInfo
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Activity { get; set; }
        public Nullable<System.DateTime> DateBirth { get; set; }
        public string Country { get; set; }
        public string Town { get; set; }
        public string Street { get; set; }
        public string HomeNumber { get; set; }
        public string SiteLink { get; set; }
        public string ICQ { get; set; }
        public string Skype { get; set; }
        public string BattleLog { get; set; }
        public string Steam { get; set; }
        public string AboutMe { get; set; }
        public Nullable<int> SexId { get; set; }
        public int UserId { get; set; }
    
        public virtual Sex Sex { get; set; }
        public virtual User User { get; set; }
    }
}
