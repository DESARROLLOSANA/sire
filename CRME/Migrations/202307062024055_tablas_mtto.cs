namespace CRME.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tablas_mtto : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Archivos",
                c => new
                    {
                        Id_Archivo = c.Int(nullable: false, identity: true),
                        Id_Requerimiento = c.Int(nullable: false),
                        Ruta = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id_Archivo);
            
            CreateTable(
                "dbo.Estados",
                c => new
                    {
                        Id_Estado = c.Int(nullable: false, identity: true),
                        descripcion = c.String(maxLength: 260),
                    })
                .PrimaryKey(t => t.Id_Estado);
            
            CreateTable(
                "dbo.Programacion",
                c => new
                    {
                        Id_Programacion = c.Int(nullable: false, identity: true),
                        Id_Solicitud = c.Int(nullable: false),
                        Descripcion = c.String(maxLength: 500),
                        Fecha_Inicio = c.DateTime(nullable: false, storeType: "date"),
                        Fecha_Fin = c.DateTime(nullable: false, storeType: "date"),
                    })
                .PrimaryKey(t => t.Id_Programacion);
            
            CreateTable(
                "dbo.Requerimiento",
                c => new
                    {
                        Id_Requerimiento = c.Int(nullable: false, identity: true),
                        Id_Solicitud = c.Int(nullable: false),
                        Descripcion = c.String(maxLength: 500),
                        Id_Estado = c.Int(nullable: false),
                        Id_Proveedor = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Requerimiento);
            
            CreateTable(
                "dbo.Solicitud_Mtto",
                c => new
                    {
                        Id_Solicitud = c.Int(nullable: false, identity: true),
                        Sm_Descripcion = c.String(maxLength: 500),
                        Id_Tipo_Sol = c.Int(nullable: false),
                        Id_Estado = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Solicitud);
            
            CreateTable(
                "dbo.Tipo_Solicitud",
                c => new
                    {
                        Id_Tipo_Sol = c.Int(nullable: false, identity: true),
                        Ts_Descripcion = c.String(maxLength: 260),
                    })
                .PrimaryKey(t => t.Id_Tipo_Sol);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tipo_Solicitud");
            DropTable("dbo.Solicitud_Mtto");
            DropTable("dbo.Requerimiento");
            DropTable("dbo.Programacion");
            DropTable("dbo.Estados");
            DropTable("dbo.Archivos");
        }
    }
}
