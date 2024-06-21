using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRME.Models
{
    public class Datos_Notificacion
    {
        public int id { get; set; }
        public string asignado { get; set; }
        public string periodo { get; set; }
        public int atraso { get; set; }
    }
}