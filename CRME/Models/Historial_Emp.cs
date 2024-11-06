using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace CRME.Models
{
    public class Historial_Emp
    {
        [Key]
        public int Id_Historial { get; set; }
        public int Id_Empleado { get; set; }
        public DateTime Fecha { get; set; }
        [StringLength(50)]
        public string Oper_Baja { get; set; }
    }
}