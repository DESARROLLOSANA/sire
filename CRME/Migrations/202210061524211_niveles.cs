namespace CRME.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class niveles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NivelAcademicoes",
                c => new
                    {
                        idNivelEstudio = c.Long(nullable: false, identity: true),
                        desNivelEstudio = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.idNivelEstudio);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.NivelAcademicoes");
        }
    }
}
