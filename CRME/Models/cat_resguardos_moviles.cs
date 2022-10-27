namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class cat_resguardos_moviles
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public cat_resguardos_moviles()
        {
            cat_fechamodificado_movil = new HashSet<cat_fechamodificado_movil>();
            cat_observaciones_movil = new HashSet<cat_observaciones_movil>();
        }

        [Key]
        public int resguardo_movil_ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha { get; set; }

        public int? inv_movil_ID { get; set; }

        public int? recibe_usuario_ID { get; set; }

        public int? entrega_usuario_ID { get; set; }

        public int? cargador { get; set; }

        public string observaciones { get; set; }

        public int? inv_linea_ID { get; set; }

        public int? estatus_ID { get; set; }

        public virtual cat_estatus_inv cat_estatus_inv { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cat_fechamodificado_movil> cat_fechamodificado_movil { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cat_observaciones_movil> cat_observaciones_movil { get; set; }

        public virtual inventario_movil inventario_movil { get; set; }
    }
}
