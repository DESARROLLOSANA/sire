

namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class Programacion
    {
        [Key]        
        public int Id_Programacion { get; set; }
        public int Id_Solicitud { get; set; }
        [StringLength(500)]
        public string Descripcion { get; set; }
        [Column(TypeName = "date")]
        public DateTime Fecha_Inicio { get; set; }
        [Column(TypeName = "date")]
        public DateTime Fecha_Fin { get; set; }
    }
}