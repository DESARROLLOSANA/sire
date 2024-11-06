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
    public class InventarioEnsamblesViewController : Controller
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
        public ActionResult SaveEmpresa(inventario_ensamble Empresas)
        {
            //Empresa empresas = new Empresa();
            var serializerCat = new JavaScriptSerializer();
            bool success = false;
            string mensajefound = "";
            //linea pendiente de revision

            //var found = db.inventario_lineas.FirstOrDefault(x => x.telefono == Empresas.telefono && x.inv_linea_ID != Empresas.inv_linea_ID);
            var found = db.inventario_ensamble.FirstOrDefault(x => x.serie == Empresas.serie && x.inv_ensamble_ID != Empresas.inv_ensamble_ID);

            if (string.IsNullOrWhiteSpace(Empresas.serie))
            {
                mensajefound = "¡No puede estar en blanco el Numero de Serie del Ensamble!";
            }
            else
            {

                if (found != null)
                {
                    mensajefound = "¡Ya existe un equipo con la misma Serie del Ensamble!";

                }
                else
                {
                    if (Empresas.inv_ensamble_ID == 0)
                    {
                        try
                        {
                            inventario_ensamble empre = new inventario_ensamble();

                            empre.serie = Empresas.serie;
                            empre.marca = Empresas.marca;
                            empre.modelo = Empresas.modelo;
                            empre.descripcion = Empresas.descripcion;
                            empre.color = Empresas.color;
                            empre.folio = Empresas.folio;
                            empre.fecha_folio = Empresas.fecha_folio;

                            empre.proveedor_ID = Empresas.proveedor_ID;
                            empre.Em_Cve_Empresa = Empresas.Em_Cve_Empresa;

                            //datos definidos por el sistema
                            empre.estatus_ID = 1;

                            // generacion de cadena codigo de inventario
                            var year = DateTime.Now;
                            string conver = Convert.ToString(year);
                            string[] ultimos = conver.Split('/', ' ', ':', '0');
                            char[] maspruba = conver.ToCharArray();

                            inventario_ensamble modificador = db.inventario_ensamble.OrderByDescending(x => x.inv_ensamble_ID).First();
                            Empresa etiqueta = db.Empresa.Find(empre.Em_Cve_Empresa);

                            //empre.cod_inventario = etiqueta.Em_Descripcion.ToUpper() + ultimos[5] + "-LP" + Convert.ToString(modificador.inv_laptop_ID + 1);
                            empre.cod_inventario = etiqueta.Em_Descripcion.ToUpper() + maspruba[8] + maspruba[9] + "-EM" + Convert.ToString(modificador.inv_ensamble_ID + 1);

                            db.inventario_ensamble.Add(empre);
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
                            inventario_ensamble Empre = db.inventario_ensamble.Find(Empresas.inv_ensamble_ID);
                            Empre.serie = Empresas.serie;
                            Empre.marca = Empresas.marca;
                            Empre.modelo = Empresas.modelo;
                            //suprimido por prueba de longitud
                            Empre.descripcion = Empresas.descripcion;
                            Empre.color = Empresas.color;
                            Empre.folio = Empresas.folio;
                            Empre.fecha_folio = Empresas.fecha_folio;

                            Empre.proveedor_ID = Empresas.proveedor_ID;
                            Empre.estatus_ID = Empresas.estatus_ID;
                            Empre.Em_Cve_Empresa = Empresas.Em_Cve_Empresa;

                            //codigo inventario
                            var year = DateTime.Now;
                            string conver = Convert.ToString(year);
                            string[] ultimos = conver.Split('/', ' ', ':', '0');
                            char[] maspruba = conver.ToCharArray();

                            inventario_ensamble modificador = db.inventario_ensamble.OrderByDescending(x => x.inv_ensamble_ID).First();
                            Empresa etiqueta = db.Empresa.Find(Empre.Em_Cve_Empresa);

                            //empre.cod_inventario = etiqueta.Em_Descripcion.ToUpper() + ultimos[5] + "-LP" + Convert.ToString(modificador.inv_laptop_ID + 1);
                            string temporal = etiqueta.Em_Descripcion.Replace(" ", "");
                            Empre.cod_inventario = temporal.ToUpper() + maspruba[8] + maspruba[9] + "-EM" + Empresas.inv_ensamble_ID;


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

 
        public ActionResult _FormularioEnsambles(long? inv_laptop_ID)
        //public ActionResult _FormularioLaptops(long? inv_laptop_ID)
        {  //codigo para agregar y editar usuarios
            //UsuariosPersonas Personas = new UsuariosPersonas();
            inventario_ensamble Empresas = new inventario_ensamble();

            if (inv_laptop_ID != null)
            {
                ViewBag.edit = 1;
                Empresas = db.inventario_ensamble.Find(inv_laptop_ID);


                if (Empresas.proveedor_ID != 0)
                {
                    ViewBag.proveedor = new SelectList(db.cat_proveedores.ToList(), "proveedor_ID", "proveedor", Empresas.proveedor_ID);
                }
                else
                {
                    ViewBag.proveedor = new SelectList(db.cat_proveedores.ToList(), "proveedor_ID", "proveedor");
                }

                if(Empresas.estatus_ID != 0)
                {
                    ViewBag.estatus = new SelectList(db.cat_estatus_inv.ToList(), "estatus_ID", "estatus", Empresas.estatus_ID);
                }
                else
                {
                    ViewBag.estatus = new SelectList(db.cat_estatus_inv.ToList(), "estatus_ID", "estatus", Empresas.estatus_ID);
                }

                if (Empresas.Em_Cve_Empresa != 0)
                {
                    ViewBag.lista_empresa = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion", Empresas.Em_Cve_Empresa);
                }
                else
                {
                    ViewBag.lista_empresa = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion");
                }

            }


            else
            {
                ViewBag.proveedor = new SelectList(db.cat_proveedores.ToList(), "proveedor_ID", "proveedor");
                ViewBag.estatus = new SelectList(db.cat_estatus_inv.ToList(), "estatus_ID", "estatus");
                ViewBag.lista_empresa = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion");

            }

            return PartialView(Empresas);
        }



        //metodo listo
        //public ActionResult _TablaLaptops(int? page, string filtro)
        public ActionResult _TablaEnsambles(int? page, string filtro)
        {
            const int pageSize = 10;
            int pageNumber = (page ?? 1);


            if (string.IsNullOrEmpty(filtro))
            {

                //var lista = db.inventario_lineas.Where(x=> x.estatus_ID == 2).ToList();
                var lista = db.inventario_ensamble.OrderByDescending(x=> x.inv_ensamble_ID).ToList();
                ViewBag.filtro = filtro;
                return PartialView(lista.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                var lista = db.inventario_ensamble.Where(x => x.serie.ToUpper().Contains(filtro.ToUpper().Trim())
                   || x.marca.ToUpper().Contains(filtro.ToUpper().Trim())
                   || x.cod_inventario.ToUpper().Contains(filtro.ToUpper().Trim())
                   //|| x.precio.ToUpper().Contains(filtro.ToUpper().Trim())
                   || x.modelo.ToUpper().Contains(filtro.ToUpper().Trim())).ToList();

                ViewBag.filtro = filtro;
                return PartialView(lista.ToPagedList(pageNumber, pageSize));
            }




        }

        //metodo casi listo- se debe cambiart nombre
        public ActionResult DeleteUsuario(long? inv_laptop_ID)
        {
            bool success = false; ;
            string mensajefound = "";

            try
            {                
                inventario_ensamble lap = db.inventario_ensamble.Find(inv_laptop_ID);
                lap.estatus_ID = 4;


                db.Entry(lap).State = EntityState.Modified;
                if (db.SaveChanges() > 0)
                {
                    success = true;
                }                              
            }
            catch (Exception ex)
            {
                mensajefound = "Ocurrio un error al dar baja a la laptop";
            }
            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult ExportarExcel(int? creado, string filtro)
        {
            bool success = false;
            string excelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            if (creado == 1)
            {
                // Aqui se debe cambiar por "Resguardos Laptops"
                var ruta = "~/Upload/Temporales/Descargas/" + "Inventario Ensambles - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx";
                return File(Url.Content(ruta), excelContentType, "Inventario Ensambles - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx");
            }
            // Aqui se debe cambiar por "Resguardos Laptops"
            if (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Inventario Ensambles - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx")))
            {
                System.IO.File.Delete(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Inventario Ensambles - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx"));
            }

            string savedFileName = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Inventario Ensambles - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx"));
            //FileStream stream = System.IO.File.Create(savedFileName);
            using (FileStream stream = new FileStream(savedFileName, FileMode.Create))
            {
                try
                {

                    var productos = db.Database.SqlQuery<Inventario_Emsamble_Excel>("Sp_Get_Inventario_Ensambles_excel @filtro", new SqlParameter("@filtro", filtro)).ToList();

                    using (var libro = new ExcelPackage())
                    {

                        var worksheet = libro.Workbook.Worksheets.Add("Inventario Ensambles");
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

                        var titlers = cellrs.RichText.Add("Inventario de Ensambles");
                        titlers.Bold = true;
                        titlers.FontName = "Calibri";
                        titlers.Size = 15;
                        titlers.Color = ColorTranslator.FromHtml("#44546A");
                        #endregion
                        #region llenado de la informacion
                        worksheet.Cells["D6"].LoadFromCollection(productos, PrintHeaders: true);
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

                        var tabla = worksheet.Tables.Add(new ExcelAddressBase(fromRow: 6, fromCol: 4, toRow: productos.Count + 6, toColumn: 9), "Resguardos");
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