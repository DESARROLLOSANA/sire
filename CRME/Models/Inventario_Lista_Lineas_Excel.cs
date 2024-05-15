using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRME.Models
{
    public class Inventario_Lista_Lineas_Excel
    {
        public int? ID { get; set; }
        public string No_linea { get; set; }
        public string Region { get; set; }
        public string Nombre_del_plan { get; set; }
        public string Renta_sin_iva { get; set; }
        public string Cuenta { get; set; }
        public string Razon_social { get; set; }
        //fecha
        public DateTime Fecha_inicio { get; set; }
        public DateTime Fecha_termino { get; set; }
        public string Estatus_adendum { get; set; }
        public string Ceco { get; set; }
        public string Ubicacion { get; set; }
        public string Departamento { get; set; }
        public string Estatus { get; set; }





    }
}