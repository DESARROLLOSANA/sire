using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;
using CRME.Models;
using PagedList;
using System.Web.Script.Serialization;
using System.IO;
using System.Collections;
using CrystalDecisions.CrystalReports.Engine;
using CRME.Reportes;
using CrystalDecisions.Shared;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using OfficeOpenXml.Table;
using System.Data.SqlClient;

namespace CRME.Controllers
{
    public class HerramientasViewController : Controller
    {
        private SIRE_Context db = new SIRE_Context();
        //List<_ruta> ruta = new List<_ruta>();

        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "AccesoView");
            }

            ViewBag.HiddenMenu = 1;
            return View();

        }

        public ActionResult filtros() // esto se debe llamar igual q como llamaremos a la vista parcial q agregaremos
        {
            ViewBag.medida = new SelectList(db.Medida.ToList(), "Id_medida", "Descripcion");
            ViewBag.tiposolicitud = new SelectList(db.Condicion.ToList(), "Id_condicion", "Descripcion");
            ViewBag.UM = new SelectList(db.Unidad_Medida.ToList(), "Id_UM", "Descripcion");
            ViewBag.empresa = new SelectList(db.Empresa.Where(x => x.Estatus == true).ToList(), "Em_Cve_Empresa", "Em_Descripcion");
            ViewBag.sucursal = new SelectList(db.Sucursal.Where(x => x.Estatus == true).ToList(), "Sc_Cve_Sucursal", "Sc_Descripcion");
            ViewBag.departamento = new SelectList(db.Departamentos.Where(x=> x.Estatus == true).ToList(), "Dp_Cve_Departamento", "Dp_Descripcion");

            return PartialView();
        }

        public ActionResult SaveEdificios(Equipo_Menor edificio)
        {
            Auditoria auditoria = new Auditoria();
            var serializerCat = new JavaScriptSerializer();
            bool success = false;
            string mensajefound = "";
                      
            if (edificio.Id == 0)
            {
                var found = db.Equipo_Menor.FirstOrDefault(x => x.Descripcion == edificio.Descripcion);

                //if (found != null)
                //{
                //    mensajefound = "¡Ya existe una herramienta igual!";
                //}
                //else
                //{
                    try
                    {
                        Equipo_Menor Edificio = new Equipo_Menor();

                        //Usuarios
                        Edificio.Descripcion = edificio.Descripcion;
                        Edificio.Em_Cve_Empresa = edificio.Em_Cve_Empresa;
                        Edificio.Sc_Cve_Sucursal = edificio.Sc_Cve_Sucursal;
                        Edificio.Dp_Cve_Departamento = edificio.Dp_Cve_Departamento;
                        Edificio.Id_condicion = edificio.Id_condicion;
                        Edificio.Id_Familia = edificio.Id_Familia;
                        Edificio.Id_Sub_fam = edificio.Id_Sub_fam;
                        Edificio.Id_medida = edificio.Id_medida;
                        Edificio.EPI = edificio.EPI;
                        Edificio.Id_UM = edificio.Id_UM;
                        Edificio.Color = edificio.Color;
                        Edificio.marca = edificio.marca;
                        Edificio.modelo = edificio.modelo;
                        //Edificio.serie = edificio.serie;
                        Edificio.estatus_ID = 1;

                        //generacion de cod_inventario
                        var year = DateTime.Now;
                        string conver = Convert.ToString(year);
                        string[] ultimos = conver.Split('/', ' ', ':', '0');
                        char[] maspruba = conver.ToCharArray();

                        Equipo_Menor modificador = new Equipo_Menor();
                        Empresa etiqueta = db.Empresa.Find(edificio.Em_Cve_Empresa);
                        string temporal = etiqueta.Em_Descripcion.Replace(" ", "");

                        if (db.Equipo_Menor.FirstOrDefault() != null)
                        {
                            modificador = db.Equipo_Menor.OrderByDescending(x => x.Id).First();
                            Edificio.Cod_Inventario = temporal.ToUpper() + maspruba[8] + maspruba[9] + "-EM" + Convert.ToString(modificador.Id + 1);
                        }
                        else
                        {
                            Edificio.Cod_Inventario = temporal.ToUpper() + maspruba[8] + maspruba[9] + "-EM" + Convert.ToString(1);
                        }

                        if(edificio.serie == "")
                        {
                            Edificio.serie = "EM" + Convert.ToString(modificador.Id + 1);
                        }
                        else
                        {
                            Edificio.serie = edificio.serie;
                        }

                        //Empresa etiqueta = db.Empresa.Find(edificio.Em_Cve_Empresa);

                        //string temporal = etiqueta.Em_Descripcion.Replace(" ", "");
                        //Edificio.Cod_Inventario = temporal.ToUpper() + maspruba[8] + maspruba[9] + "-EM" + Convert.ToString(modificador.Id + 1);
                        //Edificio.Id_Estado = 1;

                        db.Equipo_Menor.Add(Edificio);

                        if (db.SaveChanges() > 0)
                        {
                            success = true;
                        }

                        auditoria.modulo = "Herramientas";
                        auditoria.idregistro = Edificio.Id;
                        auditoria.accion = "Registro";
                        auditoria.tabla = "Equipo_menor";
                        //auditoria.idusuario = Auth.Usuario.sistemas_ID;
                        auditoria.fecha = DateTime.Now;

                        db.Auditoria.Add(auditoria);

                        if (db.SaveChanges() > 0)
                        {
                            success = true;
                        }


                        //foreach (var item in ruta)
                        //{
                        //    Archivos evidencia = new Archivos();
                        //    evidencia.Ruta = item.Ruta;
                        //    evidencia.Id_Solicitud = Edificio.Id_Solicitud;

                        //    db.Archivos.Add(evidencia);

                        //    if (db.SaveChanges() > 0)
                        //    {
                        //        success = true;
                        //    }

                        //}

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

                //}
            }

            else
            {

                try
                {
                    Equipo_Menor Edificio = db.Equipo_Menor.Find(edificio.Id);
                    //Solicitud_Mtto Edificio = new Solicitud_Mtto();
                    // find
                    //Usuarios
                    Edificio.Em_Cve_Empresa = edificio.Em_Cve_Empresa;
                    Edificio.Descripcion = edificio.Descripcion;
                    Edificio.Sc_Cve_Sucursal = edificio.Sc_Cve_Sucursal;
                    Edificio.Dp_Cve_Departamento = edificio.Dp_Cve_Departamento;
                    Edificio.Id_condicion = edificio.Id_condicion;
                    Edificio.Id_Familia = edificio.Id_Familia;
                    Edificio.Id_Sub_fam = edificio.Id_Sub_fam;
                    Edificio.Id_medida = edificio.Id_medida;
                    Edificio.EPI = edificio.EPI;
                    Edificio.Id_UM = edificio.Id_UM;
                    Edificio.Color = edificio.Color;
                    Edificio.marca = edificio.marca;
                    Edificio.modelo = edificio.modelo;
                    Edificio.serie = edificio.serie;
                    //Edificio.Id_Estado = 2;

                    db.Entry(Edificio).State = EntityState.Modified;


                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                    }

                    auditoria.modulo = "Herramientas";
                    auditoria.idregistro = Edificio.Id;
                    auditoria.accion = "Modificacion";
                    auditoria.tabla = "equipo_menor";
                    // auditoria.idusuario = Auth.Usuario.sistemas_ID;
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
           return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult SaveEvidencia(string ruta, int? Id_Solicitud)
        {
            Auditoria auditoria = new Auditoria();
            var serializerCat = new JavaScriptSerializer();
            bool success = false;
            string mensajefound = "";
            try
            {
                Archivos archivo = new Archivos();

                //Usuarios
                archivo.Id_Solicitud = Id_Solicitud;
                var pathCat = serializerCat.Deserialize<string>(ruta);
                archivo.Ruta = "~/Upload/Sistema/evidencia/" + pathCat;

                db.Archivos.Add(archivo);

                if (db.SaveChanges() > 0)
                {
                    success = true;
                }

                auditoria.modulo = "Solicitud mtto edificios";
                auditoria.idregistro = archivo.Id_Archivo;
                auditoria.accion = "Registro";
                auditoria.tabla = "Archivos";
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

            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _Formulario(long? Id_Solicitud)
        {
            Equipo_Menor edificiossolicitud = new Equipo_Menor();

            if (Id_Solicitud != null)
            {
                ViewBag.edit = 1;

                edificiossolicitud = db.Equipo_Menor.Find(Id_Solicitud);  
                
                ViewBag.Familia = new SelectList(db.Familia.ToList(), "Id_Familia", "Descripcion", edificiossolicitud.Id_Familia);
                ViewBag.Subfamilia = new SelectList(db.Subfamilia.ToList(), "Id_Sub_fam", "Descripcion", edificiossolicitud.Id_Sub_fam);
                ViewBag.empresa = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion", edificiossolicitud.Em_Cve_Empresa);
                ViewBag.sucursal = new SelectList(db.Sucursal.ToList(), "Sc_Cve_Sucursal", "Sc_Descripcion", edificiossolicitud.Sc_Cve_Sucursal);
                ViewBag.departamento = new SelectList(db.Departamentos.ToList(), "Dp_Cve_Departamento", "Dp_Descripcion", edificiossolicitud.Dp_Cve_Departamento);
                ViewBag.medida = new SelectList(db.Medida.ToList(), "Id_medida", "Descripcion", edificiossolicitud.Id_medida);
                ViewBag.tiposolicitud = new SelectList(db.Condicion.ToList(), "Id_condicion", "Descripcion", edificiossolicitud.Id_condicion);
                ViewBag.UM = new SelectList(db.Unidad_Medida.ToList(), "Id_UM", "Descripcion", edificiossolicitud.Id_UM);

            }
            else
            {
                ViewBag.Familia = new SelectList(db.Familia.ToList(), "Id_Familia", "Descripcion");
                ViewBag.Subfamilia = new SelectList("", "Id_Sub_fam", "Descripcion");
                ViewBag.empresa = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion");
                ViewBag.sucursal = new SelectList("", "Sc_Cve_Sucursal", "Sc_Descripcion");
                ViewBag.departamento = new SelectList("", "Dp_Cve_Departamento", "Dp_Descripcion");
                ViewBag.medida = new SelectList(db.Medida.ToList(), "Id_medida", "Descripcion");
                ViewBag.tiposolicitud = new SelectList(db.Condicion.ToList(), "Id_condicion", "Descripcion");
                ViewBag.UM = new SelectList(db.Unidad_Medida.ToList(), "Id_UM", "Descripcion");
            }

            return PartialView(edificiossolicitud);
        }

        [HttpGet]
        public ActionResult GetSubfamiliasByFamilia(int Em_Cve_Empresa)
        {
            var Subfamilia = db.Subfamilia
                .Where(x =>  x.Id_Familia == Em_Cve_Empresa)
                .Select(x => new { Value = x.Id_Sub_fam, Text = x.Descripcion })
                .ToList();

            return Json(Subfamilia, JsonRequestBehavior.AllowGet);
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

        public ActionResult _TablaEdificios(int? page, string nom, int? Id_UM, int? Id_medida, int? Id_condicion
            , int? Em_Cve_Empresa, int? Sc_Cve_Sucursal, int? Dp_Cve_Departamento)
        {            
            const int pageSize = 30;
            int pageNumber = (page ?? 1);

            List<Equipo_Menor> conductores = new List<Equipo_Menor>();
            if (Id_UM == null && Id_medida == null && Id_condicion == null && nom == null
                && Em_Cve_Empresa == null && Sc_Cve_Sucursal == null && Dp_Cve_Departamento == null)
            {
                conductores = db.Equipo_Menor.ToList();
            }
            else if (Id_UM != null)
            {               
                conductores = db.Equipo_Menor.Where(x => x.Id_UM == Id_UM).ToList();
                ViewBag.Id_UM = Id_UM;
            }
            else if (Id_medida != null)
            {
                conductores = db.Equipo_Menor.Where(x => x.Id_medida == Id_medida).ToList();
                ViewBag.Id_medida = Id_medida;
            }
            else if (Id_condicion != null)
            {
                conductores = db.Equipo_Menor.Where(x => x.Id_condicion == Id_condicion).ToList();
                ViewBag.Id_condicion = Id_condicion;
            }
            else if (Em_Cve_Empresa != null)
            {
                conductores = db.Equipo_Menor.Where(x => x.Em_Cve_Empresa == Em_Cve_Empresa).ToList();
                ViewBag.Em_Cve_Empresa = Em_Cve_Empresa;
            }
            else if (Sc_Cve_Sucursal != null)
            {
                conductores = db.Equipo_Menor.Where(x => x.Sc_Cve_Sucursal == Sc_Cve_Sucursal).ToList();
                ViewBag.Sc_Cve_Sucursal = Sc_Cve_Sucursal;
            }
            else if (Dp_Cve_Departamento != null)
            {
                conductores = db.Equipo_Menor.Where(x => x.Dp_Cve_Departamento == Dp_Cve_Departamento).ToList();
                ViewBag.Dp_Cve_Departamento = Dp_Cve_Departamento;
            }
            else if (nom != null)
            {

                conductores = db.Equipo_Menor.Where(x => x.Descripcion.ToUpper().Contains(nom.ToUpper().Trim())).ToList();

                ViewBag.viewnombre = nom;
            }           
            return PartialView(conductores.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult DeleteEdificios(long? Id_Solicitud)
        {
            Auditoria auditoria = new Auditoria();
            bool success = false; ;
            string mensajefound = "";

            try
            {
                var Edificio = db.Equipo_Menor.Find(Id_Solicitud);
                //var Personas = db.Personas.Find(Usuario.idPersona);
                //var MediosContacto = db.MediosContactos.Find(Personas.MediosContactos);
                Edificio.estatus_ID = 4;

                db.Entry(Edificio).State = EntityState.Modified;
                //db.Entry(Personas).State = EntityState.Deleted;
                //db.Entry(MediosContacto).State = EntityState.Deleted;

                auditoria.modulo = "Solicitud_Mtto";
                auditoria.idregistro = Edificio.Id;
                auditoria.accion = "BAJA";
                auditoria.tabla = "Solicitud_Mtto";
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
                mensajefound = "Ocurrio un error al dar baja el registro";
            }


            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CerrarSolicitud(long? Id_Solicitud)
        {
            Auditoria auditoria = new Auditoria();
            bool success = false; ;
            string mensajefound = "";

            try
            {
                var Edificio = db.Solicitud_Mtto.Find(Id_Solicitud);
                //var Personas = db.Personas.Find(Usuario.idPersona);
                //var MediosContacto = db.MediosContactos.Find(Personas.MediosContactos);
                Edificio.Id_Estado = 5;

                db.Entry(Edificio).State = EntityState.Modified;
                //db.Entry(Personas).State = EntityState.Deleted;
                //db.Entry(MediosContacto).State = EntityState.Deleted;

                auditoria.modulo = "Solicitud_Mtto";
                auditoria.idregistro = Edificio.Id_Solicitud;
                auditoria.accion = "Cerrar";
                auditoria.tabla = "Solicitud_Mtto";
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
                mensajefound = "Ocurrio un error al dar baja el registro";
            }


            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CargarLogo(int? Id_Solicitud)
        {

            bool success = false;
            string error = "";
            //var savedFileNameDownload = "";
            string savedFileName = "";
            string completeName = "";
            Archivos foto = new Archivos();
            try
            {
                foreach (string file in Request.Files)
                {

                    HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                    string name = Path.GetRandomFileName();
                    string extension = Path.GetExtension(Path.GetFileName(hpf.FileName));
                    completeName = name + extension;
                    savedFileName = Path.Combine(Server.MapPath("~/Upload/Sistema/evidencia/"), completeName);
                    foto.Ruta = savedFileName;
                    //ruta.File(savedFileName);
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

        public ActionResult EliminarLogo(string path)
        {
            bool success = false;
            try
            {
                var serializer = new JavaScriptSerializer();
                var pathh = serializer.Deserialize<string>(path);
                var rutapath = "~/Upload/Sistema/evidencia/" + pathh;
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
        }

        public ActionResult ExportarExcel(int? creado, string nom, int? Id_UM, int? Id_medida, int? Id_condicion
            , int? Em_Cve_Empresa, int? Sc_Cve_Sucursal, int? Dp_Cve_Departamento)
        {
            bool success = false;
            string excelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            if (creado == 1)
            {
                // Aqui se debe cambiar por "Resguardos Mobiliario"
                var ruta = "~/Upload/Temporales/Descargas/" + "Equipo menor - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx";
                return File(Url.Content(ruta), excelContentType, "Equipo menor - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx");
            }
            // Aqui se debe cambiar por "Resguardos Mobiliario"
            if (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Equipo menor - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx")))
            {
                System.IO.File.Delete(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Equipo menor - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx"));
            }

            string savedFileName = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Equipo menor - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx"));
            //FileStream stream = System.IO.File.Create(savedFileName);
            using (FileStream stream = new FileStream(savedFileName, FileMode.Create))
            {
                try
                {
                   
                    List<listaExcelEM> conductores = new List<listaExcelEM>();
                    if (Id_UM == null && Id_medida == null && nom == "" && Id_condicion == null
                        && Em_Cve_Empresa == null && Sc_Cve_Sucursal == null && Dp_Cve_Departamento == null)
                    {
                        //var productos = db.Database.SqlQuery<Inventario_Lista_Laptops_Excel>("Sp_Get_Inventario_Laptops_excel").ToList();
                        conductores = db.Database.SqlQuery<listaExcelEM>("sp_get_inventario_excel_todos").ToList();
                        // productos = conductores;
                    }
                    else if (Id_UM != null)
                    {//Busqueda por fecha

                        conductores = db.Database.SqlQuery<listaExcelEM>("sp_get_inventario_excel_unidad @unidad",
                            new SqlParameter("@unidad", Id_UM)).ToList();
                        //conductores = SP_Reporte.Instancia.ReporteFechaExcel(Convert.ToDateTime(fi), Convert.ToDateTime(ff));
                    }
                    else if (nom != null && nom != "")
                    {//busqueda´por nombre
                        conductores = db.Database.SqlQuery<listaExcelEM>("sp_get_inventario_excel_descripcion @descrip",
                            new SqlParameter("@descrip","%" + nom + "%")).ToList();

                        //conductores = SP_Reporte.Instancia.ReporteNombreExcel(nom);
                    }
                    else if (Id_medida != null)
                    {//busqueda por codigo
                       
                        conductores = db.Database.SqlQuery<listaExcelEM>("sp_get_inventario_excel_medida @medida",
                            new SqlParameter("@medida", Id_medida)).ToList();
                        //conductores = SP_Reporte.Instancia.ReportCodigoExcel(cod);
                    }
                    else if (Id_condicion != null)
                    {//busqueda por codigo
                       
                        conductores = db.Database.SqlQuery<listaExcelEM>("sp_get_inventario_excel_condicion @condicion",
                            new SqlParameter("@condicion", Id_condicion)).ToList();
                        //conductores = SP_Reporte.Instancia.ReportCodigoExcel(cod);
                    }
                    else if (Em_Cve_Empresa != null)
                    {//busqueda por codigo

                        conductores = db.Database.SqlQuery<listaExcelEM>("sp_get_inventario_excel_empresa @Em_Cve_Empresa",
                            new SqlParameter("@Em_Cve_Empresa", Em_Cve_Empresa)).ToList();
                        //conductores = SP_Reporte.Instancia.ReportCodigoExcel(cod);
                    }
                    else if (Sc_Cve_Sucursal != null)
                    {//busqueda por codigo

                        conductores = db.Database.SqlQuery<listaExcelEM>("sp_get_inventario_excel_sucursal @Sc_Cve_Sucursal",
                            new SqlParameter("@Sc_Cve_Sucursal", Sc_Cve_Sucursal)).ToList();
                        //conductores = SP_Reporte.Instancia.ReportCodigoExcel(cod);
                    }
                    else if (Dp_Cve_Departamento != null)
                    {//busqueda por codigo

                        conductores = db.Database.SqlQuery<listaExcelEM>("sp_get_inventario_excel_departamento @Dp_Cve_Departamento",
                            new SqlParameter("@Dp_Cve_Departamento", Dp_Cve_Departamento)).ToList();
                        //conductores = SP_Reporte.Instancia.ReportCodigoExcel(cod);
                    }
                    using (var libro = new ExcelPackage())
                    {

                        var worksheet = libro.Workbook.Worksheets.Add("Inventario equipo menor");
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
                        title.Color = ColorTranslator.FromHtml("#2196f3");
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

                        var titlers = cellrs.RichText.Add("Inventario de equipo menor");
                        titlers.Bold = true;
                        titlers.FontName = "Calibri";
                        titlers.Size = 15;
                        titlers.Color = ColorTranslator.FromHtml("#2196f3");
                        #endregion
                        #region llenado de la informacion
                        worksheet.Cells["C6"].LoadFromCollection(conductores, PrintHeaders: true);
                        for (var col = 1; col < conductores.Count + 1; col++)
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


                        var tabla = worksheet.Tables.Add(new ExcelAddressBase(fromRow: 6, fromCol: 3, toRow: conductores.Count + 6, toColumn: 11), "inventario");
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
    }
}