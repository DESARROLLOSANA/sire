

namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class ArchivosOrganigrama
    {
        [Key]
        public int Id_Archivo { get; set; }
        public int? Em_Cve_Empresa { get; set; }
        public string Ruta { get; set; }
    }
}