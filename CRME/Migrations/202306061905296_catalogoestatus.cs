namespace CRME.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class catalogoestatus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cat_Estatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Estatus = c.String(maxLength: 70),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Cat_Estatus");
        }
    }
}
