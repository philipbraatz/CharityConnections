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
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Charity_Event> Charity_Event { get; set; }
        public virtual DbSet<Contact_Info> Contact_Info { get; set; }
        public virtual DbSet<Event_Attendance> Event_Attendance { get; set; }
        public virtual DbSet<Helping_Action> Helping_Action { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Log_in> Log_in { get; set; }
        public virtual DbSet<Member_Action> Member_Action { get; set; }
        public virtual DbSet<Member_Type> Member_Type { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Preference> Preferences { get; set; }
        public virtual DbSet<Preferred_Category> Preferred_Category { get; set; }
        public virtual DbSet<Preferred_Charity> Preferred_Charity { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Charities> Charities { get; set; }
    }
}
