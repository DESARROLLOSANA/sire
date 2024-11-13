namespace CRME.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cambiocat_usuariosrfc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.cat_usuarios", "RFC", c => c.String(maxLength: 13));
        }
        
        public override void Down()
        {
            DropColumn("dbo.cat_usuarios", "RFC");
        }
    }
}
