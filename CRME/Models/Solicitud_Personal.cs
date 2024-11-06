using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace CRME.Models
{
    public class Solicitud_Personal
    {
        [Key]
        public int Id_SolPers { get; set; }
        public int Id_TpSolicitud { get; set; }

        public int Id_Empresa { get; set; }
        public int Id_Sucursal { get; set; }
        public int Id_Departamento { get; set; }
        public int Id_Puesto { get; set; }
        public int Cantidad_Sol { get; set; }
        public int Id_Herramienta { get; set; }

        public DateTime Fecha_Inicio { get; set; }
        public DateTime Fecha_Fin { get; set; }


        [Required]
        [StringLength(50)]
        public string Oper_Alta { get; set; }

        public DateTime Fecha_Alta { get; set; }

        public bool Estatus { get; set; }
    }
}