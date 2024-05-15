using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRME.Models
{
    public class Resguardos_Lista_Vehiculos
    {
        public int? Resguardo_vehiculo_ID { get; set; }
        public DateTime Fecha { get; set; }
        public string Numero_economico { get; set; }
        public string Tipo { get; set; }
        public string Placas { get; set; }
        public string Nombres { get; set; }
        public string Estatus { get; set; }
        public string Ubicacion { get; set; }
        public string Departamento { get; set; }
        //public DateTime Vencimiento_licencia { get; set; }
        //añadidos para el autorcompletado
        public string Serie_motor { get; set; }
        public string Cuadro_chasis { get; set; }
        public string Anio { get; set; }
        public string Color { get; set; }
        public string Modelo { get; set; }
        public DateTime? Vigencia_del { get; set; }
        public DateTime? Vigencia_al { get; set; }
        public DateTime? Vigencia_tarjeta { get; set; }
        public string entrega { get; set; }
        public string Marca { get; set; }
        public string Poliza_seguro { get; set; }
        public string Inciso { get; set; }
        public string Tarjeta_circulacion { get; set; }
        public string Folio { get; set; }
        public string KM_recibe { get; set; }
        public string Licencia_manejo { get; set; }
        public DateTime? Vencimiento_licencia { get; set; }

    }
}