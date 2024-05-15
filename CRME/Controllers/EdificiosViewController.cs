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

namespace CRME.Controllers
{
    public class Global
    {
        public static string ruta;
        public static string Ruta { get; set; }

    }

    public class EdificiosViewController : Controller
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

        public ActionResult SaveEdificios(Solicitud_Mtto edificio)
        {
            Auditoria auditoria = new Auditoria();
            var serializerCat = new JavaScriptSerializer();
            bool success = false;
            string mensajefound = "";
            var found = db.Solicitud_Mtto.FirstOrDefault(x => x.Sm_Descripcion == edificio.Sm_Descripcion && x.Id_Solicitud != edificio.Id_Solicitud);

            if (found != null)
            {
                mensajefound = "¡Ya existe una solicitud igual!";
            }           
            else
            {
                if (edificio.Sm_Descripcion == "")
                {
                    mensajefound = "Debe capturar una descripcion.";

                }
                else
                {
                    if (edificio.Id_Solicitud == 0)
                    {
                        try
                        {
                            Solicitud_Mtto Edificio = new Solicitud_Mtto();

                            //Usuarios
                            Edificio.Em_Cve_Empresa = edificio.Em_Cve_Empresa;
                            Edificio.Sc_Cve_Sucursal = edificio.Sc_Cve_Sucursal;
                            Edificio.Id_Tipo_Sol = edificio.Id_Tipo_Sol;
                            Edificio.Sm_Descripcion = edificio.Sm_Descripcion;
                            Edificio.Id_Estado = 1;

                            db.Solicitud_Mtto.Add(Edificio);

                            if (db.SaveChanges() > 0)
                            {
                                success = true;
                            }

                            auditoria.modulo = "Solicitud mtto edificios";
                            auditoria.idregistro = Edificio.Id_Solicitud;
                            auditoria.accion = "Registro";
                            auditoria.tabla = "solicitud_Mtto";
                            auditoria.idusuario = Auth.Usuario.sistemas_ID;
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

                    }

                    else
                    {

                        try
                        {
                            Solicitud_Mtto Edificio = db.Solicitud_Mtto.Find(edificio.Id_Solicitud);
                            //Solicitud_Mtto Edificio = new Solicitud_Mtto();
                            // find
                            //Usuarios
                            Edificio.Em_Cve_Empresa = edificio.Em_Cve_Empresa;
                            Edificio.Sc_Cve_Sucursal = edificio.Sc_Cve_Sucursal;
                            Edificio.Id_Tipo_Sol = edificio.Id_Tipo_Sol;
                            Edificio.Sm_Descripcion = edificio.Sm_Descripcion;
                            //Edificio.Id_Estado = 2;

                            db.Entry(Edificio).State = EntityState.Modified;


                            if (db.SaveChanges() > 0)
                            {
                                success = true;
                            }

                            auditoria.modulo = "Solicitud mtto edificios";
                            auditoria.idregistro = Edificio.Id_Solicitud;
                            auditoria.accion = "Modificacion";
                            auditoria.tabla = "solicitud_Mtto";
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

            }

            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Print(int? id)
        {
            //Este metodo se usa para crear el pdf  
            var reporte = new ReportClass();
            string valor1 = "";
            string valor2 = "";
            string valor3 = "";

            reporte.FileName = Server.MapPath("~/Reportes/MttoEporadico.rpt");
            reporte.Load();

            List<Archivos> archivo = new List<Archivos>();

            archivo = (from ar in db.Archivos                         
                         where (ar.Id_Solicitud == id)
                         select ar).OrderBy(x => x.Id_Archivo).ToList();

            foreach(var i in archivo)
            {
                if(valor1 == "")
                {
                    valor1 = Server.MapPath(i.Ruta);
                }
                else
                {
                    if (valor2 == "")
                    {
                        valor2 = Server.MapPath(i.Ruta);
                    }
                    else
                    {                        
                        valor3 = Server.MapPath(i.Ruta);                        
                    }
                }                               
            }

            //reporte.SetParameterValue("@Id_mobiliario", id);
            reporte.SetParameterValue("@Id_Solcitud", id);
            //var valor = Server.MapPath("~/Upload/Sistema/evidencia/1bm5l0nv.hvt.png");
            reporte.SetParameterValue("imagen1", valor1);
            reporte.SetParameterValue("imagen2", valor2);
            reporte.SetParameterValue("imagen3", valor3);

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

        public ActionResult SaveEvidencia(string ruta,int? Id_Solicitud)
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
            Solicitud_Mtto edificiossolicitud = new Solicitud_Mtto();

            if (Id_Solicitud != null)
            {
                ViewBag.edit = 1;
                edificiossolicitud = db.Solicitud_Mtto.Find(Id_Solicitud);

                if( Auth.Usuario.perfil_ID != 3 || Auth.Usuario.perfil_ID != 5)
                {
                    ViewBag.tiposolicitud = new SelectList(db.Tipo_Solicitud.ToList(), "Id_Tipo_Sol", "Ts_Descripcion", edificiossolicitud.Id_Tipo_Sol);
                }
                else
                {
                    ViewBag.tiposolicitud = new SelectList(db.Tipo_Solicitud.Where(x=> x.Id_Tipo_Sol == 2).ToList(), "Id_Tipo_Sol", "Ts_Descripcion", edificiossolicitud.Id_Tipo_Sol);
                }
                

                ViewBag.empresa = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion", edificiossolicitud.Em_Cve_Empresa);

                ViewBag.sucursal = new SelectList(db.Sucursal.ToList(), "Sc_Cve_Sucursal", "Sc_Descripcion", edificiossolicitud.Sc_Cve_Sucursal);

            }
            else
            {

                if (Auth.Usuario.perfil_ID != 3 )
                {
                    ViewBag.tiposolicitud = new SelectList(db.Tipo_Solicitud.ToList(), "Id_Tipo_Sol", "Ts_Descripcion");
                }
                else
                {
                    ViewBag.tiposolicitud = new SelectList(db.Tipo_Solicitud.Where(x => x.Id_Tipo_Sol == 2).ToList(), "Id_Tipo_Sol", "Ts_Descripcion");
                }
                //ViewBag.tiposolicitud = new SelectList(db.Tipo_Solicitud.ToList(), "Id_Tipo_Sol", "Ts_Descripcion");
                ViewBag.empresa = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion");
                ViewBag.sucursal = new SelectList(db.Sucursal.ToList(), "Sc_Cve_Sucursal", "Sc_Descripcion");
            }

            return PartialView(edificiossolicitud);
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

        public ActionResult _TablaEdificios(int? page, string filtroBusqueda)
        {
            const int pageSize = 10;
            int pageNumber = (page ?? 1);

            //obtener lista de usuarios
            List<Solicitud_Mtto> Edificios = new List<Solicitud_Mtto>();
            int[] supplierIDs = { 1, 2,3 ,4};
            Edificios = (from sm in db.Solicitud_Mtto
                         join a in db.Auditoria on sm.Id_Solicitud equals a.idregistro
                         where (a.tabla == "Solicitud_Mtto" && a.accion == "Registro" && a.idusuario == Auth.Usuario.sistemas_ID && supplierIDs.Contains((int)sm.Id_Estado) )
                         select sm).OrderBy(x => x.Id_Solicitud).ToList();
                         
            if (!string.IsNullOrEmpty(filtroBusqueda))
            {
                Edificios = Edificios.Where(x => x.Sm_Descripcion.ToUpper().Contains(filtroBusqueda.ToUpper().Trim())).ToList();
            }
            ViewBag.filtro = filtroBusqueda;

            return PartialView(Edificios.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult DeleteEdificios(long? Id_Solicitud)
        {
            Auditoria auditoria = new Auditoria();
            bool success = false; ;
            string mensajefound = "";

            try
            {
                var Edificio = db.Solicitud_Mtto.Find(Id_Solicitud);
                //var Personas = db.Personas.Find(Usuario.idPersona);
                //var MediosContacto = db.MediosContactos.Find(Personas.MediosContactos);
                Edificio.Id_Estado = 2;

                db.Entry(Edificio).State = EntityState.Modified;
                //db.Entry(Personas).State = EntityState.Deleted;
                //db.Entry(MediosContacto).State = EntityState.Deleted;

                auditoria.modulo = "Solicitud_Mtto";
                auditoria.idregistro = Edificio.Id_Solicitud;
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

    }

    
}

                         

