namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class Mope
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_mope { get; set; }
        public int modulo_ID { get; set; }
        public int permisos_ID { get; set; }
        public int User_ID { get; set; }
        public int Empresa_ID { get; set; }
    }
}