using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRME.Models
{
    public class Solicitudes_Excel
    {
        public int id { get; set; }
        public string descripcion { get; set; }
        public string usuario { get; set; }
        public string Empresa { get; set; }
        public string sucursal { get; set; }
        public string Tipo { get; set; }
        public string estado { get; set; }
    }
}