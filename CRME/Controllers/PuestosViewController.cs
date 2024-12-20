﻿using System;
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
    public class PuestosViewController : Controller
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

        public ActionResult SavePuesto(Puestos puesto)
        {
            var serializerCat = new JavaScriptSerializer();
            bool success = false;
            string mensajefound = "";
            var found = db.Puestos.FirstOrDefault(x => x.Pu_Descripcion == puesto.Pu_Descripcion && x.Dp_Cve_Departamento == puesto.Dp_Cve_Departamento);

            if (found != null)
            {
                mensajefound = "¡Ya existe un puesto que coincide con el ingresado!";
            }
            else
            {
                if (puesto.Pu_Cve_Puesto == 0)
                {
                    try
                    {
                        Puestos pue = new Puestos();
                        pue.Pu_Descripcion = puesto.Pu_Descripcion;
                        pue.Dp_Cve_Departamento = puesto.Dp_Cve_Departamento;                        
                        pue.Fecha_Alta = DateTime.Now;
                        pue.Oper_Alta = Auth.Usuario.username;
                        pue.Fecha_Ult_Modf = DateTime.Now;
                        pue.Oper_Ult_Modf = Auth.Usuario.username;
                        pue.Estatus = true;

                        db.Puestos.Add(pue);
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
                        Puestos pue = db.Puestos.Find(puesto.Pu_Cve_Puesto);
                        pue.Pu_Descripcion = puesto.Pu_Descripcion;
                        pue.Dp_Cve_Departamento = puesto.Dp_Cve_Departamento;                        
                        pue.Fecha_Ult_Modf = DateTime.Now;
                        pue.Oper_Ult_Modf = Auth.Usuario.username;

                        db.Entry(pue).State = EntityState.Modified;
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
        public ActionResult _Formulario(long? Pu_Cve_Puesto)
        {  //codigo para agregar y editar usuarios
            //UsuariosPersonas Personas = new UsuariosPersonas();
            Puestos puestos = new Puestos();
            // Personas Persona = new Personas();
            if (Pu_Cve_Puesto != null)
            {
                ViewBag.edit = 1;
                puestos = db.Puestos.Find(Pu_Cve_Puesto);

                //var departamento = db.Departamentos.FirstOrDefault(x => x.Dp_Cve_Departamento == puestos.Dp_Cve_Departamento);
                //var sucursal = db.Sucursal.FirstOrDefault(x => x.Sc_Cve_Sucursal == departamento.Sc_Cve_Sucursal);
                //var empresa = db.Empresa.FirstOrDefault(x => x.Em_Cve_Empresa == sucursal.Em_Cve_Empresa || x.Em_Cve_Empresa == departamento.Em_Cve_Sucursal);


                //ViewBag.Em_Cve_Empresa = new SelectList(db.Empresa.Where(x => x.Estatus == true && x.Em_Cve_Empresa == empresa.Em_Cve_Empresa).ToList(), "Em_Cve_Empresa", "Em_Descripcion");
                ViewBag.departamento = new SelectList(db.Departamentos.ToList(), "Dp_Cve_Departamento", "Dp_Descripcion");
                ViewBag.empresa = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion");
                ViewBag.sucursal = new SelectList(db.Sucursal.ToList(), "Sc_Cve_Sucursal", "Sc_Descripcion");
            }            
            else
            {
                ViewBag.empresa = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion");
                ViewBag.departamento = new SelectList("", "Dp_Cve_Departamento", "Dp_Descripcion");
                ViewBag.sucursal = new SelectList("", "Sc_Cve_Sucursal", "Sc_Descripcion");
            }

            return PartialView(puestos);
        }
        public ActionResult _TablaPuesto(int? page)
        {
            const int pageSize = 10;
            int pageNumber = (page ?? 1);

            var lista = db.Puestos
                .Where(x => x.Estatus == true).ToList();

            return PartialView(lista.ToPagedList(pageNumber, pageSize));
        }
       
        public ActionResult DeletePuesto(long? Pu_Cve_Puesto)
        {
            bool success = false; ;
            string mensajefound = "";

            try
            {
                Puestos pue = db.Puestos.Find(Pu_Cve_Puesto);
                pue.Estatus = false;
                pue.Fecha_Baja = DateTime.Now;
                pue.Oper_Baja = Auth.Usuario.username;

                db.Entry(pue).State = EntityState.Modified;
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

        [HttpGet]
        public ActionResult GetSucursalByEmpresa(int Em_Cve_Empresa)
        {
            var sucursal = db.Sucursal
                .Where(x => x.Estatus == true && x.Em_Cve_Empresa == Em_Cve_Empresa)
                .Select(x => new { Value = x.Sc_Cve_Sucursal, Text = x.Sc_Descripcion })
                .ToList();

            return Json(sucursal, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDepartamentosByEmpresa(int Sc_Cve_Sucursal)
        {
            var departamento = db.Departamentos
                .Where(x => x.Estatus == true && x.Sc_Cve_Sucursal == Sc_Cve_Sucursal)
                .Select(x => new { Value = x.Dp_Cve_Departamento, Text = x.Dp_Descripcion })
                .ToList();

            return Json(departamento, JsonRequestBehavior.AllowGet);
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