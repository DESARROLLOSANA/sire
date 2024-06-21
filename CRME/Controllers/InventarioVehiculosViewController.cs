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
    public class InventarioVehiculosViewController : Controller
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

        //subir el archivo

        public async Task<ActionResult> CargarfileTarjeta()
        {          
            bool success = false;
            string mensaje = "";
            string FileT = "";
            JsonResult Resp = await UploadfileTarjeta();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ResponseObjectVM Respuestas = ser.Deserialize<ResponseObjectVM>(ser.Serialize(Resp.Data));
            success = Respuestas.success;
            mensaje = Respuestas.mensaje;
            FileT = Respuestas.Filet;
            ViewBag.rutatarjeta = Respuestas.Filet;
            return Json(new { success = success, mensaje, FileT });
        }

        public async Task<JsonResult> UploadfileTarjeta()
        {
            bool success = false;
            string mensaje = "";
            string msj = "";
            string Filet = "";
            string name = Path.GetRandomFileName();
            //var year = DateTime.Now;
            //string conver = Convert.ToString(year);
            //string limpieza = conver.Replace("");
            await Task.Run(() =>
            {

            string savedFileNameDownload = "";
            string nombreArchivo = "tarjeta" + name;
            //string nombreArchivoTemp = "EgresadosTemp";
            //var codigosPostales = db.CatCodigosPostales.ToList();
            FileStream stream = null;

            try
            {
                foreach (string file in Request.Files)
                {
                    if (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Sistema/files/" + nombreArchivo + ".pdf")))
                    {
                        System.IO.File.Delete(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Sistema/files/" + nombreArchivo + ".pdf"));
                    }
                    //if (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Sistema/files/" + nombreArchivoTemp + ".pdf")))
                    //{
                    //    System.IO.File.Delete(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Sistema/files/" + nombreArchivoTemp + ".pdf"));

                    //}

                    HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                    string savedFileName = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Sistema/files/"), nombreArchivo + Path.GetExtension(Path.GetFileName(hpf.FileName)));
                     Filet = "~/Upload/Sistema/files/" + nombreArchivo + ".pdf";
                     hpf.SaveAs(savedFileName);
                     success = true;
                    }
                    
                }
                catch (DbEntityValidationException ex)
                {
                    success = false;
                    mensaje = "Ocurrió un problema al subir el archivo";
                    Console.WriteLine(ex);
                    if (stream != null)
                        stream.Close();
                    stream.Dispose();
                }
            });

            return Json(new { success = success, mensaje,Filet });
        }


        public async Task<ActionResult> CargarfilePoliza()
        {
            bool success = false;
            string mensaje = "";
            string FileT = "";
            JsonResult Resp = await UploadfilePoliza();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ResponseObjectVM Respuestas = ser.Deserialize<ResponseObjectVM>(ser.Serialize(Resp.Data));
            success = Respuestas.success;
            mensaje = Respuestas.mensaje;
            FileT = Respuestas.Filet;
            ViewBag.rutatarjeta = Respuestas.Filet;
            return Json(new { success = success, mensaje, FileT });
        }


        public async Task<JsonResult> UploadfilePoliza()
        {
            bool success = false;
            string mensaje = "";
            string msj = "";
            string Filet = "";
            //var year = DateTime.Now;
            //string conver = Convert.ToString(year);
            string name = Path.GetRandomFileName();

            await Task.Run(() =>
            {

                string savedFileNameDownload = "";
                string nombreArchivo = "poliza" + name;
                //string nombreArchivoTemp = "EgresadosTemp";
                //var codigosPostales = db.CatCodigosPostales.ToList();
                FileStream stream = null;

                try
                {
                    foreach (string file in Request.Files)
                    {
                        if (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Sistema/files/" + nombreArchivo + ".pdf")))
                        {
                            System.IO.File.Delete(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Sistema/files/" + nombreArchivo + ".pdf"));
                        }
                        //if (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Sistema/files/" + nombreArchivoTemp + ".pdf")))
                        //{
                        //    System.IO.File.Delete(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Sistema/files/" + nombreArchivoTemp + ".pdf"));

                        //}

                        HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                        string savedFileName = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Sistema/files/"), nombreArchivo + Path.GetExtension(Path.GetFileName(hpf.FileName)));
                        Filet = "~/Upload/Sistema/files/" + nombreArchivo + ".pdf";
                        hpf.SaveAs(savedFileName);
                        success = true;
                    }

                }
                catch (DbEntityValidationException ex)
                {
                    success = false;
                    mensaje = "Ocurrió un problema al subir el archivo";
                    Console.WriteLine(ex);
                    if (stream != null)
                        stream.Close();
                    stream.Dispose();
                }
            });

            return Json(new { success = success, mensaje, Filet });
        }




        // public ActionResult SaveEmpresa(inventario_vehiculos Empresas)
        public ActionResult SaveEmpresa(Inventario_lista_vehiculos Empresas)
        {
            Auditoria auditoria = new Auditoria();
            //Empresa empresas = new Empresa();
            var serializerCat = new JavaScriptSerializer();
            bool success = false;
            string mensajefound = "";



            string verificar = Empresas.tipo_vehiculo + Empresas.empresa + '-' + Empresas.numero_economico_vehiculo; ;

            //Esta pediente de revision la verificacion de identificador unico, ya que puede no cumplir con estos
            //var found = db.inventario_lineas.FirstOrDefault(x => x.telefono == Empresas.telefono && x.inv_linea_ID != Empresas.inv_linea_ID);
            var found = db.inventario_vehiculos.FirstOrDefault(x => x.numero_economico_vehiculo == verificar && x.inv_vehiculo_ID != Empresas.inv_vehiculo_ID);

            if (found != null)
            {
                mensajefound = "¡Ya existe una linea que coincide con la ingresada!";

            }
            else
            {
                if (Empresas.inv_vehiculo_ID == 0)
                {
                    try
                    {
                        inventario_vehiculos empre = new inventario_vehiculos();
                        
                        empre.empresa = Empresas.empresa;
                        empre.tipo_vehiculo = Empresas.tipo_vehiculo;      
                        //empre.numero_economico_vehiculo =  Empresas.tipo_vehiculo + Empresas.empresa  + '-' +Empresas.numero_economico_vehiculo;
                        empre.numero_economico_vehiculo = Empresas.numero_economico_vehiculo;

                        empre.serie_motor = Empresas.serie_motor;
                        empre.serie_motor = Empresas.serie_motor;
                        empre.cuadro_chasis = Empresas.cuadro_chasis;
                        empre.marca_motor = Empresas.marca_motor;
                        empre.anio = Empresas.anio;
                        empre.color = Empresas.color;
                        empre.modelo = Empresas.modelo;
                        empre.placas = Empresas.placas;
                        empre.poliza_seguro = Empresas.poliza_seguro;
                        empre.inciso = Empresas.inciso;
                        empre.vigencia_del = Empresas.vigencia_del;
                        empre.vigencia_al = Empresas.vigencia_al;
                        //archivo pdf
                        empre.tarjeta_circulacion = Empresas.tarjeta_circulacion;
                        empre.vigencia_tarjeta = Empresas.vigencia_tarjeta;
                    //otro archivo pdf
                        empre.proveedor_ID = Empresas.proveedor_ID;
                        empre.folio = Empresas.folio;
                        empre.fecha_folio = Empresas.fecha_folio;
                        empre.precio = Empresas.precio;

                        //actualizacion gps
                        empre.empresa_gps = Empresas.empresa_gps;
                        empre.imei_gps = Empresas.imei_gps;


                    //datos definidos por el sistema
                        empre.estatus_ID = 1;


                        
            
                        db.inventario_vehiculos.Add(empre);
                        if (db.SaveChanges() > 0)
                        {
                            success = true;
                        }

                    //aqui se debe establece en cero los valores de kilometros actuales y proximo mantenimiento



                    //aqui se debe establecer la ubicacion del archivo pdf de seguro
                        inventario_vehiculos ultimo = db.inventario_vehiculos.OrderByDescending(x => x.inv_vehiculo_ID).First();

                        inv_vehiculo_pdf_seguro pdf_uno = new inv_vehiculo_pdf_seguro();
                        pdf_uno.inv_vehiculo_ID = ultimo.inv_vehiculo_ID;
                        pdf_uno.url_pdf = Empresas.url_pdf_seguro;

                        db.inv_vehiculo_pdf_seguro.Add(pdf_uno);
                        if (db.SaveChanges() > 0)
                        {
                            success = true;
                        }

                    //aqui se debe establecer la ubicacion del archivo pdf de la tarjeta de circulacion

                        inv_vehiculo_pdf_tarjeta_circulacion pdf_dos = new inv_vehiculo_pdf_tarjeta_circulacion();
                        pdf_dos.inv_vehiculo_ID = ultimo.inv_vehiculo_ID;
                        pdf_dos.url_pdf = Empresas.url_pdf_tarjeta;
                    
                        db.inv_vehiculo_pdf_tarjeta_circulacion.Add(pdf_dos);
                        if (db.SaveChanges() > 0)
                        {
                            success = true;
                        }



                        auditoria.modulo = "Inventario Vehiculos";
                        auditoria.idregistro = empre.inv_vehiculo_ID;
                        auditoria.accion = "Alta";
                        auditoria.tabla = "inventario_vehiculos";
                        auditoria.idusuario = Auth.Usuario.sistemas_ID;
                        auditoria.fecha = DateTime.Now;

                        db.Auditoria.Add(auditoria);

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
                        inventario_vehiculos Empre = db.inventario_vehiculos.Find(Empresas.inv_vehiculo_ID);
                        Empre.numero_economico_vehiculo = Empresas.numero_economico_vehiculo;
                        Empre.empresa = Empresas.empresa;
                        Empre.tipo_vehiculo = Empresas.tipo_vehiculo;

                        Empre.serie_motor = Empresas.serie_motor;
                        Empre.cuadro_chasis = Empresas.cuadro_chasis;
                        Empre.marca_motor = Empresas.marca_motor;
                        Empre.anio = Empresas.anio;
                        Empre.color = Empresas.color;
                        Empre.modelo = Empresas.modelo;
                        Empre.placas = Empresas.placas;
                        Empre.poliza_seguro = Empresas.poliza_seguro;
                        Empre.inciso = Empresas.inciso;
                        Empre.vigencia_del = Empresas.vigencia_del;
                        Empre.vigencia_al = Empresas.vigencia_al;
                        //archivo pdf
                        Empre.tarjeta_circulacion = Empresas.tarjeta_circulacion;
                        Empre.vigencia_tarjeta = Empresas.vigencia_tarjeta;
                        //otro archivo pdf
                        Empre.proveedor_ID = Empresas.proveedor_ID;
                        Empre.folio = Empresas.folio;
                        Empre.fecha_folio = Empresas.fecha_folio;
                        Empre.precio = Empresas.precio;

                        //Actualizacion gps
                        Empre.empresa_gps = Empresas.empresa_gps;
                        Empre.imei_gps = Empresas.imei_gps;



                        db.Entry(Empre).State = EntityState.Modified;
                        if (db.SaveChanges() > 0)
                        {
                            success = true;
                        }


                        auditoria.modulo = "Inventario Vehiculos";
                        auditoria.idregistro = Empre.inv_vehiculo_ID;
                        auditoria.accion = "Modificacion";
                        auditoria.tabla = "inventario_vehiculos";
                        auditoria.idusuario = Auth.Usuario.sistemas_ID;
                        auditoria.fecha = DateTime.Now;

                        db.Auditoria.Add(auditoria);

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

            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult _FormularioVehiculos(long? inv_vehiculo_ID)
        {  //codigo para agregar y editar usuarios
            //UsuariosPersonas Personas = new UsuariosPersonas();
            //inventario_vehiculos Empresas = new inventario_vehiculos();
            // Personas Persona = new Personas();

            Inventario_lista_vehiculos Empresas = new Inventario_lista_vehiculos();

            if (inv_vehiculo_ID != null)
            {
                ViewBag.edit = 1;

                Empresas = db.Database.SqlQuery<Inventario_lista_vehiculos>("Sp_Get_Inventario_Vehiculo @Id_vehiculo", new SqlParameter("@Id_vehiculo", inv_vehiculo_ID)).FirstOrDefault();

                //Empresas = db.inventario_vehiculos.Find(inv_vehiculo_ID);


                //añadir controlador de listas
                //ViewBag.idGenero = new SelectList(db.CatGeneros.ToList(), "idGenero", "nbGenero");
                
                //tipo vehiculo
                List<SelectListItem> items1 = new List<SelectListItem>();
                items1.Add(new SelectListItem { Value = "C", Text = "Moto" });
                items1.Add(new SelectListItem { Value = "M", Text = "Automolvil" });
                if(Empresas.tipo_vehiculo != null){
                    ViewBag.t_vehiculo = new SelectList(items1, "Value", "Text", Empresas.tipo_vehiculo);
                }
                else
                {
                    ViewBag.t_vehiculo = new SelectList(items1, "Value", "Text");
                }


                //proveedor
                if (Empresas.proveedor_ID != null)
                {
                    ViewBag.proveedor = new SelectList(db.cat_proveedores.ToList(), "proveedor_ID", "proveedor", Empresas.proveedor_ID);
                }
                else
                {
                    ViewBag.proveedor = new SelectList(db.cat_proveedores.ToList(), "proveedor_ID", "proveedor");
                }

                //empresa
                List<SelectListItem> items3 = new List<SelectListItem>();
                items3.Add(new SelectListItem { Value = "S", Text = "Sana" });
                items3.Add(new SelectListItem { Value = "E", Text = "Ecolsur" });
                items3.Add(new SelectListItem { Value = "C", Text = "Corbase" });
                items3.Add(new SelectListItem { Value = "U", Text = "SAU" });
                items3.Add(new SelectListItem { Value = "P", Text = "Pellets" });

                if (Empresas.empresa != null){
                    ViewBag.plan = new SelectList(items3, "Value", "Text", Empresas.empresa);
                }
                else
                {
                    ViewBag.plan = new SelectList(items3, "Value", "Text");
                }


                List<SelectListItem> items4 = new List<SelectListItem>();
                items4.Add(new SelectListItem { Value = "AT&T", Text = "AT&T" });
                items4.Add(new SelectListItem { Value = "N/A", Text = "N/A" });

                if (Empresas.empresa_gps != null)
                {
                    ViewBag.gps_empr = new SelectList(items4, "Value", "Text", Empresas.empresa_gps);
                }
                else
                {
                    ViewBag.gps_empr = new SelectList(items4, "Value", "Text");
                }

            }

            else
            {
                List<SelectListItem> items1 = new List<SelectListItem>();
                items1.Add(new SelectListItem { Value = "M", Text = "Moto" });
                items1.Add(new SelectListItem { Value = "C", Text = "Automovil" });
                ViewBag.t_vehiculo = new SelectList(items1, "Value", "Text");


                 ViewBag.proveedor = new SelectList(db.cat_proveedores.ToList(), "proveedor_ID", "proveedor");


                List<SelectListItem> items3 = new List<SelectListItem>();
                items3.Add(new SelectListItem { Value = "S", Text = "Sana" });
                items3.Add(new SelectListItem { Value = "E", Text = "Ecolsur" });
                items3.Add(new SelectListItem { Value = "C", Text = "Corbase" });
                items3.Add(new SelectListItem { Value = "U", Text = "SAU" });
                items3.Add(new SelectListItem { Value = "P", Text = "Pellets" });
                ViewBag.plan = new SelectList(items3, "Value", "Text");


                List<SelectListItem> items4 = new List<SelectListItem>();
                items4.Add(new SelectListItem { Value = "AT&T", Text = "AT&T" });
                items4.Add(new SelectListItem { Value = "N/A", Text = "N/A" });

              
                ViewBag.gps_empr = new SelectList(items4, "Value", "Text");
              

            }

            return PartialView(Empresas);
        }



        //metodo listo
        //public ActionResult _TablaVehiculos(int? page)
        //{
        //    const int pageSize = 10;
        //    int pageNumber = (page ?? 1);

        //    //var lista = db.inventario_lineas.Where(x=> x.estatus_ID == 2).ToList();

        //    var lista = db.inventario_vehiculos.OrderByDescending(x => x.inv_vehiculo_ID).ToList();
        //    //var lista = db.inventario_lineas.ToList();

        //    return PartialView(lista.ToPagedList(pageNumber, pageSize));
        //}


        public ActionResult _TablaVehiculos(int? page, string filtro)
        {
            const int pageSize = 10;
            int pageNumber = (page ?? 1);

            //var lista = db.inventario_lineas.Where(x=> x.estatus_ID == 2).ToList();
            if (string.IsNullOrEmpty(filtro))
            {

                var lista = db.inventario_vehiculos.OrderByDescending(x => x.inv_vehiculo_ID).ToList();
                //var lista = db.inventario_lineas.ToList();
                ViewBag.filtro = filtro;

                return PartialView(lista.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                var lista = db.inventario_vehiculos.Where(x => x.numero_economico_vehiculo.ToUpper().Contains(filtro.ToUpper().Trim())
                   //|| x.placas.ToUpper().Contains(filtro.ToUpper().Trim()) || x.folio.ToUpper().Contains(filtro.ToUpper().Trim())
                   //|| x.ubicacion.ToUpper().Contains(filtro.ToUpper().Trim())
                   || x.placas.ToUpper().Contains(filtro.ToUpper().Trim())).ToList();

                ViewBag.filtro = filtro;

                return PartialView(lista.ToPagedList(pageNumber, pageSize));
            }

        }




        //metodo casi listo- se debe cambiart nombre
        public ActionResult DeleteUsuario(long? inv_vehiculo_ID)
        {
            bool success = false; ;
            string mensajefound = "";

            try
            {
                Auditoria auditoria = new Auditoria();
                inventario_vehiculos veh = db.inventario_vehiculos.Find(inv_vehiculo_ID);

                if (veh.estatus_ID != 4)
                {
                    veh.estatus_ID = 4;
                }
                else
                {
                    veh.estatus_ID = 1;
                }
                //lin.estatus_ID = 4;
                //Empre.Fecha_Baja = DateTime.Now;
                //Empre.Oper_Baja = Auth.Usuario.username;

                db.Entry(veh).State = EntityState.Modified;
                if (db.SaveChanges() > 0)
                {
                    success = true;
                }

                auditoria.modulo = "Inventario Vehiculos";
                auditoria.idregistro = veh.inv_vehiculo_ID;
                auditoria.accion = "Baja";
                auditoria.tabla = "inventario_vehiculos";
                auditoria.idusuario = Auth.Usuario.sistemas_ID;
                auditoria.fecha = DateTime.Now;

                db.Auditoria.Add(auditoria);

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


        //// aqui se introduce el metodo para generar el excel
        public ActionResult ExportarExcel(int? creado)
        {
            bool success = false;
            string excelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            if (creado == 1)
            {
                // Aqui se debe cambiar por "Resguardos Mobiliario"
                var ruta = "~/Upload/Temporales/Descargas/" + "Inventario Vehiculos - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx";
                return File(Url.Content(ruta), excelContentType, "Inventario Vehiculos - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx");
            }
            // Aqui se debe cambiar por "Resguardos Mobiliario"
            if (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Inventario Vehiculos - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx")))
            {
                System.IO.File.Delete(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Inventario Vehiculos - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx"));
            }

            string savedFileName = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Inventario Vehiculos - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx"));
            //FileStream stream = System.IO.File.Create(savedFileName);
            using (FileStream stream = new FileStream(savedFileName, FileMode.Create))
            {
                try
                {

                    var productos = db.Database.SqlQuery<Inventario_Lista_Vehiculo_Excel>("Sp_Get_Vehiculos_Excel").ToList();

                    using (var libro = new ExcelPackage())
                    {

                        var worksheet = libro.Workbook.Worksheets.Add("Inventario Vehiculos");
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

                        var titlers = cellrs.RichText.Add("Inventario de Vehiculos");
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
                        excelImage2.From.Column = 8;
                        //excelImage2.From.Column = 9;
                        excelImage2.From.Row = 0;
                        excelImage2.SetSize(150, 80);
                        // 2x2 px space for better alignment
                        excelImage2.From.ColumnOff = Pixel2MTU(2);
                        excelImage2.From.RowOff = Pixel2MTU(2);
                        #endregion

                        var tabla = worksheet.Tables.Add(new ExcelAddressBase(fromRow: 6, fromCol: 4, toRow: productos.Count + 6, toColumn: 8), "Resguardos");
                        tabla.ShowHeader = true;
                        tabla.TableStyle = TableStyles.Light5;
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






public class ResponseObjectVM
    {
        public ResponseObjectVM()
        {
            
            success = false;
            Data = null;
            mensaje = "Ha ocurrido un problema al conectar al servidor.";
            Filet = "";
        }

        
        public bool success { get; set; }
        public object Data { get; set; }
        public string mensaje { get; set; }
        public string Filet { get; set; }

    }






}