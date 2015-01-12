//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LeagueSoldierDeathTeam.DataBase.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class News
    {
        public News()
        {
            this.NewsPlatforms = new HashSet<NewsPlatform>();
            this.NewsComments = new HashSet<NewsComment>();
        }
    
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int CountViews { get; set; }
        public int NewsCategoryId { get; set; }
        public int WriterId { get; set; }
        public string ImagePath { get; set; }
        public string Annotation { get; set; }
        public bool BlockComments { get; set; }
    
        public virtual NewsCategory NewsCategory { get; set; }
        public virtual User Writer { get; set; }
        public virtual ICollection<NewsPlatform> NewsPlatforms { get; set; }
        public virtual ICollection<NewsComment> NewsComments { get; set; }
    }
}
