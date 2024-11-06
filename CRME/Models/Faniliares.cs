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
    public class Faniliares
    {
        [Key]
        public int Id_Familiar { get; set; }
        public int usuario_ID { get; set; }
        public int Id_Parentesco { get; set; }

        [StringLength(70)]
        public string nombres { get; set; }

        [StringLength(25)]
        public string paterno { get; set; }

        [StringLength(25)]
        public string materno { get; set; }

        public string Correo { get; set; }

        public bool Beneficiario { get; set; }

        public string Telefono_Casa { get; set; }
        public string Telefono_Celular { get; set; }
        public string Direccion { get; set; }
        public int Edad { get; set; }

        public DateTime Fecha_Alta { get; set; }

        public bool Estatus { get; set; }
    }
}