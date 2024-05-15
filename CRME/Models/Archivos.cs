

namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class Archivos
    {
        [Key]
        public int Id_Archivo { get; set; }
        public int? Id_Requerimiento { get; set; }
        public int? Id_Solicitud { get; set; }
        [StringLength(500)]
        public string Ruta { get; set; }
    }
}