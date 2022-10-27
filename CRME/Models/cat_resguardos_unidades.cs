namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class cat_resguardos_unidades
    {
        [Key]
        public int resguardo_unidad_ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha { get; set; }

        public int? inv_unidad_ID { get; set; }

        public int? recibe_usuario_ID { get; set; }

        public int? entrega_usuario_ID { get; set; }

        [StringLength(15)]
        public string jefe { get; set; }

        public int? usuario_app_ID { get; set; }

        public int? estatus_ID { get; set; }
    }
}
