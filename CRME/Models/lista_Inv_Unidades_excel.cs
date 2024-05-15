using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRME.Models
{
    public class lista_Inv_Unidades_excel
    {

        public int ID { get; set; }
        public string Numero_economico { get; set; }
        public string Tipo_caja { get; set; }
        public string Marca_chasis { get; set; }
        public string Tipo { get; set; }
        public string Capacidad { get; set; }
        public string Anio { get; set; }
        public string Serie { get; set; }
        public string Estatus { get; set; }
    }
}