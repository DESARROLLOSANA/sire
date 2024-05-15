using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRME.Models
{
    public class Resguardos_Lista_Impresoras_Excel
    {
        public int? Resguardo_Impresora_ID { get; set; }
        public DateTime Fecha_de_Resguardo { get; set; }
        public string Codigo_de_inventario { get; set; }
        public string Serie { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Parte { get; set; }
        public int Precio { get; set; }
        //public string Nombre_Completo { get; set; }
        public string Recibe { get; set; }
        public string Entrega { get; set; }
        public string Estatus { get; set; }
    }
}