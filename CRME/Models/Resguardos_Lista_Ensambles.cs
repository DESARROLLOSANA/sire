using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRME.Models
{
    public class Resguardos_Lista_Ensambles
    {
        //se usa para construir la tabla
        public int? Resguardo_ID { get; set; }
        public DateTime Fecha { get; set; }
        //cpu
        public string Codigo_inventario_ensamble { get; set; }
        public string Serie_CPU { get; set; }
        public string Marca_ensamble { get; set; }
        public string Modelo_ensamble { get; set; }
        //monitor
        public string Codigo_inventario_monitor { get; set; }
        public string Serie_monitor { get; set; }
        public string Marca_monitor { get; set; }
        public string Modelo_monitor { get; set; }
        public string Nombres { get; set; }
        public string Estatus { get; set; }

        //agregados por busqueda se serie
        public string Descripcion_ensambles { get; set; }
        public string Color_ensambles { get; set; }
        //para buscar 
        public string entrega { get; set; }


    }

}