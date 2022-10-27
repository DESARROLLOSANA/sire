namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class cat_fechamodificado_cargador
    {
        [Key]
        public int fechamodificado_ID { get; set; }

        public int? resguardo_cargador_ID { get; set; }

        public int? sistemas_ID { get; set; }

        public DateTime fecha_operacion { get; set; }

        public virtual cat_resguardos_cargadores cat_resguardos_cargadores { get; set; }

        public virtual cat_sistemas cat_sistemas { get; set; }
    }
}
