using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRME.Models
{
    public class Acciones_correctivas
    {
        public int id { get; set; }
        public int Em_Cve_Empresa { get; set; }
        public string Proceso { get; set; }
        public string DesviacionDet { get; set; }
        public string ResponsableAC { get; set; }
        public string AccionCorrectiva { get; set; }
        public string FechaCompromiso { get; set; }
        public string Avance { get; set; }// archivo fisico
        public int Estatus { get; set; }       
        public string Evidencia { get; set; }//archivo
    }
}