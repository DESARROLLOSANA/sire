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
                            empre.idNivelEstudio = 1;
                            empre.Pu_Cve_Puesto = 1;
                            empre.estatus_ID = 1;
                        
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

                            Empre.idNivelEstudio = 1;
                            Empre.Pu_Cve_Puesto = 1;

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
                //añadir controlador de listas
                //ViewBag.idGenero = new SelectList(db.CatGeneros.ToList(), "idGenero", "nbGenero");
                //ViewBag.plan = new SelectList(items3, "Value", "Text", Empresas.nombre_plan);
            }
            //else
            //{
            //}

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

                if (lin.estatus_ID != 4)
                {
                    lin.estatus_ID = 4;
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
                mensajefound = "Ocurrio un error al dar baja a la linea";
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