﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Sex> Sexes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserActivateToken> UserActivateTokens { get; set; }
        public virtual DbSet<UserExternalInfo> UserExternalInfoes { get; set; }
        public virtual DbSet<UserInfo> UserInfoes { get; set; }
        public virtual DbSet<UserResetToken> UserResetTokens { get; set; }
    }
}
