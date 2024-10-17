namespace CRME.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CambiosenCat_Usuarios : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.cat_usuarios", "correo", c => c.String(maxLength: 150));
            AddColumn("dbo.cat_usuarios", "Em_Cve_Empresa", c => c.Int());
            AddColumn("dbo.cat_usuarios", "Dp_Cve_Departamento", c => c.Int(nullable: false));
            AddColumn("dbo.cat_usuarios", "Sc_Cve_Sucursal", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.cat_usuarios", "Sc_Cve_Sucursal");
            DropColumn("dbo.cat_usuarios", "Dp_Cve_Departamento");
            DropColumn("dbo.cat_usuarios", "Em_Cve_Empresa");
            DropColumn("dbo.cat_usuarios", "correo");
        }
    }
}
