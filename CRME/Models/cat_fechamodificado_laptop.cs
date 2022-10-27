namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class cat_fechamodificado_laptop
    {
        [Key]
        public int fechamodificado_ID { get; set; }

        public int? resguardo_ID { get; set; }

        public int? sistemas_ID { get; set; }

        public DateTime fecha_operacion { get; set; }

        public virtual cat_resguardos_equipos cat_resguardos_equipos { get; set; }

        public virtual cat_sistemas cat_sistemas { get; set; }
    }
}
