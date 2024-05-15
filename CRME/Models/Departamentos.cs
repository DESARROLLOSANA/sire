namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Departamentos
    {
        public Departamentos()
        {

        }
        [Key]
        public int Dp_Cve_Departamento { get; set; }

        [StringLength(50)]
        public string Dp_Descripcion { get; set; }

        public int? Sc_Cve_Sucursal { get; set; }

        public int? Em_Cve_Sucursal { get; set; }

        [Required]
        [StringLength(50)]
        public string Oper_Alta { get; set; }

        public DateTime Fecha_Alta { get; set; }

        [StringLength(50)]
        public string Oper_Ult_Modf { get; set; }

        public DateTime? Fecha_Ult_Modf { get; set; }

        [StringLength(50)]
        public string Oper_Baja { get; set; }

        public DateTime? Fecha_Baja { get; set; }

        public bool Estatus { get; set; }

        public int? Ceco_ID { get; set; }
    }
}
