namespace CRME.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nivel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.cat_usuarios", "idNivelEstudio", c => c.Long(nullable: false));
            DropColumn("dbo.cat_usuarios", "puesto");
        }
        
        public override void Down()
        {
            AddColumn("dbo.cat_usuarios", "puesto", c => c.String(maxLength: 50));
            DropColumn("dbo.cat_usuarios", "idNivelEstudio");
        }
    }
}
