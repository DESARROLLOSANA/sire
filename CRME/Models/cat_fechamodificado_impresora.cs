namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class cat_fechamodificado_impresora
    {
        [Key]
        public int fechamodificado_ID { get; set; }

        public int? resguardo_impresora_ID { get; set; }

        public int? sistemas_ID { get; set; }

        public DateTime fecha_operacion { get; set; }

        public virtual cat_resguardos_impresoras cat_resguardos_impresoras { get; set; }

        public virtual cat_sistemas cat_sistemas { get; set; }
    }
}
