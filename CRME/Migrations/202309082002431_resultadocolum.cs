namespace CRME.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class resultadocolum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Solicitud_Mtto", "Resultado", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Solicitud_Mtto", "Resultado");
        }
    }
}
