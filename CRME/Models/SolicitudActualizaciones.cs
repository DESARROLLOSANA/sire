using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace CRME.Models
{
    public class SolicitudActualizaciones
    {
        public int Id { get; set; }
        public string NombreSolicitante { get; set; }
        public int Pu_Cve_Puesto { get; set; }
        public string DescripcionSolicitud { get; set; }
    }
}