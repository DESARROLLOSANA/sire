namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class cat_indicadores
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int indicadores_ID { get; set; }

        public int proceso { get; set; }

        [StringLength(1000)]
        public string indicador { get; set; }

        [StringLength(1000)]
        public string form_cal { get; set;  }

        public float res_esp { get; set; }

        public int resp_med { get; set; }
        public int frec_med { get; set; }

        public int resp_mej { get; set; } 
        public int ene { get; set; }
        public int feb { get; set; }
        public int mar { get; set; }
        public int abr { get; set; }
        public int may { get; set; }
        public int jun { get; set; }
        public int jul { get; set; }
        public int ago { get; set; }
        public int sep { get; set; }
        public int oct { get; set; }
        public int nov { get; set; }
        public int dec { get; set; }

        public int por_cum { get; set; }

        public int year { get; set; }

        public int mes { get; set; }

        public int dia { get; set; }


    }
}