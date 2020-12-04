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

    public partial class Articulos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Articulos()
        {
            this.Orden_Compra = new HashSet<Orden_Compra>();
            this.Solicitud_Articulos = new HashSet<Solicitud_Articulos>();
        }

        [Key]
        public int IdArt { get; set; }
        [Required]
        public string Articulo { get; set; }
        [Required]
        public int Marca { get; set; }
        public int Unidad_Medida { get; set; }
        [Required]
        [Range(1,Int32.MaxValue)]
        public int Existencia { get; set; }
        public bool Activo { get; set; }
    
        public virtual Marcas Marcas { get; set; }
        public virtual Medidas Medidas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orden_Compra> Orden_Compra { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Solicitud_Articulos> Solicitud_Articulos { get; set; }
    }
}
