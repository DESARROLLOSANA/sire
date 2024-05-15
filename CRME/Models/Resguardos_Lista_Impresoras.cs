using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRME.Models
{
    public class Resguardos_Lista_Impresoras
    {
        public int? Resguardo_Impresora_ID { get; set; }
        public DateTime Fecha { get; set; }
        public string Cod_inventario { get; set; }
        public string Serie { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Nombres { get; set; }
        public string Estatus { get; set; }
        //añadidos para el formulario
        public string entrega { get; set; }
        public string Parte { get; set; }
        public int Precio { get; set; }
        public string Proveedor { get; set; }

        public DateTime? fecha_folio { get; set; }
    }
}