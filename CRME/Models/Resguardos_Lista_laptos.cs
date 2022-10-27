using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRME.Models
{
    public class Resguardos_Lista_laptos
    {
		public int ? Resguardo_ID { get; set; }
		public DateTime  Fecha { get; set; }
		public string Serie { get; set; }
		public string Marca { get; set; }
		public string Modelo { get; set; }
		public string Color { get; set; }
		public string Cod_inventario { get; set; }
		public string Nombres { get; set; }
		public int ? Estatus_ID { get; set; }
		public string Estatus { get; set; }
		public string Proveedor { get; set; }
		public string descripcion { get; set; }
		public DateTime ? fecha_folio { get; set; }
		public string entrega { get; set; }

	}
}