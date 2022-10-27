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
    public class EmpleadosViewController : Controller
    {
        // GET: EmpleadosView
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
        public ActionResult SaveEmpleado(cat_usuarios empleados)
        {
            var serializerCat = new JavaScriptSerializer();
            bool success = false;
            string mensajefound = "";
            var found = db.cat_usuarios.FirstOrDefault(x => x.nombre_completo == empleados.nombre_completo);

            if (found != null)
            {
                mensajefound = "¡Ya existe un empleado que coincide con el ingresado!";
            }
            else
            {
                if (empleados.usuario_ID == 0)
                {
                    try
                    {
                        cat_usuarios emple = new cat_usuarios();
                        emple.nombres = empleados.nombres;
                        emple.paterno = empleados.paterno;
                        emple.materno = empleados.materno;
                        emple.nombre_completo = empleados.nombre_completo;
                        emple.Pu_Cve_Puesto = empleados.Pu_Cve_Puesto;
                        emple.nombre_completo = empleados.nombres + " " + empleados.paterno + " " + empleados.materno;
                        //emple.Fecha_Alta = DateTime.Now;
                        //pue.Oper_Alta = Auth.Usuario.username;
                        //pue.Fecha_Ult_Modf = DateTime.Now;
                        //pue.Oper_Ult_Modf = Auth.Usuario.username;
                        emple.estatus_ID = 1;

                        db.cat_usuarios.Add(emple);
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
                        cat_usuarios emple = db.cat_usuarios.Find(empleados.usuario_ID);                         
                        emple.nombres = empleados.nombres;
                        emple.paterno = empleados.paterno;
                        emple.materno = empleados.materno;
                        emple.nombre_completo = empleados.nombre_completo;
                        emple.Pu_Cve_Puesto = empleados.Pu_Cve_Puesto;
                        //pue.Fecha_Ult_Modf = DateTime.Now;
                        //pue.Oper_Ult_Modf = Auth.Usuario.username;

                        db.Entry(emple).State = EntityState.Modified;
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
        public ActionResult _Formulario(long? usuario_ID)
        {  //codigo para agregar y editar usuarios
            cat_usuarios empleado = new cat_usuarios();
            if (usuario_ID != null)
            {
                ViewBag.edit = 1;
                empleado = db.cat_usuarios.Find(usuario_ID);
                ViewBag.idNivelEstudio = new SelectList(db.NivelAcademico.Where(x => x.Estatus == true && x.idNivelEstudio == empleado.idNivelEstudio).ToList(), "idNivelEstudio", "desNivelEstudio");
            }
            else
            {
                ViewBag.idNivelEstudio = new SelectList(db.NivelAcademico.Where(x => x.Estatus == true).ToList(), "idNivelEstudio", "desNivelEstudio");
            }

            return PartialView(empleado);
        }
        public ActionResult _TablaEmpleado(int? page)
        {
            const int pageSize = 10;
            int pageNumber = (page ?? 1);

            var lista = db.cat_usuarios
                .Where(x => x.estatus_ID == 1).ToList().OrderByDescending(x=> x.usuario_ID);

            return PartialView(lista.ToPagedList(pageNumber, pageSize));
        }       
        public ActionResult DeleteEmpleado(long? usuario_ID)
        {
            bool success = false; ;
            string mensajefound = "";

            try
            {
                cat_usuarios emple = db.cat_usuarios.Find(usuario_ID);
                emple.estatus_ID = 0;
                //pue.Fecha_Baja = DateTime.Now;
                //pue.Oper_Baja = Auth.Usuario.username;

                db.Entry(emple).State = EntityState.Modified;
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
        public ActionResult comboEmpresa(int? Em_Cve_Empresa)
        {
            if (Em_Cve_Empresa != null)
            {
                ViewBag.Em_Cve_Empresa = new SelectList(db.Empresa.Where(x => x.Estatus == true).ToList(), "Em_Cve_Empresa", "Em_Descripcion");
            }
            else
            {
                ViewBag.Em_Cve_Empresa = new SelectList(db.Empresa.Where(x => x.Estatus == true).ToList(), "Em_Cve_Empresa", "Em_Descripcion");
            }

            return View();
        }
        public ActionResult ComboSucursal(int? Em_Cve_Empresa, int? Sc_Cve_Sucursal)
        {
            if (Sc_Cve_Sucursal != null)
            {
                ViewBag.Sc_Cve_Sucursal = new SelectList(db.Sucursal.Where(x => x.Estatus == true && x.Sc_Cve_Sucursal == Sc_Cve_Sucursal).ToList(), "Sc_Cve_Sucursal", "Sc_Descripcion");
            }
            else
            {
                ViewBag.Sc_Cve_Sucursal = new SelectList(db.Sucursal.Where(x => x.Estatus == true && x.Em_Cve_Empresa == Em_Cve_Empresa).ToList(), "Sc_Cve_Sucursal", "Sc_Descripcion");
            }
            return View();
        }
        public ActionResult ComboDepartamento(int? Em_Cve_Empresa, int? Sc_Cve_Empresa, int? Dp_cve_Departamento)
        {
            if (Dp_cve_Departamento != null)
            {
                ViewBag.Dp_cve_Departamento = new SelectList(db.Departamentos.Where(x => x.Estatus == true && x.Dp_Cve_Departamento == Dp_cve_Departamento).ToList(), "Dp_Cve_Departamento", "Dp_Descripcion");
            }
            else
            {
                if (Em_Cve_Empresa != null && Sc_Cve_Empresa != null)
                {
                    ViewBag.Dp_cve_Departamento = new SelectList(db.Departamentos.Where(x => x.Estatus == true && x.Em_Cve_Sucursal == Em_Cve_Empresa && x.Sc_Cve_Sucursal == Sc_Cve_Empresa).ToList(), "Dp_Cve_Departamento", "Dp_Descripcion");
                }
                else
                {
                    ViewBag.Dp_cve_Departamento = new SelectList(db.Departamentos.Where(x => x.Estatus == true && x.Em_Cve_Sucursal == Em_Cve_Empresa).ToList(), "Dp_Cve_Departamento", "Dp_Descripcion");
                }
            }

            return View();
        }
        public ActionResult ComboPuesto( int? Dp_cve_Departamento, int? Pu_Cve_Puesto)
        {
            if (Pu_Cve_Puesto != null)
            {
                ViewBag.Pu_Cve_Puesto = new SelectList(db.Puestos.Where(x => x.Estatus == true && x.Pu_Cve_Puesto == Pu_Cve_Puesto).ToList(), "Pu_Cve_Puesto", "Pu_Descripcion");
            }
            else
            {
                if ( Dp_cve_Departamento != null)
                {
                    ViewBag.Pu_Cve_Puesto = new SelectList(db.Puestos.Where(x => x.Estatus == true && x.Dp_Cve_Departamento == Dp_cve_Departamento).ToList(), "Pu_Cve_Puesto", "Pu_Descripcion");
                }
                else
                {
                    ViewBag.Pu_Cve_Puesto = new SelectList(db.Puestos.Where(x => x.Estatus == true && x.Dp_Cve_Departamento == Dp_cve_Departamento).ToList(), "Dp_Cve_Departamento", "Dp_Descripcion");
                }
            }

            return View();
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