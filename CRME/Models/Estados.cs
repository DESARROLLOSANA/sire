

namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class Estados
    {
        [Key]
        public int Id_Estado { get; set; }
        [StringLength(260)]
        public string descripcion { get; set; }
    }
}