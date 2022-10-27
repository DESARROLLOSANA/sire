//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using System.Data.Entity.Validation;
//using CRME.Models;
//using PagedList;
//using System.Web.Script.Serialization;
//using System.IO;

//namespace CRME.Controllers
//{
//    public class UsuarioViewController : Controller
//    {
//        private SIRE_Context db = new SIRE_Context();
//        public ActionResult Index()
//        {
//            if (!User.Identity.IsAuthenticated)
//            {
//                return RedirectToAction("Index", "AccesoView");
//            }

//            return View();
//        }
//        public ActionResult SaveUsuarios(UsuariosPersonas usuario)
//        {
//            UsuariosPersonas usu = new UsuariosPersonas();
//            var serializerCat = new JavaScriptSerializer();
//            bool success = false;
//            string mensajefound = "";
//            var found = db.UsuariosPersonas.FirstOrDefault(x => x.nbUsuario == usuario.nbUsuario && x.idUsuario != usuario.idUsuario);

//            if (found != null)
//            {
//                mensajefound = "¡Ya existe un usuario que coincide con el correo ingresado!";

//            }
//            else
//            {
//                if (usuario.idUsuario == 0)
//                {
//                    try
//                    {
//                        Personas personas = new Personas();
//                        UsuariosPersonas Usuario = new UsuariosPersonas();

//                        //Personas
//                        personas.nbCompleto = usuario.Personas.nbCompleto;
//                        personas.idTipoPersona = 1;
//                        personas.idGenero = usuario.Personas.idGenero;
//                        var pathCat = serializerCat.Deserialize<string>(usuario.Personas.pathFoto);
//                        personas.pathFoto = "~/Upload/Usuarios/" + pathCat;
//                        //personas.idEstatus = usuario.Personas.idEstatus;


//                        //Usuarios
//                        Usuario.idPerfil = 1;
//                        Usuario.SetPassword(usuario.pwdContrasenia);
//                        Usuario.feVigencia = usuario.feVigencia;
//                        Usuario.feRegistro = DateTime.Now;
//                        Usuario.idEstatus = usuario.idEstatus;
//                        Usuario.nbUsuario = usuario.nbUsuario;
//                        Usuario.idPersona = personas.idPersona;
//                        db.Personas.Add(personas);
//                        db.UsuariosPersonas.Add(Usuario);

//                        if (db.SaveChanges() > 0)
//                        {
//                            success = true;
//                        }
//                    }
//                    catch (DbEntityValidationException ex)
//                    {
//                        var errorMessages = ex.EntityValidationErrors
//                        .SelectMany(x => x.ValidationErrors)
//                        .Select(x => x.ErrorMessage);

//                        // Join the list to a single string.
//                        var fullErrorMessage = string.Join("; ", errorMessages);

//                        // Combine the original exception message with the new one.
//                        var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

//                        // Throw a new DbEntityValidationException with the improved exception message.
//                        //throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

//                        mensajefound = exceptionMessage + "fatal error";

//                    }

//                }

//                else
//                {

//                    try
//                    {
//                        UsuariosPersonas Usuario = db.UsuariosPersonas.Find(usuario.idUsuario);
//                        Personas personas = new Personas();
//                        personas = db.Personas.Find(Usuario.Personas.idPersona);

//                        personas.nbCompleto = usuario.Personas.nbCompleto;
//                        personas.idTipoPersona = 1;
//                        personas.idGenero = usuario.Personas.idGenero;
//                        if (usuario.Personas.pathFoto == personas.pathFoto)
//                        {
//                            personas.pathFoto = personas.pathFoto;
//                        }
//                        else
//                        {
//                            var pathCat = serializerCat.Deserialize<string>(usuario.Personas.pathFoto);
//                            personas.pathFoto = "~/Upload/Usuarios/" + pathCat;
//                        }

//                        //int lengthfoto1 = personas.pathFoto.Length;
//                        //var quitar1 = personas.pathFoto.Remove(1, lengthfoto1 - 1);
//                        //if (quitar1 == "~")
//                        //{
//                        //    personas.pathFoto = personas.pathFoto;
//                        //}
//                        //else
//                        //{


//                        //}
//                        //personas.pathFoto = usuario.Personas.pathFoto;
//                        //personas.idEstatus = usuario.Personas.idEstatus;


//                        //Usuarios
//                        Usuario.idPerfil = 1;
//                        if (usuario.pwdContrasenia != "QWERTY123*")
//                        {
//                            Usuario.SetPassword(usuario.pwdContrasenia);
//                        }
//                        Usuario.feVigencia = usuario.feVigencia;
//                        Usuario.feRegistro = DateTime.Now;
//                        Usuario.idEstatus = usuario.idEstatus;
//                        Usuario.nbUsuario = usuario.nbUsuario;
//                        db.Entry(personas).State = EntityState.Modified;
//                        db.Entry(Usuario).State = EntityState.Modified;
//                        if (db.SaveChanges() > 0)
//                        {
//                            success = true;
//                        }
//                    }
//                    catch (DbEntityValidationException ex)
//                    {
//                        var errorMessages = ex.EntityValidationErrors
//                       .SelectMany(x => x.ValidationErrors)
//                       .Select(x => x.ErrorMessage);

//                        // Join the list to a single string.
//                        var fullErrorMessage = string.Join("; ", errorMessages);

//                        // Combine the original exception message with the new one.
//                        var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

//                        // Throw a new DbEntityValidationException with the improved exception message.
//                        //throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
//                        mensajefound = exceptionMessage + "fatal error";
//                    }
//                }

//            }

//            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);

//        }
//        public ActionResult _Formulario(long? idUsuario)
//        {
//            //codigo para agregar y editar usuarios
//            //UsuariosPersonas Personas = new UsuariosPersonas();
//            UsuariosPersonas Usuarios = new UsuariosPersonas();
//            Personas Persona = new Personas();

//            if (idUsuario != null)
//            {

//                ViewBag.edit = 1;
//                Usuarios = db.UsuariosPersonas.Find(idUsuario);

//                Usuarios.pwdContrasenia = "QWERTY123*";
//                ViewBag.idEstatus = new SelectList(db.Estatus.Where(x => x.desClasificacion == "GENERAL").ToList(), "idEstatus", "desEstatus", Usuarios.idEstatus);
//                ViewBag.idGenero = new SelectList(db.CatGeneros.ToList(), "idGenero", "nbGenero", Usuarios.Personas.idGenero);
//            }
//            else
//            {
//                ViewBag.idEstatus = new SelectList(db.Estatus.Where(x => x.desClasificacion == "GENERAL").ToList(), "idEstatus", "desEstatus");
//                ViewBag.idGenero = new SelectList(db.CatGeneros.ToList(), "idGenero", "nbGenero");
//                //ViewBag.idPerfil = new SelectList(db. "idPerfil", "nbPerfil");
//            }


//            return PartialView(Usuarios);
//        }
//        public ActionResult _TablaUsuarios(int? page, string filtroBusqueda)
//        {
//            const int pageSize = 10;
//            int pageNumber = (page ?? 1);

//            //obtener lista de usuarios
//            List<UsuariosPersonas> Personas = new List<UsuariosPersonas>();

//            Personas = db.UsuariosPersonas.Include(x => x.Personas).OrderBy(x => x.Personas.nbCompleto).ToList();
//            if (!string.IsNullOrEmpty(filtroBusqueda))
//            {
//                Personas = Personas.Where(x => x.Personas.nbCompleto.ToUpper().Contains(filtroBusqueda.ToUpper().Trim()) || x.Personas.nbCompleto.ToUpper().Contains(filtroBusqueda.ToUpper().Trim())).ToList();
//            }
//            ViewBag.filtro = filtroBusqueda;
//            return PartialView(Personas.ToPagedList(pageNumber, pageSize));
//        }
//        public ActionResult DeleteUsuario(long? idUsuario)
//        {
//            bool success = false; ;
//            string mensajefound = "";

//            try
//            {
//                var Usuario = db.UsuariosPersonas.Find(idUsuario);
//                var Personas = db.Personas.Find(Usuario.idPersona);
//                //var MediosContacto = db.MediosContactos.Find(Personas.MediosContactos);

//                db.Entry(Usuario).State = EntityState.Deleted;
//                db.Entry(Personas).State = EntityState.Deleted;
//                //db.Entry(MediosContacto).State = EntityState.Deleted;

//                if (db.SaveChanges() > 0)
//                {
//                    success = true;
//                }
//            }
//            catch (Exception ex)
//            {
//                mensajefound = "Ocurrio un error al eliminar la regla";
//            }


//            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);
//        }
//        public JsonResult CargarLogo()
//        {
//            bool success = false;
//            string error = "";
//            var savedFileNameDownload = "";
//            string savedFileName = "";
//            string completeName = "";
//            try
//            {
//                foreach (string file in Request.Files)
//                {

//                    HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
//                    string name = Path.GetRandomFileName();
//                    string extension = Path.GetExtension(Path.GetFileName(hpf.FileName));
//                    completeName = name + extension;
//                    savedFileName = Path.Combine(Server.MapPath("~/Upload/Usuarios/"), completeName);
//                    hpf.SaveAs(savedFileName);
//                    success = true;

//                }
//            }
//            catch (Exception ex)
//            {
//                error = "Archivo Invalido, Error al procesar el archivo";
//            }

//            return Json(new { success = success, error = error, savedFileName = completeName }, JsonRequestBehavior.AllowGet);
//        }
//        public ActionResult EliminarLogo(string path)
//        {
//            bool success = false;
//            try
//            {
//                var serializer = new JavaScriptSerializer();
//                var pathh = serializer.Deserialize<string>(path);
//                var rutapath = "~/Upload/Usuarios/" + pathh;
//                if (System.IO.File.Exists(Server.MapPath(rutapath)))
//                {
//                    try
//                    {
//                        System.IO.File.Delete(Server.MapPath(rutapath));
//                        success = true;
//                    }
//                    catch (System.IO.IOException e)
//                    {
//                        Console.WriteLine(e.Message);
//                    }

//                }

//            }
//            catch (Exception exp)
//            {
//                ViewBag.ResultMessage = "Error occured." + exp;
//            }
//            return Json(new { success = success }, JsonRequestBehavior.AllowGet);
//        }
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}
