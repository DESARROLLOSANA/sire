using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRME.Models
{
    public class Inventario_Lista_Laptops_Excel
    {
        public int? ID { get; set; }
        public string Serie { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Cod_inventario { get; set; }
        public string Descripcion { get; set; }
        public string Color { get; set; }
        public string Empresa { get; set; }
        public string Provedor { get; set; }
        public string Estatus { get; set; }

    }
}