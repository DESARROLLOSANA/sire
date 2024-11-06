using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace CRME.Models
{
    public partial class SIRE_Context : DbContext
    {
        public SIRE_Context()
            : base("name=SIRE_Context1")
        {
        }
        //public virtual DbSet<Resguardos_Lista_laptos> Resguardos_Lista_laptos { get; set; }
        public virtual DbSet<cat_estatus_inv> cat_estatus_inv { get; set; }
        public virtual DbSet<cat_fechamodificado_cargador> cat_fechamodificado_cargador { get; set; }
        public virtual DbSet<cat_fechamodificado_ensamble> cat_fechamodificado_ensamble { get; set; }
        public virtual DbSet<cat_fechamodificado_impresora> cat_fechamodificado_impresora { get; set; }
        public virtual DbSet<cat_fechamodificado_laptop> cat_fechamodificado_laptop { get; set; }
        public virtual DbSet<cat_fechamodificado_movil> cat_fechamodificado_movil { get; set; }
        public virtual DbSet<cat_fechamodificado_nobreak> cat_fechamodificado_nobreak { get; set; }
        public virtual DbSet<cat_km_actuales_vehiculos> cat_km_actuales_vehiculos { get; set; }
        public virtual DbSet<cat_km_proximo_vehiculos> cat_km_proximo_vehiculos { get; set; }
        public virtual DbSet<cat_mantenimiento_vehiculos> cat_mantenimiento_vehiculos { get; set; }
        public virtual DbSet<cat_observaciones_movil> cat_observaciones_movil { get; set; }
        public virtual DbSet<cat_perfiles> cat_perfiles { get; set; }
        public virtual DbSet<cat_proveedores> cat_proveedores { get; set; }
        public virtual DbSet<cat_resguardos_cargadores> cat_resguardos_cargadores { get; set; }
        public virtual DbSet<cat_resguardos_equipos> cat_resguardos_equipos { get; set; }
        public virtual DbSet<cat_resguardos_impresoras> cat_resguardos_impresoras { get; set; }
        public virtual DbSet<cat_resguardos_mobiliarios> cat_resguardos_mobiliarios { get; set; }
        public virtual DbSet<cat_resguardos_moviles> cat_resguardos_moviles { get; set; }
        public virtual DbSet<cat_resguardos_nobreaks> cat_resguardos_nobreaks { get; set; }
        public virtual DbSet<cat_resguardos_unidades> cat_resguardos_unidades { get; set; }
        public virtual DbSet<cat_resguardos_vehiculos> cat_resguardos_vehiculos { get; set; }
        public virtual DbSet<cat_sistemas> cat_sistemas { get; set; }
        public virtual DbSet<cat_usuarios> cat_usuarios { get; set; }
        public virtual DbSet<Departamentos> Departamentos { get; set; }
        public virtual DbSet<Empresa> Empresa { get; set; }
        public virtual DbSet<events> events { get; set; }
        public virtual DbSet<inv_vehiculo_pdf_seguro> inv_vehiculo_pdf_seguro { get; set; }
        public virtual DbSet<inv_vehiculo_pdf_tarjeta_circulacion> inv_vehiculo_pdf_tarjeta_circulacion { get; set; }
        public virtual DbSet<inventario_cargador_impresora> inventario_cargador_impresora { get; set; }
        public virtual DbSet<inventario_ensamble> inventario_ensamble { get; set; }
        public virtual DbSet<inventario_impresora> inventario_impresora { get; set; }
        public virtual DbSet<inventario_laptop> inventario_laptop { get; set; }
        public virtual DbSet<inventario_lineas> inventario_lineas { get; set; }
        public virtual DbSet<inventario_mobiliario> inventario_mobiliario { get; set; }
        public virtual DbSet<inventario_monitor> inventario_monitor { get; set; }
        public virtual DbSet<inventario_movil> inventario_movil { get; set; }
        public virtual DbSet<inventario_nobreak> inventario_nobreak { get; set; }
        public virtual DbSet<inventario_tipo_mobiliario> inventario_tipo_mobiliario { get; set; }
        public virtual DbSet<inventario_unidades> inventario_unidades { get; set; }
        public virtual DbSet<inventario_vehiculos> inventario_vehiculos { get; set; }
        public virtual DbSet<Puestos> Puestos { get; set; }
        public virtual DbSet<Sucursal> Sucursal { get; set; }
        public virtual DbSet<Ce_cos> Ce_cos { get; set; }
        public virtual DbSet<Auditoria> Auditoria { get; set; }
        public virtual DbSet<Cat_Estatus> Cat_Estatus { get; set; }
        public virtual DbSet<NivelAcademico> NivelAcademico { get; set; }

        // neuvas tablas mtto solicitud
        public virtual DbSet<Solicitud_Mtto> Solicitud_Mtto { get; set; }
        public virtual DbSet<Tipo_Solicitud> Tipo_Solicitud { get; set; }
        public virtual DbSet<Estados> Estados { get; set; }
        public virtual DbSet<Archivos> Archivos { get; set; }
        public virtual DbSet<Requerimiento> Requerimiento { get; set; }
        public virtual DbSet<Programacion> Programacion { get; set; }
        public virtual DbSet<Calendario> Calendario { get; set; }

        // nuevas tablas herramientas

        public virtual DbSet<Equipo_Menor> Equipo_Menor { get; set; }
        public virtual DbSet<Familia> Familia { get; set; }
        public virtual DbSet<Subfamilia> Subfamilia { get; set; }
        public virtual DbSet<Condicion> Condicion { get; set; }
        public virtual DbSet<Medida> Medida { get; set; }
        public virtual DbSet<cat_resguardo_herramientas> cat_resguardo_herramientas { get; set; }
        public virtual DbSet<Unidad_Medida> Unidad_Medida { get; set; }

        //TABLAS DEL SGC

        public virtual DbSet<Procesos> Procesos { get; set; }
        public virtual DbSet<TipoDocumento> TipoDocumento { get; set; }
        public virtual DbSet<cat_indicadores> cat_indicadores { get; set; }
        public virtual DbSet<cat_periodos> cat_periodos { get; set; }
        public virtual DbSet<Control_Interno> Control_Interno { get; set; }
        public virtual DbSet<Auditoria_Interna> Auditoria_Interna { get; set; }
        public virtual DbSet<Acciones_correctivas> Acciones_correctivas { get; set; }
        public virtual DbSet<ArchivosOrganigrama> ArchivosOrganigrama { get; set; }
        public virtual DbSet<Doc_apoyo> Doc_apoyo { get; set; }
        public virtual DbSet<Permisos> Permisos { get; set; }
        public virtual DbSet<EstadosAC> EstadosAC { get; set; }
        public virtual DbSet<Mope> Mope { get; set; }
        public virtual DbSet<modulos> modulos { get; set; }
        public virtual DbSet<cat_perm> cat_perms { get; set; }
        public virtual DbSet<actualizaciones> actualizaciones { get; set; }
        public virtual DbSet<SolicitudActualizaciones> SolicitudActualizaciones { get; set; }
        public virtual DbSet<Correos_Calidad> Correos_Calidad { get; set; }

        //TABLAS DEL SRH

        public virtual DbSet<Responsabilidades> Responsabilidades { get; set; }
        public virtual DbSet<Req_Personal> Req_Personal { get; set; }
        public virtual DbSet<Relacion_Puesto> Relacion_Puesto { get; set; }
        public virtual DbSet<Perfil_Puesto> Perfil_Puesto { get; set; }
        public virtual DbSet<Origen_Reclutamiento> Origen_Reclutamiento { get; set; }
        public virtual DbSet<Indicador_Puesto> Indicador_Puesto { get; set; }
        public virtual DbSet<Historial_Emp> Historial_Emp { get; set; }
        public virtual DbSet<Herramientas_Personal> Herramientas_Personal { get; set; }
        public virtual DbSet<Funciones_Puesto> Funciones_Puesto { get; set; }
        public virtual DbSet<Fecha_Entrevista> Fecha_Entrevista { get; set; }
        public virtual DbSet<Cat_Horario> Cat_Horario { get; set; }
        public virtual DbSet<Tipo_Contratacion> Tipo_Contratacion { get; set; }
        public virtual DbSet<Cat_Genero> Cat_Genero { get; set; }
        public virtual DbSet<Cat_EdoCivil> Cat_EdoCivil { get; set; }
        public virtual DbSet<Documentacion_Emp> Documentacion_Emp { get; set; }
        public virtual DbSet<Faniliares> Faniliares { get; set; }
        public virtual DbSet<Parentesco> Parentesco { get; set; }
        public virtual DbSet<Candidatos> Candidatos { get; set; }
        public virtual DbSet<Contactos_Empleado> Contactos_Empleado { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Departamentos>()
                .Property(e => e.Dp_Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Departamentos>()
                .Property(e => e.Oper_Alta)
                .IsUnicode(false);

            modelBuilder.Entity<Departamentos>()
                .Property(e => e.Oper_Ult_Modf)
                .IsUnicode(false);

            modelBuilder.Entity<Departamentos>()
                .Property(e => e.Oper_Baja)
                .IsUnicode(false);

            modelBuilder.Entity<Empresa>()
                .Property(e => e.Em_Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Empresa>()
                .Property(e => e.Em_Razon_Social)
                .IsUnicode(false);

            modelBuilder.Entity<Empresa>()
                .Property(e => e.Em_logo)
                .IsUnicode(false);

            modelBuilder.Entity<Empresa>()
                .Property(e => e.Em_RFC)
                .IsUnicode(false);

            modelBuilder.Entity<Empresa>()
                .Property(e => e.Em_Direccion)
                .IsUnicode(false);

            modelBuilder.Entity<Empresa>()
                .Property(e => e.Oper_Alta)
                .IsUnicode(false);

            modelBuilder.Entity<Empresa>()
                .Property(e => e.Oper_Ult_Mod)
                .IsUnicode(false);

            modelBuilder.Entity<Empresa>()
                .Property(e => e.Oper_Baja)
                .IsUnicode(false);

            modelBuilder.Entity<events>()
                .Property(e => e.start)
                .HasPrecision(0);

            modelBuilder.Entity<events>()
                .Property(e => e.end)
                .HasPrecision(0);

            modelBuilder.Entity<inv_vehiculo_pdf_seguro>()
                .Property(e => e.url_pdf)
                .IsUnicode(false);

            modelBuilder.Entity<inv_vehiculo_pdf_seguro>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<inv_vehiculo_pdf_tarjeta_circulacion>()
                .Property(e => e.url_pdf)
                .IsUnicode(false);

            modelBuilder.Entity<inv_vehiculo_pdf_tarjeta_circulacion>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<Puestos>()
                .Property(e => e.Pu_Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Puestos>()
                .Property(e => e.Oper_Alta)
                .IsUnicode(false);

            modelBuilder.Entity<Puestos>()
                .Property(e => e.Oper_Ult_Modf)
                .IsUnicode(false);

            modelBuilder.Entity<Puestos>()
                .Property(e => e.Oper_Baja)
                .IsUnicode(false);

            modelBuilder.Entity<Sucursal>()
                .Property(e => e.Sc_Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Sucursal>()
                .Property(e => e.Sc_Direccion)
                .IsUnicode(false);

            modelBuilder.Entity<Sucursal>()
                .Property(e => e.Oper_Alta)
                .IsUnicode(false);

            modelBuilder.Entity<Sucursal>()
                .Property(e => e.Oper_Ult_Modif)
                .IsUnicode(false);

            modelBuilder.Entity<Sucursal>()
                .Property(e => e.Oper_Baja)
                .IsUnicode(false);
        }
    }
}
