namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Puestos
    {
        [Key]
        public int Pu_Cve_Puesto { get; set; }

        [StringLength(150)]
        public string Pu_Descripcion { get; set; }

        public int? Dp_Cve_Departamento { get; set; }
        
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

        public bool? Estatus { get; set; }
    }
}
