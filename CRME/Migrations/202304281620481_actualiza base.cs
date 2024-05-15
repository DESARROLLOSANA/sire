namespace CRME.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class actualizabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ce_cos",
                c => new
                    {
                        Ceco_ID = c.Int(nullable: false, identity: true),
                        Ceco_Cve_Ceco = c.String(maxLength: 10),
                        Ceco_Descripcion = c.String(maxLength: 150),
                        Em_Cve_Empresa = c.Int(nullable: false),
                        Estatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Ceco_ID);
            
            //AddColumn("dbo.Departamentos", "Ceco_ID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Departamentos", "Ceco_ID");
            DropTable("dbo.Ce_cos");
        }
    }
}
