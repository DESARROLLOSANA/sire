namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class inv_vehiculo_pdf_seguro
    {
        public int id { get; set; }

        public int inv_vehiculo_ID { get; set; }

        [StringLength(500)]
        public string url_pdf { get; set; }

        [StringLength(10)]
        public string type { get; set; }
    }
}
