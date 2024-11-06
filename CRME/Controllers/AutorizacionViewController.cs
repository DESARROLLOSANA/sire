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
    public class AutorizacionViewController : Controller
    {
        // GET: PuestosView
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

        #region saves

        public ActionResult SaveCalendario(Calendario calendario, int Id_Solicitud)
        {
            bool success = false;
            string mensajefound = "";
            try
            {
                /*codigo*/
                var found = db.Calendario.FirstOrDefault(x => x.Descripcion == calendario.Descripcion);
                if (found != null)
                {
                    mensajefound = "Ya tienes registrada una actividad con esa descripcion!!";

                }
                else
                {
                    Calendario Calendario = new Calendario();
                    if (calendario.Id_Calendario == 0)
                    {
                        Calendario.Id_Solicitud = Id_Solicitud;
                        Calendario.Descripcion = calendario.Descripcion;
                        Calendario.inicio = calendario.inicio;
                        Calendario.Fin = calendario.Fin;
                        Calendario.Id_Estado = 1;

                        db.Calendario.Add(Calendario);
                    }           
                }

                if (db.SaveChanges() > 0)
                {
                    success = true;
                }
            }
            catch (Exception ex)
            {
                ViewBag.ResultMessage = "Error occured, records rolledback." + ex;
            }

            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveRequerimiento(Requerimiento reque, string ruta, int Id_Solicitud)
        {
            bool success = false;
            string mensajefound = "";
            try
            {
                /*codigo*/
                var found = db.Requerimiento.FirstOrDefault(x => x.Descripcion == reque.Descripcion);
                if (found != null)
                {
                    mensajefound = "Ya tienes registrada una actividad con esa descripcion!!";
                }
                else
                {
                    Requerimiento Requerimiento = new Requerimiento();
                   
                    if (reque.Id_Requerimiento == 0)
                    {
                        Requerimiento.Id_Solicitud = Id_Solicitud;
                        Requerimiento.Descripcion = reque.Descripcion;
                        Requerimiento.Id_Proveedor = reque.Id_Proveedor;
                        Requerimiento.Id_Estado = 1;

                        db.Requerimiento.Add(Requerimiento);

                        if (db.SaveChanges() > 0)
                        {
                            success = true;
                        }
                    }                   
                    Archivos archivo = new Archivos();
                    archivo.Id_Requerimiento = Requerimiento.Id_Requerimiento;
                    archivo.Ruta = ruta;
                    db.Archivos.Add(archivo);

                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                    }
                }                              
            }
            catch (Exception ex)
            {
                ViewBag.ResultMessage = "Error occured, records rolledback." + ex;
            }

            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveRequerimientoCal(Requerimiento reque, string ruta, int Id_Calendario)
        {
            bool success = false;
            string mensajefound = "";
            try
            {             
                Requerimiento Requerimiento = new Requerimiento();

                if (reque.Id_Requerimiento == 0)
                {
                    Requerimiento.Id_Calendario = Id_Calendario;
                    Requerimiento.Descripcion = reque.Descripcion;
                    Requerimiento.Id_Proveedor = reque.Id_Proveedor;
                    Requerimiento.Id_Estado = 1;

                    db.Requerimiento.Add(Requerimiento);

                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                    }
                }
                Archivos archivo = new Archivos();
                archivo.Id_Requerimiento = Requerimiento.Id_Requerimiento;
                archivo.Ruta = ruta;
                db.Archivos.Add(archivo);

                if (db.SaveChanges() > 0)
                {
                    success = true;
                }               
            }
            catch (Exception ex)
            {
                ViewBag.ResultMessage = "Error occured, records rolledback." + ex;
            }

            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region formularios

        public ActionResult _FormularioCalendario(long? Id_Calendario)
        {
            Calendario calendario = new Calendario();

            if (Id_Calendario != null)
            {
                ViewBag.edit = 1;
                calendario = db.Calendario.Find(Id_Calendario);               
            }            

            return PartialView("_FormularioCalendario", calendario);
        }

        public ActionResult _FormularioCalendarioRequerimiento(long? Id_requerimiento)
        {
            Requerimiento requerimiento = new Requerimiento();

            if (Id_requerimiento != null)
            {
                ViewBag.edit = 1;
                requerimiento = db.Requerimiento.Find(Id_requerimiento);
            }
            else
            {
                ViewBag.Proveedor = new SelectList(db.cat_proveedores.ToList(), "Proveedor_Id", "proveedor");
            }

            return PartialView("_FormularioCalendarioRequerimiento", requerimiento);           
        }

        public ActionResult _FormularioRequerimiento(long? Id_requerimiento)
        {
            Requerimiento requerimiento = new Requerimiento();

            if (Id_requerimiento != null)
            {
                ViewBag.edit = 1;
                requerimiento = db.Requerimiento.Find(Id_requerimiento);                
            }
            else
            {
                ViewBag.Proveedor = new SelectList(db.cat_proveedores.ToList(), "Proveedor_Id", "proveedor");
            }
           
            return PartialView("_FormularioRequerimiento", requerimiento);
        }

        #endregion

        #region tablas
        public ActionResult _TablaCalendario(int? page,  long? Id_Solicitud)
        {
            const int pageSize = 40;
            int pageNumber = (page ?? 1);
            
            List<Calendario> Calendario = new List<Calendario>();
            Calendario = db.Calendario.Where(x => x.Id_Estado == 1 && x.Id_Solicitud == Id_Solicitud).OrderBy(x => x.Id_Calendario).ToList();

            return PartialView(Calendario.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult _TablaEdificiosAuth(int? page, string filtroBusqueda)
        {
            const int pageSize = 10;
            int pageNumber = (page ?? 1);

            //obtener lista de usuarios
            List<Solicitud_Mtto> Edificios = new List<Solicitud_Mtto>();

            Edificios = db.Solicitud_Mtto.Where(x => x.Id_Estado == 1 || x.Id_Estado == 3 || x.Id_Estado == 4).OrderBy(x => x.Id_Solicitud).ToList();
            if (!string.IsNullOrEmpty(filtroBusqueda))
            {
                Edificios = Edificios.Where(x => x.Sm_Descripcion.ToUpper().Contains(filtroBusqueda.ToUpper().Trim())).ToList();
            }
            ViewBag.tecnico = new SelectList(db.cat_usuarios.Where(x=> x.estatus_ID == 1 && x.Pu_Cve_Puesto == 6).ToList(), "usuario_ID", "nombre_completo");
            ViewBag.filtro = filtroBusqueda;

            return PartialView(Edificios.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult _TablaRequerimientoCalendario(int? page, long? Id_Calendario)
        {
            const int pageSize = 40;
            int pageNumber = (page ?? 1);

            List<Requerimiento> Requerimeinto = new List<Requerimiento>();
            int[] supplierIDs = { 1, 3 };
            Requerimeinto = db.Requerimiento.Where(x => x.Id_Calendario == Id_Calendario && supplierIDs.Contains((int)x.Id_Estado)).OrderBy(x => x.Id_Requerimiento).ToList();

            return PartialView(Requerimeinto.ToPagedList(pageNumber, pageSize));
        } // pendiente

        public ActionResult _TablaRequerimiento(int? page, long? Id_Solicitud)
        {
            const int pageSize = 40;
            int pageNumber = (page ?? 1);
            
            List<Requerimiento> Requerimeinto = new List<Requerimiento>();
            int[] supplierIDs = { 1, 3 };
            Requerimeinto = db.Requerimiento.Where(x=> x.Id_Solicitud == Id_Solicitud && supplierIDs.Contains((int)x.Id_Estado)).OrderBy(x => x.Id_Requerimiento).ToList();

            return PartialView(Requerimeinto.ToPagedList(pageNumber, pageSize));
        }

        #endregion

        #region visualizaciones

        public ActionResult VisualizarEvidencia(long? Id_Solicitud)
        {
            List<Archivos> archivo = new List<Archivos>();
            archivo = db.Archivos.Where(x => x.Id_Solicitud == Id_Solicitud).ToList();
            return PartialView(archivo);
        }

        public ActionResult VisualizarRequerimientos(long? Id_Solicitud)
        {
            ViewBag.Id_Solicitud = Id_Solicitud;
            return View();
        }

        public ActionResult VisualizarCalendario(long? Id_Solicitud)
        {

            ViewBag.Id_Solicitud = Id_Solicitud;
            return View();
        }

        public ActionResult VisualizarRequerimientoCalendario(long? Id_Calendario)
        {

            ViewBag.Id_Calendario = Id_Calendario;
            return View();
        }

        #endregion

        #region autorizaciones y cancelaciones

        public ActionResult Autorizar(long? Id_Solicitud)
        {
            Auditoria auditoria = new Auditoria();
            bool success = false; ;
            string mensajefound = "";

            try
            {
                var Edificio = db.Solicitud_Mtto.Find(Id_Solicitud);                
                Edificio.Id_Estado = 3;

                db.Entry(Edificio).State = EntityState.Modified;                

                auditoria.modulo = "Autorizacion";
                auditoria.idregistro = Edificio.Id_Solicitud;
                auditoria.accion = "Autorizar";
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

        public ActionResult Cancelar(long? Id_Solicitud, string motivo)
        {
            Auditoria auditoria = new Auditoria();
            bool success = false; ;
            string mensajefound = "";

            try
            {
                var Edificio = db.Solicitud_Mtto.Find(Id_Solicitud);                
                Edificio.Motivo = motivo; 
                Edificio.Id_Estado = 2;

                db.Entry(Edificio).State = EntityState.Modified;               

                auditoria.modulo = "Autorizacion";
                auditoria.idregistro = Edificio.Id_Solicitud;
                auditoria.accion = "Cancelar";
                auditoria.tabla = "Solicitud_Mtto";
                auditoria.idusuario = Auth.Usuario.sistemas_ID;
                auditoria.fecha = DateTime.Now;

                db.Auditoria.Add(auditoria);

                

                if (db.SaveChanges() > 0)
                {
                    success = true;
                }

                //SendMailCancelacion(motivo, Id_Solicitud);

            }
            catch (Exception ex)
            {
                mensajefound = "Ocurrio un error al dar baja el registro";
            }


            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Realizado(long? Id_Solicitud)
        {
            Auditoria auditoria = new Auditoria();
            bool success = false; ;
            string mensajefound = "";

            try
            {
                var Edificio = db.Solicitud_Mtto.Find(Id_Solicitud);          
                Edificio.Id_Estado = 4;

                db.Entry(Edificio).State = EntityState.Modified;

                auditoria.modulo = "Autorizacion";
                auditoria.idregistro = Edificio.Id_Solicitud;
                auditoria.accion = "Realizado";
                auditoria.tabla = "Solicitud_Mtto";
                auditoria.idusuario = Auth.Usuario.sistemas_ID;
                auditoria.fecha = DateTime.Now;

                db.Auditoria.Add(auditoria);



                if (db.SaveChanges() > 0)
                {
                    success = true;
                }

                //SendMailCancelacion(motivo, Id_Solicitud);

            }
            catch (Exception ex)
            {
                mensajefound = "Ocurrio un error al dar baja el registro";
            }


            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutorizarCal(long? Id_Solicitud)
        {
            Auditoria auditoria = new Auditoria();
            bool success = false; ;
            string mensajefound = "";

            try
            {
                var Edificio = db.Calendario.Find(Id_Solicitud);
                Edificio.Id_Estado = 3;

                db.Entry(Edificio).State = EntityState.Modified;

                auditoria.modulo = "Autorizacion";
                auditoria.idregistro = Edificio.Id_Calendario;
                auditoria.accion = "Autorizar";
                auditoria.tabla = "Calendario";
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

        public ActionResult CancelarCal(long? Id_Solicitud, string motivo)
        {
            Auditoria auditoria = new Auditoria();
            bool success = false; ;
            string mensajefound = "";

            try
            {
                var Edificio = db.Calendario.Find(Id_Solicitud);
               // Edificio.Motivo = motivo;
                Edificio.Id_Estado = 2;

                db.Entry(Edificio).State = EntityState.Modified;

                auditoria.modulo = "Autorizacion";
                auditoria.idregistro = Edificio.Id_Solicitud;
                auditoria.accion = "Cancelar";
                auditoria.tabla = "Solicitud_Mtto";
                auditoria.idusuario = Auth.Usuario.sistemas_ID;
                auditoria.fecha = DateTime.Now;

                db.Auditoria.Add(auditoria);



                if (db.SaveChanges() > 0)
                {
                    success = true;
                }

                //SendMailCancelacion(motivo, Id_Solicitud);

            }
            catch (Exception ex)
            {
                mensajefound = "Ocurrio un error al dar baja el registro";
            }


            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutorizarReq(long? Id_Solicitud)
        {
            Auditoria auditoria = new Auditoria();
            bool success = false; ;
            string mensajefound = "";

            try
            {
                var Edificio = db.Requerimiento.Find(Id_Solicitud);
                Edificio.Id_Estado = 3;

                db.Entry(Edificio).State = EntityState.Modified;

                auditoria.modulo = "Autorizacion";
                auditoria.idregistro = Edificio.Id_Requerimiento;
                auditoria.accion = "Autorizar";
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

        public ActionResult CancelarReq(long? Id_Solicitud)
        {
            Auditoria auditoria = new Auditoria();
            bool success = false; ;
            string mensajefound = "";

            try
            {
                var Edificio = db.Requerimiento.Find(Id_Solicitud);
                //Edificio.Motivo = motivo;
                Edificio.Id_Estado = 2;

                db.Entry(Edificio).State = EntityState.Modified;

                auditoria.modulo = "Autorizacion";
                auditoria.idregistro = Edificio.Id_Requerimiento;
                auditoria.accion = "Cancelar";
                auditoria.tabla = "Solicitud_Mtto";
                auditoria.idusuario = Auth.Usuario.sistemas_ID;
                auditoria.fecha = DateTime.Now;

                db.Auditoria.Add(auditoria);



                if (db.SaveChanges() > 0)
                {
                    success = true;
                }

                //SendMailCancelacion(motivo, Id_Solicitud);

            }
            catch (Exception ex)
            {
                mensajefound = "Ocurrio un error al dar baja el registro";
            }


            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public ActionResult SaveEvidencia(string ruta, string ruta1, string ruta2, int? Id_Solicitud,string resultadotexto)
        {
            Auditoria auditoria = new Auditoria();
            var serializerCat = new JavaScriptSerializer();
            bool success = false;
            string mensajefound = "";
            //var found = db.Archivos.FirstOrDefault(x => x.Sm_Descripcion == edificio.Sm_Descripcion && x.Id_Solicitud != edificio.Id_Solicitud);

            //if (found != null)
            //{
            //    mensajefound = "¡Ya existe una solicitud igual!";

            //}
            //else
            //{
            //if (Archico.Id_Archivo == 0)
            //{
            try
            {
                Archivos archivo = new Archivos();
                Archivos archivo1 = new Archivos();
                Archivos archivo2 = new Archivos();
                List<Archivos> archolist = new List<Archivos>();

                //Usuarios
                if(ruta != "")
                {
                    archivo.Id_Solicitud = Id_Solicitud;
                    var pathCat = serializerCat.Deserialize<string>(ruta);
                    archivo.Ruta = "~/Upload/Sistema/evidencia/" + pathCat;
                    //archivo.Id_Solicitud = edificio.Id_Tipo_Sol;
                    //archivo.Sm_Descripcion = edificio.Sm_Descripcion;
                    //archivo.Id_Estado = 1;
                    
                    db.Archivos.Add(archivo);
                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                    }
                    archolist.Add(archivo);
                }
                if (ruta1 != "")
                {
                    archivo1.Id_Solicitud = Id_Solicitud;
                    var pathCat1 = serializerCat.Deserialize<string>(ruta1);
                    archivo1.Ruta = "~/Upload/Sistema/evidencia/" + pathCat1;
                    //archivo.Id_Solicitud = edificio.Id_Tipo_Sol;
                    //archivo.Sm_Descripcion = edificio.Sm_Descripcion;
                    //archivo.Id_Estado = 1;
                    db.Archivos.Add(archivo1);
                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                    }
                    archolist.Add(archivo1);
                }
                if (ruta2 != "")
                {
                    archivo2.Id_Solicitud = Id_Solicitud;
                    var pathCat2 = serializerCat.Deserialize<string>(ruta2);
                    archivo2.Ruta = "~/Upload/Sistema/evidencia/" + pathCat2;
                    //archivo.Id_Solicitud = edificio.Id_Tipo_Sol;
                    //archivo.Sm_Descripcion = edificio.Sm_Descripcion;
                    //archivo.Id_Estado = 1;
                    db.Archivos.Add(archivo2);
                    if (db.SaveChanges() > 0)
                    {
                        success = true;
                    }
                    archolist.Add(archivo2);
                }
                                               

                

                if(archolist.Count > 1)
                {
                    foreach(var arc in archolist)
                    {
                        auditoria.modulo = "Resultado";
                        auditoria.idregistro = arc.Id_Archivo;
                        auditoria.accion = "Registro";
                        auditoria.tabla = "Archivos";
                        auditoria.idusuario = Auth.Usuario.sistemas_ID;
                        auditoria.fecha = DateTime.Now;
                        db.Auditoria.Add(auditoria);
                    }
                }
                else
                {
                    auditoria.modulo = "Resultado";
                    auditoria.idregistro = archivo.Id_Archivo;
                    auditoria.accion = "Registro";
                    auditoria.tabla = "Archivos";
                    auditoria.idusuario = Auth.Usuario.sistemas_ID;
                    auditoria.fecha = DateTime.Now;
                    db.Auditoria.Add(auditoria);
                }               
               
                if (db.SaveChanges() > 0)
                {
                    success = true;
                }

                Solicitud_Mtto Edificio = db.Solicitud_Mtto.Find(archivo.Id_Solicitud);
                //Solicitud_Mtto Edificio = new Solicitud_Mtto();
                // find
                //Usuarios
                Edificio.Resultado = resultadotexto;
                //Edificio.Id_Estado = 2;

                db.Entry(Edificio).State = EntityState.Modified;


                if (db.SaveChanges() > 0)
                {
                    success = true;
                }

                Realizado(Id_Solicitud);
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

            // }

            //else
            //{

            //    try
            //    {
            //        Solicitud_Mtto Edificio = db.Solicitud_Mtto.Find(edificio.Id_Solicitud);
            //        //Solicitud_Mtto Edificio = new Solicitud_Mtto();
            //        // find
            //        //Usuarios
            //        Edificio.Em_Cve_Empresa = edificio.Em_Cve_Empresa;
            //        Edificio.Sc_Cve_Sucursal = edificio.Sc_Cve_Sucursal;
            //        Edificio.Id_Tipo_Sol = edificio.Id_Tipo_Sol;
            //        Edificio.Sm_Descripcion = edificio.Sm_Descripcion;
            //        //Edificio.Id_Estado = 2;

            //        db.Entry(Edificio).State = EntityState.Modified;


            //        if (db.SaveChanges() > 0)
            //        {
            //            success = true;
            //        }

            //        auditoria.modulo = "Solicitud mtto edificios";
            //        auditoria.idregistro = Edificio.Id_Solicitud;
            //        auditoria.accion = "Modificacion";
            //        auditoria.tabla = "solicitud_Mtto";
            //        auditoria.idusuario = Auth.Usuario.sistemas_ID;
            //        auditoria.fecha = DateTime.Now;

            //        db.Auditoria.Add(auditoria);

            //        if (db.SaveChanges() > 0)
            //        {
            //            success = true;
            //        }
            //    }
            //    catch (DbEntityValidationException ex)
            //    {
            //        var errorMessages = ex.EntityValidationErrors
            //       .SelectMany(x => x.ValidationErrors)
            //       .Select(x => x.ErrorMessage);

            //        // Join the list to a single string.
            //        var fullErrorMessage = string.Join("; ", errorMessages);

            //        // Combine the original exception message with the new one.
            //        var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

            //        // Throw a new DbEntityValidationException with the improved exception message.
            //        //throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            //        mensajefound = exceptionMessage + "fatal error";
            //    }
            //}


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
        public ActionResult CargarLogo1(int? Id_Solicitud)
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
        public ActionResult CargarLogo2(int? Id_Solicitud)
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
        public async Task<ActionResult> Cargarfile()
        {
            bool success = false;
            string mensaje = "";
            string FileT = "";
            JsonResult Resp = await Uploadfile();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            ResponseObjectVM Respuestas = ser.Deserialize<ResponseObjectVM>(ser.Serialize(Resp.Data));
            success = Respuestas.success;
            mensaje = Respuestas.mensaje;
            FileT = Respuestas.Filet;
            ViewBag.rutatarjeta = Respuestas.Filet;
            return Json(new { success = success, mensaje, FileT });
        }
        public async Task<JsonResult> Uploadfile()
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
                string nombreArchivo = "Archivo" + name;                
                FileStream stream = null;

                try
                {
                    foreach (string file in Request.Files)
                    {
                        if (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Sistema/files/" + nombreArchivo + ".pdf")))
                        {
                            System.IO.File.Delete(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Sistema/files/" + nombreArchivo + ".pdf"));
                        }

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


        public ActionResult ExportarExcel(int? creado, string filtro)
        {
            bool success = false;
            string excelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            if (creado == 1)
            {
                // Aqui se debe cambiar por "Resguardos Laptops"
                var ruta = "~/Upload/Temporales/Descargas/" + "Solicitudes Mantenimientos y obras - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx";
                return File(Url.Content(ruta), excelContentType, "Solicitudes Mantenimientos y obras - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx");
            }
            // Aqui se debe cambiar por "Resguardos Laptops"
            if (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Solicitudes Mantenimientos y obras - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx")))
            {
                System.IO.File.Delete(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Solicitudes Mantenimientos y obras - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx"));
            }

            string savedFileName = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Solicitudes Mantenimientos y obras - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx"));
            //FileStream stream = System.IO.File.Create(savedFileName);
            using (FileStream stream = new FileStream(savedFileName, FileMode.Create))
            {
                try
                {

                    var productos = db.Database.SqlQuery<Solicitudes_Excel>("Sp_Get_Solicitudes_excel").ToList();

                    using (var libro = new ExcelPackage())
                    {

                        var worksheet = libro.Workbook.Worksheets.Add("Solicitudes Mantenimientos y obras");
                        #region titulo para poner la razon social de la empresa
                        worksheet.Cells["D3:J3"].Merge = true;
                        worksheet.Cells["D3:J3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["D3:J3"].Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                        var cell = worksheet.Cells["D3"];
                        cell.IsRichText = true;     // Cell contains RichText rather than basic values


                        var title = cell.RichText.Add("SIRE - SOLICITUDES");
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

                        var titlers = cellrs.RichText.Add("Solicitudes Mantenimientos y obras");
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

                        var tabla = worksheet.Tables.Add(new ExcelAddressBase(fromRow: 6, fromCol: 4, toRow: productos.Count + 6, toColumn: 10), "Solicitudes");
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

        #region enviocorreo

        public bool SendMailCancelacion(string motivo, long? idsolisitud)
        {


            var usuario = db.cat_sistemas.Find(db.Auditoria.Where(x => x.idregistro == idsolisitud && x.accion.ToUpper() == "Cancelar" && x.tabla == "solicitud_Mtto").Select(x => x.idusuario).FirstOrDefault());
            var nombreUsuario = usuario != null ? usuario.nombre + " " + usuario.apellido_paterno + " " + usuario.apellido_materno : string.Empty;

            bool success = false;
            string Remitente = ConfigurationManager.AppSettings["Remitente"].ToString();
            string Destinatario = usuario.correo; 
            string Contrasenia = ConfigurationManager.AppSettings["Contrasenia"].ToString();
            string Host = ConfigurationManager.AppSettings["Host"].ToString();
            int Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"].ToString());
            bool Ssl = Convert.ToBoolean(ConfigurationManager.AppSettings["Ssl"].ToString());
            string html = string.Empty;                      

            MailMessage msg = new MailMessage();

            msg.From = new MailAddress(Remitente, "SIRE");
            msg.To.Add(Destinatario);
            msg.Subject = "Se ha cancelado tu solicitud de mantenimiento.";
            msg.Priority = MailPriority.Normal;
            msg.IsBodyHtml = true;

            html += "<div> <div style=\"background-color:#FF8C00\"><img src='cid:imagen' width=\"100\"/></div>";
            html += "<div>";
            html += "<p> Hola " + nombreUsuario + ", </p>";
            html += "<p>Se ha cancelado tu solicitud por el de mantenimieto por el siguiente motivo:</p>";
            html += "</br>";
            html += motivo;
            //html += "<p>Para cambiar su contraseña dé click en el siguiente enlace.<p>";
            //html += "</br>";
            //html += "<a href='" + ruta + "' target='_blank'>Restablecer Contraseña</a>";
            //html += "</br>";
            //html += "<p>El enlace caducará en 24 hrs., así que asegúrese de utilizarlo inmediatamente.</p>";
            html += "</br>";
            html += "<p>¡Gracias por utilizar SIRE!</p>";
            html += "<hr style=\"color:#FF8C00;\"/>";
            html += "<i style=\"color:#FF8C00;\">&copy; Todos los derechos reservados | SIRE " + DateTime.Now.Year + "</i>";
            html += "</br>";
            html += "</br>";
            html += "</div>";

            AlternateView html2 = AlternateView.CreateAlternateViewFromString(html, null, "text/html");
            LinkedResource logo = new LinkedResource(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Sistema/SIRE_FINAL.png"));
            logo.ContentId = "imagen";
            html2.LinkedResources.Add(logo);
            msg.AlternateViews.Add(html2);

            SmtpClient client = new SmtpClient();          
            client.Host = Host;
            client.Port = Port;
            client.EnableSsl = Ssl;
            client.Timeout = 100;
            NetworkCredential myCreds = new NetworkCredential(Remitente, Contrasenia);
            client.Credentials = myCreds;
            client.Send(msg);



            success = true;

            return success;
        }

        #endregion

    }
}