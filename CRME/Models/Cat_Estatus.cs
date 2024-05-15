namespace CRME.Models
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class Cat_Estatus
    {

        [Key]
        public int Id { get; set; }

        [StringLength(70)]
        public string Estatus { get; set; }
    }
}