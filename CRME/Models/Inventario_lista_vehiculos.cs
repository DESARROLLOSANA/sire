using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRME.Models
{
    public class Inventario_lista_vehiculos
    {
        public int inv_vehiculo_ID { get; set; }
        public string empresa { get; set; }
        public string tipo_vehiculo { get; set; }
        public string numero_economico_vehiculo { get; set; }
        public string serie_motor { get; set; }
        public string cuadro_chasis { get; set; }
        public string marca_motor { get; set; }
        public string anio { get; set; }
        public string color { get; set; }
        public string modelo { get; set; }
        public string placas { get; set; }
        public string poliza_seguro { get; set; }
        public DateTime? vigencia_del { get; set; }
        public DateTime? vigencia_al { get; set; }
        public string inciso { get; set; }
        public string tarjeta_circulacion { get; set; }
        public DateTime? vigencia_tarjeta { get; set; }
        public int? proveedor_ID { get; set; }
        public string folio { get; set; }
        public DateTime? fecha_folio { get; set; }
        public int? precio { get; set; }
        public int? estatus_ID { get; set; }
        public int Em_Cve_Empresa { get; set; }

        // valores para los archivos
        public string url_pdf_seguro { get; set; }
        public string url_pdf_tarjeta { get; set; }
        //
        public string empresa_gps { get; set; }
        public string imei_gps { get; set; }

    }
}