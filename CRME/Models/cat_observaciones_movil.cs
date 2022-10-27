namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class cat_observaciones_movil
    {
        [Key]
        public int observacion_movil_ID { get; set; }

        public int? resguardo_movil_ID { get; set; }

        public int? usuario_ID { get; set; }

        public string observaciones { get; set; }

        public virtual cat_resguardos_moviles cat_resguardos_moviles { get; set; }

        public virtual cat_usuarios cat_usuarios { get; set; }
    }
}
