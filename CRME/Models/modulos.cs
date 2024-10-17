namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class modulos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public modulos()
        {
            cat_sistemas = new HashSet<cat_sistemas>();
        }

        // TABLA PARA ASIGNAR LOS PERMISOS NECESARIOS POR USUARIO PARA CRUD
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        // LLAVE
        [StringLength(50)]
        public string modulo  { get; set; }
        //NOMBRE MODULO

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cat_sistemas> cat_sistemas { get; set; }

    }
}