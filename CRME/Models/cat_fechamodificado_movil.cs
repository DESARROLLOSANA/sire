namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class cat_fechamodificado_movil
    {
        [Key]
        public int fechamodificado_ID { get; set; }

        public int? resguardo_movil_ID { get; set; }

        public int? sistemas_ID { get; set; }

        public DateTime fecha_operacion { get; set; }

        public virtual cat_resguardos_moviles cat_resguardos_moviles { get; set; }

        public virtual cat_sistemas cat_sistemas { get; set; }
    }
}
