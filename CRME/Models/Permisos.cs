namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class Permisos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_per { get; set; }
        public int empresa_ID { get; set; }
        public int User_ID { get; set; }
        public int modulo1_ID { get; set; }
        public int modulo2_ID { get; set; }
        public int cre { get; set; }
        public int rea { get; set; }
        public int del { get; set; }
        public int upd { get; set; }
    }
}