namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class cat_km_actuales_vehiculos
    {
        [Key]
        public int km_actuales_vehiculo_ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_registro { get; set; }

        public int? inv_vehiculo_ID { get; set; }

        public int? km_actuales { get; set; }

        public int? usuario_app_ID { get; set; }

        public int? estatus_ID { get; set; }

        public DateTime created_time_actuales { get; set; }
    }
}
