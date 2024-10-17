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
using System.Data.Entity.Validation;

namespace CRME.Controllers
{
    public class InventarioMovilesViewController : Controller
    {
        // GET: EmpresasView

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


        // public ActionResult SaveEmpresa(inventario_lineas inventario_lineas)
        public ActionResult SaveEmpresa(inventario_movil Empresas)
        {
            //Empresa empresas = new Empresa();
            var serializerCat = new JavaScriptSerializer();
            bool success = false;
            string mensajefound = "";
            //linea pendiente de revision

            var found = db.inventario_movil.FirstOrDefault(x => x.serie == Empresas.serie && x.inv_movil_ID != Empresas.inv_movil_ID);
            //var found = db.Empresa.FirstOrDefault(x => x.Em_Descripcion == Empresas.Em_Descripcion && x.Em_Cve_Empresa != Empresas.Em_Cve_Empresa);

            if (string.IsNullOrWhiteSpace(Empresas.serie))
            {
                mensajefound = "¡no puede estar en blanco el IMEI del movil!";
            }
            else
            {





                if (found != null)
                {
                    mensajefound = "¡Ya existe un Movil que tiene el mismo IMEI que estan ingresado!";

                }
                else
                {
                    if (Empresas.inv_movil_ID == 0)
                    {
                        try
                        {
                            inventario_movil empre = new inventario_movil();
                            empre.serie = Empresas.serie;
                            empre.marca = Empresas.marca;
                            empre.modelo = Empresas.modelo;
                            empre.precio = Empresas.precio;
                            empre.folio = Empresas.folio;
                            empre.fecha_folio = Empresas.fecha_folio;
                            empre.proveedor_ID = Empresas.proveedor_ID;
                            empre.Em_Cve_Empresa = Empresas.Em_Cve_Empresa;

                            //definidos por el sistema
                            empre.estatus_ID = 1;
                            empre.linea = "N/A";

                            //generar año
                            var year = DateTime.Now;
                            //var anio = year.Year;
                            string conver = Convert.ToString(year);
                            string[] ultimos = conver.Split('/', ' ', ':', '0');
                            char[] maspruba = conver.ToCharArray();


                            //se construyo una busqueda para enconstrar el valor mas grande en la tabla y añadir el valor al codigo de inventario
                            inventario_movil modificador = db.inventario_movil.OrderByDescending(x => x.inv_movil_ID).First(); //añadir una excepecion en el futuro

                            Empresa etiqueta = db.Empresa.Find(Empresas.Em_Cve_Empresa);

                            empre.cod_inventario = etiqueta.Em_Descripcion.ToUpper() + maspruba[8] + maspruba[9] + "-MV" + Convert.ToString(modificador.inv_movil_ID + 1);
                            //empre.cod_inventario = etiqueta.Em_Descripcion.ToUpper() + ultimos[5] + "-MV" + Convert.ToString(modificador.inv_movil_ID + 1);
                            //empre.cod_inventario = etiqueta.Em_Descripcion.ToUpper() + conver + "-MV" + Convert.ToString(modificador.inv_movil_ID + 1);
                            //empre.cod_inventario = etiqueta.Em_Descripcion.ToUpper() + "23" + "-MV" + Convert.ToString(modificador.inv_movil_ID + 1);
                            //empre.cod_inventario = "SANA" + "23" + "-MV" + Convert.ToString(modificador.inv_movil_ID + 1);
                            //empre.cod_inventario ="SANA" +"23"+"-MV" + Convert.ToString(Empresas.inv_movil_ID);


                            db.inventario_movil.Add(empre);
                            if (db.SaveChanges() > 0)
                            {
                                success = true;
                            }
                        }

                        catch (DbEntityValidationException ex)
                        {
                            var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                            // Join the list to a single string.
                            var fullErrorMessage = string.Join("; ", errorMessages);

                            // Combine the original exception message with the new one.
                            var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                            // Throw a new DbEntityValidationException with the improved exception message.
                            //throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

                            mensajefound = exceptionMessage + "fatal error";

                        }
                    }
                    else
                    {
                        try
                        {
                            inventario_movil Empre = db.inventario_movil.Find(Empresas.inv_movil_ID);

                            Empre.serie = Empresas.serie;
                            Empre.marca = Empresas.marca;
                            Empre.modelo = Empresas.modelo;
                            Empre.precio = Empresas.precio;
                            Empre.folio = Empresas.folio;
                            Empre.fecha_folio = Empresas.fecha_folio;

                            //combos
                            Empre.proveedor_ID = Empresas.proveedor_ID;
                            Empre.Em_Cve_Empresa = Empresas.Em_Cve_Empresa;
                            Empre.estatus_ID = Empresas.estatus_ID;

                            db.Entry(Empre).State = EntityState.Modified;
                            if (db.SaveChanges() > 0)
                            {
                                success = true;
                            }
                        }
                        catch (DbEntityValidationException ex)
                        {
                            var errorMessages = ex.EntityValidationErrors
                           .SelectMany(x => x.ValidationErrors)
                           .Select(x => x.ErrorMessage);

                            // Join the list to a single string.
                            var fullErrorMessage = string.Join("; ", errorMessages);

                            // Combine the original exception message with the new one.
                            var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                            // Throw a new DbEntityValidationException with the improved exception message.
                            //throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                            mensajefound = exceptionMessage + "fatal error";
                        }
                    }

                }
            }
            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);

        }

        //_FormularioMoviles
        public ActionResult _FormularioMoviles(long? inv_movil_ID)
        //public ActionResult _FormularioEmpresa(long? inv_linea_ID)
        {  //codigo para agregar y editar usuarios
            //UsuariosPersonas Personas = new UsuariosPersonas();
            inventario_movil Empresas = new inventario_movil();
            // Personas Persona = new Personas();

            if (inv_movil_ID != null)
            {
                ViewBag.edit = 1;
                Empresas = db.inventario_movil.Find(inv_movil_ID);


                if (Empresas.proveedor_ID != 0)
                {
                    ViewBag.proveedor = new SelectList(db.cat_proveedores.ToList(), "proveedor_ID", "proveedor", Empresas.proveedor_ID);
                }
                else
                {
                    ViewBag.proveedor = new SelectList(db.cat_proveedores.ToList(), "proveedor_ID", "proveedor");
                }

                if (Empresas.estatus_ID != 0)
                {
                    ViewBag.estatus = new SelectList(db.cat_estatus_inv.ToList(), "estatus_ID", "estatus", Empresas.estatus_ID);
                }
                else
                {
                    ViewBag.estatus = new SelectList(db.cat_estatus_inv.ToList(), "estatus_ID", "estatus");
                }


                if (Empresas.Em_Cve_Empresa !=0)
                {

                    ViewBag.empresa = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion", Empresas.Em_Cve_Empresa);
                }
                else
                {
                    ViewBag.empresa = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion");
                }
  
            }

            else
            {

                ViewBag.proveedor = new SelectList(db.cat_proveedores.ToList(), "proveedor_ID", "proveedor");

                ViewBag.estatus = new SelectList(db.cat_estatus_inv.ToList(), "estatus_ID", "estatus");

                ViewBag.empresa = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion");

            }

            return PartialView(Empresas);
        }





        // aqui se introduce el metodo para generar el excel
        public ActionResult ExportarExcel(int? creado, string filtro)
        {
            bool success = false;
            string excelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            if (creado == 1)
            {
                // Aqui se debe cambiar por "Resguardos Mobiliario"
                var ruta = "~/Upload/Temporales/Descargas/" + "Inventario Moviles - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx";
                return File(Url.Content(ruta), excelContentType, "Inventario Moviles - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx");
            }
            // Aqui se debe cambiar por "Resguardos Mobiliario"
            if (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Inventario Moviles - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx")))
            {
                System.IO.File.Delete(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Inventario Moviles - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx"));
            }

            string savedFileName = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Inventario Moviles - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx"));
            //FileStream stream = System.IO.File.Create(savedFileName);
            using (FileStream stream = new FileStream(savedFileName, FileMode.Create))
            {
                try
                {

                    //var productos = db.Database.SqlQuery<Inventario_Lista_Moviles_Excel>("Sp_Get_Inventario_Moviles_excel").ToList();
                    List<Inventario_Lista_Moviles_Excel> productos = new List<Inventario_Lista_Moviles_Excel>();
                    if (filtro == null || filtro == "")
                    {
                        productos = db.Database.SqlQuery<Inventario_Lista_Moviles_Excel>("Sp_Get_Inventario_Moviles_excel").ToList();
                    }
                    else
                    {
                        productos = db.Database.SqlQuery<Inventario_Lista_Moviles_Excel>("Sp_Get_Inventario_Moviles_excel_filtro @filtro", new SqlParameter("@filtro", filtro)).ToList();
                    }

                    using (var libro = new ExcelPackage())
                    {

                        var worksheet = libro.Workbook.Worksheets.Add("Inventario Moviles");
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

                        var titlers = cellrs.RichText.Add("Inventario de Moviles");
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

                        var tabla = worksheet.Tables.Add(new ExcelAddressBase(fromRow: 6, fromCol: 2, toRow: productos.Count + 6, toColumn: 8), "Resguardos");
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
        public int Pixel2MTU(int pixels)
        {
            int mtus = pixels * 9525;
            return mtus;
        }

        //aqui se deben hacer cambios para introducir la busqueda
        public ActionResult _TablaMoviles(int? page, string filtro)
        {
            const int pageSize = 10;
            int pageNumber = (page ?? 1);

            if (string.IsNullOrEmpty(filtro))
            {


                //var lista = db.inventario_lineas.Where(x=> x.estatus_ID == 2).ToList();
                var lista = db.inventario_movil.OrderByDescending(x => x.inv_movil_ID).ToList();
                //var lista = db.inventario_movil.ToList();
                ViewBag.filtro = filtro;
                return PartialView(lista.ToPagedList(pageNumber, pageSize));

            }
            else
            {
                var lista = db.inventario_movil.Where(x => x.serie.ToUpper().Contains(filtro.ToUpper().Trim())
                   || x.marca.ToUpper().Contains(filtro.ToUpper().Trim()) 
                   || x.cod_inventario.ToUpper().Contains(filtro.ToUpper().Trim())
                   //|| x.precio.ToUpper().Contains(filtro.ToUpper().Trim())
                   || x.modelo.ToUpper().Contains(filtro.ToUpper().Trim())).ToList();

                ViewBag.filtro = filtro;
                return PartialView(lista.ToPagedList(pageNumber, pageSize));
            }


            
        }

        //metodo casi listo- se debe cambiart nombre
        public ActionResult DeleteUsuario(long? inv_movil_ID)
        {
            bool success = false;
            string mensajefound = "";

            try
            {                
                inventario_movil lin = db.inventario_movil.Find(inv_movil_ID);
                lin.estatus_ID = 4;
                //Empre.Fecha_Baja = DateTime.Now;
                //Empre.Oper_Baja = Auth.Usuario.username;

                db.Entry(lin).State = EntityState.Modified;
                if (db.SaveChanges() > 0)
                {
                    success = true;
                }                              
            }
            catch (Exception ex)
            {
                mensajefound = "Ocurrio un error al dar baja al equipo movil";
            }
            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);
        }


        //metodo no necesario
        public JsonResult CargarLogo()
        {
            #region 'metodo cargar logo'
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
            #endregion
        }

        //metodo aun menos necesario
        public ActionResult EliminarLogo(string path)
        {
            #region 'metodo eliminar logo'
            bool success = false;
            try
            {
                var serializer = new JavaScriptSerializer();
                var pathh = serializer.Deserialize<string>(path);
                var rutapath = "~/Upload/Empresa/" + pathh;
                if (System.IO.File.Exists(Server.MapPath(rutapath)))
                {
                    try
                    {
                        System.IO.File.Delete(Server.MapPath(rutapath));
                        success = true;
                    }
                    catch (System.IO.IOException e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }

            }
            catch (Exception exp)
            {
                ViewBag.ResultMessage = "Error occured." + exp;
            }
            return Json(new { success = success }, JsonRequestBehavior.AllowGet);
            #endregion
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}