namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    //[Table("Cecos")]
    public class Ce_cos
    {
        //[Key]
        [Key]
        public int Ceco_ID { get; set; }

        [StringLength(10)]
        public string Ceco_Cve_Ceco { get; set; }

        [StringLength(150)]
        public string Ceco_Descripcion { get; set; }

        public int Em_Cve_Empresa { get; set; }

        public bool Estatus { get; set; }

    }
}