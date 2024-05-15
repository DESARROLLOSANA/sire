using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRME.Models
{
    public class Resguardos_Lista_Moviles
    {
        public int? Resguardo_movil_ID { get; set; }
        public int? Inv_movil_ID { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Serie { get; set; }
        public string Nombres { get; set; }
        public string Cod_inventario { get; set; }
        public DateTime Fecha { get; set; }
        public string Telefono { get; set; }
        public string Ceco { get; set; }
        public string entrega { get; set; } 
        public string Estatus { get; set; }
        public string Departamento { get; set; }
        public string Ubicacion { get; set; }
        //añadidos
        public DateTime? fecha_folio { get; set; }
        public string Linea { get; set; }
        public int Precio { get; set; }
        public string Proveedor { get; set; }
    }
}