namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sucursal")]
    public partial class Sucursal
    {
        [Key]
        public int Sc_Cve_Sucursal { get; set; }

        [StringLength(300)]
        public string Sc_Descripcion { get; set; }

        [StringLength(500)]
        public string Sc_Direccion { get; set; }

        public int Em_Cve_Empresa { get; set; }

        [Required]
        [StringLength(50)]
        public string Oper_Alta { get; set; }

        public DateTime Fecha_Alta { get; set; }

        [StringLength(50)]
        public string Oper_Ult_Modif { get; set; }

        public DateTime? Fecha_Ult_Modif { get; set; }

        [StringLength(50)]
        public string Oper_Baja { get; set; }

        public DateTime? Fecha_Baja { get; set; }

        public bool Estatus { get; set; }
    }
}
