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
    
    public partial class Platform
    {
        public Platform()
        {
            this.GamePlatforms = new HashSet<GamePlatform>();
            this.NewsPlatforms = new HashSet<NewsPlatform>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<GamePlatform> GamePlatforms { get; set; }
        public virtual ICollection<NewsPlatform> NewsPlatforms { get; set; }
    }
}
