

namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class Solicitud_Mtto
    {
        [Key]
        public int Id_Solicitud { get; set; }

        [StringLength(500)]
        public string Sm_Descripcion { get; set; }

        public int Id_Tipo_Sol { get; set; }

        public int Id_Estado { get; set; }

        public int Em_Cve_Empresa { get; set; }

        public int Sc_Cve_Sucursal { get; set; }

        public string Motivo { get; set; }

        public string Resultado { get; set; }

    }
}