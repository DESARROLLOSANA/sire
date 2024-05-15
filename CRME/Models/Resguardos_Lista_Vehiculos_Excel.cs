using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRME.Models
{
    public class Resguardos_Lista_Vehiculos_Excel
    {
        public int? Resguardo_vehiculo_ID { get; set; }
        public DateTime Fecha { get; set; }
        public string Numero_economico { get; set; }
        public string Tipo { get; set; }
        public string Placas { get; set; }
        public string Nombres { get; set; }
        public string Estatus { get; set; }
        public string Ubicacion { get; set; }
        public string Departamento { get; set; }
    }
}