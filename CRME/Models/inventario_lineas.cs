namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class inventario_lineas
    {
        [Key]
        public int inv_linea_ID { get; set; }

        [StringLength(4)]
        public string region { get; set; }

        [StringLength(25)]
        public string cuenta { get; set; }

        [StringLength(100)]
        public string razon_social { get; set; }

        [StringLength(10)]
        public string telefono { get; set; }

        [StringLength(35)]
        public string nombre_plan { get; set; }

        [StringLength(6)]
        public string renta_sin_iva { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_inicio { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_termino { get; set; }

        [StringLength(15)]
        public string estatus_adendum { get; set; }

        [StringLength(150)]
        public string ceco { get; set; }

        [StringLength(30)]
        public string ubicacion { get; set; }

        [StringLength(40)]
        public string departamento { get; set; }

        public int? usuario_app_ID { get; set; }

        public DateTime created_time_actuales { get; set; }

        public int? estatus_ID { get; set; }

        public int Em_Cve_Empresa { get; set; }
    }
}
