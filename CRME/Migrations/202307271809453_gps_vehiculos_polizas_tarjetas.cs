namespace CRME.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gps_vehiculos_polizas_tarjetas : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.inventario_unidades", "url_pdf_tarjeta", c => c.String(maxLength: 500));
            AddColumn("dbo.inventario_unidades", "type_tarjeta", c => c.String(maxLength: 10));
            AddColumn("dbo.inventario_unidades", "url_pdf_poliza", c => c.String(maxLength: 500));
            AddColumn("dbo.inventario_unidades", "type_poliza", c => c.String(maxLength: 10));
            AddColumn("dbo.inventario_vehiculos", "empresa_gps", c => c.String(maxLength: 10));
            AddColumn("dbo.inventario_vehiculos", "imei_gps", c => c.String(maxLength: 15));
            AddColumn("dbo.Solicitud_Mtto", "Em_Cve_Empresa", c => c.Int(nullable: false));
            AddColumn("dbo.Solicitud_Mtto", "Sc_Cve_Sucursal", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Solicitud_Mtto", "Sc_Cve_Sucursal");
            DropColumn("dbo.Solicitud_Mtto", "Em_Cve_Empresa");
            DropColumn("dbo.inventario_vehiculos", "imei_gps");
            DropColumn("dbo.inventario_vehiculos", "empresa_gps");
            DropColumn("dbo.inventario_unidades", "type_poliza");
            DropColumn("dbo.inventario_unidades", "url_pdf_poliza");
            DropColumn("dbo.inventario_unidades", "type_tarjeta");
            DropColumn("dbo.inventario_unidades", "url_pdf_tarjeta");
        }
    }
}
