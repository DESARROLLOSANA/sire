namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class inventario_vehiculos
    {
        [Key]
        public int inv_vehiculo_ID { get; set; }

        [StringLength(10)]
        public string empresa { get; set; }

        [StringLength(10)]
        public string tipo_vehiculo { get; set; }

        [StringLength(10)]
        public string numero_economico_vehiculo { get; set; }

        [StringLength(30)]
        public string serie_motor { get; set; }

        [StringLength(30)]
        public string cuadro_chasis { get; set; }

        [StringLength(30)]
        public string marca_motor { get; set; }

        [StringLength(4)]
        public string anio { get; set; }

        [StringLength(15)]
        public string color { get; set; }

        [StringLength(15)]
        public string modelo { get; set; }

        [StringLength(15)]
        public string placas { get; set; }

        [StringLength(15)]
        public string poliza_seguro { get; set; }

        [Column(TypeName = "date")]
        public DateTime? vigencia_del { get; set; }

        [Column(TypeName = "date")]
        public DateTime? vigencia_al { get; set; }

        [StringLength(5)]
        public string inciso { get; set; }

        [StringLength(15)]
        public string tarjeta_circulacion { get; set; }

        [Column(TypeName = "date")]
        public DateTime? vigencia_tarjeta { get; set; }

        public int? proveedor_ID { get; set; }

        [StringLength(10)]
        public string folio { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_folio { get; set; }

        public int? precio { get; set; }

        public int? estatus_ID { get; set; }

        public int Em_Cve_Empresa { get; set; }
    }
}
