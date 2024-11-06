using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace CRME.Models
{
    public class Funciones_Puesto
    {
        [Key]
        public int Id_Fun_Puesto { get; set; }
        public int Id_Perfil { get; set; }

        
        public string Actividad { get; set; }
        
        public string Frecuencia { get; set; }

        [Required]
        [StringLength(50)]
        public string Oper_Alta { get; set; }

        public DateTime Fecha_Alta { get; set; }

        public bool Estatus { get; set; }
    }
}