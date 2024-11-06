using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace CRME.Models
{
    public class Herramientas_Personal
    {
        [Key]
        public int Id_Herramienta { get; set; }
        public int Id_PerPuesto { get; set; }
        public int Id_SolPers { get; set; }
        public int Id_Empresa { get; set; }
        public int Id_Sucursal { get; set; }
        public int Id_Departamento { get; set; }
        public int Id_Puesto { get; set; }
        public int Tipo_Solicitud { get; set; }
        public string Tipo_Herramienta { get; set; }
        public string Descripcion { get; set; }
        public string Observaciones { get; set; }

        [Required]
        [StringLength(50)]
        public string Oper_Alta { get; set; }

        public DateTime Fecha_Alta { get; set; }

       
    }
}