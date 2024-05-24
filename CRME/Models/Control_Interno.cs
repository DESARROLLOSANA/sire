using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRME.Models
{
    public class Control_Interno
    {

        public int id { get; set; }
        public string descripcion { get; set; }
        public int idTD { get; set; }
        public string version { get; set; }
        public string FechaEmision { get; set; }
        public string UltimaActu { get; set; }
        public string ControlCambios { get; set; }
        public string Indicadores { get; set; } // archivo fisico
        public string responsable { get; set; }
        public int Dp_cve_Departamento { get; set; }
        public int Em_Cve_Empresa { get; set; }
    }
}