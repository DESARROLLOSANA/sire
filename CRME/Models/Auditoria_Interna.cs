using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRME.Models
{
    public class Auditoria_Interna
    {
        public int id { get; set; }
        public int Em_Cve_Empresa { get; set; }        
        public string Certificacion { get; set; }
        public int Vigencia { get; set; }
        public string FechaAuditoria { get; set; }
        public string version { get; set; }        
        public string planAuditoria { get; set; }// archivo fisico
        public string Informe { get; set; }
        public string Resultado { get; set; } 
        public string FechaSigAudi { get; set; }
        public int planAccionNC { get; set; }//archivo
        
    }
}