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
    public class NuevoEmpleadosViewController : Controller
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
        public ActionResult SaveEmpresa(cat_usuarios Empresas)
        {
            //Empresa empresas = new Empresa();
            var serializerCat = new JavaScriptSerializer();
            bool success = false;
            string mensajefound = "";
            //linea pendiente de revision

            var temporal = Empresas.nombres + " " + Empresas.paterno + " " + Empresas.materno;

            //var found = db.inventario_tipo_mobiliario.FirstOrDefault(x => x.mobiliario == Empresas.mobiliario && x.tipo_mobiliario_ID != Empresas.tipo_mobiliario_ID);
            var found = db.cat_usuarios.FirstOrDefault(x => x.nombre_completo == temporal && x.usuario_ID != Empresas.usuario_ID);

            if (string.IsNullOrWhiteSpace(temporal))
            {
              mensajefound = "¡No puede estar en blanco el nombre del Empleado!";
            }
            else
            {
                if (found != null)
                {
                    mensajefound = "¡Ya existe un empleado llamado asi!";
                }
                else
                {

                    if (Empresas.usuario_ID == 0)
                    {
                        try
                        {
                            cat_usuarios empre = new cat_usuarios();
                            empre.nombres = Empresas.nombres;
                            empre.paterno = Empresas.paterno;
                            empre.materno = Empresas.materno;

                            empre.nombre_completo = Empresas.nombres + " " + Empresas.paterno + " " + Empresas.materno;
                            empre.idNivelEstudio = Empresas.idNivelEstudio;
                            empre.Pu_Cve_Puesto = Empresas.Pu_Cve_Puesto;
                            empre.estatus_ID = 1;
                            empre.correo = Empresas.correo;
                            empre.Dp_Cve_Departamento = Empresas.Dp_Cve_Departamento;
                            empre.Em_Cve_Empresa = Empresas.Em_Cve_Empresa;
                            empre.Sc_Cve_Sucursal = Empresas.Sc_Cve_Sucursal;
                            empre.RFC = Empresas.RFC;
                        
                            db.cat_usuarios.Add(empre);
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
                            cat_usuarios Empre = db.cat_usuarios.Find(Empresas.usuario_ID);
                            Empre.nombres = Empresas.nombres;
                            Empre.paterno = Empresas.paterno;
                            Empre.materno = Empresas.materno;
                            Empre.nombre_completo = Empresas.nombres + " " + Empresas.paterno + " " + Empresas.materno;

                            Empre.idNivelEstudio = Empresas.idNivelEstudio;
                            Empre.correo = Empresas.correo;
                            Empre.Pu_Cve_Puesto = Empresas.Pu_Cve_Puesto;
                            Empre.Dp_Cve_Departamento = Empresas.Dp_Cve_Departamento;
                            Empre.Em_Cve_Empresa = Empresas.Em_Cve_Empresa;
                            Empre.Sc_Cve_Sucursal = Empresas.Sc_Cve_Sucursal;
                            Empre.RFC = Empresas.RFC;

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
            cat_usuarios Empresas = new cat_usuarios();
           // Personas Persona = new Personas();

            if (inv_linea_ID != null)
            {
                ViewBag.edit = 1;
                Empresas = db.cat_usuarios.Find(inv_linea_ID);
                ViewBag.puesto = new SelectList(db.Puestos.ToList(), "Pu_Cve_Puesto", "Pu_Descripcion", Empresas.Pu_Cve_Puesto);
                ViewBag.departamento = new SelectList(db.Departamentos.ToList(), "Dp_Cve_Departamento", "Dp_Descripcion", Empresas.Dp_Cve_Departamento);
                ViewBag.empresa = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion", Empresas.Em_Cve_Empresa);
                ViewBag.sucursal = new SelectList(db.Sucursal.ToList(), "Sc_Cve_Sucursal", "Sc_Descripcion", Empresas.Sc_Cve_Sucursal);
                ViewBag.nvlest = new SelectList(db.NivelAcademico.ToList(), "idNivelEstudio", "desNivelEstudio", Empresas.idNivelEstudio);
                //añadir controlador de listas
                //ViewBag.idGenero = new SelectList(db.CatGeneros.ToList(), "idGenero", "nbGenero");
                //ViewBag.plan = new SelectList(items3, "Value", "Text", Empresas.nombre_plan);
            }
            else
            {
                ViewBag.empresa = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion");
                ViewBag.departamento = new SelectList("", "Dp_Cve_Departamento", "Dp_Descripcion");
                ViewBag.puesto = new SelectList("", "Pu_Cve_Puesto", "Pu_Descripcion");
                ViewBag.sucursal = new SelectList("", "Sc_Cve_Sucursal", "Sc_Descripcion");
                ViewBag.nvlest = new SelectList(db.NivelAcademico.ToList(), "idNivelEstudio", "desNivelEstudio");
            }

            return PartialView(Empresas);
        }



        //metodo listo
        public ActionResult _TablaLineas(int? page, string filtro)
        {
            const int pageSize = 10;
            int pageNumber = (page ?? 1);

           if (string.IsNullOrEmpty(filtro))
           {
                //var lista = db.inventario_lineas.Where(x=> x.estatus_ID == 2).ToList();

                var lista = db.cat_usuarios.OrderByDescending(x=> x.usuario_ID).ToList();
            //var lista = db.inventario_lineas.ToList();

            return PartialView(lista.ToPagedList(pageNumber, pageSize));

            }
            else
            {
                var lista = db.cat_usuarios.Where(x => x.nombre_completo.ToUpper().Contains(filtro.ToUpper().Trim())
                   //|| x.cod_inventario.ToUpper().Contains(filtro.ToUpper().Trim()) || x.folio.ToUpper().Contains(filtro.ToUpper().Trim())
                   //|| x.ubicacion.ToUpper().Contains(filtro.ToUpper().Trim())
                   //|| x.departamento.ToUpper().Contains(filtro.ToUpper().Trim())
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
                cat_usuarios lin = db.cat_usuarios.Find(inv_linea_ID);

                if (lin.estatus_ID != 2)
                {
                    lin.estatus_ID = 2;
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
                mensajefound = "Ocurrio un error al dar baja al empleado";
            }
            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult HabilitarUsuario(long? inv_linea_ID)
        {
            bool success = false; ;
            string mensajefound = "";

            try
            {
                cat_usuarios lin = db.cat_usuarios.Find(inv_linea_ID);

                if (lin.estatus_ID != 1)
                {
                    lin.estatus_ID = 1;
                }
                else
                {
                    lin.estatus_ID = 2;
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
                mensajefound = "Ocurrio un error al habilitar al empleado";
            }
            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ExportarExcel(int? creado)
        {
            bool success = false;
            string excelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            if (creado == 1)
            {
                // Aqui se debe cambiar por "Resguardos Mobiliario"
                var ruta = "~/Upload/Temporales/Descargas/" + "Empleados - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx";
                return File(Url.Content(ruta), excelContentType, "Empleados - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx");
            }
            // Aqui se debe cambiar por "Resguardos Mobiliario"
            if (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Empleados - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx")))
            {
                System.IO.File.Delete(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Empleados - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx"));
            }

            string savedFileName = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Empleados - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx"));
            //FileStream stream = System.IO.File.Create(savedFileName);
            using (FileStream stream = new FileStream(savedFileName, FileMode.Create))
            {
                try
                {
                    var productos = db.Database.SqlQuery<lista_cat_usuarios_excel>("Sp_Get_Empleados_excel").ToList();

                    using (var libro = new ExcelPackage())
                    {

                        var worksheet = libro.Workbook.Worksheets.Add("Empleados");
                        #region titulo para poner la razon social de la empresa
                        worksheet.Cells["C3:H3"].Merge = true;
                        worksheet.Cells["C3:H3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["C3:H3"].Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                        var cell = worksheet.Cells["C3"];
                        cell.IsRichText = true;     // Cell contains RichText rather than basic values


                        var title = cell.RichText.Add("SIRE - ");
                        title.Bold = true;
                        title.FontName = "Calibri";
                        title.Size = 15;
                        title.Color = ColorTranslator.FromHtml("#44546A");
                        #endregion
                        #region titulo para el reporte
                        worksheet.Cells["C4:H4"].Merge = true;
                        worksheet.Cells["C4:H4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["C4:Hs4"].Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                        //worksheet.Cells["D4:I4"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                        //worksheet.Cells["D4:I4"].Style.Border.Bottom.Color.SetColor(Color.Blue);

                        var cellrs = worksheet.Cells["C4"];
                        cellrs.IsRichText = true;     // Cell contains RichText rather than basic values
                                                      //cell.Style.WrapText = true; // Required to honor new lines

                        var titlers = cellrs.RichText.Add("Reporte Empleados");
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
                        excelImage.From.Column = 0;
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

                        var tabla = worksheet.Tables.Add(new ExcelAddressBase(fromRow: 6, fromCol: 3, toRow: productos.Count + 6, toColumn: 8), "Empleados");
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



        [HttpGet]
        public ActionResult GetSucursalByEmpresa(int Em_Cve_Empresa)
        {
            var sucursal = db.Sucursal
                .Where(x => x.Estatus == true && x.Em_Cve_Empresa == Em_Cve_Empresa)
                .Select(x => new { Value = x.Sc_Cve_Sucursal, Text = x.Sc_Descripcion })
                .ToList();

            return Json(sucursal, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDepartamentosByEmpresa(int Sc_Cve_Sucursal)
        {
            var departamento = db.Departamentos
                .Where(x => x.Estatus == true && x.Sc_Cve_Sucursal == Sc_Cve_Sucursal)
                .Select(x => new { Value = x.Dp_Cve_Departamento, Text = x.Dp_Descripcion })
                .ToList();

            return Json(departamento, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetpuestoByEmpresa(int Dp_Cve_Departamento)
        {
            var puesto = db.Puestos
                .Where(x => x.Estatus == true && x.Dp_Cve_Departamento == Dp_Cve_Departamento)
                .Select(x => new { Value = x.Pu_Cve_Puesto, Text = x.Pu_Descripcion })
                .ToList();

            return Json(puesto, JsonRequestBehavior.AllowGet);
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