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
    public class DepartamentosViewController : Controller
    {
        // GET: DepartamentosView
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
        public ActionResult SaveDepartamento(Departamentos departamento)
        {
            var serializerCat = new JavaScriptSerializer();
            bool success = false;
            string mensajefound = "";
            var found = db.Departamentos.FirstOrDefault(x => x.Dp_Descripcion == departamento.Dp_Descripcion);

            if (found != null)
            {
                mensajefound = "¡Ya existe una sucursal que coincide con el ingresado!";
            }
            else
            {
                if (departamento.Dp_Cve_Departamento == 0)
                {
                    try
                    {
                        Departamentos depa = new Departamentos();
                        depa.Dp_Descripcion = departamento.Dp_Descripcion;
                        depa.Sc_Cve_Sucursal = departamento.Sc_Cve_Sucursal;
                        depa.Em_Cve_Sucursal = departamento.Em_Cve_Sucursal; // es empresa
                        depa.Fecha_Alta = DateTime.Now;
                        depa.Oper_Alta = Auth.Usuario.username;
                        depa.Fecha_Ult_Modf = DateTime.Now;
                        depa.Oper_Ult_Modf = Auth.Usuario.username;
                        depa.Estatus = true;

                        db.Departamentos.Add(depa);
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
                        Departamentos depa = db.Departamentos.Find(departamento.Dp_Cve_Departamento);
                        depa.Dp_Descripcion = departamento.Dp_Descripcion;
                        depa.Sc_Cve_Sucursal = departamento.Sc_Cve_Sucursal;
                        depa.Em_Cve_Sucursal = departamento.Em_Cve_Sucursal;
                        depa.Fecha_Ult_Modf = DateTime.Now;
                        depa.Oper_Ult_Modf = Auth.Usuario.username;

                        db.Entry(depa).State = EntityState.Modified;
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
        public ActionResult _Formulario(long? Dp_Cve_Departamento)
        {  //codigo para agregar y editar usuarios
            //UsuariosPersonas Personas = new UsuariosPersonas();
            Departamentos departamentos = new Departamentos();
            // Personas Persona = new Personas();
            if (Dp_Cve_Departamento != null)
            {
                ViewBag.edit = 1;
                departamentos = db.Departamentos.Find(Dp_Cve_Departamento);
                if(departamentos.Sc_Cve_Sucursal != null)
                {
                    ViewBag.Sc_Cve_Sucursal = new SelectList(db.Sucursal.ToList(), "Em_Cve_Empresa", "Em_Descripcion", departamentos.Sc_Cve_Sucursal);
                }
                else
                {
                    ViewBag.Sc_Cve_Sucursal = new SelectList(db.Sucursal.ToList(), "Sc_Cve_Sucursal", "Sc_Descripcion");
                }
                if(departamentos.Em_Cve_Sucursal != null)
                {
                    ViewBag.Em_Cve_Empresa = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion", departamentos.Em_Cve_Sucursal);
                }
                else
                {
                    ViewBag.Em_Cve_Empresa = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion");
                }                                                
            }
            else
            {
                ViewBag.Sc_Cve_Sucursal = new SelectList(db.Sucursal.ToList(), "Sc_Cve_Sucursal", "Sc_Descripcion");
                ViewBag.Em_Cve_Empresa = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion");
            }

            return PartialView(departamentos);
        }
        public ActionResult _TablaDepartamento(int? page)
        {
            const int pageSize = 10;
            int pageNumber = (page ?? 1);

            var lista = db.Departamentos.Where(x => x.Estatus == true).ToList();

            return PartialView(lista.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult DeleteDepartamento(long? Dp_Cve_Departamento)
        {
            bool success = false; ;
            string mensajefound = "";

            try
            {
                Departamentos depa = db.Departamentos.Find(Dp_Cve_Departamento);
                depa.Estatus = false;
                depa.Fecha_Baja = DateTime.Now;
                depa.Oper_Baja = Auth.Usuario.username;

                db.Entry(depa).State = EntityState.Modified;
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