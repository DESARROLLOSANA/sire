using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRME.Models
{
    public class Inventario_Lista_Moviles_Excel
    {
        public int? inv_movil_ID { get; set; }
        public string IMEI { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Precio { get; set; }
        public string Cod_Inventario { get; set; }
        public string Estatus { get; set; }

    }
}