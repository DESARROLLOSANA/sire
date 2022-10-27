namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class cat_mantenimiento_vehiculos
    {
        [Key]
        public int mantenimiento_vehiculo_ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_registro { get; set; }

        public int? inv_vehiculo_ID { get; set; }

        public int? kilometros { get; set; }

        [StringLength(20)]
        public string servicio { get; set; }

        public string descripcion { get; set; }

        [StringLength(12)]
        public string costo { get; set; }

        public int? proveedor_ID { get; set; }

        public int? usuario_app_ID { get; set; }

        public int? estatus_ID { get; set; }
    }
}
