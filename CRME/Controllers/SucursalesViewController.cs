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
    public class SucursalesViewController : Controller
    {
        // GET: SucursalesView
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
        public ActionResult SaveSucursal(Sucursal sucursal)
        {           
            var serializerCat = new JavaScriptSerializer();
            bool success = false;
            string mensajefound = "";
            var found = db.Sucursal.FirstOrDefault(x => x.Sc_Descripcion == sucursal.Sc_Descripcion && x.Sc_Cve_Sucursal != sucursal.Sc_Cve_Sucursal);

            if (found != null)
            {
                mensajefound = "¡Ya existe una sucursal que coincide con el ingresado!";

            }
            else
            {
                if (sucursal.Sc_Cve_Sucursal == 0)
                {
                    try
                    {
                        Sucursal sucur = new Sucursal();
                        sucur.Sc_Descripcion = sucursal.Sc_Descripcion;
                        sucur.Sc_Descripcion = sucursal.Sc_Direccion;
                        sucur.Em_Cve_Empresa = sucursal.Em_Cve_Empresa;
                        sucur.Fecha_Alta = DateTime.Now;
                        sucur.Oper_Alta = Auth.Usuario.username;
                        sucur.Fecha_Ult_Modif = DateTime.Now;
                        sucur.Oper_Ult_Modif = Auth.Usuario.username;
                        sucur.Estatus = true;

                        db.Sucursal.Add(sucur);
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
                        Sucursal sucur = db.Sucursal.Find(sucursal.Sc_Cve_Sucursal);
                        sucur.Sc_Descripcion = sucursal.Sc_Descripcion;
                        sucur.Sc_Direccion = sucursal.Sc_Direccion;
                        sucur.Em_Cve_Empresa = sucur.Em_Cve_Empresa;                                              
                        sucur.Fecha_Ult_Modif = DateTime.Now;
                        sucur.Oper_Ult_Modif = Auth.Usuario.username;

                        db.Entry(sucur).State = EntityState.Modified;
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
        public ActionResult _Fromulario(long? Sc_Cve_Sucursal)
        {  //codigo para agregar y editar usuarios
            //UsuariosPersonas Personas = new UsuariosPersonas();
            Sucursal sucursal = new Sucursal();
            // Personas Persona = new Personas();
            if (Sc_Cve_Sucursal != null)
            {
                ViewBag.edit = 1;
                sucursal = db.Sucursal.Find(Sc_Cve_Sucursal);
                ViewBag.Em_Cve_Empresa = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion", sucursal.Em_Cve_Empresa);
                
            }
            else
            {
                ViewBag.Em_Cve_Empresa = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion");                

            }

            return PartialView(sucursal);
        }
        public ActionResult _TablaSucursal(int? page)
        {
            const int pageSize = 10;
            int pageNumber = (page ?? 1);

            var lista = db.Sucursal.Where(x => x.Estatus == true).ToList();            

            return PartialView(lista.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult DeleteSucursal(long? Sc_Cve_Sucursal)
        {
            bool success = false; ;
            string mensajefound = "";

            try
            {
                Sucursal sucur = db.Sucursal.Find(Sc_Cve_Sucursal);
                sucur.Estatus = false;
                sucur.Fecha_Baja = DateTime.Now;
                sucur.Oper_Baja = Auth.Usuario.username;

                db.Entry(sucur).State = EntityState.Modified;
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