﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sistema_Compras
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ComprasEntities : DbContext
    {
        public ComprasEntities()
            : base("name=ComprasEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Articulos> Articulos { get; set; }
        public virtual DbSet<Departamentos> Departamentos { get; set; }
        public virtual DbSet<Empleados> Empleados { get; set; }
        public virtual DbSet<Marcas> Marcas { get; set; }
        public virtual DbSet<Medidas> Medidas { get; set; }
        public virtual DbSet<Orden_Compra> Orden_Compra { get; set; }
        public virtual DbSet<Proveedores> Proveedores { get; set; }
        public virtual DbSet<Solicitud_Articulos> Solicitud_Articulos { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
    }
}
