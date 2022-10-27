using CRME.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.IO;
using CRME.Helpers;


namespace CRME.Controllers
{
    public class CorreoViewController : Controller
    {
        private CRME_Context db = new CRME_Context();
        HelpersController helper = new HelpersController();

        public ActionResult Index()
        {
          
            return View();
        }

       

        //public ActionResult EnviarEmail(List<Alumnos> Alummnos)
        //{
        //    bool success = false;
        //    string msj = string.Empty;

        //    var savedFileNameDownload = "";
        //    string savedFileName = "";
        //    string completeName = "";

        //    foreach (var item in Alummnos)
        //    {
        //        try
        //        {
        //            Correos Correo = new Correos();
        //            string path = ConfigurationManager.AppSettings["path"].ToString();
        //            var ruta = path + "AlumnoView/Edit/?id=" + item.idAlumno;
        //            Correo.desAsunto = "Actualización de Información del CRME";
        //            Correo.nbReceptor = item.Personas.MediosContactos.Where(x => x.idTipoMedioContacto == 1).FirstOrDefault().desMedioContacto;
        //            Correo.pathLogo = "~/Upload/Logo.png";
        //            Correo.pathLink = ruta;
        //            Correo.idEstatus = 1;
        //            Correo.tpCorreo = "msj";
        //            Correo.desColorCuerpo = "#9e9e9e";
        //            Correo.desColorHeader = "#ff9800";
        //            Correo.desColorTitulo = "#ffb74d";
        //            Correo.desMensaje = "<p> Hola " + item.Personas.nbCompleto + " </p>";
        //            Correo.desMensaje += "<p>Requerimos que actualice información de su perfil</p>";
        //            Correo.desMensaje += "<p>Para actualizar sus datos de click en el enlace de abajo.<p>";
        //            Correo.desMensaje += "</br>";
        //            Correo.desMensaje += "<p>¡Gracias por utilizar CRME!</p>";
        //            Correo.desMensaje += "<hr style=\"color:#ccc;\"/>";
        //            Correo.desMensaje += "<i style=\"color:#ccc;\">&copy; Todos los derechos reservados | CRME" + DateTime.Now.Year + "</i>";
        //            Correo.desMensaje += "</br>";
        //            Correo.desMensaje += "</br>";
        //            db.Correos.Add(Correo);

        //            var count = 0;
        //            foreach (string file in Request.Files)
        //            {
        //                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
        //                if (hpf.FileName != "")
        //                {
        //                    string extension = Path.GetExtension(Path.GetFileName(hpf.FileName));
        //                    string name = "ArchivoCorreo_" + count;
        //                    completeName = name + extension;
        //                    savedFileName = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/ArchivosCorreos"), completeName);
        //                    hpf.SaveAs(savedFileName);
        //                    // success = true;

        //                    ArchivosCorreos ArchivosCorreos = new ArchivosCorreos();
        //                    ArchivosCorreos.idCorreo = Correo.idCorreo;
        //                    ArchivosCorreos.pathArchivo = "~/Upload/ArchivosCorreos/" + completeName;
        //                    db.ArchivosCorreos.Add(ArchivosCorreos);
        //                }
        //                count++;
        //            }
        //            if (db.SaveChanges() > 0)
        //            {
        //                success = true;
        //                msj = "Correo enviado con éxito";
        //            }
        //            else
        //            {
        //                msj = "Error al enviar el correo, Intente nuevamente o contacte al Administrador";
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.Message.ToString());
        //            msj = "Error al enviar el correo, Contacte al Administrador";
        //        }
        //    }

        //    return Json(new { success = success, msj }, JsonRequestBehavior.AllowGet);
        //}

    }
}