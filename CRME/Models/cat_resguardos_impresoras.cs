namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class cat_resguardos_impresoras
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public cat_resguardos_impresoras()
        {
            cat_fechamodificado_impresora = new HashSet<cat_fechamodificado_impresora>();
        }

        [Key]
        public int resguardo_impresora_ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha { get; set; }

        public int? inv_impresora_ID { get; set; }

        public int? recibe_usuario_ID { get; set; }

        public int? entrega_usuario_ID { get; set; }

        public string observaciones { get; set; }

        public int? estatus_ID { get; set; }

        public virtual cat_estatus_inv cat_estatus_inv { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cat_fechamodificado_impresora> cat_fechamodificado_impresora { get; set; }

        public virtual inventario_impresora inventario_impresora { get; set; }
    }
}
