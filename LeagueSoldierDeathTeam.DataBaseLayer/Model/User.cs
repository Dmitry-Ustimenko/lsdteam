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
    
    public partial class User
    {
        public User()
        {
            this.UserInfoes = new HashSet<UserInfo>();
            this.UserRoles = new HashSet<UserRole>();
        }
    
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime LastActivity { get; set; }
    
        public virtual ICollection<UserInfo> UserInfoes { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
