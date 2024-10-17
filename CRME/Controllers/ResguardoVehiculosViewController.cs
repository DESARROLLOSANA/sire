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
    public class ResguardoVehiculosViewController : Controller
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
        public ActionResult SaveResguardo(Resguardos_Lista_Vehiculos resguardo, int id_recibe, int Id_entrega)
        {

            var serializerCat = new JavaScriptSerializer();
            bool success = false;
            string mensajefound = "";
            
            try
            {
                #region verificacion
                //revisar los datos
                //puede presentar un error, ya que no existe la empresa en el lista de empresas 
                /*
                var empresamobiliario = db.inventario_mobiliario.FirstOrDefault(x => x.cod_inventario == resguardo.Cod_inventario).Em_Cve_Empresa;

                var puesto = db.cat_usuarios.FirstOrDefault(c => c.usuario_ID == id_recibe).Pu_Cve_Puesto;
                var dpto = db.Puestos.FirstOrDefault(b => b.Pu_Cve_Puesto == puesto).Dp_Cve_Departamento;
                var sucur = db.Departamentos.FirstOrDefault(a => a.Dp_Cve_Departamento == dpto).Sc_Cve_Sucursal;
                var empresa = db.Sucursal.FirstOrDefault(x => x.Sc_Cve_Sucursal == sucur).Em_Cve_Empresa;
                var empresarecibe = db.Empresa.FirstOrDefault(e => e.Em_Cve_Empresa == empresa).Em_Cve_Empresa;
                */
                //puede presentar un error, ya que no existe la empresa en el lista de empresas 
                //presenta un error porque no existe el puesto
                //if (empresamobiliario != empresarecibe)
                //{
                //  mensajefound = "La empresa del Mobiliario no coincide con la asiganada al usuario que recibe";
                //}
                //else
                //{
                #endregion

                    cat_resguardos_vehiculos resguar = new cat_resguardos_vehiculos();
                    resguar.fecha = DateTime.Now;
                    //resguar.inv_laptop_ID = db.inventario_laptop.FirstOrDefault(x => x.serie == resguardo.Cod_inventario).inv_laptop_ID;
                    resguar.inv_vehiculo_ID = db.inventario_vehiculos.FirstOrDefault(x => x.numero_economico_vehiculo == resguardo.Numero_economico && x.estatus_ID == 1).inv_vehiculo_ID;
                    resguar.recibe_usuario_ID = id_recibe;
                    resguar.entrega_usuario_ID = Id_entrega;
                    
                    resguar.estatus_ID = 2;
                    resguar.km_recibe = resguardo.KM_recibe;
                    
                    resguar.ubicacion = resguardo.Ubicacion;
                    resguar.departamento = resguardo.Departamento;

                    db.cat_resguardos_vehiculos.Add(resguar);

                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                    }

                    
                    inventario_vehiculos invmob = db.inventario_vehiculos.Find(db.inventario_vehiculos.FirstOrDefault(x =>x.numero_economico_vehiculo == resguardo.Numero_economico && x.estatus_ID == 1).inv_vehiculo_ID);
                    invmob.estatus_ID = 2;                    
                    db.Entry(invmob).State = EntityState.Modified;
                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                    }
               // }
            }
            catch (Exception ex)
            {
                mensajefound = ex.Message;
            }

            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Devolver(Resguardos_Lista_Vehiculos id)
        {
            var serializerCat = new JavaScriptSerializer();
            bool success = false;
            string mensajefound = "";
            try
            {
                cat_resguardos_vehiculos resuni = db.cat_resguardos_vehiculos.Find(db.cat_resguardos_vehiculos.FirstOrDefault(x => x.resguardo_vehiculo_ID == id.Resguardo_vehiculo_ID).resguardo_vehiculo_ID);
                resuni.estatus_ID = 1;//cambiar el estatus del resguardo en la tabla
                db.Entry(resuni).State = EntityState.Modified;
                if (db.SaveChanges() > 0)
                {
                    success = true;
                }

                inventario_vehiculos invmob = db.inventario_vehiculos.Find(db.inventario_vehiculos.FirstOrDefault(x => x.inv_vehiculo_ID == resuni.inv_vehiculo_ID).inv_vehiculo_ID);
                invmob.estatus_ID = 1;   //cambiar el estatus del inventario en la tabla
                db.Entry(invmob).State = EntityState.Modified;
                if (db.SaveChanges() > 0)
                {
                    success = true;
                }
            }
            catch (Exception ex)
            {
                mensajefound = ex.Message;
            }
            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);
        }




        //lista
        public ActionResult _TablaResguardos(int? page, string orden, string filtro)
        {
            const int pageSize = 10;
            int pageNumber = (page ?? 1);
            List<Resguardos_Lista_Vehiculos> lista = new List<Resguardos_Lista_Vehiculos>();
            try
            {
                // se cambio aqui el nombre aqui del procedimiento almacenado

                var resguardo = db.Database.SqlQuery<Resguardos_Lista_Vehiculos>("Sp_Get_Resguardos_Vehiculos").ToList();
               
                if (string.IsNullOrEmpty(filtro))
                {
                    #region condiones de ordenado              
                    if (orden == "Resguardo_vehiculo_ID" || string.IsNullOrEmpty(orden))
                    {
                        orden = "Resguardo_vehiculo_ID";
                        lista = resguardo.OrderBy(c => c.Resguardo_vehiculo_ID).ToList();
                    }
                    else if (orden == "Resguardo_vehiculo_IDdesc")
                        lista = resguardo.OrderByDescending(c => c.Resguardo_vehiculo_ID).ToList();
                    else if (orden == "Fecha")
                        lista = resguardo.OrderBy(c => c.Fecha).ToList();
                    else if (orden == "Fechadesc")
                        lista = resguardo.OrderByDescending(c => c.Fecha).ToList();
                    else if (orden == "Numero_economico")
                        lista = resguardo.OrderBy(c => c.Numero_economico).ToList();
                    else if (orden == "Numero_economicodesc")
                        lista = resguardo.OrderByDescending(c => c.Numero_economico).ToList();
                    else if (orden == "Tipo")
                        lista = resguardo.OrderBy(c => c.Tipo).ToList();
                    else if (orden == "Tipodesc")
                        lista = resguardo.OrderByDescending(c => c.Tipo).ToList();
                    
                    else if (orden == "Placas")
                        lista = resguardo.OrderBy(c => c.Placas).ToList();
                    else if (orden == "Placasdesc")
                        lista = resguardo.OrderByDescending(c => c.Placas).ToList();

                    else if (orden == "Ubicacion")
                        lista = resguardo.OrderBy(c => c.Ubicacion).ToList();
                    else if (orden == "Ubicaciondesc")
                        lista = resguardo.OrderByDescending(c => c.Ubicacion).ToList();

                    else if (orden == "Departamento")
                        lista = resguardo.OrderBy(c => c.Departamento).ToList();
                    else if (orden == "Departamentodesc")
                        lista = resguardo.OrderByDescending(c => c.Departamento).ToList();

                    else if (orden == "Nombres")
                        lista = resguardo.OrderBy(c => c.Nombres).ToList();
                    else if (orden == "Nombresdesc")
                        lista = resguardo.OrderByDescending(c => c.Nombres).ToList();

                    ViewBag.order = orden;
                    #endregion
                }
                else
                {
                    var filtroUpper = filtro.ToUpper().Trim();

                     lista = resguardo.Where(x =>
                        (x.Nombres ?? string.Empty).ToUpper().Contains(filtroUpper) ||
                        (x.Numero_economico ?? string.Empty).ToUpper().Contains(filtroUpper) ||
                        (x.Tipo ?? string.Empty).ToUpper().Contains(filtroUpper) ||
                        (x.Placas ?? string.Empty).ToUpper().Contains(filtroUpper) ||
                        (x.Ubicacion ?? string.Empty).ToUpper().Contains(filtroUpper) ||
                        (x.Departamento ?? string.Empty).ToUpper().Contains(filtroUpper)
                    ).ToList();

                    ViewBag.filtro = filtro;
                }
                return PartialView(lista.ToPagedList(pageNumber, pageSize));
            }
            catch
            {
                return PartialView();
            }
        }
        public ActionResult ExportarExcel(int? creado, string filtro)
        {
            bool success = false;
            string excelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            if (creado == 1)
            {
                // Aqui se debe cambiar por "Resguardos Mobiliario"
                var ruta = "~/Upload/Temporales/Descargas/" + "Resguardos Vehiculos - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx";
                return File(Url.Content(ruta), excelContentType, "Resguardos Vehiculos - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx");
            }
            // Aqui se debe cambiar por "Resguardos Mobiliario"
            if (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Resguardos Vehiculos - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx")))
            {
                System.IO.File.Delete(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Resguardos Vehiculos - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx"));
            }

            string savedFileName = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Resguardos Vehiculos - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx"));
            //FileStream stream = System.IO.File.Create(savedFileName);
            using (FileStream stream = new FileStream(savedFileName, FileMode.Create))
            {
                try
                {

                    List<Resguardos_Lista_Vehiculos_Excel> productos = new List<Resguardos_Lista_Vehiculos_Excel>();
                    productos = db.Database.SqlQuery<Resguardos_Lista_Vehiculos_Excel>("Sp_Get_Resguardos_Vehiculos_excel").ToList();
                    if (filtro == null || filtro == "")
                    {
                        productos = db.Database.SqlQuery<Resguardos_Lista_Vehiculos_Excel>("Sp_Get_Resguardos_Vehiculos_excel").ToList();
                    }
                    else
                    {
                        productos = db.Database.SqlQuery<Resguardos_Lista_Vehiculos_Excel>("Sp_Get_Resguardos_Vehiculos_excel_filtro @filtro", new SqlParameter("@filtro", filtro)).ToList();
                    }

                    using (var libro = new ExcelPackage())
                    {

                        var worksheet = libro.Workbook.Worksheets.Add("Resguardos Vehiculos");
                        #region titulo para poner la razon social de la empresa
                        worksheet.Cells["D3:J3"].Merge = true;
                        worksheet.Cells["D3:J3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["D3:J3"].Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                        var cell = worksheet.Cells["D3"];
                        cell.IsRichText = true;     // Cell contains RichText rather than basic values


                        var title = cell.RichText.Add("SIRE - ");
                        title.Bold = true;
                        title.FontName = "Calibri";
                        title.Size = 15;
                        title.Color = ColorTranslator.FromHtml("#44546A");
                        #endregion
                        #region titulo para el reporte
                        worksheet.Cells["D4:J4"].Merge = true;
                        worksheet.Cells["D4:J4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["D4:J4"].Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                        //worksheet.Cells["D4:I4"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                        //worksheet.Cells["D4:I4"].Style.Border.Bottom.Color.SetColor(Color.Blue);

                        var cellrs = worksheet.Cells["D4"];
                        cellrs.IsRichText = true;     // Cell contains RichText rather than basic values
                                                      //cell.Style.WrapText = true; // Required to honor new lines

                        var titlers = cellrs.RichText.Add("Resguardos de Vehiculos");
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
                        excelImage2.From.Column = 10;
                        //excelImage2.From.Column = 9;
                        excelImage2.From.Row = 0;
                        excelImage2.SetSize(150, 80);
                        // 2x2 px space for better alignment
                        excelImage2.From.ColumnOff = Pixel2MTU(2);
                        excelImage2.From.RowOff = Pixel2MTU(2);
                        #endregion

                        var tabla = worksheet.Tables.Add(new ExcelAddressBase(fromRow: 6, fromCol: 2, toRow: productos.Count + 6, toColumn: 10), "Resguardos");
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
        //public ActionResult _formularioResguardo(long? idmobiliario)
        {
            Resguardos_Lista_Vehiculos resguardoVeh = new Resguardos_Lista_Vehiculos();
            //Resguardos_Lista_laptos resguardo = new Resguardos_Lista_laptos();
            if (idresguardo != null)
            {
                ViewBag.edit = 1;
                // cambiar el nombre aqui del procedimiento almacenado

                //resguardo = db.Database.SqlQuery<Resguardos_Lista_laptos>("Sp_Get_Datos_Resguardo_Laptop @Id_resguardo", new SqlParameter("@Id_resguardo", idresguardo)).FirstOrDefault();
                resguardoVeh = db.Database.SqlQuery<Resguardos_Lista_Vehiculos>("Sp_Get_Datos_Resguardo_Vehiculos @Id_vehiculo", new SqlParameter("@Id_vehiculo", idresguardo)).FirstOrDefault();



            }
            else
            {
                List<SelectListItem> items2 = new List<SelectListItem>();
                items2.Add(new SelectListItem { Value = "Operaciones", Text = "Operaciones" });
                items2.Add(new SelectListItem { Value = "Ciclo Ambiental", Text = "Ciclo Ambiental" });
                items2.Add(new SelectListItem { Value = "Corbase", Text = "Corbase" });
                items2.Add(new SelectListItem { Value = "Kanasín", Text = "Kanasín" });
                items2.Add(new SelectListItem { Value = "Ecolsur-Mid", Text = "Ecolsur MID" });
                items2.Add(new SelectListItem { Value = "Ecolsur-CUN", Text = "Ecolsur CUN" });
                ViewBag.ubicacion1 = new SelectList(items2, "Value", "Text");

                List<SelectListItem> items3 = new List<SelectListItem>();
                items3.Add(new SelectListItem { Value = "Administracion", Text = "Administracion" });
                items3.Add(new SelectListItem { Value = "Almacén", Text = "Almacén" });
                items3.Add(new SelectListItem { Value = "Comercial", Text = "Comercial" });
                items3.Add(new SelectListItem { Value = "Domiciliar", Text = "Domiciliar" });
                items3.Add(new SelectListItem { Value = "Mantenimiento", Text = "Mantenimiento" });
                items3.Add(new SelectListItem { Value = "Recursos Humanos", Text = "Recursos Humanos" });
                items3.Add(new SelectListItem { Value = "Ventas RME", Text = "Ventas RME" });
                items3.Add(new SelectListItem { Value = "Supervisores", Text = "Supervisores" });
                items3.Add(new SelectListItem { Value = "Ventas RSU", Text = "Ventas RSU" });
                ViewBag.departamento1 = new SelectList(items3, "Value", "Text");


            }


            return PartialView(resguardoVeh);

        }


        // subir archivo
        public JsonResult CargarArchivo()
        {
            bool success = false;
            string error = "";
            var savedFileNameDownload = "";
            string savedFileName = "";
            string completeName = "";
            try
            {
                foreach (string file in Request.Files)
                {

                    HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                    string name = Path.GetRandomFileName();
                    string extension = Path.GetExtension(Path.GetFileName(hpf.FileName));
                    completeName = name + extension;
                    savedFileName = Path.Combine(Server.MapPath("~/Upload/Empresa/"), completeName);
                    hpf.SaveAs(savedFileName);
                    success = true;

                }
            }
            catch (Exception ex)
            {
                error = "Archivo Invalido, Error al procesar el archivo";
            }

            return Json(new { success = success, error = error, savedFileName = completeName }, JsonRequestBehavior.AllowGet);
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

            //int empresa = MOB.Em_Cve_Empresa;

            //int empresa = LAP.Em_Cve_Empresa;
            var reporte = new ReportClass();
            //if(empresa == 1)
            //{
            //aqui debo cambiar el nombre del archivo para que se la responsiva de mobiliiario sana
            reporte.FileName = Server.MapPath("~/Reportes/ResponsivaVehSana.rpt");
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

            reporte.SetParameterValue("@Id_vehiculo", id);
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
            List<ElementJsonObjectKeyveh> lst = new List<ElementJsonObjectKeyveh>();
            // cambiar el nombre aqui del procedimiento almacenado
            //var resguardo = db.Database.SqlQuery<Resguardos_Lista_laptos>("Sp_Get_Datos_Laptos").ToList();
            //presenta error aqui
            var resguardomob = db.Database.SqlQuery<Resguardos_Lista_Vehiculos>("Sp_Get_Datos_Vehiculos").ToList();

            lst = (from d in resguardomob
                   where d.Numero_economico.ToUpper().Contains(cad.ToUpper())
                   select new ElementJsonObjectKeyveh
                   {
                       label = d.Numero_economico,
                       Value = d.Numero_economico,
                       Serie_motor = d.Serie_motor,
                       Cuadro_chasis = d.Cuadro_chasis,
                       Marca = d.Marca,
                       Anio = d.Anio,
                       Color = d.Color,
                       Modelo = d.Modelo,
                       Placas = d.Placas,
                       Poliza_seguro = d.Poliza_seguro,
                       Inciso = d.Inciso,
                       Vigencia_del = d.Vigencia_del,
                       Vigencia_al = d.Vigencia_al,
                       Folio = d.Folio,
                       Tarjeta_circulacion = d.Tarjeta_circulacion,
                       Vigencia_tarjeta = d.Vigencia_tarjeta
                       //fecha_folio = d.fecha_folio
                   }).Take(5).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        


        //no requiere cambios
        //este funciona bien
        public JsonResult BuscarRecibe(string cad)
        {
            List<ElementJsonRecibeveh> lst = new List<ElementJsonRecibeveh>();

            lst = (from d in db.cat_usuarios
                   where d.nombre_completo.ToUpper().Contains(cad.ToUpper())
                   select new ElementJsonRecibeveh
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
            List<ElementJsonEntregaveh> lst = new List<ElementJsonEntregaveh>();

            lst = (from d in db.cat_usuarios
                   where d.nombre_completo.ToUpper().Contains(cad.ToUpper())
                   select new ElementJsonEntregaveh
                   {
                       label = d.nombre_completo,
                       Value = d.nombre_completo,
                       identrega = d.usuario_ID

                   }).Take(5).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }




    }

    //no requiere cambios
    public class ElementJsonRecibeveh
    {
        public string Value { get; set; }
        public object label { get; set; }
        public int idRecibe { get; set; }
    //no requiere cambios
    }
    public class ElementJsonEntregaveh
    {
        public string Value { get; set; }
        public object label { get; set; }
        public int identrega { get; set; }
    }

    //hacer cambios aqui
    public class ElementJsonObjectKeyveh
    {
        public object label { get; set; }
        public string Value { get; set; }
        //public string Mobiliario { get; set; }
        public string Color { get; set; }
        //public string Proveedor { get; set; } 
        public string Caracteristicas { get; set; }
        public int Precio { get; set; }
        public string Ubicacion { get; set; }
        public DateTime? fecha_folio { get; set; }
        public string Proveedor { get; set; }
        //añadidos
        public string Serie_motor { get; set; }
        public string Cuadro_chasis { get; set; }
        public string Marca { get; set; }
        public string Anio { get; set; }
        public string Modelo { get; set; }
        public string Placas { get; set; }
        public string Poliza_seguro { get; set; }
        public string Inciso { get; set; }
        public DateTime? Vigencia_del { get; set; }
        public DateTime? Vigencia_al { get; set; }
        public DateTime? Vigencia_tarjeta { get; set; }
        public string Tarjeta_circulacion { get; set; }
        public string Folio { get; set; }

    }
}