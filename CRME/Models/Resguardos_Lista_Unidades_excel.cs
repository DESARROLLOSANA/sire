using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRME.Models
{
    public class Resguardos_Lista_Unidades_excel
    {
        public int? Resguardo_unidad_ID { get; set; }
        public DateTime Fecha { get; set; }
        public string Numero_economico { get; set; }
        public string Placas { get; set; }
        public string Tipo_de_caja { get; set; }
        public string Motor { get; set; }
        public string Chasis { get; set; }
        public string Area { get; set; }
        //public string Nombres { get; set; }
        //public string entrega { get; set; }
        public string Recibe { get; set; }
        public string Entregado_por { get; set; }
        public string Estatus { get; set; }

    }
}