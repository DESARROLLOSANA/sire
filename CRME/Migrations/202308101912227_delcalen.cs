namespace CRME.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class delcalen : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Calendarizacion");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Calendarizacion",
                c => new
                    {
                        Id_Calendario = c.Int(nullable: false, identity: true),
                        Id_Solicitud = c.Int(nullable: false),
                        inicio = c.DateTime(nullable: false),
                        Fin = c.DateTime(nullable: false),
                        Descripcion = c.String(maxLength: 500),
                        Id_Estado = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id_Calendario);
            
        }
    }
}
