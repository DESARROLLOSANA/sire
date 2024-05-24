namespace CRME.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class cat_sistemas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public cat_sistemas()
        {
            cat_fechamodificado_cargador = new HashSet<cat_fechamodificado_cargador>();
            cat_fechamodificado_ensamble = new HashSet<cat_fechamodificado_ensamble>();
            cat_fechamodificado_impresora = new HashSet<cat_fechamodificado_impresora>();
            cat_fechamodificado_laptop = new HashSet<cat_fechamodificado_laptop>();
            cat_fechamodificado_movil = new HashSet<cat_fechamodificado_movil>();
            cat_fechamodificado_nobreak = new HashSet<cat_fechamodificado_nobreak>();
        }

        [Key]
        public int sistemas_ID { get; set; }

        [StringLength(70)]
        public string nombre { get; set; }

        [StringLength(25)]
        public string apellido_paterno { get; set; }

        [StringLength(25)]
        public string apellido_materno { get; set; }

        [StringLength(15)]
        public string username { get; set; }

        [StringLength(150)]
        public string correo { get; set; }

        [StringLength(260)]
        public string password { get; set; }

        public int? perfil_ID { get; set; }

        public int? empresa_ID { get; set; }

        public int? estatus_ID { get; set; }

        public int Pu_Cve_Puesto { get; set; }
        

        [StringLength(260)]
        public string foto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cat_fechamodificado_cargador> cat_fechamodificado_cargador { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cat_fechamodificado_ensamble> cat_fechamodificado_ensamble { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cat_fechamodificado_impresora> cat_fechamodificado_impresora { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cat_fechamodificado_laptop> cat_fechamodificado_laptop { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cat_fechamodificado_movil> cat_fechamodificado_movil { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cat_fechamodificado_nobreak> cat_fechamodificado_nobreak { get; set; }

        public virtual cat_perfiles cat_perfiles { get; set; }

        #region Metodos
        public static void FakeHash()
        {
            BCrypt.Net.BCrypt.HashPassword("", 13);
        }

        public virtual bool CheckPassword(string pass)
        {
            return BCrypt.Net.BCrypt.Verify(pass, password);
        }
        public virtual void SetPassword(string pass)
        {
            password = BCrypt.Net.BCrypt.HashPassword(pass, 13);
        }
        #endregion
    }
}
