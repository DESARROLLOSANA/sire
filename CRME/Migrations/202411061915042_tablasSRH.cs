namespace CRME.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tablasSRH : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Candidatos",
                c => new
                    {
                        Id_Candidato = c.Int(nullable: false, identity: true),
                        Id_Requisicion = c.Int(nullable: false),
                        Tipo_Candidato = c.Int(nullable: false),
                        Nombre = c.String(),
                        Apellido_Paterno = c.String(),
                        Apellido_materno = c.String(),
                        Cv_Ruta = c.String(),
                        Direccion = c.String(),
                        CP = c.String(),
                        Colonia = c.String(),
                        Municipio = c.String(),
                        Estado = c.String(),
                        Nacionalidad = c.String(),
                        Correo = c.String(),
                        Tel_Fijo = c.String(),
                        Tel_Celular = c.String(),
                        Id_Reclutamiento = c.Int(nullable: false),
                        Oper_Alta = c.String(nullable: false, maxLength: 50),
                        Fecha_Alta = c.DateTime(nullable: false),
                        Estatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Candidato);
            
            CreateTable(
                "dbo.Cat_EdoCivil",
                c => new
                    {
                        Id_EdoCivil = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(maxLength: 50),
                        Oper_Alta = c.String(nullable: false, maxLength: 50),
                        Fecha_Alta = c.DateTime(nullable: false),
                        Estatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id_EdoCivil);
            
            CreateTable(
                "dbo.Cat_Genero",
                c => new
                    {
                        Id_Genero = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(maxLength: 50),
                        Oper_Alta = c.String(nullable: false, maxLength: 50),
                        Fecha_Alta = c.DateTime(nullable: false),
                        Estatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Genero);
            
            CreateTable(
                "dbo.Cat_Horario",
                c => new
                    {
                        Id_Horario = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(maxLength: 50),
                        Hora_Entrada = c.DateTime(nullable: false),
                        Hora_Salida = c.DateTime(nullable: false),
                        Oper_Alta = c.String(nullable: false, maxLength: 50),
                        Fecha_Alta = c.DateTime(nullable: false),
                        Estatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Horario);
            
            CreateTable(
                "dbo.Contactos_Empleado",
                c => new
                    {
                        Id_Contacto = c.Int(nullable: false, identity: true),
                        usuario_ID = c.Int(nullable: false),
                        nombres = c.String(maxLength: 70),
                        paterno = c.String(maxLength: 25),
                        materno = c.String(maxLength: 25),
                        Correo = c.String(),
                        Telefono_Casa = c.String(),
                        Telefono_Celular = c.String(),
                        Direccion = c.String(),
                        Edad = c.Int(nullable: false),
                        Fecha_Alta = c.DateTime(nullable: false),
                        Estatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Contacto);
            
            CreateTable(
                "dbo.Documentacion_Emp",
                c => new
                    {
                        Id_Documenacion = c.Int(nullable: false, identity: true),
                        usuario_ID = c.Int(nullable: false),
                        Rt_Solcitud_Empleo = c.String(),
                        Rt_Acta_Nac = c.String(),
                        Rt_Comprobande_Dom = c.String(),
                        Rt_Imss = c.String(),
                        Rt_Curp = c.String(),
                        Rt_Cert_Est = c.String(),
                        Rt_Cons_Laboral = c.String(),
                        Rt_Cert_salud = c.String(),
                        Rt_Ant_NOPenales = c.String(),
                        Rt_Ret_Infonavit = c.String(),
                        Rt_Sit_Fiscal = c.String(),
                        Rt_Sit_Migratoria = c.String(),
                        Fecha_Alta = c.DateTime(nullable: false),
                        Estatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Documenacion);
            
            CreateTable(
                "dbo.Faniliares",
                c => new
                    {
                        Id_Familiar = c.Int(nullable: false, identity: true),
                        usuario_ID = c.Int(nullable: false),
                        Id_Parentesco = c.Int(nullable: false),
                        nombres = c.String(maxLength: 70),
                        paterno = c.String(maxLength: 25),
                        materno = c.String(maxLength: 25),
                        Correo = c.String(),
                        Beneficiario = c.Boolean(nullable: false),
                        Telefono_Casa = c.String(),
                        Telefono_Celular = c.String(),
                        Direccion = c.String(),
                        Edad = c.Int(nullable: false),
                        Fecha_Alta = c.DateTime(nullable: false),
                        Estatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Familiar);
            
            CreateTable(
                "dbo.Fecha_Entrevista",
                c => new
                    {
                        Id_Fec_Entrevista = c.Int(nullable: false, identity: true),
                        Id_Candidato = c.Int(nullable: false),
                        Cita_Fecha = c.DateTime(nullable: false),
                        Cita_Hora = c.DateTime(nullable: false),
                        Entrevista = c.String(),
                        Comentarios = c.String(),
                        Oper_Alta = c.String(nullable: false, maxLength: 50),
                        Fecha_Alta = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Fec_Entrevista);
            
            CreateTable(
                "dbo.Funciones_Puesto",
                c => new
                    {
                        Id_Fun_Puesto = c.Int(nullable: false, identity: true),
                        Id_Perfil = c.Int(nullable: false),
                        Actividad = c.String(),
                        Frecuencia = c.String(),
                        Oper_Alta = c.String(nullable: false, maxLength: 50),
                        Fecha_Alta = c.DateTime(nullable: false),
                        Estatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Fun_Puesto);
            
            CreateTable(
                "dbo.Herramientas_Personal",
                c => new
                    {
                        Id_Herramienta = c.Int(nullable: false, identity: true),
                        Id_PerPuesto = c.Int(nullable: false),
                        Id_SolPers = c.Int(nullable: false),
                        Id_Empresa = c.Int(nullable: false),
                        Id_Sucursal = c.Int(nullable: false),
                        Id_Departamento = c.Int(nullable: false),
                        Id_Puesto = c.Int(nullable: false),
                        Tipo_Solicitud = c.Int(nullable: false),
                        Tipo_Herramienta = c.String(),
                        Descripcion = c.String(),
                        Observaciones = c.String(),
                        Oper_Alta = c.String(nullable: false, maxLength: 50),
                        Fecha_Alta = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Herramienta);
            
            CreateTable(
                "dbo.Historial_Emp",
                c => new
                    {
                        Id_Historial = c.Int(nullable: false, identity: true),
                        Id_Empleado = c.Int(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        Oper_Baja = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id_Historial);
            
            CreateTable(
                "dbo.Indicador_Puesto",
                c => new
                    {
                        Id_Ind_Puesto = c.Int(nullable: false, identity: true),
                        Id_Perfil = c.Int(nullable: false),
                        Nombre_Indicador = c.String(),
                        Medicion = c.String(),
                        Resultado = c.String(),
                        Frecuencia = c.String(),
                        Oper_Alta = c.String(nullable: false, maxLength: 50),
                        Fecha_Alta = c.DateTime(nullable: false),
                        Estatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Ind_Puesto);
            
            CreateTable(
                "dbo.Origen_Reclutamiento",
                c => new
                    {
                        Id_Reclutamiento = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(),
                        Oper_Alta = c.String(nullable: false, maxLength: 50),
                        Fecha_Alta = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Reclutamiento);
            
            CreateTable(
                "dbo.Parentescoes",
                c => new
                    {
                        Id_Parentesco = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(maxLength: 100),
                        Fecha_Alta = c.DateTime(nullable: false),
                        Estatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Parentesco);
            
            CreateTable(
                "dbo.Perfil_Puesto",
                c => new
                    {
                        Id_PerPuesto = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(maxLength: 50),
                        Edad_Minima = c.Int(nullable: false),
                        Edad_Maxima = c.Int(nullable: false),
                        Disp_Tiempo = c.Boolean(nullable: false),
                        Man_Maquinaria = c.Boolean(nullable: false),
                        Otro_Inf = c.String(),
                        Com_Escolaridad = c.String(),
                        Com_Experiencia = c.String(),
                        Com_Formacion = c.String(),
                        Org_Ruta = c.String(),
                        Genero = c.String(),
                        Edo_Civil = c.String(),
                        Escolaridad = c.String(),
                        Disponibilidad_Viajar = c.Boolean(nullable: false),
                        Conducir = c.Boolean(nullable: false),
                        Id_Puesto_Cargo = c.Int(nullable: false),
                        Id_Relacion_Puesto = c.Int(nullable: false),
                        Id_Funcione_Puesto = c.Int(nullable: false),
                        Id_Indicador = c.Int(nullable: false),
                        Id_Resposbilidades = c.Int(nullable: false),
                        Id_Horario = c.Int(nullable: false),
                        Oper_Alta = c.String(nullable: false, maxLength: 50),
                        Fecha_Alta = c.DateTime(nullable: false),
                        Estatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id_PerPuesto);
            
            CreateTable(
                "dbo.Relacion_Puesto",
                c => new
                    {
                        Id_Rel_Puesto = c.Int(nullable: false, identity: true),
                        Id_Perfil = c.Int(nullable: false),
                        Id_Departamento = c.Int(nullable: false),
                        Id_Puesto = c.Int(nullable: false),
                        Asunto_Com = c.String(),
                        Oper_Alta = c.String(nullable: false, maxLength: 50),
                        Fecha_Alta = c.DateTime(nullable: false),
                        Estatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Rel_Puesto);
            
            CreateTable(
                "dbo.Req_Personal",
                c => new
                    {
                        Id_Requisicion = c.Int(nullable: false, identity: true),
                        Id_SolPers = c.Int(nullable: false),
                        Id_Empresa = c.Int(nullable: false),
                        Id_Sucursal = c.Int(nullable: false),
                        Id_Departamento = c.Int(nullable: false),
                        Id_Puesto = c.Int(nullable: false),
                        Id_EmpleadoBaja = c.Int(nullable: false),
                        Sueldo_Diario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Sueldo_Mesual = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Cant_Vacaciones = c.Int(nullable: false),
                        Obsevaciones = c.String(),
                        Permanente = c.Boolean(nullable: false),
                        Becario = c.Boolean(nullable: false),
                        Temporal = c.Int(nullable: false),
                        Oper_Alta = c.String(nullable: false, maxLength: 50),
                        Fecha_Alta = c.DateTime(nullable: false),
                        Estatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Requisicion);
            
            CreateTable(
                "dbo.Responsabilidades",
                c => new
                    {
                        Id_Responsabilidad = c.Int(nullable: false, identity: true),
                        Id_Perfil = c.Int(nullable: false),
                        Descripcion = c.String(),
                        Oper_Alta = c.String(nullable: false, maxLength: 50),
                        Fecha_Alta = c.DateTime(nullable: false),
                        Estatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Responsabilidad);
            
            CreateTable(
                "dbo.Tipo_Contratacion",
                c => new
                    {
                        Id_Tp_Contratacion = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(maxLength: 100),
                        Rango_Contrato = c.String(),
                        Fecha_Alta = c.DateTime(nullable: false),
                        Estatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Tp_Contratacion);
            
            AddColumn("dbo.cat_usuarios", "Id_Horario", c => c.Int(nullable: false));
            AddColumn("dbo.cat_usuarios", "Sueldo_Diario", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.cat_usuarios", "Sueldo_Mesual", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.cat_usuarios", "Numero_Empleado", c => c.String());
            AddColumn("dbo.cat_usuarios", "Registro_Petronal", c => c.String());
            AddColumn("dbo.cat_usuarios", "Ruta_Fotografia", c => c.String());
            AddColumn("dbo.cat_usuarios", "Ruta_Firma", c => c.String());
            AddColumn("dbo.cat_usuarios", "Curp", c => c.String());
            AddColumn("dbo.cat_usuarios", "Imms", c => c.String());
            AddColumn("dbo.cat_usuarios", "Fecha_Imms", c => c.String());
            AddColumn("dbo.cat_usuarios", "Fecha_Nacimiento", c => c.DateTime(nullable: false));
            AddColumn("dbo.cat_usuarios", "Lugar_Nacimiento", c => c.String());
            AddColumn("dbo.cat_usuarios", "Sodexo", c => c.String());
            AddColumn("dbo.cat_usuarios", "Infonavit", c => c.String());
            AddColumn("dbo.cat_usuarios", "Fonacot", c => c.String());
            AddColumn("dbo.cat_usuarios", "Ine", c => c.String());
            AddColumn("dbo.cat_usuarios", "Direccion", c => c.String());
            AddColumn("dbo.cat_usuarios", "CP", c => c.String());
            AddColumn("dbo.cat_usuarios", "Colonia", c => c.String());
            AddColumn("dbo.cat_usuarios", "Municipio", c => c.String());
            AddColumn("dbo.cat_usuarios", "Estado", c => c.String());
            AddColumn("dbo.cat_usuarios", "Nacinalidad", c => c.String());
            AddColumn("dbo.cat_usuarios", "Ubicacion", c => c.String());
            AddColumn("dbo.cat_usuarios", "Telefono_Casa", c => c.String());
            AddColumn("dbo.cat_usuarios", "Telefono_Celular", c => c.String());
            AddColumn("dbo.cat_usuarios", "Id_EdoCivil", c => c.Int(nullable: false));
            AddColumn("dbo.cat_usuarios", "Cuenta_Bancaria", c => c.String());
            AddColumn("dbo.cat_usuarios", "Numero_Cuenta", c => c.String());
            AddColumn("dbo.cat_usuarios", "Correo_Electronico", c => c.String());
            AddColumn("dbo.cat_usuarios", "Motivo_Alta", c => c.String());
            AddColumn("dbo.cat_usuarios", "Sustituye_a", c => c.Int(nullable: false));
            AddColumn("dbo.cat_usuarios", "Movimiento_Efectivo", c => c.DateTime(nullable: false));
            AddColumn("dbo.cat_usuarios", "Fecha_Baja", c => c.DateTime(nullable: false));
            AddColumn("dbo.cat_usuarios", "Fecha_ALta", c => c.DateTime(nullable: false));
            AddColumn("dbo.cat_usuarios", "Usuario_Alta", c => c.String());
            AddColumn("dbo.cat_usuarios", "Tp_Contratacion", c => c.String());
            AddColumn("dbo.cat_usuarios", "Id_Genero", c => c.Int(nullable: false));
            AddColumn("dbo.cat_usuarios", "Edad", c => c.Int(nullable: false));
            AddColumn("dbo.cat_usuarios", "Disp_Viajar", c => c.Boolean(nullable: false));
            AddColumn("dbo.cat_usuarios", "Disp_Rotar", c => c.Boolean(nullable: false));
            AddColumn("dbo.cat_usuarios", "Correo_Empresa", c => c.String());
            AddColumn("dbo.cat_usuarios", "Estatus", c => c.Int(nullable: false));
            AddColumn("dbo.Puestos", "Plantilla", c => c.Int(nullable: false));
            AddColumn("dbo.Puestos", "Salario_Min", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Puestos", "Salario_Max", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Puestos", "Id_PerPuesto", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Puestos", "Id_PerPuesto");
            DropColumn("dbo.Puestos", "Salario_Max");
            DropColumn("dbo.Puestos", "Salario_Min");
            DropColumn("dbo.Puestos", "Plantilla");
            DropColumn("dbo.cat_usuarios", "Estatus");
            DropColumn("dbo.cat_usuarios", "Correo_Empresa");
            DropColumn("dbo.cat_usuarios", "Disp_Rotar");
            DropColumn("dbo.cat_usuarios", "Disp_Viajar");
            DropColumn("dbo.cat_usuarios", "Edad");
            DropColumn("dbo.cat_usuarios", "Id_Genero");
            DropColumn("dbo.cat_usuarios", "Tp_Contratacion");
            DropColumn("dbo.cat_usuarios", "Usuario_Alta");
            DropColumn("dbo.cat_usuarios", "Fecha_ALta");
            DropColumn("dbo.cat_usuarios", "Fecha_Baja");
            DropColumn("dbo.cat_usuarios", "Movimiento_Efectivo");
            DropColumn("dbo.cat_usuarios", "Sustituye_a");
            DropColumn("dbo.cat_usuarios", "Motivo_Alta");
            DropColumn("dbo.cat_usuarios", "Correo_Electronico");
            DropColumn("dbo.cat_usuarios", "Numero_Cuenta");
            DropColumn("dbo.cat_usuarios", "Cuenta_Bancaria");
            DropColumn("dbo.cat_usuarios", "Id_EdoCivil");
            DropColumn("dbo.cat_usuarios", "Telefono_Celular");
            DropColumn("dbo.cat_usuarios", "Telefono_Casa");
            DropColumn("dbo.cat_usuarios", "Ubicacion");
            DropColumn("dbo.cat_usuarios", "Nacinalidad");
            DropColumn("dbo.cat_usuarios", "Estado");
            DropColumn("dbo.cat_usuarios", "Municipio");
            DropColumn("dbo.cat_usuarios", "Colonia");
            DropColumn("dbo.cat_usuarios", "CP");
            DropColumn("dbo.cat_usuarios", "Direccion");
            DropColumn("dbo.cat_usuarios", "Ine");
            DropColumn("dbo.cat_usuarios", "Fonacot");
            DropColumn("dbo.cat_usuarios", "Infonavit");
            DropColumn("dbo.cat_usuarios", "Sodexo");
            DropColumn("dbo.cat_usuarios", "Lugar_Nacimiento");
            DropColumn("dbo.cat_usuarios", "Fecha_Nacimiento");
            DropColumn("dbo.cat_usuarios", "Fecha_Imms");
            DropColumn("dbo.cat_usuarios", "Imms");
            DropColumn("dbo.cat_usuarios", "Curp");
            DropColumn("dbo.cat_usuarios", "Ruta_Firma");
            DropColumn("dbo.cat_usuarios", "Ruta_Fotografia");
            DropColumn("dbo.cat_usuarios", "Registro_Petronal");
            DropColumn("dbo.cat_usuarios", "Numero_Empleado");
            DropColumn("dbo.cat_usuarios", "Sueldo_Mesual");
            DropColumn("dbo.cat_usuarios", "Sueldo_Diario");
            DropColumn("dbo.cat_usuarios", "Id_Horario");
            DropTable("dbo.Tipo_Contratacion");
            DropTable("dbo.Responsabilidades");
            DropTable("dbo.Req_Personal");
            DropTable("dbo.Relacion_Puesto");
            DropTable("dbo.Perfil_Puesto");
            DropTable("dbo.Parentescoes");
            DropTable("dbo.Origen_Reclutamiento");
            DropTable("dbo.Indicador_Puesto");
            DropTable("dbo.Historial_Emp");
            DropTable("dbo.Herramientas_Personal");
            DropTable("dbo.Funciones_Puesto");
            DropTable("dbo.Fecha_Entrevista");
            DropTable("dbo.Faniliares");
            DropTable("dbo.Documentacion_Emp");
            DropTable("dbo.Contactos_Empleado");
            DropTable("dbo.Cat_Horario");
            DropTable("dbo.Cat_Genero");
            DropTable("dbo.Cat_EdoCivil");
            DropTable("dbo.Candidatos");
        }
    }
}
