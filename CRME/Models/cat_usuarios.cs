namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class cat_usuarios
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public cat_usuarios()
        {
            cat_observaciones_movil = new HashSet<cat_observaciones_movil>();
        }

        [Key]
        public int usuario_ID { get; set; }

        [StringLength(70)]
        public string nombres { get; set; }

        [StringLength(25)]
        public string paterno { get; set; }

        [StringLength(25)]
        public string materno { get; set; }

        [StringLength(200)]
        public string nombre_completo { get; set; }
        
        public long idNivelEstudio { get; set; }

        public int? estatus_ID { get; set; }

        public int Pu_Cve_Puesto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cat_observaciones_movil> cat_observaciones_movil { get; set; }
    }
}
