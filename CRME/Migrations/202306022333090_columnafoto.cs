namespace CRME.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class columnafoto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.cat_sistemas", "foto", c => c.String(maxLength: 260));
        }
        
        public override void Down()
        {
            DropColumn("dbo.cat_sistemas", "foto");
        }
    }
}
