namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class inventario_unidades
    {
        [Key]
        public int inv_unidad_ID { get; set; }

        [StringLength(50)]
        public string tipo_caja { get; set; }

        [StringLength(25)]
        public string marca_chasis { get; set; }

        [StringLength(25)]
        public string tipo_de_caja { get; set; }

        [StringLength(100)]
        public string modelo_capacidad { get; set; }

        [StringLength(4)]
        public string anio_chasis { get; set; }

        [StringLength(20)]
        public string numero_motor { get; set; }

        [StringLength(20)]
        public string numero_serie_chasis { get; set; }

        [StringLength(4)]
        public string anio_adaptacion { get; set; }

        [StringLength(15)]
        public string numero_serie_adaptacion { get; set; }

        [StringLength(6)]
        public string numero_economico_unidad { get; set; }

        [StringLength(2)]
        public string sistema_levante { get; set; }

        [StringLength(2)]
        public string duplicado_llaves { get; set; }

        [StringLength(10)]
        public string placas_nuevas { get; set; }

        [StringLength(30)]
        public string empresa_chasis { get; set; }

        [StringLength(30)]
        public string empresa_adaptación { get; set; }

        [StringLength(10)]
        public string empresa_gps { get; set; }

        [StringLength(15)]
        public string imei_gps { get; set; }

        [StringLength(15)]
        public string ceco { get; set; }

        [StringLength(20)]
        public string compania_aseguradora { get; set; }

        [StringLength(10)]
        public string poliza { get; set; }

        public int? inciso { get; set; }

        [Column(TypeName = "date")]
        public DateTime? inicio_vigencia { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fin_vigencia { get; set; }

        [StringLength(6)]
        public string numero_gasomatic { get; set; }

        [StringLength(2)]
        public string permiso_estatal { get; set; }

        public int? usuario_app_ID { get; set; }

        public DateTime created_time_actuales { get; set; }

        [StringLength(500)]
        public string url_pdf_tarjeta { get; set; }

        [StringLength(10)]
        public string type_tarjeta { get; set; }

        [StringLength(500)]
        public string url_pdf_poliza { get; set; }

        [StringLength(10)]
        public string type_poliza { get; set; }



        public int? estatus_ID { get; set; }

        public int Em_Cve_Empresa { get; set; }
    }
}
