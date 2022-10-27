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

namespace CRME.Controllers
{
    public class PanelInvViewController : Controller
    {
        private SIRE_Context db = new SIRE_Context();
        HelpersController helper = new HelpersController();
        SqlConnection conexion = new SqlConnection();
        // GET: PanelInvView
        public ActionResult Index()
        {
            //if (!User.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction("Index", "AccesoView");
            //}
            ViewBag.HiddenMenu = 1;

            var laptop = db.inventario_laptop.Where(x => x.estatus_ID == 1).Count();
            var monitor = db.inventario_monitor.Where(x => x.estatus_ID == 1).Count();
            var cpu = db.inventario_ensamble.Where(x => x.estatus_ID == 1).Count();
            var linea = db.inventario_lineas.Where(x => x.estatus_ID == 1).Count();
            var movil = db.inventario_movil.Where(x => x.estatus_ID == 1).Count();
            var impresora = db.inventario_impresora.Where(x => x.estatus_ID == 1).Count();
            var cargador = db.inventario_cargador_impresora.Where(x => x.estatus_ID == 1).Count();
            var tipomobi = db.inventario_tipo_mobiliario.Where(x => x.estatus_ID == 1).Count();
            var mobiliaria = db.inventario_mobiliario.Where(x => x.estatus_ID == 1).Count();
            var vehiculo = db.inventario_vehiculos.Where(x => x.estatus_ID == 1).Count();
            var unidad = db.inventario_unidades.Where(x => x.estatus_ID == 1).Count();

            ViewBag.CantidadLap = laptop;
            ViewBag.CantidadMoni = monitor;
            ViewBag.CantidadCpu = cpu;
            ViewBag.CantidadLin = linea;
            ViewBag.CantidadMov = movil;
            ViewBag.CantidadImp = impresora;
            ViewBag.CantidadCar = cargador;
            ViewBag.CantidadTMo = tipomobi;
            ViewBag.CantidadMob = mobiliaria;
            ViewBag.CantidadVeh = vehiculo;
            ViewBag.CantidadUni = unidad;
         
            return View();
        } 

    }
}