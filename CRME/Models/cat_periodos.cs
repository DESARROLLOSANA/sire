namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class cat_periodos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int periodo_ID { get; set; }

        [StringLength(30)]
        public string periodo_des { get; set; }

    }
}