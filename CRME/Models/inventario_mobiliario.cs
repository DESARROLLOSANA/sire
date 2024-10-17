namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class inventario_mobiliario
    {
        [Key]
        public int inv_mobiliario_ID { get; set; }

        [Required]
        public int? tipo_mobiliario_ID { get; set; }

        [Required]
        [StringLength(20)]
        public string color { get; set; }

        [Required]
        public string tipo { get; set; }

        [StringLength(60)]
        public string cod_inventario { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public int? precio { get; set; }

        [StringLength(10)]
        public string folio { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_folio { get; set; }

        [Required]
        public int? proveedor_ID { get; set; }

        public int Sc_Cve_Sucursal { get; set; }

        public int Dp_Cve_Departamento { get; set; }

        public int? estatus_ID { get; set; }

        [Required]
        public int Em_Cve_Empresa { get; set; }
    }
}
