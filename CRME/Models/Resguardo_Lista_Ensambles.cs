using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRME.Models
{
    public class Resguardo_Lista_Ensambles
    {
        public int? Resguardo_ID { get; set; }
        public DateTime fecha { get; set; }
        public string Serie_CPU { get; set; }
        public string Modelo_ensamble { get; set; }
        public string Marca_ensamble { get; set; }
        public string Ensamble_codigo { get; set; }
        public string Serie_monitor { get; set; }
        public string Modelo_monitor { get; set; }
        public string Nombres { get; set; }
        public string entrega { get; set; }
        public DateTime Fecha { get; set; }
        public string Estatus { get; set; }


    }
}