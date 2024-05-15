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
    public class InventarioTipoMobiliarioViewController : Controller
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
        public ActionResult SaveEmpresa(inventario_tipo_mobiliario Empresas)
        {
            //Empresa empresas = new Empresa();
            var serializerCat = new JavaScriptSerializer();
            bool success = false;
            string mensajefound = "";
            //linea pendiente de revision

            var found = db.inventario_tipo_mobiliario.FirstOrDefault(x => x.mobiliario == Empresas.mobiliario && x.tipo_mobiliario_ID != Empresas.tipo_mobiliario_ID);
            //var found = db.Empresa.FirstOrDefault(x => x.Em_Descripcion == Empresas.Em_Descripcion && x.Em_Cve_Empresa != Empresas.Em_Cve_Empresa);

            if (string.IsNullOrWhiteSpace(Empresas.mobiliario))
            {
                mensajefound = "¡no puede estar en blanco!";
            }
            else
            {



                if (found != null)
                {
                    mensajefound = "¡Ya existe una linea que coincide con la ingresada!";

                }


                else
                {

                    if (Empresas.tipo_mobiliario_ID == 0)
                    {
                        try
                        {
                            inventario_tipo_mobiliario empre = new inventario_tipo_mobiliario();
                            empre.mobiliario = Empresas.mobiliario;

                            empre.estatus_ID = 1;
                            //empre.usuario_app_ID = null;
                            //empre.created_time_actuales = DateTime.Now;
                            //empre.Em_Cve_Empresa = 0;

                            db.inventario_tipo_mobiliario.Add(empre);
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
                            inventario_tipo_mobiliario Empre = db.inventario_tipo_mobiliario.Find(Empresas.tipo_mobiliario_ID);
                            Empre.mobiliario = Empresas.mobiliario;

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
            inventario_tipo_mobiliario Empresas = new inventario_tipo_mobiliario();
           // Personas Persona = new Personas();

            if (inv_linea_ID != null)
            {
                ViewBag.edit = 1;
                Empresas = db.inventario_tipo_mobiliario.Find(inv_linea_ID);
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

                var lista = db.inventario_tipo_mobiliario.OrderByDescending(x => x.tipo_mobiliario_ID).ToList();
                //var lista = db.inventario_lineas.ToList();
                ViewBag.filtro = filtro;
                return PartialView(lista.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                var lista = db.inventario_tipo_mobiliario.Where(x => x.mobiliario.ToUpper().Contains(filtro.ToUpper().Trim())
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
                inventario_tipo_mobiliario lin = db.inventario_tipo_mobiliario.Find(inv_linea_ID);

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