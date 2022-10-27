namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class inventario_impresora
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public inventario_impresora()
        {
            cat_resguardos_impresoras = new HashSet<cat_resguardos_impresoras>();
        }

        [Key]
        public int inv_impresora_ID { get; set; }

        [StringLength(40)]
        public string serie { get; set; }

        [StringLength(20)]
        public string marca { get; set; }

        [StringLength(50)]
        public string modelo { get; set; }

        [StringLength(30)]
        public string parte { get; set; }

        public int? precio { get; set; }

        [StringLength(60)]
        public string cod_inventario { get; set; }

        [StringLength(10)]
        public string folio { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_folio { get; set; }

        public int? proveedor_ID { get; set; }

        public int? estatus_ID { get; set; }

        public int Em_Cve_Empresa { get; set; }

        public virtual cat_estatus_inv cat_estatus_inv { get; set; }

        public virtual cat_proveedores cat_proveedores { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cat_resguardos_impresoras> cat_resguardos_impresoras { get; set; }
    }
}
