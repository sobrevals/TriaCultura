﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TriaCulturaDesktopApp.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class triaculturaDB_localEntities : DbContext
    {
        public triaculturaDB_localEntities()
            : base("name=triaculturaDB_localEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<author> authors { get; set; }
        public DbSet<capacity> capacities { get; set; }
        public DbSet<discipline> disciplines { get; set; }
        public DbSet<email> emails { get; set; }
        public DbSet<@event> events { get; set; }
        public DbSet<file> files { get; set; }
        public DbSet<phone> phones { get; set; }
        public DbSet<place> places { get; set; }
        public DbSet<project> projects { get; set; }
        public DbSet<request> requests { get; set; }
        public DbSet<type> types { get; set; }
    }
}
