//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Proveedores
    {
        [Key]
        public int IdProv { get; set; }
        public string Nombre { get; set; }
        [Required]
        public string Cedula_o_RNC { get; set; }
        [Required]
        public string Nombre_Comercial { get; set; }
        public bool Activo { get; set; }
    }
}
