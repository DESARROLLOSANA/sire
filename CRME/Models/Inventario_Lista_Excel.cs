using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRME.Models
{
    public class Inventario_Lista_Excel
    {
        public int? inv_mobiliario_ID { get; set; }
        public string mobiliario { get; set; }
        public string tipo { get; set; }
        public string Cod_inventario { get; set; }
        public string folio { get; set; }
        public DateTime? Fecha { get; set; }
        public string Estatus { get; set; }
    }
}