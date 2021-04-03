﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CC.Connections.PL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CCEntities : DbContext
    {
        public CCEntities()
            : base("name=CCEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Charity>().HasKey<String>(c => c.CharityEmail);
            modelBuilder.Entity<ContactInfo>().HasKey<String>(c => c.MemberEmail);
            modelBuilder.Entity<LogIn>().HasKey<String>(c => c.MemberEmail);
            modelBuilder.Entity<Volunteer>().HasKey<String>(c => c.VolunteerEmail);
            modelBuilder.Entity<CharityEvent>().HasKey<Guid>(c => c.ID);
            modelBuilder.Entity<EventAttendance>().HasKey<Guid>(c => c.ID);
            modelBuilder.Entity<Location>().HasKey<Guid>(c => c.ID);
            modelBuilder.Entity<MemberAction>().HasKey<Guid>(c => c.ID);
            modelBuilder.Entity<Preference>().HasKey<Guid>(c => c.ID);
            modelBuilder.Entity<PreferredCategory>().HasKey<Guid>(c => c.ID);
            modelBuilder.Entity<PreferredCharity>().HasKey<Guid>(c => c.ID);
            //throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CharityEvent> CharityEvents { get; set; }
        public virtual DbSet<ContactInfo> ContactInfoes { get; set; }
        public virtual DbSet<EventAttendance> EventAttendances { get; set; }
        public virtual DbSet<HelpingAction> HelpingActions { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<MemberAction> MemberActions { get; set; }
        public virtual DbSet<Preference> Preferences { get; set; }
        public virtual DbSet<PreferredCategory> PreferredCategories { get; set; }
        public virtual DbSet<PreferredCharity> PreferredCharities { get; set; }
        public virtual DbSet<Charity> Charities { get; set; }
        public virtual DbSet<Volunteer> Volunteers { get; set; }
        public virtual DbSet<LogIn> logins { get; set; }
    }
}
