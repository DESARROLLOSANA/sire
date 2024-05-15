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
    public class InventarioImpresorasViewController : Controller
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
        public ActionResult SaveEmpresa(inventario_impresora Empresas)
        {
            //Empresa empresas = new Empresa();
            var serializerCat = new JavaScriptSerializer();
            bool success = false;
            string mensajefound = "";
            //linea pendiente de revision

            var found = db.inventario_impresora.FirstOrDefault(x => x.serie == Empresas.serie && x.inv_impresora_ID != Empresas.inv_impresora_ID);
            //var found = db.Empresa.FirstOrDefault(x => x.Em_Descripcion == Empresas.Em_Descripcion && x.Em_Cve_Empresa != Empresas.Em_Cve_Empresa);

            if (string.IsNullOrWhiteSpace(Empresas.serie))
            {
                mensajefound = "¡No puede estar en blanco la Serie de la impresora!";
            }
            else
            {


                if (found != null)
                {
                    mensajefound = "¡Ya existe una impresora que coincide con la informacion ingresada!";

                }
                else
                {
                    if (Empresas.inv_impresora_ID == 0)
                    {
                        try
                        {
                            inventario_impresora empre = new inventario_impresora();
                            empre.serie = Empresas.serie;
                            empre.marca = Empresas.marca;
                            empre.modelo = Empresas.modelo;
                            empre.parte = Empresas.parte;
                            empre.precio = Empresas.precio;
                            empre.folio = Empresas.folio;
                            empre.fecha_folio = Empresas.fecha_folio;
                            empre.proveedor_ID = Empresas.proveedor_ID;

                            //datos definidos por e l sistema
                            empre.estatus_ID = 1;
                            empre.Em_Cve_Empresa = Empresas.Em_Cve_Empresa;
                            //empre.Em_Cve_Empresa = 1;

                            //codigo de inventario
                            var year = DateTime.Now;
                            string conver = Convert.ToString(year);
                            string[] ultimos = conver.Split('/', ' ', ':');
                            char[] maspruba = conver.ToCharArray();
                            //string[] ultimos = conver.Split('/', ' ', ':', '0');

                            inventario_impresora modificador = db.inventario_impresora.OrderByDescending(x => x.inv_impresora_ID).First();
                            Empresa etiqueta = db.Empresa.Find(empre.Em_Cve_Empresa);

                            //empre.cod_inventario = etiqueta.Em_Descripcion.ToUpper() + ultimos[2] + "-IM" + Convert.ToString(modificador.inv_impresora_ID + 1);
                            empre.cod_inventario = etiqueta.Em_Descripcion.ToUpper() + maspruba[8] + maspruba[9] + "-IM" + Convert.ToString(modificador.inv_impresora_ID + 1);



                            db.inventario_impresora.Add(empre);
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
                            inventario_impresora Empre = db.inventario_impresora.Find(Empresas.inv_impresora_ID);
                            Empre.serie = Empresas.serie;
                            Empre.marca = Empresas.marca;
                            Empre.modelo = Empresas.modelo;
                            Empre.parte = Empresas.parte;
                            Empre.precio = Empresas.precio;
                            Empre.folio = Empresas.folio;
                            Empre.fecha_folio = Empresas.fecha_folio;
                            Empre.proveedor_ID = Empresas.proveedor_ID;

                            //datos definidos por el sistema
                            Empre.estatus_ID = Empresas.estatus_ID;
                            //empre.Em_Cve_Empresa = 1;


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

        //public ActionResult _FormularioEmpresa(long? inv_linea_ID)
        public ActionResult _FormularioImpresoras(long? inv_impresora_ID)
        {  //codigo para agregar y editar usuarios
            //UsuariosPersonas Personas = new UsuariosPersonas();
            inventario_impresora Empresas = new inventario_impresora();
           // Personas Persona = new Personas();

            if (inv_impresora_ID != null)
            {
                ViewBag.edit = 1;
                Empresas = db.inventario_impresora.Find(inv_impresora_ID);

                if (Empresas.proveedor_ID != 0)
                {
                    ViewBag.proveedor = new SelectList(db.cat_proveedores.ToList(), "proveedor_ID", "proveedor", Empresas.proveedor_ID);
                }
                else
                {
                    ViewBag.proveedor = new SelectList(db.cat_proveedores.ToList(), "proveedor_ID", "proveedor");
                }

                if (Empresas.estatus_ID != 0)
                {
                    ViewBag.estatus = new SelectList(db.cat_estatus_inv.ToList(), "estatus_ID", "estatus", Empresas.estatus_ID);
                }
                else
                {
                    ViewBag.estatus = new SelectList(db.cat_estatus_inv.ToList(), "estatus_ID", "estatus");
                }



                if (Empresas.Em_Cve_Empresa != 0)
                {

                    ViewBag.empresa = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion", Empresas.Em_Cve_Empresa);
                }
                else
                {
                    ViewBag.empresa = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion");
                }


            }

            else
            {


                ViewBag.estatus = new SelectList(db.cat_estatus_inv.ToList(), "estatus_ID", "estatus");
                ViewBag.proveedor = new SelectList(db.cat_proveedores.ToList(), "proveedor_ID", "proveedor");
                ViewBag.empresa = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion");
            }

            return PartialView(Empresas);
        }



        //metodo listo
        //public ActionResult _TablaLineas(int? page)
        public ActionResult _TablaImpresoras(int? page, string filtro)
        {
            const int pageSize = 10;
            int pageNumber = (page ?? 1);

            if (string.IsNullOrEmpty(filtro))
            {
                var lista = db.inventario_impresora.OrderByDescending(x=> x.inv_impresora_ID).ToList();
                ViewBag.filtro = filtro;
                return PartialView(lista.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                var lista = db.inventario_impresora.Where(x => x.serie.ToUpper().Contains(filtro.ToUpper().Trim())
                   || x.marca.ToUpper().Contains(filtro.ToUpper().Trim()) || x.modelo.ToUpper().Contains(filtro.ToUpper().Trim())
                   || x.parte.ToUpper().Contains(filtro.ToUpper().Trim())
                   //|| x.cod_inventario.ToUpper().Contains(filtro.ToUpper().Trim())
                   || x.cod_inventario.ToUpper().Contains(filtro.ToUpper().Trim())
                   ).ToList();

                ViewBag.filtro = filtro;

                return PartialView(lista.ToPagedList(pageNumber, pageSize));
            }
        }

        //metodo casi listo- se debe cambiart nombre
        public ActionResult DeleteUsuario(long? inv_impresora_ID)
        {
            bool success = false; ;
            string mensajefound = "";

            try
            {                
                inventario_impresora impr = db.inventario_impresora.Find(inv_impresora_ID);

                if (impr.estatus_ID != 4)
                {
                    impr.estatus_ID = 4;
                }
                else
                {
                    impr.estatus_ID = 1;
                }
                //lin.estatus_ID = 4;
                //Empre.Fecha_Baja = DateTime.Now;
                //Empre.Oper_Baja = Auth.Usuario.username;

                db.Entry(impr).State = EntityState.Modified;
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