namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Empresa")]
    public partial class Empresa
    {
        [Key]
        public int Em_Cve_Empresa { get; set; }

        [StringLength(100)]
        public string Em_Descripcion { get; set; }

        [StringLength(100)]
        public string Em_Razon_Social { get; set; }

        [StringLength(200)]
        public string Em_logo { get; set; }

        [StringLength(50)]
        public string Em_RFC { get; set; }

        [StringLength(300)]
        public string Em_Direccion { get; set; }

        [Required]
        [StringLength(50)]
        public string Oper_Alta { get; set; }

        public DateTime Fecha_Alta { get; set; }

        [StringLength(50)]
        public string Oper_Ult_Mod { get; set; }

        public DateTime? Fecha_Ult_Mod { get; set; }

        [StringLength(50)]
        public string Oper_Baja { get; set; }

        public DateTime? Fecha_Baja { get; set; }

        public bool Estatus { get; set; }
    }
}
