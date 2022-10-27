namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class cat_resguardos_mobiliarios
    {
        [Key]
        public int resguardo_mobiliario_ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha { get; set; }

        public int? inv_mobiliario_ID { get; set; }

        public int? recibe_usuario_ID { get; set; }

        public int? entrega_usuario_ID { get; set; }

        public string observaciones { get; set; }

        public int? estatus_ID { get; set; }
    }
}
