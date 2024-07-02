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
    public class ResguardosHeramientasController : Controller
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


        public ActionResult SaveResguardo(long[] idAlumno, int id_recibe, int Id_entrega, int temporalidad, int tiempo, int cantidad, int id_recibeco )
         {

            var serializerCat = new JavaScriptSerializer();
            bool success = false;
            string mensajefound = "";
            var codigo = 0;
                
            if(db.cat_resguardo_herramientas.FirstOrDefault() == null)
            {
                codigo = 1;
            }
            else
            {
                codigo = db.cat_resguardo_herramientas.OrderByDescending(x => x.resguardo_mobiliario_ID).Take(1).SingleOrDefault().resguardo_mobiliario_ID;
            }
            int dato = 0;
            dato = codigo;
            if(dato == 0)
            {
                dato = 1;

            }
            else
            {
                dato = dato + 1;
            }
            try
            {
                foreach (var alumnno in idAlumno)
                {
                    try
                    {
                        cat_resguardo_herramientas resguar = new cat_resguardo_herramientas();
                        var cod_inventario = db.Equipo_Menor.Find(alumnno).Cod_Inventario;
                        resguar.fecha = DateTime.Now;
                        //resguar.inv_laptop_ID = db.inventario_laptop.FirstOrDefault(x => x.serie == resguardo.Cod_inventario).inv_laptop_ID;
                        resguar.resguardo_mobiliario_ID = dato;
                        resguar.inv_mobiliario_ID = db.Equipo_Menor.FirstOrDefault(x => x.Cod_Inventario == cod_inventario).Id;
                        resguar.recibe_usuario_ID = id_recibe;
                        resguar.recibe_cousuario_ID = id_recibeco;
                        resguar.entrega_usuario_ID = Id_entrega;
                        resguar.temporalidad = temporalidad;
                        resguar.tipo = tiempo;
                        resguar.cantidad = cantidad;
                        resguar.fechaperiodo = DateTime.Now;

                        resguar.estatus_ID = 2;

                        resguar.observaciones = "N/A";


                        db.cat_resguardo_herramientas.Add(resguar);

                        if (db.SaveChanges() > 0)
                        {
                            success = true;
                        }

                        Equipo_Menor invmob = db.Equipo_Menor.Find(db.Equipo_Menor.FirstOrDefault(x => x.Cod_Inventario == cod_inventario).Id);
                        invmob.estatus_ID = 2;
                        db.Entry(invmob).State = EntityState.Modified;
                        if (db.SaveChanges() > 0)
                        {
                            success = true;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message.ToString());
                        mensajefound = "Error Contacte al Administrador";
                    }
                }
            }
            catch (Exception ex)
            {
                mensajefound = ex.Message;
            }

            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);
        }      
        public ActionResult Devolver(Resguardos_lista_Em id)
        {
            var serializerCat = new JavaScriptSerializer();
            bool success = false;
            string mensajefound = "";
            try
            {
                List<cat_resguardo_herramientas> resuni = new List<cat_resguardo_herramientas>();
                resuni = db.Database.SqlQuery<cat_resguardo_herramientas>("Sp_Get_Datos_Resguardo_EMD @Id_mobiliario", new SqlParameter("@Id_mobiliario", id.Resguardo_Mobiliario_ID)).ToList();
                foreach(var idem in resuni)
                {
                    cat_resguardo_herramientas res = db.cat_resguardo_herramientas.FirstOrDefault(x => x.ID == idem.ID);
                    res.estatus_ID = 1;//cambiar el estatus del resguardo en la tabla
                    db.Entry(res).State = EntityState.Modified;
                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                    }

                    Equipo_Menor invmob = db.Equipo_Menor.Find(db.Equipo_Menor.FirstOrDefault(x => x.Id == idem.inv_mobiliario_ID).Id);
                    invmob.estatus_ID = 1;   //cambiar el estatus del inventario en la tabla
                    db.Entry(invmob).State = EntityState.Modified;
                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                mensajefound = ex.Message;
            }

            return Json(new { success = success, mensajefound}, JsonRequestBehavior.AllowGet);
        }
        //lista
        public ActionResult _TablaResguardos(int? page, string orden, string filtro)
        {
            const int pageSize = 10;
            int pageNumber = (page ?? 1);
            List<Resguardos_lista_Em> lista = new List<Resguardos_lista_Em>();
            try
            {
                // se cambio aqui el nombre aqui del procedimiento almacenado

                var resguardo = db.Database.SqlQuery<Resguardos_lista_Em>("Sp_Get_Resguardo_EM").ToList();

                if (string.IsNullOrEmpty(filtro))
                {
                    lista = resguardo.ToList();                    
                }
                else
                {
                    lista = resguardo.Where(x => x.Nombres.ToUpper().Contains(filtro.ToUpper().Trim())).ToList();

                    ViewBag.filtro = filtro;
                }
                return PartialView(lista.ToPagedList(pageNumber, pageSize));
            }
            catch
            {
                return PartialView();
            }
        }

        public ActionResult _TablaHerramientas( int? Dp_Cve_Departamento, string filtro)
        {
            const int pageSize = 50000;            
            List<Equipo_Menor> conductores = new List<Equipo_Menor>();
            if (Dp_Cve_Departamento != null)
            {
                if(string.IsNullOrEmpty(filtro))
                {
                    conductores = db.Equipo_Menor.Where(x => x.Dp_Cve_Departamento == Dp_Cve_Departamento && x.estatus_ID == 1).ToList();
                }
                else
                {
                    conductores = db.Equipo_Menor.Where(x => x.Dp_Cve_Departamento == Dp_Cve_Departamento && x.estatus_ID == 1 && x.Descripcion.ToUpper().Contains(filtro.ToUpper().Trim())).ToList();                    
                }
                
            }
            else
            {

                conductores = db.Equipo_Menor.Where(x => x.Dp_Cve_Departamento == Dp_Cve_Departamento && x.estatus_ID == 1).ToList();
            }
            return PartialView(conductores.ToPagedList(1, pageSize));
        }

        [HttpGet]
        public ActionResult GetSucursalesByEmpresa(int Em_Cve_Empresa)
        {
            var sucursales = db.Sucursal
                .Where(x => x.Estatus == true && x.Em_Cve_Empresa == Em_Cve_Empresa)
                .Select(x => new { Value = x.Sc_Cve_Sucursal, Text = x.Sc_Descripcion })
                .ToList();

            return Json(sucursales, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetdepartamentosBySucursal(int Sc_Cve_Sucursal)
        {
            var departamento = db.Departamentos
                .Where(x => x.Estatus == true && x.Sc_Cve_Sucursal == Sc_Cve_Sucursal)
                .Select(x => new { Value = x.Dp_Cve_Departamento, Text = x.Dp_Descripcion })
                .ToList();

            return Json(departamento, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportarExcel(int? creado)
        {
            bool success = false;
            string excelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            if (creado == 1)
            {
                // Aqui se debe cambiar por "Resguardos Mobiliario"
                var ruta = "~/Upload/Temporales/Descargas/" + "Resguardos Equipo menor - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx";
                return File(Url.Content(ruta), excelContentType, "Resguardos Equipo menor - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx");
            }
            // Aqui se debe cambiar por "Resguardos Mobiliario"
            if (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Resguardos Equipo menor - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx")))
            {
                System.IO.File.Delete(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Resguardos Equipo menor - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx"));
            }

            string savedFileName = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Resguardos Equipo menor - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx"));
            //FileStream stream = System.IO.File.Create(savedFileName);
            using (FileStream stream = new FileStream(savedFileName, FileMode.Create))
            {
                try
                {

                    var productos = db.Database.SqlQuery<Resguardos_lista_Em_Excel>("Sp_Get_Resguardo_EM_Excel").ToList();

                    using (var libro = new ExcelPackage())
                    {

                        var worksheet = libro.Workbook.Worksheets.Add("Resguardos Equipo menor");
                        #region titulo para poner la razon social de la empresa
                        worksheet.Cells["C3:I3"].Merge = true;
                        worksheet.Cells["C3:I3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["C3:I3"].Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                        var cell = worksheet.Cells["C3"];
                        cell.IsRichText = true;     // Cell contains RichText rather than basic values


                        var title = cell.RichText.Add("SIRE - ");
                        title.Bold = true;
                        title.FontName = "Calibri";
                        title.Size = 15;
                        title.Color = ColorTranslator.FromHtml("#44546A");
                        #endregion
                        #region titulo para el reporte
                        worksheet.Cells["C4:I4"].Merge = true;
                        worksheet.Cells["C4:I4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["C4:I4"].Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                        //worksheet.Cells["D4:I4"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                        //worksheet.Cells["D4:I4"].Style.Border.Bottom.Color.SetColor(Color.Blue);

                        var cellrs = worksheet.Cells["C4"];
                        cellrs.IsRichText = true;     // Cell contains RichText rather than basic values
                                                      //cell.Style.WrapText = true; // Required to honor new lines

                        var titlers = cellrs.RichText.Add("Resguardos de Equipo menor");
                        titlers.Bold = true;
                        titlers.FontName = "Calibri";
                        titlers.Size = 15;
                        titlers.Color = ColorTranslator.FromHtml("#44546A");
                        #endregion
                        #region llenado de la informacion
                        worksheet.Cells["C6"].LoadFromCollection(productos, PrintHeaders: true);
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
                        excelImage2.From.Column = 8;
                        //excelImage2.From.Column = 9;
                        excelImage2.From.Row = 0;
                        excelImage2.SetSize(150, 80);
                        // 2x2 px space for better alignment
                        excelImage2.From.ColumnOff = Pixel2MTU(2);
                        excelImage2.From.RowOff = Pixel2MTU(2);
                        #endregion

                        var tabla = worksheet.Tables.Add(new ExcelAddressBase(fromRow: 6, fromCol: 3, toRow: productos.Count + 6, toColumn: 8), "Resguardos");
                        tabla.ShowHeader = true;
                        tabla.TableStyle = TableStyles.Light6;
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
        public ActionResult VisualizarResguardo(long? tg)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "AccesoView");
            }

            return View();
        }
        //hacer cambio aqui
        public ActionResult _formularioResguardo(long? idresguardo)        
        {
            Resguardos_lista_Em resguardoMob = new Resguardos_lista_Em();           
            if (idresguardo != null)
            {
                ViewBag.edit = 1;                
                resguardoMob = db.Database.SqlQuery<Resguardos_lista_Em>("Sp_Get_Datos_Resguardo_EML @Id_mobiliario", new SqlParameter("@Id_mobiliario", idresguardo)).FirstOrDefault();
            }
            ViewBag.empresa = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion");
            ViewBag.sucursal = new SelectList("", "Sc_Cve_Sucursal", "Sc_Descripcion");
            ViewBag.departamento = new SelectList("", "Dp_Cve_Departamento", "Dp_Descripcion");

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Value = "1", Text = "Indefinido" });
            items.Add(new SelectListItem { Value = "2", Text = "Temporal" });

            List<SelectListItem> items2 = new List<SelectListItem>();
            items2.Add(new SelectListItem { Value = "1", Text = "DIA" });
            items2.Add(new SelectListItem { Value = "2", Text = "MES" });
            items2.Add(new SelectListItem { Value = "3", Text = "AÑO" });
            //items.Add(new SelectListItem { Value = "Corbase", Text = "Corbase" });
            //items.Add(new SelectListItem { Value = "Kanasín", Text = "Kanasín" });

            ViewBag.Tempralidad = new SelectList(items, "Value", "Text");
            ViewBag.Tiempo = new SelectList(items2, "Value", "Text");
            return PartialView(resguardoMob);

        }

        public ActionResult Report()
        {
            return PartialView();
        }

        public ActionResult Print(int? id)
        {
            //Este metodo se usa para crear el pdf
            //RespoonsivaSana responsiva = new RespoonsivaSana();

            //var idequipoMob = db.cat_resguardos_mobiliarios.Find(id).inv_mobiliario_ID;
            //var idequipoLAP = db.cat_resguardos_equipos.Find(id).inv_laptop_ID;

            //var MOB = db.inventario_mobiliario.Find(idequipoMob);
            //var LAP = db.inventario_laptop.Find(idequipoLAP);
            var resguardo = db.cat_resguardo_herramientas.FirstOrDefault(X=> X.resguardo_mobiliario_ID == id);
            //int empresa = MOB.Em_Cve_Empresa;

            //int empresa = LAP.Em_Cve_Empresa;
            var reporte = new ReportClass();
            //if(empresa == 1)
            //{
            //aqui debo cambiar el nombre del archivo para que se la responsiva de mobiliiario sana

            if(resguardo.temporalidad == 1)
            {
                reporte.FileName = Server.MapPath("~/Reportes/ResponsivaEm.rpt"); // segundo formato 
            }
            else
            {
                reporte.FileName = Server.MapPath("~/Reportes/ResponsivaEmTemp.rpt"); // segundo formato 
            }
            

           // reporte.FileName = Server.MapPath("~/Reportes/ResponsivaEmtemp.rpt"); // segundo formato
                                                                                  
            //reporte.FileName = Server.MapPath("/Reportes/RespoonsivaEcolsur.rpt");
            //}
            //else if(empresa == 2)
            //{
            //    reporte.FileName = Server.MapPath("/Reportes/RespoonsivaSana.rpt");
            //}
            //else
            //{
            //    reporte.FileName = Server.MapPath("/Reportes/RespoonsivaSau.rpt");
            //}
            //¿este archivo sirve para algo?
            //var rutafoto = Server.MapPath("/Upload/Sistema/ecolsur.png");
            reporte.Load();

            reporte.SetParameterValue("@Id_mobiliario", id);
            //reporte.SetParameterValue("@Id_resguardo", id);
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

        //requiere grandes cambios
        public JsonResult BuscarSerie(string cad)
        {
            List<ElementJsonObjectKeymob> lst = new List<ElementJsonObjectKeymob>();
            // cambiar el nombre aqui del procedimiento almacenado
            //var resguardo = db.Database.SqlQuery<Resguardos_Lista_laptos>("Sp_Get_Datos_Laptos").ToList();
            //presenta error aqui
            var resguardomob = db.Database.SqlQuery<Resguardos_Lista_Mobiliario>("Sp_Get_Datos_Mobiliario").ToList();

            lst = (from d in resguardomob
                   where d.Cod_inventario.ToUpper().Contains(cad.ToUpper())
                   select new ElementJsonObjectKeymob
                   {
                       /*
                       label = d.Serie,
                       Value = d.Serie,
                       //Mobiliario = d.Mobiliario,
                       Marca = d.Marca,
                       Modelo = d.Modelo,
                       Color = d.Color,
                       Cod_inventario = d.Cod_inventario,
                       Proveedor = d.Proveedor,
                       descripcion = d.descripcion,
                       fecha_folio = d.fecha_folio
                       */
                       label = d.Cod_inventario,
                       Value = d.Cod_inventario,
                       Mobiliario = d.Mobiliario,
                       Color = d.Color,
                       Caracteristicas = d.Caracteristicas,
                       Precio = d.Precio,
                       Ubicacion = d.Ubicacion,
                       Proveedor = d.Proveedor,
                       fecha_folio = d.fecha_folio
                   }).Take(5).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        //no requiere cambios
        //este funciona bien
        public JsonResult BuscarRecibe(string cad)
        {
            List<ElementJsonRecibeher> lst = new List<ElementJsonRecibeher>();

            lst = (from d in db.cat_usuarios
                   where d.nombre_completo.ToUpper().Contains(cad.ToUpper())
                   select new ElementJsonRecibeher
                   {
                       label = d.nombre_completo,
                       Value = d.nombre_completo,
                       idRecibe = d.usuario_ID

                   }).Take(5).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        //no requiere cambios
        // ya funciona bien
        public JsonResult BuscarEntrega(string cad)
        {
            List<ElementJsonEntregaher> lst = new List<ElementJsonEntregaher>();

            lst = (from d in db.cat_usuarios
                   where d.nombre_completo.ToUpper().Contains(cad.ToUpper())
                   select new ElementJsonEntregaher
                   {
                       label = d.nombre_completo,
                       Value = d.nombre_completo,
                       identrega = d.usuario_ID

                   }).Take(5).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
    }

    //no requiere cambios
    public class ElementJsonRecibeher
    {
        public string Value { get; set; }
        public object label { get; set; }
        public int idRecibe { get; set; }
        //no requiere cambios
    }
    public class ElementJsonEntregaher
    {
        public string Value { get; set; }
        public object label { get; set; }
        public int identrega { get; set; }
    }
    //hacer cambios aqui
    public class ElementJsonObjectKeyher
    {
        public object label { get; set; }
        public string Value { get; set; }
        public string Mobiliario { get; set; }
        public string Color { get; set; }
        //public string Proveedor { get; set; } 
        public string Caracteristicas { get; set; }
        public int Precio { get; set; }
        public string Ubicacion { get; set; }
        public DateTime? fecha_folio { get; set; }
        public string Proveedor { get; set; }


    }
}