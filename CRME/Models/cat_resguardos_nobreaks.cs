namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class cat_resguardos_nobreaks
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public cat_resguardos_nobreaks()
        {
            cat_fechamodificado_nobreak = new HashSet<cat_fechamodificado_nobreak>();
        }

        [Key]
        public int resguardo_nobreak_ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha { get; set; }

        public int? inv_nobreak_ID { get; set; }

        public int? recibe_usuario_ID { get; set; }

        public int? entrega_usuario_ID { get; set; }

        public int? estatus_ID { get; set; }

        public virtual cat_estatus_inv cat_estatus_inv { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cat_fechamodificado_nobreak> cat_fechamodificado_nobreak { get; set; }

        public virtual inventario_nobreak inventario_nobreak { get; set; }
    }
}
