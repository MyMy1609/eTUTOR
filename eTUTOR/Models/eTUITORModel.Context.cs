﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eTUTOR.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class eTUITOREntities : DbContext
    {
        public eTUITOREntities()
            : base("name=eTUITOREntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<admin> admins { get; set; }
        public DbSet<comment> comments { get; set; }
        public DbSet<common> commons { get; set; }
        public DbSet<contact_admin> contact_admin { get; set; }
        public DbSet<contact_tutor> contact_tutor { get; set; }
        public DbSet<contact> contacts { get; set; }
        public DbSet<history_lessons> history_lessons { get; set; }
        public DbSet<parent> parents { get; set; }
        public DbSet<schedule> schedules { get; set; }
        public DbSet<session> sessions { get; set; }
        public DbSet<status> status { get; set; }
        public DbSet<student> students { get; set; }
        public DbSet<subject> subjects { get; set; }
        public DbSet<submenu> submenus { get; set; }
        public DbSet<tutor> tutors { get; set; }
        public DbSet<typeUser> typeUsers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<notice> notices { get; set; }
    }
}
