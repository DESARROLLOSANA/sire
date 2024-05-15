namespace CRME.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class auditoria : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Auditorias",
                c => new
                    {
                        iD = c.Int(nullable: false, identity: true),
                        modulo = c.String(maxLength: 260),
                        accion = c.String(maxLength: 260),
                        idregistro = c.Int(nullable: false),
                        tabla = c.String(maxLength: 260),
                        idusuario = c.Int(nullable: false),
                        fecha = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.iD);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Auditorias");
        }
    }
}
