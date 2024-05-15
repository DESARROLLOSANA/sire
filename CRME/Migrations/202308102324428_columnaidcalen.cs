namespace CRME.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class columnaidcalen : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requerimiento", "Id_Calendario", c => c.Int());
            AlterColumn("dbo.Requerimiento", "Id_Solicitud", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Requerimientoes", "Id_Solicitud", c => c.Int(nullable: false));
            DropColumn("dbo.Requerimientoes", "Id_Calendario");
        }
    }
}
