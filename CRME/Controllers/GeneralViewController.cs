//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using CRME.Models;
//namespace CRME.Controllers
//{
//    public class GeneralViewController : Controller
//    {
//        private CRME_Context db = new CRME_Context();
//        public JsonResult ObtenerCiudad(string estado, string codigopostal)
//        {
//            if (!string.IsNullOrEmpty(codigopostal))
//            {
//                var Ciudad = (from c in db.CatCodigosPostales where c.numCodigoPostal == codigopostal select new { nbCiudad = c.nbMunicipio, nbEstado = c.nbEstado }).Distinct().ToList();

//                if (!string.IsNullOrEmpty(estado))
//                {
//                    Ciudad = Ciudad.Where(e => e.nbEstado == estado).ToList();
//                }

//                return Json(new SelectList(Ciudad, "nbCiudad", "nbCiudad"), JsonRequestBehavior.AllowGet);

//            }
//            else if (string.IsNullOrEmpty(codigopostal) && !string.IsNullOrEmpty(estado))
//            {
//                var Ciudad = (from c in db.CatCodigosPostales where c.nbEstado == estado select new { nbCiudad = c.nbMunicipio, nbEstado = c.nbEstado }).Distinct().ToList();

//                return Json(new SelectList(Ciudad, "ciudad", "ciudad"), JsonRequestBehavior.AllowGet);
//            }
//            return Json(new SelectList(string.Empty), JsonRequestBehavior.AllowGet);
//        }
//        public JsonResult ObtenerEstado(string codigopostal)
//        {
//            if (codigopostal != "")
//            {
//                var Estado = (from c in db.CatCodigosPostales where c.numCodigoPostal == codigopostal select new { nbEstado = c.nbEstado }).Distinct().ToList();

//                return Json(new SelectList(Estado, "nbEstado", "nbEstado"), JsonRequestBehavior.AllowGet);
//            }
//            return Json(new SelectList(string.Empty), JsonRequestBehavior.AllowGet);
//        }
//        public JsonResult ObtenerColonia(string codigopostal)
//        {
//            if (codigopostal != "")
//            {
//                var Colonia = (from c in db.CatCodigosPostales where c.numCodigoPostal == codigopostal select new { nbAsentamiento = c.nbAsentamiento }).Distinct().ToList();

//                return Json(new SelectList(Colonia, "nbAsentamiento", "nbAsentamiento"), JsonRequestBehavior.AllowGet);
//            }
//            return Json(new SelectList(string.Empty), JsonRequestBehavior.AllowGet);
//        }
//        public JsonResult ValidarCodigoPostal(string codigopostal, string pais)
//        {
//            bool success = false;
//            string mensaje = "";

//            if (pais == "México" && pais != string.Empty)
//            {
//                if (!string.IsNullOrEmpty(codigopostal))
//                {
//                    var buscarcodigopostal = db.CatCodigosPostales.Where(x => x.numCodigoPostal == codigopostal);

//                    if (buscarcodigopostal.Count() > 0)
//                    {
//                        success = true;
//                    }
//                    else
//                    {
//                        success = false;
//                        mensaje = "No se encontró el código postal, por favor ingrese uno válido.";
//                    }
//                }
//            }
//            else
//            {
//                success = true;
//            }

//            return Json(new { success = success, mensaje }, JsonRequestBehavior.AllowGet);
//        }
//    }
//}