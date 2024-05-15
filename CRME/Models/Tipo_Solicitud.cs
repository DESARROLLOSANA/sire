
namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class Tipo_Solicitud
    {
        [Key]
        public int Id_Tipo_Sol { get; set; }
        [StringLength(260)]
        public string Ts_Descripcion { get; set; }
    }
}