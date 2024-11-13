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

        [StringLength(150)]
        public string correo { get; set; }

        public int? Em_Cve_Empresa { get; set; }

        public int Dp_Cve_Departamento { get; set; }

        public int Sc_Cve_Sucursal { get; set; }

        public int Id_Horario { get; set; }

        public decimal Sueldo_Diario { get; set; }
        public decimal Sueldo_Mesual { get; set; }

        public string Numero_Empleado { get; set; }
        public string Registro_Petronal { get; set; }
        public string Ruta_Fotografia { get; set; }
        public string Ruta_Firma { get; set; }
        public string Curp { get; set; }
        public string Imms { get; set; }
        public string Fecha_Imms { get; set; }

        public DateTime Fecha_Nacimiento { get; set; }
        public string Lugar_Nacimiento { get; set; }
        public string Sodexo { get; set; }
        public string Infonavit { get; set; }
        public string Fonacot { get; set; }
        public string Ine { get; set; }
        public string Direccion { get; set; }
        public string CP { get; set; }
        public string Colonia { get; set; }
        public string Municipio { get; set; }
        public string Estado { get; set; }
        public string Nacinalidad { get; set; }
        public string Ubicacion { get; set; } // se revisa
        public string Telefono_Casa { get; set; }
        public string Telefono_Celular { get; set; }
        public int Id_EdoCivil { get; set; }
        public string Cuenta_Bancaria { get; set; }
        public string Numero_Cuenta { get; set; }
        public string Correo_Electronico { get; set; }

        public string Motivo_Alta { get; set; }
        public int Sustituye_a { get; set; }
        public DateTime Movimiento_Efectivo { get; set; }

        public DateTime Fecha_Baja { get; set; }
        public DateTime Fecha_ALta { get; set; }
        public string Usuario_Alta { get; set; }
        public string Tp_Contratacion { get; set; }
        public int    Id_Genero { get; set; }
        public int Edad { get; set; }
        public bool Disp_Viajar { get; set; }
        public bool Disp_Rotar { get; set; }
        public string Correo_Empresa { get; set; }
       
        [StringLength(13)]
        public string RFC { get; set; }
        public int Estatus { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cat_observaciones_movil> cat_observaciones_movil { get; set; }
    }
}
