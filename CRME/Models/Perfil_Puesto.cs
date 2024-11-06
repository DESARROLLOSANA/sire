using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace CRME.Models
{
    public class Perfil_Puesto
    {
        [Key]
        public int Id_PerPuesto { get; set; }

        [StringLength(50)]
        public string Descripcion { get; set; }

        public int Edad_Minima { get; set; }

        public int Edad_Maxima { get; set; }
        public bool Disp_Tiempo { get; set; }
        public bool Man_Maquinaria { get; set; }

        public string Otro_Inf { get; set; }
       

        public string Com_Escolaridad { get; set; }

        public string Com_Experiencia { get; set; }

        public string Com_Formacion { get; set; }

        public string Org_Ruta { get; set; }

        public string Genero { get; set; }
        public string Edo_Civil { get; set; }
        public string Escolaridad { get; set; }

        public bool Disponibilidad_Viajar { get; set; }
        public bool Conducir { get; set; }

        public int Id_Puesto_Cargo { get; set; }
        public int Id_Relacion_Puesto { get; set; }
        public int Id_Funcione_Puesto { get; set; }
        public int Id_Indicador { get; set; }
        public int Id_Resposbilidades { get; set; }
        public int Id_Horario { get; set; }

        [Required]
        [StringLength(50)]
        public string Oper_Alta { get; set; }

        public DateTime Fecha_Alta { get; set; }

        public bool Estatus { get; set; }
    }
}