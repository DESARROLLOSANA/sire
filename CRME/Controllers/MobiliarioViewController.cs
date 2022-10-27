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
    public class MobiliarioViewController : Controller
    {
        // GET: MobiliarioView
        private SIRE_Context db = new SIRE_Context();
        HelpersController helper = new HelpersController();
        SqlConnection conexion = new SqlConnection();

        public ActionResult Index()
        {
            return View();
        }          
        public ActionResult _Formulario(long? inv_mobiliario_ID)
        {
            inventario_mobiliario mobiliario = new inventario_mobiliario();
            if (inv_mobiliario_ID != null)
            {
                ViewBag.edit = 1;
                mobiliario = db.inventario_mobiliario.Find(inv_mobiliario_ID);
            }
            return PartialView(mobiliario);
        }
        public ActionResult _TablaMobiliario(int? page)
        {
            const int pageSize = 10;
            int pageNumber = (page ?? 1);

            var lista = db.inventario_mobiliario.ToList();// filtros?? y exportacion exce?

            return PartialView(lista.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult SaveMobiliario(inventario_mobiliario mobiliario)
        {
            var serializerCat = new JavaScriptSerializer();
            bool success = false;
            string mensajefound = "";
            int idmob = 0;
            var found = db.inventario_mobiliario.FirstOrDefault(x => x.cod_inventario == mobiliario.cod_inventario);
           
            if (mobiliario.inv_mobiliario_ID == 0)
            {
                try
                {                        
                    inventario_mobiliario mobi = new inventario_mobiliario();
                    mobi.tipo_mobiliario_ID = mobiliario.tipo_mobiliario_ID;
                    mobi.color = mobiliario.color;
                    mobi.tipo = mobiliario.tipo;
                    mobi.precio = mobiliario.precio;
                    mobi.folio = mobiliario.folio;
                    mobi.fecha_folio = mobiliario.fecha_folio;
                    mobi.proveedor_ID = mobiliario.proveedor_ID;
                    mobi.Em_Cve_Empresa = mobiliario.Em_Cve_Empresa;
                    mobi.estatus_ID = 1;
                                                
                    db.inventario_mobiliario.Add(mobi);
                    if (db.SaveChanges() > 0)
                    {
                        idmob = mobi.inv_mobiliario_ID;

                        inventario_mobiliario MOBILI = db.inventario_mobiliario.Find(idmob);
                        MOBILI.cod_inventario = db.Empresa.Find(mobi.Em_Cve_Empresa).Em_Descripcion.ToUpper() + 
                        DateTime.Now.Year.ToString().Substring(2, 2) + "-MB" + idmob.ToString();

                        db.Entry(MOBILI).State = EntityState.Modified;
                        db.SaveChanges();

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

                    //Puestos pue = db.Puestos.Find(puesto.Pu_Cve_Puesto);
                    //pue.Pu_Descripcion = puesto.Pu_Descripcion;
                    //pue.Dp_Cve_Departamento = puesto.Dp_Cve_Departamento;
                    //pue.Fecha_Ult_Modf = DateTime.Now;
                    //pue.Oper_Ult_Modf = Auth.Usuario.username;

                    //db.Entry(pue).State = EntityState.Modified;
                    //if (db.SaveChanges() > 0)
                    //{
                    //    success = true;
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
           
            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteMobiliario(long? inv_mobiliario_ID)
        {
            bool success = false; ;
            string mensajefound = "";

            try
            {
                inventario_mobiliario mob = db.inventario_mobiliario.Find(inv_mobiliario_ID);
                mob.estatus_ID = 0;
                //pue.Fecha_Baja = DateTime.Now;
                //pue.Oper_Baja = Auth.Usuario.username;

                db.Entry(mob).State = EntityState.Modified;
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
        public ActionResult ComboEmpresa(int? Em_Cve_Empresa)
        {
            if (Em_Cve_Empresa != null)
            {
                ViewBag.Em_Cve_Empresa = new SelectList(db.Empresa.Where(x => x.Estatus == true).ToList(), "Em_Cve_Empresa", "Em_Descripcion", Em_Cve_Empresa);
            }
            else
            {
                ViewBag.Em_Cve_Empresa = new SelectList(db.Empresa.Where(x => x.Estatus == true).ToList(), "Em_Cve_Empresa", "Em_Descripcion");
            }

            return View();
        }
        public ActionResult ComboTipo(int? tipo_mobiliario_ID)
        {
            if (tipo_mobiliario_ID != null)
            {
                ViewBag.tipo_mobiliario_ID = new SelectList(db.inventario_tipo_mobiliario.Where(x => x.estatus_ID == 1).ToList(), "tipo_mobiliario_ID", "mobiliario", tipo_mobiliario_ID);
            }
            else
            {
                ViewBag.tipo_mobiliario_ID = new SelectList(db.inventario_tipo_mobiliario.Where(x => x.estatus_ID == 1).ToList(), "tipo_mobiliario_ID", "mobiliario");
            }
            return View();
        }
        public ActionResult ComboProveedor(int? proveedor_ID)
        {
            if (proveedor_ID != null)
            {
                ViewBag.proveedor_ID = new SelectList(db.cat_proveedores.Where(x => x.estatus_ID == 1 ).ToList(), "proveedor_ID", "proveedor", proveedor_ID);
            }
            else
            {
                ViewBag.proveedor_ID = new SelectList(db.cat_proveedores.Where(x => x.estatus_ID == 1).ToList(), "proveedor_ID", "proveedor");
            }

            return View();
        }
    }
}