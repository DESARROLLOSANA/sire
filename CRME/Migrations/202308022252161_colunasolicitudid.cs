namespace CRME.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class colunasolicitudid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Archivos", "Id_Solicitud", c => c.Int());
            AlterColumn("dbo.Archivos", "Id_Requerimiento", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Archivos", "Id_Requerimiento", c => c.Int(nullable: false));
            DropColumn("dbo.Archivos", "Id_Solicitud");
        }
    }
}
