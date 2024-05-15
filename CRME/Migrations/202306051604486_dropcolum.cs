namespace CRME.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dropcolum : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.cat_sistemas", "foto");
        }
        
        public override void Down()
        {
            AddColumn("dbo.cat_sistemas", "foto", c => c.String(maxLength: 260));
        }
    }
}
