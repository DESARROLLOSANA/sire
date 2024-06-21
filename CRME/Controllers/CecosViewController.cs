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
    public class CecosViewController : Controller
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
        public ActionResult SaveSucursal(Ce_cos sucursal)
        {           
            var serializerCat = new JavaScriptSerializer();
            bool success = false;
            string mensajefound = "";
            var found = db.Ce_cos.FirstOrDefault(x => x.Ceco_Descripcion == sucursal.Ceco_Descripcion && x.Em_Cve_Empresa != sucursal.Em_Cve_Empresa); // listo
            //var found = db.Sucursal.FirstOrDefault(x => x.Sc_Descripcion == sucursal.Sc_Descripcion && x.Sc_Cve_Sucursal != sucursal.Sc_Cve_Sucursal);

            if (found != null)
            {
                mensajefound = "¡Ya existe un centro de costo que coincide con el ingresado!";

            }
            else
            {
                //sucursal.Ceco_ID
                //Sc_Cve_Sucursal 
                if (sucursal.Ceco_ID== 0)
                {
                    try
                    {
                        Ce_cos sucur = new Ce_cos();
                        //Sucursal sucur = new Sucursal();

                        sucur.Ceco_Descripcion = sucursal.Ceco_Descripcion;
                        //sucur.Sc_Descripcion = sucursal.Sc_Descripcion
                        //Aqui se añade el autogenerador de codigos de secos

                        //se debe remplazar este por el autogenerador de codigos cecos.
                        //sucur.Ceco_Cve_Ceco = sucursal.Ceco_Cve_Ceco;

                        //sucur.Ceco_Cve_Ceco = "200";
                        //sucur.Sc_Direccion = sucursal.Sc_Direccion;
                       
                        var pruebas=db.Ce_cos.FirstOrDefault(x => x.Em_Cve_Empresa ==sucursal.Em_Cve_Empresa);

                        if (pruebas != null)
                        {
                            //en el caso de que exista previamente la empresa
                            //sucur.Ceco_Cve_Ceco = sucursal.Ceco_Cve_Ceco;
                            Lista_Cecos temportal = new Lista_Cecos();

                            temportal = db.Database.SqlQuery<Lista_Cecos>("Sp_Get_Max_Cecos @Id_sucursal",new SqlParameter("@Id_sucursal",sucursal.Em_Cve_Empresa)).FirstOrDefault();

                            sucur.Ceco_Cve_Ceco = temportal.resultado;
                        }
                        else
                        {
                            // en el caso de que no exista previamente la empresa dara por valor inicial 1
                            sucur.Ceco_Cve_Ceco = "0000000001";

                        }


                        sucur.Em_Cve_Empresa = sucursal.Em_Cve_Empresa;
                        
                        //suprimir
                        //sucur.Fecha_Alta = DateTime.Now;
                        //sucur.Oper_Alta = Auth.Usuario.username;
                        //sucur.Fecha_Ult_Modif = DateTime.Now;
                        //sucur.Oper_Ult_Modif = Auth.Usuario.username;
                        
                        sucur.Estatus = true;

                        db.Ce_cos.Add(sucur);
                        //db.Sucursal.Add(sucur);
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

                        Ce_cos sucur = db.Ce_cos.Find(sucursal.Ceco_ID);
                        //Sucursal sucur = db.Sucursal.Find(sucursal.Sc_Cve_Sucursal);
                        sucur.Ceco_Descripcion = sucursal.Ceco_Descripcion;
                        //sucur.Sc_Descripcion = sucursal.Sc_Descripcion;
                        sucur.Ceco_Cve_Ceco = sucursal.Ceco_Cve_Ceco;
                        //sucur.Sc_Direccion = sucursal.Sc_Direccion;
                        
                        sucur.Em_Cve_Empresa = sucursal.Em_Cve_Empresa;                                              
                        
                        // suprimir estas columnas
                        //sucur.Fecha_Ult_Modif = DateTime.Now;
                        //sucur.Oper_Ult_Modif = Auth.Usuario.username;

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
        //sin cambios aparentes
        public ActionResult _Formulario(long? Sc_Cve_Sucursal)
        {  //codigo para agregar y editar usuarios
            //UsuariosPersonas Personas = new UsuariosPersonas();
            Ce_cos sucursal = new Ce_cos();
            // Personas Persona = new Personas();
            if (Sc_Cve_Sucursal != null)
            {
                ViewBag.edit = 1;
                sucursal = db.Ce_cos.Find(Sc_Cve_Sucursal);
                if(sucursal.Em_Cve_Empresa != 0){
                    ViewBag.Em_Cve_Empresa1 = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion", sucursal.Em_Cve_Empresa);
                }
                else
                {
                    ViewBag.Em_Cve_Empresa1 = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion");
                }
            }
            else
            {
                ViewBag.Em_Cve_Empresa1 = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion");                

            }

            return PartialView(sucursal);
        }

        //sin cambios necesarios
        public ActionResult _TablaCecos(int? page)
        {
            const int pageSize = 10;
            int pageNumber = (page ?? 1);

            var lista = db.Ce_cos.OrderByDescending(x=> x.Ceco_ID).Where(x => x.Estatus == true).ToList();
            //var lista = db.Ce_cos.Where(x => x.Estatus == true).ToList();

            return PartialView(lista.ToPagedList(pageNumber, pageSize));
        }
        
        //cambios minimos, pendientes de verificacion
        public ActionResult DeleteSucursal(long? Ceco_ID)
        {
            bool success = false; ;
            string mensajefound = "";

            try
            {
                Ce_cos sucur = db.Ce_cos.Find(Ceco_ID);
                sucur.Estatus = false;
                //sucur.Fecha_Baja = DateTime.Now;
                //sucur.Oper_Baja = Auth.Usuario.username;

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