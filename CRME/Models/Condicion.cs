
namespace CRME.Models
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class Condicion
    {

        [Key]
        public int Id_condicion { get; set; }
        public string Descripcion { get; set; }
    }
}