﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace web_banvemaybay.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class web_banvemaybayEntities : DbContext
    {
        public web_banvemaybayEntities()
            : base("name=web_banvemaybayEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Chucvu> Chucvu { get; set; }
        public virtual DbSet<Chuyenbay> Chuyenbay { get; set; }
        public virtual DbSet<Dattruoc> Dattruoc { get; set; }
        public virtual DbSet<HangHK> HangHK { get; set; }
        public virtual DbSet<Hangve> Hangve { get; set; }
        public virtual DbSet<Hanhkhach> Hanhkhach { get; set; }
        public virtual DbSet<Hanhli> Hanhli { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoan { get; set; }
        public virtual DbSet<TTlienhe> TTlienhe { get; set; }
        public virtual DbSet<Ve> Ve { get; set; }
    }
}
