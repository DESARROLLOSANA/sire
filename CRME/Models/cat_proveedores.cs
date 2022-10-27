namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class cat_proveedores
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public cat_proveedores()
        {
            inventario_cargador_impresora = new HashSet<inventario_cargador_impresora>();
            inventario_ensamble = new HashSet<inventario_ensamble>();
            inventario_impresora = new HashSet<inventario_impresora>();
            inventario_laptop = new HashSet<inventario_laptop>();
            inventario_monitor = new HashSet<inventario_monitor>();
            inventario_movil = new HashSet<inventario_movil>();
            inventario_nobreak = new HashSet<inventario_nobreak>();
        }

        [Key]
        public int proveedor_ID { get; set; }

        [StringLength(80)]
        public string proveedor { get; set; }

        public int? estatus_ID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inventario_cargador_impresora> inventario_cargador_impresora { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inventario_ensamble> inventario_ensamble { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inventario_impresora> inventario_impresora { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inventario_laptop> inventario_laptop { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inventario_monitor> inventario_monitor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inventario_movil> inventario_movil { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<inventario_nobreak> inventario_nobreak { get; set; }
    }
}
