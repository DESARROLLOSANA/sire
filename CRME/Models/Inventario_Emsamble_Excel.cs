using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRME.Models
{
    public class Inventario_Emsamble_Excel
    {
        public int Numero_Entrada { get; set; }

        public string Serie { get; set; }

        public string Marca {get; set;}

        public string Modelo { get; set; }

        public string Codigo_Inventario { get; set; }

        public string Estatus { get; set; }

    }
}