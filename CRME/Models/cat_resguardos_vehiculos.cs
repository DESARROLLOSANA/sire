namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class cat_resguardos_vehiculos
    {
        [Key]
        public int resguardo_vehiculo_ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha { get; set; }

        public int? inv_vehiculo_ID { get; set; }

        public int? recibe_usuario_ID { get; set; }

        public int? entrega_usuario_ID { get; set; }

        [StringLength(30)]
        public string licencia_manejo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? vencimiento_licencia { get; set; }

        [StringLength(12)]
        public string km_recibe { get; set; }

        [StringLength(50)]
        public string ubicacion { get; set; }

        [StringLength(50)]
        public string departamento { get; set; }

        public string observaciones { get; set; }

        public int? usuario_app_ID { get; set; }

        public int? estatus_ID { get; set; }
    }
}
