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
    public class ResguardoMovilesViewController : Controller
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

        // se debe añadir el numero de linea que se unira con el movil 
        //public ActionResult SaveResguardo(Resguardos_Lista_Moviles resguardo, int id_recibe, int Id_entrega)
        public ActionResult SaveResguardo(Resguardos_Lista_Moviles resguardo, int id_recibe, int Id_entrega, int id_linea)
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
                cat_resguardos_moviles resguar = new cat_resguardos_moviles();
                    resguar.fecha = DateTime.Now;
                    //resguar.inv_laptop_ID = db.inventario_laptop.FirstOrDefault(x => x.serie == resguardo.Cod_inventario).inv_laptop_ID;
                    resguar.inv_movil_ID = db.inventario_movil.FirstOrDefault(x => x.serie == resguardo.Serie).inv_movil_ID;
                    resguar.recibe_usuario_ID = id_recibe;
                    resguar.entrega_usuario_ID = Id_entrega;
                    // Añadido
                    resguar.inv_linea_ID = id_linea;
                    
                    resguar.estatus_ID = 2;

                    resguar.cargador = 1;
                    resguar.observaciones = "N/A";

                    db.cat_resguardos_moviles.Add(resguar);

                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                    }
                // se debe añadir la modificacion tanto del status de la linea como del equipo movil al que se le añade el id de la linea
                 ///*
                inventario_lineas invlin = db.inventario_lineas.Find(db.inventario_lineas.FirstOrDefault(x => x.inv_linea_ID == id_linea ).inv_linea_ID);
                    invlin.estatus_ID = 2;
                    db.Entry(invlin).State = EntityState.Modified;
                    if (db.SaveChanges() > 0)
                    {
                       success = true;
                    }
                //*/
                inventario_movil invmov = db.inventario_movil.Find(db.inventario_movil.FirstOrDefault(x =>x.serie == resguardo.Serie).inv_movil_ID);
                    invmov.estatus_ID = 2;                    
                    db.Entry(invmov).State = EntityState.Modified;
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


        public ActionResult Devolver(Resguardos_Lista_Moviles id)
        {
            var serializerCat = new JavaScriptSerializer();
            bool success = false;
            string mensajefound = "";
            try
            {
                cat_resguardos_moviles resuni = db.cat_resguardos_moviles.Find(db.cat_resguardos_moviles.FirstOrDefault(x => x.resguardo_movil_ID == id.Resguardo_movil_ID).resguardo_movil_ID);
                resuni.estatus_ID = 1;//cambiar el estatus del resguardo en la tabla
                db.Entry(resuni).State = EntityState.Modified;
                if (db.SaveChanges() > 0)
                {
                    success = true;
                }

                inventario_movil invmob = db.inventario_movil.Find(db.inventario_movil.FirstOrDefault(x => x.inv_movil_ID == resuni.inv_movil_ID).inv_movil_ID);
                invmob.estatus_ID = 1;   //cambiar el estatus del inventario en la tabla
                db.Entry(invmob).State = EntityState.Modified;
                if (db.SaveChanges() > 0)
                {
                    success = true;
                }

                // se necesita añadio una excepcion para verificar si tiene o no linea
                if (resuni.inv_linea_ID!= null)
                {
                    inventario_lineas invlinea = db.inventario_lineas.Find(db.inventario_lineas.FirstOrDefault(x => x.inv_linea_ID == resuni.inv_linea_ID).inv_linea_ID);
                    invlinea.estatus_ID = 1;   //cambiar el estatus del inventario en la tabla
                    db.Entry(invlinea).State = EntityState.Modified;
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
            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);
        }



        //lista
        public ActionResult _TablaResguardos(int? page, string orden, string filtro)
        {
            const int pageSize = 10;
            int pageNumber = (page ?? 1);

            List<Resguardos_Lista_Moviles> lista = new List<Resguardos_Lista_Moviles>(); 
            //List<Resguardos_Lista_Mobiliario> lista = new List<Resguardos_Lista_Mobiliario>();
            try
            {
                // se cambio aqui el nombre aqui del procedimiento almacenado
                var resguardo = db.Database.SqlQuery<Resguardos_Lista_Moviles>("Sp_Get_Resguardos_Moviles").ToList();
                //var resguardo = db.Database.SqlQuery<Resguardos_Lista_Mobiliario>("Sp_Get_Resguardo_Mobiliario").ToList();
               
                if (string.IsNullOrEmpty(filtro))
                {
                    #region condiones de ordenado              
                    if (orden == "Resguardo_Movil_ID" || string.IsNullOrEmpty(orden))
                    {
                        orden = "Resguardo_Movil_ID";
                        lista = resguardo.OrderBy(c => c.Resguardo_movil_ID).ToList();
                    }
                    else if (orden == "Resguardo_Movil_IDdesc")
                        lista = resguardo.OrderByDescending(c => c.Resguardo_movil_ID).ToList();
                    else if (orden == "Fecha")
                        lista = resguardo.OrderBy(c => c.Fecha).ToList();
                    else if (orden == "Fechadesc")
                        lista = resguardo.OrderByDescending(c => c.Fecha).ToList();
                    else if (orden == "Cod_inventario")
                        lista = resguardo.OrderBy(c => c.Cod_inventario).ToList();
                    else if (orden == "Cod_inventariodesc")
                        lista = resguardo.OrderByDescending(c => c.Cod_inventario).ToList();
                    else if (orden == "Marca")
                        lista = resguardo.OrderBy(c => c.Marca).ToList();
                    else if (orden == "Marcadesc")
                        lista = resguardo.OrderByDescending(c => c.Marca).ToList();
                    
                    else if (orden == "Modelo")
                        lista = resguardo.OrderBy(c => c.Modelo).ToList();
                    else if (orden == "Modelodesc")
                        lista = resguardo.OrderByDescending(c => c.Modelo).ToList();
                    //añadido
                    else if (orden == "Serie")
                        lista = resguardo.OrderBy(c => c.Serie).ToList();
                    else if (orden == "Seriedesc")
                        lista = resguardo.OrderByDescending(c => c.Serie).ToList();

                    else if (orden == "Telefono")
                        lista = resguardo.OrderBy(c => c.Telefono).ToList();
                    else if (orden == "Telefonodesc")
                        lista = resguardo.OrderByDescending(c => c.Telefono).ToList();

                    else if (orden == "Departamento")
                        lista = resguardo.OrderBy(c => c.Departamento).ToList();
                    else if (orden == "Departamentodesc")
                        lista = resguardo.OrderByDescending(c => c.Departamento).ToList();

                    //añadido

                    else if (orden == "Ubicacion")
                        lista = resguardo.OrderBy(c => c.Ubicacion).ToList();
                    else if (orden == "Ubicaciondesc")
                        lista = resguardo.OrderByDescending(c => c.Ubicacion).ToList();

                    else if (orden == "Nombres")
                        lista = resguardo.OrderBy(c => c.Nombres).ToList();
                    else if (orden == "Nombresdesc")
                        lista = resguardo.OrderByDescending(c => c.Nombres).ToList();
                    
                    //añadido
                    
                    else if (orden == "Ceco")
                        lista = resguardo.OrderBy(c => c.Ceco).ToList();
                    else if (orden == "Cecodesc")
                        lista = resguardo.OrderByDescending(c => c.Ceco).ToList();
                    

                    ViewBag.order = orden;
                    #endregion
                }
                else
                {
                    lista = resguardo.Where(x => x.Nombres.ToUpper().Contains(filtro.ToUpper().Trim())
                    || x.Cod_inventario.ToUpper().Contains(filtro.ToUpper().Trim()) || x.Marca.ToUpper().Contains(filtro.ToUpper().Trim())
                    || x.Modelo.ToUpper().Contains(filtro.ToUpper().Trim()) || x.Serie.ToUpper().Contains(filtro.ToUpper().Trim())
                    || x.Telefono.ToUpper().Contains(filtro.ToUpper().Trim()) || x.Departamento.ToUpper().Contains(filtro.ToUpper().Trim())
                    || x.Ubicacion.ToUpper().Contains(filtro.ToUpper().Trim()) || x.Ceco.ToUpper().Contains(filtro.ToUpper().Trim())).ToList(); 

                    ViewBag.filtro = filtro;
                }
                return PartialView(lista.ToPagedList(pageNumber, pageSize));
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
                // Aqui se debe cambiar por "Resguardos Mobiliario"
                var ruta = "~/Upload/Temporales/Descargas/" + "Resguardos Moviles - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx";
                return File(Url.Content(ruta), excelContentType, "Resguardos Moviles - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx");
            }
            // Aqui se debe cambiar por "Resguardos Mobiliario"
            if (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Resguardos Moviles - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx")))
            {
                System.IO.File.Delete(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Resguardos Moviles - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx"));
            }

            string savedFileName = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Resguardos Moviles - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx"));
            //FileStream stream = System.IO.File.Create(savedFileName);
            using (FileStream stream = new FileStream(savedFileName, FileMode.Create))
            {
                try
                {
 
                    var productos = db.Database.SqlQuery<Resguardos_Lista_Moviles_Excel>("Sp_Get_Resguardos_Moviles_excel").ToList();

                    using (var libro = new ExcelPackage())
                    {

                        var worksheet = libro.Workbook.Worksheets.Add("Resguardos Moviles");
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

                        var titlers = cellrs.RichText.Add("Resguardos de Mobiliario");
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

                        var tabla = worksheet.Tables.Add(new ExcelAddressBase(fromRow: 6, fromCol: 2, toRow: productos.Count + 6, toColumn: 15), "Resguardos");
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
            Resguardos_Lista_Moviles resguardoMov = new Resguardos_Lista_Moviles();
            //Resguardos_Lista_Mobiliario resguardoMob = new Resguardos_Lista_Mobiliario();
            if (idresguardo != null)
            {
                ViewBag.edit = 1;
                // cambiar el nombre aqui del procedimiento almacenado

                resguardoMov = db.Database.SqlQuery<Resguardos_Lista_Moviles>("Sp_Get_Datos_Resguardo_Moviles @Id_moviles", new SqlParameter("@Id_moviles", idresguardo)).FirstOrDefault();
                //resguardoMob = db.Database.SqlQuery<Resguardos_Lista_Mobiliario>("Sp_Get_Datos_Resguardo_Mobiliario @Id_mobiliario", new SqlParameter("@Id_mobiliario", idresguardo)).FirstOrDefault();

            }
            return PartialView(resguardoMov);
            //return PartialView(resguardoMob);

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
            reporte.FileName = Server.MapPath("~/Reportes/ResponsivaLinSana.rpt");
            
            /*
            reporte.FileName = Server.MapPath("/Reportes/RespoonsivaEcolsur.rpt");
            }
            else if(empresa == 2)
            {
                reporte.FileName = Server.MapPath("/Reportes/RespoonsivaSana.rpt");
            }
            else
            {
                reporte.FileName = Server.MapPath("/Reportes/RespoonsivaSau.rpt");
            }
            ¿este archivo sirve para algo?
            var rutafoto = Server.MapPath("/Upload/Sistema/ecolsur.png");
            */
            reporte.Load();

            reporte.SetParameterValue("@Id_moviles", id);
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
            
            List<ElementJsonObjectKeymoviles> lst = new List<ElementJsonObjectKeymoviles>();
            // cambiar el nombre aqui del procedimiento almacenado
            //var resguardo = db.Database.SqlQuery<Resguardos_Lista_laptos>("Sp_Get_Datos_Laptos").ToList();
            //presenta error aqui

            var resguardomov = db.Database.SqlQuery<Resguardos_Lista_Moviles>("Sp_Get_Datos_Moviles").ToList();
            //var resguardomob = db.Database.SqlQuery<Resguardos_Lista_Mobiliario>("Sp_Get_Datos_Mobiliario").ToList();

            lst = (from d in resguardomov
                   where d.Serie.ToUpper().Contains(cad.ToUpper())
                   select new ElementJsonObjectKeymoviles
                   {
                       label = d.Serie,
                       Value = d.Serie,
                       Linea = d.Linea,
                       Marca = d.Marca,
                       Modelo = d.Modelo,
                       Precio = d.Precio,
                       Cod_inventario = d.Cod_inventario,
                       Proveedor = d.Proveedor,
                       fecha_folio = d.fecha_folio
                       
                   }).Take(5).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        


        //no requiere cambios
        //este funciona bien
        public JsonResult BuscarRecibe(string cad)
        {
            List<ElementJsonRecibemoviles> lst = new List<ElementJsonRecibemoviles>();

            lst = (from d in db.cat_usuarios
                   where d.nombre_completo.ToUpper().Contains(cad.ToUpper())
                   select new ElementJsonRecibemoviles
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
            List<ElementJsonEntregamoviles> lst = new List<ElementJsonEntregamoviles>();

            lst = (from d in db.cat_usuarios
                   where d.nombre_completo.ToUpper().Contains(cad.ToUpper())
                   select new ElementJsonEntregamoviles
                   {
                       label = d.nombre_completo,
                       Value = d.nombre_completo,
                       identrega = d.usuario_ID

                   }).Take(5).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BuscarLinea_mov(string cad)
        {
            //List<ElementJsonEntregamoviles> lst = new List<ElementJsonEntregamoviles>();
            List<ElementJsonObjectKeylineas> lst = new List<ElementJsonObjectKeylineas>();
            
            var resguardoMovLin = db.Database.SqlQuery<Resguardos_Lista_Lineas_Moviles>("Sp_Get_Datos_Lineas").ToList();

            lst = (from d in resguardoMovLin
                   where d.Linea.Contains(cad.ToUpper())
                   select new ElementJsonObjectKeylineas
                   {
                       label = d.Linea,
                       Value = d.Linea,
                       idlinea= d.inv_linea_ID

                   }).Take(5).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }



    }

    //no requiere cambios
    public class ElementJsonRecibemoviles
    {
        public string Value { get; set; }
        public object label { get; set; }
        public int idRecibe { get; set; }
    //no requiere cambios
    }
    public class ElementJsonEntregamoviles
    {
        public string Value { get; set; }
        public object label { get; set; }
        public int identrega { get; set; }
    }

    //hacer cambios aqui
    public class ElementJsonObjectKeymoviles
    {
        public object label { get; set; }
        public string Value { get; set; }
        public string Linea { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        //public string Caracteristicas { get; set; }
        public int Precio { get; set; }
        public string Cod_inventario { get; set; }
        public DateTime? fecha_folio { get; set; }
        public string Proveedor { get; set; }


    }

    public class ElementJsonObjectKeylineas
    {
        public string Value { get; set; }
        public object label { get; set; }
        public string linea_mov { get; set; }
        public int idlinea { get; set; }
    }


}