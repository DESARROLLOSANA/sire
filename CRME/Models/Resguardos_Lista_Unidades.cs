using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRME.Models
{
    public class Resguardos_Lista_Unidades
    {
        public int? Resguardo_unidad_ID { get; set; }
        public DateTime Fecha { get; set; }
        public string Numero_economico { get; set; }
        public string Placas { get; set; }
        public string Motor { get; set; }
        public string Chasis { get; set; }
        public string Area { get; set; }
        public string Nombres { get; set; }
        public string Estatus { get; set; }

        //añadidos para completar la busqueda
        public string Marca_chasis { get; set; }
        public string Anio_chasis { get; set; }
        public string Numero_serie_adaptacion { get; set; }
        public string Numero_motor { get; set; }
        public string Numero_serie { get; set; }
        public string entrega { get; set; }

        //para el estatus
        //public int estatus_ID { get; set; }

    }
}