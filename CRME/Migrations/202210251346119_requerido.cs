namespace CRME.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requerido : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.inventario_mobiliario", "tipo_mobiliario_ID", c => c.Int(nullable: false));
            AlterColumn("dbo.inventario_mobiliario", "color", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.inventario_mobiliario", "tipo", c => c.String(nullable: false));
            AlterColumn("dbo.inventario_mobiliario", "precio", c => c.Int(nullable: false));
            AlterColumn("dbo.inventario_mobiliario", "proveedor_ID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.inventario_mobiliario", "proveedor_ID", c => c.Int());
            AlterColumn("dbo.inventario_mobiliario", "precio", c => c.Int());
            AlterColumn("dbo.inventario_mobiliario", "tipo", c => c.String());
            AlterColumn("dbo.inventario_mobiliario", "color", c => c.String(maxLength: 20));
            AlterColumn("dbo.inventario_mobiliario", "tipo_mobiliario_ID", c => c.Int());
        }
    }
}
