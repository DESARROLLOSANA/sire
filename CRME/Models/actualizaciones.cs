using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace CRME.Models
{
    public class actualizaciones
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public int Dp_Cver_Departamento { get; set; }
        public DateTime Fecha { get; set; }
       
    }
} 