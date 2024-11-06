using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace CRME.Models
{
    public class Puesto_Cargo
    {
        [Key]
        public int Id_Puesto_Cargo { get; set; }
        public int Id_PerPuesto { get; set; }
        public int Id_Puesto_Padre { get; set; }
        public int Id_Puesto_hijo{ get; set; }

        [Required]
        [StringLength(50)]
        public string Oper_Alta { get; set; }

        public DateTime Fecha_Alta { get; set; }

        public bool Estatus { get; set; }
    }
}