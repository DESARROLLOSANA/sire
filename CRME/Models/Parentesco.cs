using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace CRME.Models
{
    public class Parentesco
    {
        [Key]
        public int Id_Parentesco { get; set; }

        [StringLength(100)]
        public string Descripcion { get; set; }

        public DateTime Fecha_Alta { get; set; }

        public bool Estatus { get; set; }
    }
}