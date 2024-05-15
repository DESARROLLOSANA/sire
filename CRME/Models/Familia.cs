

namespace CRME.Models
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class Familia
    {

        [Key]
        public int Id_Familia { get; set; }
        public string Descripcion { get; set; }
    }
}