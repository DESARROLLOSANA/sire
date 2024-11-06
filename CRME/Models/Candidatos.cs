using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace CRME.Models
{
    public class Candidatos
    {
        [Key]
        public int Id_Candidato { get; set; }
        public int Id_Requisicion { get; set; }
        public int Tipo_Candidato { get; set; }
        public string Nombre { get; set; }
        public string Apellido_Paterno { get; set; }
        public string Apellido_materno { get; set; }
        public string Cv_Ruta { get; set; }
        public string Direccion { get; set; }
        public string CP { get; set; }
        public string Colonia { get; set; }
        public string Municipio { get; set; }
        public string Estado { get; set; }
        public string Nacionalidad { get; set; }
        public string Correo { get; set; }
        public string Tel_Fijo { get; set; }
        public string Tel_Celular { get; set; }
        public int Id_Reclutamiento { get; set; }

        [Required]
        [StringLength(50)]
        public string Oper_Alta { get; set; }

        public DateTime Fecha_Alta { get; set; }

        public bool Estatus { get; set; }
    }
}