

namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class NivelAcademico
    {                   
        [Key]
        public long idNivelEstudio { get; set; }

        [StringLength(50)]
        public string desNivelEstudio { get; set; }  
        public string abrebiatura { get; set; }
        public bool Estatus { get; set; }
    }
}