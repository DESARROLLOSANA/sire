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
    public class CondicionViewController : Controller
    {
        // GET: CondicionView
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
        public ActionResult SaveCondicion(Condicion condicion)
        {
            //Empresa empresas = new Empresa();
            var serializerCat = new JavaScriptSerializer();
            bool success = false;
            string mensajefound = "";
            var found = db.Condicion.FirstOrDefault(x => x.Descripcion == condicion.Descripcion);

            if (found != null)
            {
                mensajefound = "¡Ya existe una empresa que coincide con el ingresado!";

            }
            else
            {
                if (condicion.Id_condicion == 0)
                {
                    try
                    {
                        Condicion condi = new Condicion();
                        condi.Descripcion = condicion.Descripcion;
                        db.Condicion.Add(condi);
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
                        Condicion Condi = db.Condicion.Find(condicion.Id_condicion);
                        Condi.Descripcion = condicion.Descripcion;
                        //Empre.Em_Razon_Social = Empresas.Em_Razon_Social;
                        //Empre.Em_RFC = Empresas.Em_RFC;
                        //if (Empre.Em_logo == Empresas.Em_logo)
                        //{
                        //    Empre.Em_logo = Empresas.Em_logo;
                        //}
                        //else
                        //{
                        //    var pathCat = serializerCat.Deserialize<string>(Empresas.Em_logo);
                        //    Empre.Em_logo = "~/Upload/Empresa/" + pathCat;
                        //}
                        //Empre.Em_Direccion = Empresas.Em_Direccion;
                        //Empre.Fecha_Ult_Mod = DateTime.Now;
                        //Empre.Oper_Ult_Mod = Auth.Usuario.username;

                        db.Entry(Condi).State = EntityState.Modified;
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
        public ActionResult _FormularioCondicion(long? Id_Condicion)
        {  //codigo para agregar y editar usuarios
            //UsuariosPersonas Personas = new UsuariosPersonas();
            Condicion condicion = new Condicion();
            // Personas Persona = new Personas();

            if (Id_Condicion != null)
            {
                ViewBag.edit = 1;
                condicion = db.Condicion.Find(Id_Condicion);
            }


            return PartialView(condicion);
        }
        public ActionResult _TablaCondicion(int? page)
        {
            const int pageSize = 10;
            int pageNumber = (page ?? 1);

            var lista = db.Condicion.ToList();

            return PartialView(lista.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult DeleteUsuario(long? Em_Cve_Empresa)
        {
            bool success = false; ;
            string mensajefound = "";

            try
            {
                Condicion condi = db.Condicion.Find(Em_Cve_Empresa);              
                db.Entry(condi).State = EntityState.Deleted;
                if (db.SaveChanges() > 0)
                {
                    success = true;
                }
            }
            catch (Exception ex)
            {
                mensajefound = "Ocurrio un error al dar baja la empresa";
            }
            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CargarLogo()
        {
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
        }
        public ActionResult EliminarLogo(string path)
        {
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