using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace CRME.Models
{
    public class Documentacion_Emp
    {
        [Key]
        public int Id_Documenacion { get; set; }
        public int usuario_ID { get; set; }
        public string Rt_Solcitud_Empleo { get; set; }
        public string Rt_Acta_Nac { get; set; }
        public string Rt_Comprobande_Dom { get; set; }
        public string Rt_Imss { get; set; }
        public string Rt_Curp { get; set; }
        public string Rt_Cert_Est { get; set; }
        public string Rt_Cons_Laboral { get; set; }
        public string Rt_Cert_salud { get; set; }
        public string Rt_Ant_NOPenales { get; set; }
        public string Rt_Ret_Infonavit { get; set; }
        public string Rt_Sit_Fiscal { get; set; }
        public string Rt_Sit_Migratoria { get; set; }

        public DateTime Fecha_Alta { get; set; }

        public bool Estatus { get; set; }
    }
}