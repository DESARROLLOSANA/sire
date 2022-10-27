namespace CRME.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class columnaVe : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.inventario_vehiculos", "Em_Cve_Empresa", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.inventario_vehiculos", "Em_Cve_Empresa");
        }
    }
}
