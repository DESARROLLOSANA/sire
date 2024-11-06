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
    public class Tipo_Contratacion
    {
        [Key]
        public int Id_Tp_Contratacion { get; set; }

        [StringLength(100)]
        public string Descripcion { get; set; }

        public string Rango_Contrato { get; set; }

        public DateTime Fecha_Alta { get; set; }

        public bool Estatus { get; set; }
    }
}