namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class inventario_tipo_mobiliario
    {
        [Key]
        public int tipo_mobiliario_ID { get; set; }

        [StringLength(50)]
        public string mobiliario { get; set; }

        public int? estatus_ID { get; set; }
    }
}
