using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRME.Models
{
	public class Resguardos_Lista_Mobiliario
	{
		public int? Resguardo_Mobiliario_ID { get; set; }
		public DateTime Fecha { get; set; }
		//desechar
		//public string Serie { get; set; }
		//public string Marca { get; set; }
		//public string Modelo { get; set; }
		// nuevos atributos
		public string Cod_inventario { get; set; }
		public string Mobiliario { get; set; }
		public string Color { get; set; }
		public string Ubicacion { get; set; }
		public string Departamento { get; set; }
		public string Nombres { get; set; }
		public string Estatus { get; set; }
		public string Estado { get; set; }
		//public string descripcion { get; set; }
		public string Caracteristicas { get; set; }
		public int Precio { get; set; }

		public string Proveedor { get; set; }
		
		//public string Departamento { get; set; }
		public DateTime? fecha_folio { get; set; }
		//public string Recibio { get; set; }
		public string entrega { get; set; }


	}
}