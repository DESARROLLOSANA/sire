using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRME.Models
{
    public class Resguardos_Lista_Lineas
    {
        public int? Inv_linea_ID { get; set; }
        public string Telefono { get; set; }
        public string Nombre_plan { get; set; }

        public string Renta_sin_iva { get; set; }
        public string Razon_social { get; set; }
        public string Cuenta { get; set; }
        public DateTime fecha_inicio { get; set; }
        public DateTime fecha_termino { get; set; }
        public string Ubicacion { get; set; }
        public string Departamento { get; set; }
        public string Estatus { get; set; }
        //anexados posteriormente
        public string Region { get; set; }
        public string Estatus_adendum { get; set; }

    }
}