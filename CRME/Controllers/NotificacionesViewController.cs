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
    public class NotificacionesViewController : Controller
    {

        private SIRE_Context db = new SIRE_Context();
        HelpersController helper = new HelpersController();
        SqlConnection conexion = new SqlConnection();
        // GET: NotificacionesView
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "AccesoView");
            }
            ViewBag.HiddenMenu = 1;

            return View();            
        }

        public ActionResult _TablaNotificacion(int? page)
        {
            const int pageSize = 10;
            int pageNumber = (page ?? 1);
            List<Datos_Notificacion> lista = new List<Datos_Notificacion>();
            try
            {
                var resguardo = db.Database.SqlQuery<Datos_Notificacion>("sp_Get_Datos_Notificacion").ToList();             
                lista = resguardo.ToList();
                return PartialView(lista.ToPagedList(pageNumber, pageSize));
            }
            catch
            {
                return PartialView();
            }
        }
    }
}