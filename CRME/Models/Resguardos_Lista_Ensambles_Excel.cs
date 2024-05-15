using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRME.Models
{
    public class Resguardos_Lista_Ensambles_Excel
    {
        public int? Resguardo_ID { get; set; }
        public DateTime Fecha { get; set; }
        public string Serie_CPU { get; set; }
        public string Marca_ensamble { get; set; }
        public string Modelo_ensamble { get; set; }
        public string Ensamble_codigo { get; set; }
        public string Descripcion { get; set; }
        //datos monitor
        public string Serie_Monitor { get; set; }
        public string Marca_Monitor { get; set; }
        public string Modelo_Monitor { get; set; }
        public string Monitor_codigo { get; set; }
        public string Recibio { get; set; }
        public string Entregado_por { get; set; }
        public string Estatus { get; set; }

    }
}