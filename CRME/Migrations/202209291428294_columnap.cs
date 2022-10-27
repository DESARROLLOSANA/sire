namespace CRME.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class columnap : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.cat_sistemas", "Pu_Cve_Puesto", c => c.Int(nullable: false));
            AddColumn("dbo.cat_usuarios", "Pu_Cve_Puesto", c => c.Int(nullable: false));
           // DropColumn("dbo.cat_sistemas", "Pu_Cve_Pusto");
        }
        
        public override void Down()
        {
            AddColumn("dbo.cat_sistemas", "Pu_Cve_Pusto", c => c.Int(nullable: false));
            DropColumn("dbo.cat_usuarios", "Pu_Cve_Puesto");
            DropColumn("dbo.cat_sistemas", "Pu_Cve_Puesto");
        }
    }
}
