using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace CRME.Models
{
    public class Req_Personal
    {
        [Key]
        public int Id_Requisicion { get; set; }
        public int Id_SolPers { get; set; }
        public int Id_Empresa { get; set; }
        public int Id_Sucursal { get; set; }
        public int Id_Departamento { get; set; }
        public int Id_Puesto { get; set; }
        public int Id_EmpleadoBaja { get; set; } // solo si es por baja

        public decimal Sueldo_Diario { get; set; }
        public decimal Sueldo_Mesual { get; set; }

        public int Cant_Vacaciones { get; set; }
        public string Obsevaciones { get; set; }
        public bool Permanente { get; set; }
        public bool Becario { get; set; }
        public int Temporal { get; set; }

        [Required]
        [StringLength(50)]
        public string Oper_Alta { get; set; }

        public DateTime Fecha_Alta { get; set; }

        public bool Estatus { get; set; }
    }
}