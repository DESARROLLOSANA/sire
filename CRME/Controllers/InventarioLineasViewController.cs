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
    public class InventarioLineasViewController : Controller
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
        public ActionResult SaveEmpresa(inventario_lineas Empresas)
        {
            //Empresa empresas = new Empresa();
            var serializerCat = new JavaScriptSerializer();
            bool success = false;
            string mensajefound = "";
            //linea pendiente de revision

            var found = db.inventario_lineas.FirstOrDefault(x => x.telefono == Empresas.telefono && x.inv_linea_ID != Empresas.inv_linea_ID);
            //var found = db.Empresa.FirstOrDefault(x => x.Em_Descripcion == Empresas.Em_Descripcion && x.Em_Cve_Empresa != Empresas.Em_Cve_Empresa);

            if (string.IsNullOrWhiteSpace(Empresas.telefono))
            {
                mensajefound = "¡No puede estar en blanco el numero de telefono!";
            }
            else
            {


                if (found != null)
                {
                    mensajefound = "¡Ya existe una linea que coincide con la ingresada!";

                }
                else
                {
                    if (Empresas.inv_linea_ID == 0)
                    {
                        try
                        {
                            inventario_lineas empre = new inventario_lineas();
                            empre.region = Empresas.region;
                            empre.telefono = Empresas.telefono;
                            empre.cuenta = Empresas.cuenta;
                            empre.renta_sin_iva = Empresas.renta_sin_iva;

                            //aun no añadidos en el formulario

                            //empre.fecha_inicio = null;
                            empre.fecha_inicio = Empresas.fecha_inicio;
                            empre.fecha_termino = null;
                            empre.estatus_adendum = Empresas.estatus_adendum;
                            empre.nombre_plan = Empresas.nombre_plan;


                            empre.ceco = "SIN CECO";
                            empre.ubicacion = "SIN UBICACION";
                            empre.departamento = "SIN DEPARTAMENTO";
                            empre.razon_social = "SANEAMIENTO SANA SC DE RL";


                            //datos definidos por el sistema
                            empre.estatus_ID = 1;
                            empre.usuario_app_ID = null;
                            empre.created_time_actuales = DateTime.Now;
                            empre.Em_Cve_Empresa = 0;




                            db.inventario_lineas.Add(empre);
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
                            inventario_lineas Empre = db.inventario_lineas.Find(Empresas.inv_linea_ID);
                            Empre.region = Empresas.region;
                            Empre.telefono = Empresas.telefono;
                            Empre.cuenta = Empresas.cuenta;
                            Empre.renta_sin_iva = Empresas.renta_sin_iva;


                            Empre.fecha_inicio = Empresas.fecha_inicio;
                            Empre.fecha_termino = Empresas.fecha_termino;
                            Empre.estatus_adendum = Empresas.estatus_adendum;
                            Empre.nombre_plan = Empresas.nombre_plan;


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

        public ActionResult _FormularioEmpresa(long? inv_linea_ID)
        {  //codigo para agregar y editar usuarios
            //UsuariosPersonas Personas = new UsuariosPersonas();
            inventario_lineas Empresas = new inventario_lineas();
           // Personas Persona = new Personas();

            if (inv_linea_ID != null)
            {
                ViewBag.edit = 1;
                Empresas = db.inventario_lineas.Find(inv_linea_ID);
                //añadir controlador de listas
                //ViewBag.idGenero = new SelectList(db.CatGeneros.ToList(), "idGenero", "nbGenero");
                
                List<SelectListItem> items1 = new List<SelectListItem>();
                items1.Add(new SelectListItem { Value = "R07", Text = "R07" });
                items1.Add(new SelectListItem { Value = "R08", Text = "R08" });
                //ViewBag.region_ID = new SelectList(items1, "Value", "Text", Empresas.region);

                if(Empresas.region != null){
                    ViewBag.region_ID = new SelectList(items1, "Value", "Text", Empresas.region);
                }
                else
                {
                    ViewBag.region_ID = new SelectList(items1, "Value", "Text");
                }


                List<SelectListItem> items2 = new List<SelectListItem>();
                items2.Add(new SelectListItem { Value = "CON ADENDUM" , Text = "CON ADENDUM" });
                items2.Add(new SelectListItem{ Value = "SIN ADENDUM", Text = "SIN ADENDUM" });
                
                if (Empresas.estatus_adendum != null)
                {
                    ViewBag.adendum = new SelectList(items2,"Value","Text", Empresas.estatus_adendum);
                }
                else
                {
                    ViewBag.adendum = new SelectList(items2, "Value", "Text");
                }

                List<SelectListItem> items3 = new List<SelectListItem>();
                items3.Add(new SelectListItem { Value = "MAXD SL5000 I VPN24M", Text = "MAXD SL5000 I VPN24M" });
                items3.Add(new SelectListItem { Value = "TM CONTINGENCIA", Text = "TM CONTINGENCIA" });
                items3.Add(new SelectListItem { Value = "TME C 1000 VC 18M", Text = "TME C 1000 VC 18M" });
                items3.Add(new SelectListItem { Value = "TME D 1000 VD 18M", Text = "TME D 1000 VD 18M" });
                items3.Add(new SelectListItem { Value = "TMSLE C 5000 VC 18M", Text = "TMSLE C 5000 VC 18M" });
                items3.Add(new SelectListItem { Value = "TMSLE D 1500 VC 18M", Text = "TMSLE D 1500 VC 18M" });
                items3.Add(new SelectListItem { Value = "TMSLE D 1500 VC 24M ", Text = "TMSLE D 1500 VC 24M" });
                items3.Add(new SelectListItem { Value = "TMSLE D 2000 VC 24M", Text = "TMSLE D 2000 VC 24M" });
                items3.Add(new SelectListItem { Value = "TMSLE D 5000 IVC 24M", Text = "TMSLE D 5000 IVC 24M" });
                
                if(Empresas.nombre_plan != null){
                    ViewBag.plan = new SelectList(items3, "Value", "Text", Empresas.nombre_plan);
                }
                else
                {
                    ViewBag.plan = new SelectList(items3, "Value", "Text");
                }
                //ViewBag.plan = new SelectList(items3, "Value", "Text", Empresas.nombre_plan);

            }

            else
            {
                List<SelectListItem> items1 = new List<SelectListItem>();
                items1.Add(new SelectListItem { Value = "R07", Text = "R07" });
                items1.Add(new SelectListItem { Value = "R08", Text = "R08" });
                ViewBag.region_ID = new SelectList(items1, "Value", "Text");



                List<SelectListItem> items2 = new List<SelectListItem>();
                items2.Add(new SelectListItem { Value = "CON ADENDUM", Text = "CON ADENDUM" });
                items2.Add(new SelectListItem { Value = "SIN ADENDUM", Text = "SIN ADENDUM" });
                ViewBag.adendum = new SelectList(items2, "Value", "Text");

                

                List<SelectListItem> items3 = new List<SelectListItem>();
                items3.Add(new SelectListItem { Value = "MAXD SL5000 I VPN24M", Text = "MAXD SL5000 I VPN24M" });
                items3.Add(new SelectListItem { Value = "TM CONTINGENCIA", Text = "TM CONTINGENCIA" });
                items3.Add(new SelectListItem { Value = "TME C 1000 VC 18M", Text = "TME C 1000 VC 18M" });
                items3.Add(new SelectListItem { Value = "TME D 1000 VD 18M", Text = "TME D 1000 VD 18M" });
                items3.Add(new SelectListItem { Value = "TMSLE C 5000 VC 18M", Text = "TMSLE C 5000 VC 18M" });
                items3.Add(new SelectListItem { Value = "TMSLE D 1500 VC 18M", Text = "TMSLE D 1500 VC 18M" });
                items3.Add(new SelectListItem { Value = "TMSLE D 1500 VC 24M ", Text = "TMSLE D 1500 VC 24M" });
                items3.Add(new SelectListItem { Value = "TMSLE D 2000 VC 24M", Text = "TMSLE D 2000 VC 24M" });
                items3.Add(new SelectListItem { Value = "TMSLE D 5000 IVC 24M", Text = "TMSLE D 5000 IVC 24M" });
                ViewBag.plan = new SelectList(items3, "Value", "Text");

            }

            return PartialView(Empresas);
        }

        public ActionResult ExportarExcel(int? creado, string filtro)
        {
            bool success = false;
            string excelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            if (creado == 1)
            {
                // Aqui se debe cambiar por "Resguardos Mobiliario"
                var ruta = "~/Upload/Temporales/Descargas/" + "Inventario Lineas - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx";
                return File(Url.Content(ruta), excelContentType, "Inventario Lineas - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx");
            }
            // Aqui se debe cambiar por "Resguardos Mobiliario"
            if (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Inventario Lineas - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx")))
            {
                System.IO.File.Delete(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Inventario Lineas - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx"));
            }

            string savedFileName = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Inventario Lineas - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx"));
            //FileStream stream = System.IO.File.Create(savedFileName);
            using (FileStream stream = new FileStream(savedFileName, FileMode.Create))
            {
                try
                {

                  var productos = db.Database.SqlQuery<Inventario_Lista_Lineas_Excel>("Sp_Get_Inventario_Lineas_excel @filtro", new SqlParameter("@filtro", filtro)).ToList();

                    using (var libro = new ExcelPackage())
                    {

                        var worksheet = libro.Workbook.Worksheets.Add("Inventario Lineas");
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

                        var titlers = cellrs.RichText.Add("Inventario de Lineas");
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

        public int Pixel2MTU(int pixels)
        {
            int mtus = pixels * 9525;
            return mtus;
        }


        //metodo listo
        public ActionResult _TablaLineas(int? page, string filtro)
        {
            const int pageSize = 10;
            int pageNumber = (page ?? 1);

            //var lista = db.inventario_lineas.Where(x => x.estatus_ID == 2).ToList();
            if (string.IsNullOrEmpty(filtro))
            {
                var lista = db.inventario_lineas.OrderByDescending(x=> x.inv_linea_ID).ToList();
                //var lista = db.inventario_lineas.ToList();
                ViewBag.filtro = filtro;
                return PartialView(lista.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                var lista = db.inventario_lineas.Where(x => x.telefono.ToUpper().Contains(filtro.ToUpper().Trim())
                   || x.region.ToUpper().Contains(filtro.ToUpper().Trim()) || x.nombre_plan.ToUpper().Contains(filtro.ToUpper().Trim())
                   || x.renta_sin_iva.ToUpper().Contains(filtro.ToUpper().Trim()) || x.cuenta.ToUpper().Contains(filtro.ToUpper().Trim())
                   || x.razon_social.ToUpper().Contains(filtro.ToUpper().Trim()) || x.ubicacion.ToUpper().Contains(filtro.ToUpper().Trim())
                   ).ToList();

                ViewBag.filtro = filtro;

                return PartialView(lista.ToPagedList(pageNumber, pageSize));
            }



        }

        //metodo casi listo- se debe cambiart nombre
        public ActionResult DeleteUsuario(long? inv_linea_ID)
        {
            bool success = false; ;
            string mensajefound = "";

            try
            {                
                inventario_lineas lin = db.inventario_lineas.Find(inv_linea_ID);

                if (lin.estatus_ID != 4)
                {
                    lin.estatus_ID = 4;
                }
                else
                {
                    lin.estatus_ID = 1;
                }
                //lin.estatus_ID = 4;
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
                mensajefound = "Ocurrio un error al dar baja a la linea";
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