using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRME.Models
{
    public class Resguardos_Lista_mobiliario_excel
    {
        public int? Resguardo_Mobiliario_ID { get; set; }
        public DateTime? Fecha { get; set; }
        public string Cod_inventario { get; set; }
        public string Mobiliario { get; set; }
        public string Color { get; set; }
        public string Ubicacion { get; set; }
        public string Departamento { get; set; }
        public int Precio { get; set; }
        public string Proveedor { get; set; }
        public string Nombres { get; set; }
        public string Estatus { get; set; }

    }
}