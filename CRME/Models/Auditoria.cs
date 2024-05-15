namespace CRME.Models
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class Auditoria
    {
        [Key]
        public int iD { get; set; }

        [StringLength(260)]
        public string modulo { get; set; }

        [StringLength(260)]
        public string accion { get; set; }

        public int idregistro { get; set; }

        [StringLength(260)]
        public string tabla { get; set; }

        public int idusuario { get; set; }

        public DateTime fecha { get; set; }
    }
}