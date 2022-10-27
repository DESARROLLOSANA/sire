namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class cat_resguardos_equipos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public cat_resguardos_equipos()
        {
            cat_fechamodificado_ensamble = new HashSet<cat_fechamodificado_ensamble>();
            cat_fechamodificado_laptop = new HashSet<cat_fechamodificado_laptop>();
        }

        [Key]
        public int resguardo_ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha { get; set; }

        public int? inv_laptop_ID { get; set; }

        public int? inv_ensamble_ID { get; set; }

        public int? inv_monitor_ID { get; set; }

        public int? recibe_usuario_ID { get; set; }

        public int? entrega_usuario_ID { get; set; }

        public int? estatus_ID { get; set; }

        public virtual cat_estatus_inv cat_estatus_inv { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cat_fechamodificado_ensamble> cat_fechamodificado_ensamble { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cat_fechamodificado_laptop> cat_fechamodificado_laptop { get; set; }

        public virtual inventario_ensamble inventario_ensamble { get; set; }

        public virtual inventario_laptop inventario_laptop { get; set; }

        public virtual inventario_monitor inventario_monitor { get; set; }
    }
}
