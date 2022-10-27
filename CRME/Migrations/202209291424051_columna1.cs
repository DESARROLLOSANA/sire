namespace CRME.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class columna1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.cat_sistemas", "Pu_Cve_Puesto", c => c.Int(nullable: false));
           // DropColumn("dbo.cat_sistemas", "Pu_Cve_Puesto");
        }
        
        public override void Down()
        {
            AddColumn("dbo.cat_sistemas", "Pu_Cve_Puesto", c => c.Int());
            DropColumn("dbo.cat_sistemas", "Pu_Cve_Pusto");
        }
    }
}
