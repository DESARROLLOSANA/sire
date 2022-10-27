using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CRME.Models;
using PagedList;
using System.Threading.Tasks;
using System.IO;
using CRME.Helpers;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using OfficeOpenXml.Table;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CRME.Reportes;

namespace CRME.Controllers
{
    public class ResguardosViewController : Controller
    {
        // GET: ResguardosView
        private SIRE_Context db = new SIRE_Context();
        HelpersController helper = new HelpersController();
        SqlConnection conexion = new SqlConnection();

        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "AccesoView");
            }
            ViewBag.HiddenMenu = 1;
            return View();
        }
        public ActionResult SaveResguardo(Resguardos_Lista_laptos resguardo, int id_recibe, int Id_entrega)
        {

            var serializerCat = new JavaScriptSerializer();
            bool success = false;
            string mensajefound = "";
            try
            {
                var empresaequipo = db.inventario_laptop.FirstOrDefault(x => x.serie == resguardo.Serie).Em_Cve_Empresa;

                var puesto = db.cat_usuarios.FirstOrDefault(c => c.usuario_ID == id_recibe).Pu_Cve_Puesto;
                var dpto = db.Puestos.FirstOrDefault(b => b.Pu_Cve_Puesto == puesto).Dp_Cve_Departamento;
                var sucur = db.Departamentos.FirstOrDefault(a => a.Dp_Cve_Departamento == dpto).Sc_Cve_Sucursal;
                var empresa = db.Sucursal.FirstOrDefault(x => x.Sc_Cve_Sucursal == sucur).Em_Cve_Empresa;
                var empresarecibe = db.Empresa.FirstOrDefault(e=> e.Em_Cve_Empresa ==empresa ).Em_Cve_Empresa;

                if (empresaequipo != empresarecibe)
                {
                    mensajefound = "La empresa del equipo no coincide con la del usuario que recibe";                    
                }
                else
                {
                    cat_resguardos_equipos resguar = new cat_resguardos_equipos();
                    resguar.fecha = DateTime.Now;
                    resguar.inv_laptop_ID = db.inventario_laptop.FirstOrDefault(x => x.serie == resguardo.Serie).inv_laptop_ID;
                    resguar.recibe_usuario_ID = id_recibe;
                    resguar.entrega_usuario_ID = Id_entrega;
                    resguar.estatus_ID = 2;

                    db.cat_resguardos_equipos.Add(resguar);
                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                    }

                    inventario_laptop invlap = db.inventario_laptop.Find(db.inventario_laptop.FirstOrDefault(x => x.serie == resguardo.Serie).inv_laptop_ID);
                    invlap.estatus_ID = 2;
                    db.Entry(invlap).State = EntityState.Modified;
                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                    }
                }               
            }
            catch(Exception ex)
            {
                mensajefound = ex.Message;
            }
            
            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _TablaReguardos(int? page, string orden, string filtro)
        {
            const int pageSize = 10;
            int pageNumber = (page ?? 1);
            List<Resguardos_Lista_laptos> lista = new List<Resguardos_Lista_laptos>();
            try
            {
                var resguardo = db.Database.SqlQuery<Resguardos_Lista_laptos>("Sp_Get_Resguardos_Latop").ToList();
                 
                if (string.IsNullOrEmpty(filtro))
                {
                    #region condiones de ordenado              
                    if (orden == "Resguardo_ID" || string.IsNullOrEmpty(orden))
                    {
                        orden = "Resguardo_ID";
                        lista = resguardo.OrderBy(c => c.Resguardo_ID).ToList();
                    }
                    else if (orden == "Resguardo_IDdesc")
                        lista = resguardo.OrderByDescending(c => c.Resguardo_ID).ToList();
                    else if (orden == "Fecha")
                        lista = resguardo.OrderBy(c => c.Fecha).ToList();
                    else if (orden == "Fechadesc")
                        lista = resguardo.OrderByDescending(c => c.Fecha).ToList();
                    else if (orden == "Serie")
                        lista = resguardo.OrderBy(c => c.Serie).ToList();
                    else if (orden == "Seriedesc")
                        lista = resguardo.OrderByDescending(c => c.Serie).ToList();
                    else if (orden == "Marca")
                        lista = resguardo.OrderBy(c => c.Marca).ToList();
                    else if (orden == "Marcadesc")
                        lista = resguardo.OrderByDescending(c => c.Marca).ToList();
                    else if (orden == "Modelo")
                        lista = resguardo.OrderBy(c => c.Modelo).ToList();
                    else if (orden == "Modelodesc")
                        lista = resguardo.OrderByDescending(c => c.Modelo).ToList();
                    else if (orden == "Color")
                        lista = resguardo.OrderBy(c => c.Color).ToList();
                    else if (orden == "Colordesc")
                        lista = resguardo.OrderByDescending(c => c.Color).ToList();
                    else if (orden == "Cod_inventario")
                        lista = resguardo.OrderBy(c => c.Cod_inventario).ToList();
                    else if (orden == "Cod_inventariodesc")
                        lista = resguardo.OrderByDescending(c => c.Cod_inventario).ToList();
                    else if (orden == "Nombres")
                        lista = resguardo.OrderBy(c => c.Nombres).ToList();
                    else if (orden == "Nombresdesc")
                        lista = resguardo.OrderByDescending(c => c.Nombres).ToList();

                    ViewBag.order = orden;
                    #endregion
                }
                else
                {
                    lista = resguardo.Where(x => x.Nombres.ToUpper().Contains(filtro.ToUpper().Trim())
                    || x.Serie.ToUpper().Contains(filtro.ToUpper().Trim()) || x.Modelo.ToUpper().Contains(filtro.ToUpper().Trim())
                    || x.Marca.ToUpper().Contains(filtro.ToUpper().Trim()) || x.Color.ToUpper().Contains(filtro.ToUpper().Trim()) 
                    || x.Cod_inventario.ToUpper().Contains(filtro.ToUpper().Trim())).ToList();

                    ViewBag.filtro = filtro;
                }
                return PartialView(lista.ToPagedList(pageNumber,pageSize));                     
            }
            catch
            {
                return PartialView();
            }
        }
        public ActionResult ExportarExcel(int? creado)
        {
            bool success = false;
            string excelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            if (creado == 1)
            {
                var ruta = "~/Upload/Temporales/Descargas/" + "Resguardos laptops - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx";
                return File(Url.Content(ruta), excelContentType, "Resguardos laptops - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx");
                //return File(Url.Content(ruta), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Egresados.xlsx");
            }                       
            if (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Resguardos laptops - " + DateTime.Now.ToShortDateString().Replace("/","-") + ".xlsx")))
            {
                System.IO.File.Delete(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Resguardos laptops - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx"));
            }

            string savedFileName = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Resguardos laptops - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx"));
            //FileStream stream = System.IO.File.Create(savedFileName);
            using (FileStream stream = new FileStream(savedFileName, FileMode.Create))
            {
                try
                {
                    
                    var productos = db.Database.SqlQuery<resguardos_lista_laptos_excel>("Sp_Get_Resguardos_Latop_excel").ToList();
                    using (var libro = new ExcelPackage())
                    {
                        
                        var worksheet = libro.Workbook.Worksheets.Add("Resguardos laptops");
                        #region titulo para poner la razon social de la empresa
                        worksheet.Cells["D3:I3"].Merge = true;
                        worksheet.Cells["D3:I3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["D3:I3"].Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                        var cell = worksheet.Cells["D3"];
                        cell.IsRichText = true;     // Cell contains RichText rather than basic values
                        
                        
                        var title = cell.RichText.Add("SIRE - ");
                        title.Bold = true;
                        title.FontName = "Calibri";
                        title.Size = 15;
                        title.Color = ColorTranslator.FromHtml("#44546A");
                        #endregion
                        #region titulo para el reporte
                        worksheet.Cells["D4:I4"].Merge = true;
                        worksheet.Cells["D4:I4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["D4:I4"].Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                        //worksheet.Cells["D4:I4"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                        //worksheet.Cells["D4:I4"].Style.Border.Bottom.Color.SetColor(Color.Blue);

                        var cellrs = worksheet.Cells["D4"];
                        cellrs.IsRichText = true;     // Cell contains RichText rather than basic values
                                                    //cell.Style.WrapText = true; // Required to honor new lines

                        var titlers = cellrs.RichText.Add("Resguardos de Laptops");
                        titlers.Bold = true;
                        titlers.FontName = "Calibri";
                        titlers.Size = 15;
                        titlers.Color = ColorTranslator.FromHtml("#44546A");
                        #endregion
                        #region llenado de la informacion
                        worksheet.Cells["B6"].LoadFromCollection(productos, PrintHeaders: true);
                        for (var col = 1; col < productos.Count + 1; col++)
                        {
                            worksheet.Column(col).AutoFit();
                        }
                        #endregion
                        #region incrutacion de imagen
                        Image logo = Image.FromFile(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Sistema/cicloAmb.png"));

                        //get the image from disk                        
                        var excelImage = worksheet.Drawings.AddPicture("logo ciclo", logo);
                        //add the image to row 20, column E
                        excelImage.From.Column = 1;
                        excelImage.From.Row = 0;
                        excelImage.SetSize(150, 80);
                        // 2x2 px space for better alignment
                        excelImage.From.ColumnOff = Pixel2MTU(2);
                        excelImage.From.RowOff = Pixel2MTU(2);

                        Image logo2 = Image.FromFile(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Sistema/cicloAmb.png"));
                        //get the image from disk                        
                        var excelImage2 = worksheet.Drawings.AddPicture("logo empresa", logo2);
                        //add the image to row 20, column E
                        excelImage2.From.Column = 9;
                        excelImage2.From.Row = 0;
                        excelImage2.SetSize(150, 80);
                        // 2x2 px space for better alignment
                        excelImage2.From.ColumnOff = Pixel2MTU(2);
                        excelImage2.From.RowOff = Pixel2MTU(2);
                        #endregion
                        
                        var tabla = worksheet.Tables.Add(new ExcelAddressBase(fromRow: 6, fromCol: 2, toRow: productos.Count + 6, toColumn: 10), "Resguardos");
                        tabla.ShowHeader = true;
                        tabla.TableStyle = TableStyles.Light6;
                        //tabla.ShowTotal = true;
                        //libro.Workbook.Properties.Author = Auth.Usuario.ToString();
                        libro.Workbook.Properties.Company = "Ciclo ambiental";
                        libro.Workbook.Properties.Keywords = "Excel";
                        libro.SaveAs(stream);
                        success = true;

                    }
                    stream.Close();
                    stream.Dispose();
                }
                catch (Exception ex)                
                {
                    stream.Close();
                    stream.Dispose();
                }
               // return File(Url.Content(ruta), excelContentType, "Resguardos laptops - " + DateTime.Now.ToShortDateString() + ".xlsx");
            }
            return Json(new { success = success });
        }
        public ActionResult VisualizarResguardo(long ? tg)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "AccesoView");
            }

            return View();
        }
        public ActionResult _formularioResguardo(long? idresguardo)
        {
             Resguardos_Lista_laptos resguardo = new Resguardos_Lista_laptos();
            if (idresguardo != null)
            {
                ViewBag.edit = 1;
                resguardo = db.Database.SqlQuery<Resguardos_Lista_laptos>("Sp_Get_Datos_Resguardo_Laptop @Id_resguardo", new SqlParameter("@Id_resguardo", idresguardo)).FirstOrDefault();
            }            
            return PartialView(resguardo);
        }
        public ActionResult Report()
        {
            return PartialView();
        }
        public ActionResult Print(int ? id)
        {
            //RespoonsivaSana responsiva = new RespoonsivaSana();
            var idequipoLAP = db.cat_resguardos_equipos.Find(id).inv_laptop_ID;
            var LAP = db.inventario_laptop.Find(idequipoLAP);

            int empresa = LAP.Em_Cve_Empresa;
            var reporte = new ReportClass();
            //if(empresa == 1)
            //{
                reporte.FileName = Server.MapPath("/Reportes/RespoonsivaEcolsur.rpt");
            //}
            //else if(empresa == 2)
            //{
            //    reporte.FileName = Server.MapPath("/Reportes/RespoonsivaSana.rpt");
            //}
            //else
            //{
            //    reporte.FileName = Server.MapPath("/Reportes/RespoonsivaSau.rpt");
            //}
           
            var rutafoto = Server.MapPath("/Upload/Sistema/ecolsur.png"); 
            reporte.Load();

            reporte.SetParameterValue("@Id_resguardo", id);
            //reporte.SetParameterValue("@numlic", "OPB001TPA21-41");

            //reporte.FileName = Server.MapPath("/Reportes/RespoonsivaSana.rpt");
            // reporte.SetDataSource(id);
            //reporte.SetParameterValue("Foto", rutafoto);

            // Report connection
            var connInfo = ReportesConexion.GetConnectionInfo();
            TableLogOnInfo logonInfo = new TableLogOnInfo();
            Tables tables;
            tables = reporte.Database.Tables;
            foreach (Table table in tables)
            {
                logonInfo = table.LogOnInfo;
                logonInfo.ConnectionInfo = connInfo;
                table.ApplyLogOnInfo(logonInfo);
            }

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            //En PDF
            Stream stream = reporte.ExportToStream(ExportFormatType.PortableDocFormat);
            reporte.Dispose();
            reporte.Close();
            return new FileStreamResult(stream, "application/pdf");
        }
        public int Pixel2MTU(int pixels)
        {
            int mtus = pixels * 9525;
            return mtus;
        }
        public JsonResult BuscarSerie(string cad)
        {
            List<ElementJsonObjectKey> lst = new List<ElementJsonObjectKey>();
           
            var resguardo = db.Database.SqlQuery<Resguardos_Lista_laptos>("Sp_Get_Datos_Laptos").ToList();

            lst = (from d in resguardo
                   where d.Serie.ToUpper().Contains(cad.ToUpper())
                   select new ElementJsonObjectKey
                   {
                       label = d.Serie,
                       Value = d.Serie,
                       Marca = d.Marca,
                       Modelo = d.Modelo,
                       Color = d.Color,
                       Cod_inventario = d.Cod_inventario,
                       Proveedor = d.Proveedor,
                       descripcion = d.descripcion,
                       fecha_folio = d.fecha_folio
                   }).Take(5).ToList();         
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BuscarRecibe(string cad)
        {
            List<ElementJsonRecibe> lst = new List<ElementJsonRecibe>();

            lst = (from d in db.cat_usuarios
                   where d.nombre_completo.ToUpper().Contains(cad.ToUpper())
                   select new ElementJsonRecibe
                   {
                       label = d.nombre_completo,
                       Value = d.nombre_completo,
                       idRecibe = d.usuario_ID

                   }).Take(5).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BuscarEntrega(string cad)
        {
            List<ElementJsonEntrega> lst = new List<ElementJsonEntrega>();

            lst = (from d in db.cat_usuarios
                   where d.nombre_completo.ToUpper().Contains(cad.ToUpper())
                   select new ElementJsonEntrega
                   {
                       label = d.nombre_completo,
                       Value = d.nombre_completo,
                       identrega = d.usuario_ID

                   }).Take(5).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

    }
    public class ElementJsonRecibe
    {
        public string Value { get; set; }
        public object label { get; set; }
        public int idRecibe { get; set; }
    }
    public class ElementJsonEntrega
    {
        public string Value { get; set; }
        public object label { get; set; }
        public int identrega { get; set; }
    }
    public class ElementJsonObjectKey
    {
        public object label { get; set; }
        public string Value { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Color { get; set; }
        public string Cod_inventario { get; set; }
        public string Proveedor { get; set; }
        public string descripcion { get; set; }
        public DateTime? fecha_folio { get; set; }
    }
}