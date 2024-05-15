

namespace CRME.Models
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class Subfamilia
    {

        [Key]
        public int Id_Sub_fam { get; set; }
        public string Descripcion { get; set; }
        public int Id_Familia { get; set; }
    }
}