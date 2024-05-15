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
    public class InventarioMobiliarioViewController : Controller
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
        public ActionResult SaveEmpresa(inventario_mobiliario Empresas)
        {
            //Empresa empresas = new Empresa();
            var serializerCat = new JavaScriptSerializer();
            bool success = false;
            string mensajefound = "";
            
            //se tuvo que omitir la verificacion de identificador unico(ya se serie o EMEI del equipo) porque al no contar con esto se complica el proceso
            //var found = db.inventario_lineas.FirstOrDefault(x => x.telefono == Empresas.telefono && x.inv_linea_ID != Empresas.inv_linea_ID);
            //var found = db.inventario_mobiliario.FirstOrDefault(x=> x.inv_mobiliario_ID == Empresas.inv_mobiliario_ID);
            //var found = db.Empresa.FirstOrDefault(x => x.Em_Descripcion == Empresas.Em_Descripcion && x.Em_Cve_Empresa != Empresas.Em_Cve_Empresa);
            //if (found != null)
            //{
            //    mensajefound = "¡Ya existe un mobiliario que coincide con el ingresado!";
            //}
           // else
            //{
                if (Empresas.inv_mobiliario_ID == 0)
                {
                    try
                    {
                        inventario_mobiliario empre = new inventario_mobiliario();
                        empre.tipo_mobiliario_ID = Empresas.tipo_mobiliario_ID;
                        empre.color = Empresas.color;
                        empre.tipo = Empresas.tipo;
                        empre.precio = Empresas.precio;
                        empre.folio = Empresas.folio;
                        //combox
                        empre.proveedor_ID = Empresas.proveedor_ID;
                        empre.fecha_folio = Empresas.fecha_folio;
                        empre.ubicacion = Empresas.ubicacion;
                        empre.departamento = Empresas.departamento;

                        //datos definidos por el sistema
                        empre.estatus_ID = 1;
                        empre.Em_Cve_Empresa = Empresas.Em_Cve_Empresa;
                        //empre.Em_Cve_Empresa = 1;

                        //generacion de cod_inventario
                        var year = DateTime.Now;
                        string conver = Convert.ToString(year);
                        string[] ultimos = conver.Split('/', ' ', ':', '0');
                        char[] maspruba = conver.ToCharArray();

                        inventario_mobiliario modificador = db.inventario_mobiliario.OrderByDescending(x => x.inv_mobiliario_ID).First();
                        Empresa etiqueta = db.Empresa.Find(empre.Em_Cve_Empresa);
                    //se añade la cadena generada como codigo de inventario
                    //empre.cod_inventario = etiqueta.Em_Descripcion.ToUpper() + ultimos[5] + "-MB" + Convert.ToString(modificador.inv_mobiliario_ID + 1);
                    //empre.cod_inventario = etiqueta.Em_Descripcion.ToUpper() + maspruba[8] + maspruba[9] + "-MB" + Convert.ToString(modificador.inv_mobiliario_ID + 1);
                        string temporal = etiqueta.Em_Descripcion.Replace(" ","");
                        empre.cod_inventario = temporal.ToUpper() + maspruba[8] + maspruba[9] + "-MB" + Convert.ToString(modificador.inv_mobiliario_ID + 1);


                    db.inventario_mobiliario.Add(empre);
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
                        inventario_mobiliario Empre = db.inventario_mobiliario.Find(Empresas.inv_mobiliario_ID);
                        Empre.tipo_mobiliario_ID = Empresas.tipo_mobiliario_ID;
                        Empre.color = Empresas.color;
                        Empre.tipo = Empresas.tipo;
                        //Empre.precio = Empresas.precio;
                        Empre.folio = Empresas.folio;
                        //combox
                        Empre.proveedor_ID = Empresas.proveedor_ID;
                        Empre.fecha_folio = Empresas.fecha_folio;
                        Empre.ubicacion = Empresas.ubicacion;
                        Empre.departamento = Empresas.departamento;
                        
                        //añadidos
                        Empre.estatus_ID = Empresas.estatus_ID;
                        Empre.Em_Cve_Empresa = Empresas.Em_Cve_Empresa;

                        //generacion de cod_inventario
                        var year = DateTime.Now;
                        string conver = Convert.ToString(year);
                        string[] ultimos = conver.Split('/', ' ', ':', '0');
                        char[] maspruba = conver.ToCharArray();

                        inventario_mobiliario modificador = db.inventario_mobiliario.OrderByDescending(x => x.inv_mobiliario_ID).First();
                        Empresa etiqueta = db.Empresa.Find(Empre.Em_Cve_Empresa);
                        //se añade la cadena generada como codigo de inventario
                        //empre.cod_inventario = etiqueta.Em_Descripcion.ToUpper() + ultimos[5] + "-MB" + Convert.ToString(modificador.inv_mobiliario_ID + 1);
                        //empre.cod_inventario = etiqueta.Em_Descripcion.ToUpper() + maspruba[8] + maspruba[9] + "-MB" + Convert.ToString(modificador.inv_mobiliario_ID + 1);
                        string temporal = etiqueta.Em_Descripcion.Replace(" ", "");
                        Empre.cod_inventario = temporal.ToUpper() + maspruba[8] + maspruba[9] + "-MB" + Empresas.inv_mobiliario_ID;





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

            ///}

            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult _FormularioMobiliario(long? inv_mobiliario_ID)
        {  //codigo para agregar y editar usuarios
            //UsuariosPersonas Personas = new UsuariosPersonas();
            inventario_mobiliario Empresas = new inventario_mobiliario();
           // Personas Persona = new Personas();

            if (inv_mobiliario_ID != null)
            {
                ViewBag.edit = 1;
                Empresas = db.inventario_mobiliario.Find(inv_mobiliario_ID);
                //añadir controlador de listas
                //ViewBag.idGenero = new SelectList(db.CatGeneros.ToList(), "idGenero", "nbGenero");     

                List<SelectListItem> items2 = new List<SelectListItem>();
                items2.Add(new SelectListItem { Value = "Caucel", Text = "Caucel" });
                items2.Add(new SelectListItem { Value = "Ecolsur", Text = "Ecolsur" });
                items2.Add(new SelectListItem { Value = "Ecolsur Cancún", Text = "Ecolsur Cancún" });
                items2.Add(new SelectListItem { Value = "Kanasín", Text = "Kanasín" });
                items2.Add(new SelectListItem { Value = "Oficina matriz", Text = "Oficina matriz" });
                items2.Add(new SelectListItem { Value = "Operaciones", Text = "Operaciones" });
                items2.Add(new SelectListItem { Value = "Relleno sanitario", Text = "Relleno sanitario" });
                items2.Add(new SelectListItem { Value = "SAU", Text = "SAU" });
                items2.Add(new SelectListItem { Value = "Uman", Text = "Uman" });
                //ViewBag.estatus_adendum = new SelectList( "Con adendum", "Sin adendum");

                if (Empresas.ubicacion != null)
                {
                    ViewBag.ubicacion1 = new SelectList(items2, "Value", "Text", Empresas.ubicacion);
                }
                else
                {
                    ViewBag.ubicacion1 = new SelectList(items2, "Value", "Text");
                }


                List<SelectListItem> items3 = new List<SelectListItem>();
                items3.Add(new SelectListItem { Value = "Administración", Text = "Administración" });
                items3.Add(new SelectListItem { Value = "Almacen", Text = "Almacen" });
                items3.Add(new SelectListItem { Value = "Atencion a clientes", Text = "Atencion a clientes" });
                items3.Add(new SelectListItem { Value = "Caja", Text = "Caja" });
                items3.Add(new SelectListItem { Value = "Calidad", Text = "Calidad" });
                //ViewBag.estatus_adendum = new SelectList( "Con adendum", "Sin adendum");
                ViewBag.departamento1 = new SelectList(items3, "Value", "Text", Empresas.departamento);

                if (Empresas.departamento != null)
                {
                    ViewBag.departamento1 = new SelectList(items3.ToList(), "Value", "Text", Empresas.departamento);
                }
                else
                {
                    ViewBag.departamento1 = new SelectList(items3.ToList(), "Value", "Text");
                }


                if (Empresas.proveedor_ID != 0)
                {
                    ViewBag.proveedor = new SelectList(db.cat_proveedores.ToList(), "proveedor_ID", "proveedor", Empresas.proveedor_ID);
                }
                else
                {
                    ViewBag.proveedor = new SelectList(db.cat_proveedores.ToList(), "proveedor_ID", "proveedor");
                }

                //añadidos
                


                if (Empresas.tipo_mobiliario_ID != 0)
                {
                    ViewBag.tipo_mobiliario = new SelectList(db.inventario_tipo_mobiliario.OrderBy(x => x.mobiliario).Where(x => x.estatus_ID == 1).ToList(), "tipo_mobiliario_ID", "mobiliario", Empresas.tipo_mobiliario_ID);
                }
                else
                {
                    ViewBag.tipo_mobiliario = new SelectList(db.inventario_tipo_mobiliario.OrderBy(x => x.mobiliario).Where(x => x.estatus_ID == 1).ToList(), "tipo_mobiliario_ID", "mobiliario");
                }

                if (Empresas.estatus_ID != 0)
                {
                    ViewBag.estatus = new SelectList(db.cat_estatus_inv.ToList(), "estatus_ID", "estatus", Empresas.estatus_ID);
                }
                else
                {
                    ViewBag.estatus = new SelectList(db.cat_estatus_inv.ToList(), "estatus_ID", "estatus");
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
                List<SelectListItem> items2 = new List<SelectListItem>();
                items2.Add(new SelectListItem { Value = "Caucel", Text = "Caucel" });
                items2.Add(new SelectListItem { Value = "Ecolsur", Text = "Ecolsur" });
                items2.Add(new SelectListItem { Value = "Ecolsur Cancún", Text = "Ecolsur Cancún" });
                items2.Add(new SelectListItem { Value = "Kanasín", Text = "Kanasín" });
                items2.Add(new SelectListItem { Value = "Oficina matriz", Text = "Oficina matriz" });
                items2.Add(new SelectListItem { Value = "Operaciones", Text = "Operaciones" });
                items2.Add(new SelectListItem { Value = "Relleno sanitario", Text = "Relleno sanitario" });
                items2.Add(new SelectListItem { Value = "SAU", Text = "SAU" });
                items2.Add(new SelectListItem { Value = "Uman", Text = "Uman" });
                //ViewBag.estatus_adendum = new SelectList( "Con adendum", "Sin adendum");
                ViewBag.ubicacion1 = new SelectList(items2, "Value", "Text");


                List<SelectListItem> items3 = new List<SelectListItem>();
                items3.Add(new SelectListItem { Value = "Administración", Text = "Administración" });
                items3.Add(new SelectListItem { Value = "Almacen", Text = "Almacen" });
                items3.Add(new SelectListItem { Value = "Atencion a clientes", Text = "Atencion a clientes" });
                items3.Add(new SelectListItem { Value = "Caja", Text = "Caja" });
                items3.Add(new SelectListItem { Value = "Calidad", Text = "Calidad" });
                ViewBag.departamento1 = new SelectList(items3, "Value", "Text");

                //añadido
                ViewBag.proveedor = new SelectList(db.cat_proveedores.ToList(), "proveedor_ID", "proveedor");
                //ViewBag.tipo_mobiliario_ID = new SelectList(db.inventario_tipo_mobiliario.ToList(), "tipo_mobiliario_ID", "mobiliario");
                ViewBag.tipo_mobiliario = new SelectList(db.inventario_tipo_mobiliario.OrderBy(x => x.mobiliario).Where(x => x.estatus_ID == 1).ToList(), "tipo_mobiliario_ID", "mobiliario");
                ViewBag.estatus = new SelectList(db.cat_estatus_inv.ToList(), "estatus_ID", "estatus");

                ViewBag.lista_empresa = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion");
            }

            return PartialView(Empresas);
        }


        public ActionResult ExportarExcel(int? creado)
        {
            bool success = false;
            string excelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            if (creado == 1)
            {
                // Aqui se debe cambiar por "Resguardos Mobiliario"
                var ruta = "~/Upload/Temporales/Descargas/" + "Inventario mobiliario - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx";
                return File(Url.Content(ruta), excelContentType, "Inventario mobiliario - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx");
            }
            // Aqui se debe cambiar por "Resguardos Mobiliario"
            if (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Inventario Mobiliario - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx")))
            {
                System.IO.File.Delete(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Inventario Mobiliario - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx"));
            }

            string savedFileName = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Inventario mobiliario - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx"));
            //FileStream stream = System.IO.File.Create(savedFileName);
            using (FileStream stream = new FileStream(savedFileName, FileMode.Create))
            {
                try
                {

                    var productos = db.Database.SqlQuery<Inventario_Lista_Excel>("Sp_Get_Inventario_mobiliario_excel").ToList();

                    using (var libro = new ExcelPackage())
                    {

                        var worksheet = libro.Workbook.Worksheets.Add("Inventario Mobiliario");
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

                        var titlers = cellrs.RichText.Add("Inventario de Mobiliario");
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
        //metodo listo
        public ActionResult _TablaMobiliario(int? page, string filtro)
        {
            const int pageSize = 10;
            int pageNumber = (page ?? 1);
            
            if (string.IsNullOrEmpty(filtro))
            {
                var lista = db.inventario_mobiliario.OrderByDescending(x => x.inv_mobiliario_ID).ToList();
                ViewBag.filtro = filtro;
                return PartialView(lista.ToPagedList(pageNumber, pageSize));

                
            }
            else
            {
                var lista = db.inventario_mobiliario.Where(x => x.tipo.ToUpper().Contains(filtro.ToUpper().Trim())
                   || x.cod_inventario.ToUpper().Contains(filtro.ToUpper().Trim()) || x.folio.ToUpper().Contains(filtro.ToUpper().Trim())
                   || x.ubicacion.ToUpper().Contains(filtro.ToUpper().Trim())
                   || x.departamento.ToUpper().Contains(filtro.ToUpper().Trim())).ToList();

                ViewBag.filtro = filtro;

                return PartialView(lista.ToPagedList(pageNumber, pageSize));
            }

        }

        //metodo casi listo- se debe cambiart nombre
        public ActionResult DeleteUsuario(long? inv_mobiliario_ID)
        {
            bool success = false; ;
            string mensajefound = "";

            try
            {                
                inventario_mobiliario mob = db.inventario_mobiliario.Find(inv_mobiliario_ID);
                mob.estatus_ID = 4;
                //Empre.Fecha_Baja = DateTime.Now;
                //Empre.Oper_Baja = Auth.Usuario.username;

                db.Entry(mob).State = EntityState.Modified;
                if (db.SaveChanges() > 0)
                {
                    success = true;
                }                              
            }
            catch (Exception ex)
            {
                mensajefound = "Ocurrio un error al dar baja al Mobiliario";
            }
            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);
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