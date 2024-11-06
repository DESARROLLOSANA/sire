using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;


namespace CRME.Models
{
    public class Cat_Horario
    {
        [Key]
        public int Id_Horario { get; set; }

        [StringLength(50)]
        public string Descripcion { get; set; }

        public DateTime Hora_Entrada { get; set; }

        public DateTime Hora_Salida { get; set; }

        [Required]
        [StringLength(50)]
        public string Oper_Alta { get; set; }

        public DateTime Fecha_Alta { get; set; }

        public bool Estatus { get; set; }
    }
}