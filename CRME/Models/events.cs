namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class events
    {
        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string title { get; set; }

        [StringLength(7)]
        public string color { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime start { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? end { get; set; }
    }
}
