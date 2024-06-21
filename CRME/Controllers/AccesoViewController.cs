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
//SIRE_Context
namespace CRME.Controllers
{
    public class AccesoViewController : Controller
    {
        private SIRE_Context db = new SIRE_Context();

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "PanelView");
                ViewBag.HiddenMenu = 1;
            }            
            return View();
        }
        public async Task<ActionResult> Ingresar(LoginViewModel login, string ReturnUrl )
        {
            bool result = false;
            string Controlador = "AccesoView";
            cat_sistemas usuario = db.cat_sistemas.FirstOrDefault(x => x.username == login.Usuario);
            //result = await AutenticateUser(login);
            //ViewBag.HiddenMenu = 1;
            if (usuario == null)
            {
                cat_sistemas.FakeHash();
            }
            else
            {
                result = await CheckPassword(usuario, login.Password);

                if (usuario.estatus_ID == 2)
                {
                    result = false;
                }
            }
            if(result == false && usuario != null)
            {
                if(usuario.estatus_ID == 2)
                {
                    ModelState.AddModelError("Password", "El Usuario esta inactivo.");
                }
                //else if (usuario.perfil_ID == 2)
                //{
                //    ModelState.AddModelError("Password", "el Usuario no tiene acceso");
                //}
                else
                {
                    
                    ModelState.AddModelError("Password", "La contraseña es incorrecta.");
                }
            }

            if (usuario == null)
            {
                ModelState.AddModelError("Password", "El usuario o contraseña son incorrectos.");
            }

            if (!ModelState.IsValid)
            {
                return View("Index",login);
            }

            FormsAuthentication.SetAuthCookie(login.Usuario,true);
            if(!string.IsNullOrWhiteSpace(ReturnUrl))
            {
                return RedirectToAction(ReturnUrl);
            }
            if(result)
            {
                Controlador = "PanelView";
            }
            return RedirectToAction("Index", Controlador);
        }
        public  Task<bool> CheckPassword(cat_sistemas usuario, string password)
        {
            bool response = usuario.CheckPassword(password);
            return Task.Factory.StartNew(() => { return response; });
        }
        public bool SendMail(cat_sistemas usuario)
        {
            bool success = false;
            string Remitente = ConfigurationManager.AppSettings["Remitente"].ToString();
            string Destinatario = usuario.username;
            string Contrasenia = ConfigurationManager.AppSettings["Contrasenia"].ToString();
            string Host = ConfigurationManager.AppSettings["Host"].ToString();
            int Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"].ToString());
            bool Ssl = Convert.ToBoolean(ConfigurationManager.AppSettings["Ssl"].ToString());
            string fecha = string.Empty;
            string ruta = string.Empty;
            string html = string.Empty;

            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(DateTime.Now.ToString("dd/MM/yyyy hh:mm"));
            fecha = Convert.ToBase64String(encryted);
            string path = ConfigurationManager.AppSettings["path"].ToString();

            ruta = path + "/AccesoView/Recuperar/?op2=" + fecha + "&op1=" + usuario.sistemas_ID;

            MailMessage msg = new MailMessage();

            msg.From = new MailAddress(Remitente, "SIRE");
            msg.To.Add(Destinatario);
            msg.Subject = "Restablecimiento de contraseña de la cuenta en SIRE";
            msg.Priority = MailPriority.Normal;
            msg.IsBodyHtml = true;

            html += "<div> <div style=\"background-color:#FF8C00\"><img src='cid:imagen' width=\"100\"/></div>";
            html += "<div>";
            html += "<p> Hola " + usuario.nombre + ", </p>";
            html += "<p>Nos ha notificado que no recuerda su contraseña para ingresar a SIRE.</p>";
            html += "<p>Para cambiar su contraseña dé click en el siguiente enlace.<p>";
            html += "</br>";
            html += "<a href='" + ruta + "' target='_blank'>Restablecer Contraseña</a>";
            html += "</br>";
            html += "<p>El enlace caducará en 24 hrs., así que asegúrese de utilizarlo inmediatamente.</p>";
            html += "</br>";
            html += "<p>¡Gracias por utilizar SIRE!</p>";
            html += "<hr style=\"color:#FF8C00;\"/>";
            html += "<i style=\"color:#FF8C00;\">&copy; Todos los derechos reservados | SIRE " + DateTime.Now.Year + "</i>";
            html += "</br>";
            html += "</br>";
            html += "</div>";

            AlternateView html2 = AlternateView.CreateAlternateViewFromString(html, null, "text/html");
            LinkedResource logo = new LinkedResource(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Sistema/Logo.png"));
            logo.ContentId = "imagen";
            html2.LinkedResources.Add(logo);
            msg.AlternateViews.Add(html2);

            SmtpClient client = new SmtpClient();
            client.Host = Host;
            client.Port = Port;
            client.EnableSsl = Ssl;
            NetworkCredential myCreds = new NetworkCredential(Remitente, Contrasenia);
            client.Credentials = myCreds;
            client.Send(msg);

            success = true;

            return success;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Recuperar(LoginViewModel login)
        {
            bool success = false;
            string mensaje = "";
            string correo = login.Usuario;
            cat_sistemas usuarios = new cat_sistemas();

            if (!string.IsNullOrEmpty(correo))
            {
                // la consulta debe ser SELECT desMedioContacto FROM [dbo].[MediosContactos] WHERE idPersona= (SELECT idPersona from UsuariosPersonas WHERE UsuariosPersonas.nbUsuario = 'variable')
                usuarios = db.cat_sistemas.FirstOrDefault(x => x.username == correo);
            }

            if (usuarios == null)
            {
                mensaje = "El correo electrónico ingresado no se encuentra registrado en el sistema";
            }
            else
            {
                success=SendMail(usuarios);
                if (success)
                {
                    mensaje = "Se ha enviado un correo electrónico con un enlace de recuperación";
                }
            }

            return Json(new { success = success, mensaje }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Recuperar(long? op1)
        {
            try
            {
                var usuario = db.cat_sistemas.Find(op1);
                if (usuario != null)
                {
                    String fecha_Registro = Request.QueryString.Get(0);
                    string result = string.Empty;
                    byte[] decryted = Convert.FromBase64String(fecha_Registro);
                    result = System.Text.Encoding.Unicode.GetString(decryted);
                    DateTime dateTimeUrl = DateTime.Parse(result);
                    dateTimeUrl.ToString("dd/MM/yyyy hh:mm");
                    DateTime dateTimeNow = DateTime.Parse(DateTime.Now.ToString());
                    TimeSpan diferencia = dateTimeNow - dateTimeUrl;

                    if (diferencia.Hours < 24 && diferencia.Days < 1)
                    {
                        ViewBag.Id = usuario.sistemas_ID;
                        ViewBag.usuario = usuario.username;
                        return View("Confirmar", new SetPasswordViewModel());
                    }
                }
                return HttpNotFound();
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
        }
        public ActionResult Confirmar(SetPasswordViewModel SetPasswordViewModel, long? op1)
        {
            if (SetPasswordViewModel != null && SetPasswordViewModel.NewPassword != null)
            {
                bool success = false;
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        cat_sistemas user = db.cat_sistemas.Find(op1);
                        if (user != null)
                        {

                           

                            if (!user.CheckPassword(SetPasswordViewModel.NewPassword))
                            {

                                user.SetPassword(SetPasswordViewModel.NewPassword);
                                db.Entry(user).State = EntityState.Modified;
                                if (db.SaveChanges() > 0)
                                {
                                    success = true;
                                    transaction.Commit();
                                }
                            }
                            else
                            {
                                success = false;
                            }

                        }
                    }
                    catch (Exception exp)
                    {
                        transaction.Rollback();
                        Console.WriteLine(exp.InnerException);
                    }
                }
                return Json(new { success = success }, JsonRequestBehavior.AllowGet);
            }
            return View("Confirmar", SetPasswordViewModel);
        }

        public ActionResult salir()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "AccesoView");
        }
    }
}