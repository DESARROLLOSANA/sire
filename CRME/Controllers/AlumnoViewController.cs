//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Net.Mail;
//using System.Configuration;
//using System.Web;
//using System.Web.Mvc;
//using System.Web.Security;
//using CRME.Models;
//using PagedList;
//using System.Threading.Tasks;
//using System.IO;
//using CRME.Helpers;
//using System.Web.Script.Serialization;


//namespace CRME.Controllers
//{
//    public class AlumnoViewController : Controller
//    {
//        private CRME_Context db = new CRME_Context();
//        HelpersController helper = new HelpersController();
//        public ActionResult Index()
//        {
//            if (!User.Identity.IsAuthenticated)
//            {
//                return RedirectToAction("Index", "AccesoView");
//            }
//            return View();
//        }
//        public ActionResult _TablaAlumnos( string filtroBusqueda, string filtrobusquedaFe)
//        {
//            try
//            {
                
//                if (filtroBusqueda != null && filtroBusqueda != "" && filtrobusquedaFe != null && filtrobusquedaFe != "")
//                {
//                    var fecha = Convert.ToDateTime(filtrobusquedaFe);
//                    var Alumno = (from a in db.Alumnos
//                                  join r in db.RelAlumnosMatriculas on a.idAlumno equals r.idAlumno
//                                  join fa in db.FormacionesAcademicas on a.idAlumno equals fa.idAlumno
//                                  where (a.Personas.nbCompleto.ToUpper().Contains(filtroBusqueda.Trim())
//                        || r.noMatricula.ToUpper().Contains(filtroBusqueda.Trim())
//                            || fa.Licenciaturas.nbLicenciatura.ToUpper().Contains(filtroBusqueda.Trim())) && a.feActualizacion <= fecha
//                                  select a).Distinct().OrderBy(x => x.Personas.nbCompleto);

//                    ViewBag.filtro = filtroBusqueda;
//                    ViewBag.filtrofe = filtrobusquedaFe;
//                    return PartialView(Alumno.ToList());
//                }
//                else
//                {
//                    if (filtroBusqueda != null && filtroBusqueda !="")
//                    {
//                        var Alumno = (from a in db.Alumnos
//                                      join r in db.RelAlumnosMatriculas on a.idAlumno equals r.idAlumno
//                                      join fa in db.FormacionesAcademicas on a.idAlumno equals fa.idAlumno
//                                      where (a.Personas.nbCompleto.ToUpper().Contains(filtroBusqueda.Trim())
//                            || r.noMatricula.ToUpper().Contains(filtroBusqueda.Trim())
//                                || fa.Licenciaturas.nbLicenciatura.ToUpper().Contains(filtroBusqueda.Trim()))
//                                      select a).Distinct().OrderBy(x => x.Personas.nbCompleto);

//                        ViewBag.filtro = filtroBusqueda;
//                        ViewBag.filtrofe = filtrobusquedaFe;
//                        return PartialView(Alumno.ToList());
//                    }
//                    else
//                    {
//                        if(filtrobusquedaFe != null && filtrobusquedaFe !="")
//                        {
//                            var fecha = Convert.ToDateTime(filtrobusquedaFe);
//                            var Alumno = (from a in db.Alumnos
//                                          join r in db.RelAlumnosMatriculas on a.idAlumno equals r.idAlumno
//                                          join fa in db.FormacionesAcademicas on a.idAlumno equals fa.idAlumno
//                                          where a.feActualizacion <= fecha
//                                          select a).Distinct().OrderBy(x => x.Personas.nbCompleto);

//                            ViewBag.filtro = filtroBusqueda;
//                            ViewBag.filtrofe = filtrobusquedaFe;
//                            return PartialView(Alumno.ToList());
//                        }
//                        else
//                        {
//                            var Alumno = (from a in db.Alumnos
//                                          join r in db.RelAlumnosMatriculas on a.idAlumno equals r.idAlumno
//                                          join fa in db.FormacionesAcademicas on a.idAlumno equals fa.idAlumno
//                                          select a).Distinct().OrderBy(x => x.Personas.nbCompleto);


//                            return PartialView(Alumno.ToList());
//                        }
                        
//                    }

//                }
//                return PartialView();
//            }
//            catch
//            {
//                return PartialView();
//            }
            
//        }
//        public ActionResult EditarAlumno(long tg)
//        {
//            try
//            {

//                var Alumno = db.Alumnos.Find(tg);
//                AlumnoViewModel AlumnoViewModel = new AlumnoViewModel();
//                string valida = Request.QueryString.Keys[0].ToString();
//                string result = string.Empty;

//                if (valida != "idA2")
//                {
//                    if (!User.Identity.IsAuthenticated)
//                    {
//                        return RedirectToAction("Index", "AccesoView");
//                    }

//                    AlumnoViewModel.Alumnos = Alumno;
//                    AlumnoViewModel.MediosContactos = Alumno.Personas.MediosContactos.Where(x => x.idTipoMedioContacto == 2).ToList();
//                    AlumnoViewModel.MediosContactos1 = Alumno.Personas.MediosContactos.Where(x => x.idTipoMedioContacto == 1).ToList();
//                    ViewBag.idEstado = new SelectList(db.CatEstados.ToList(), "nbEstado", "nbEstado");
//                    ViewBag.idPais = new SelectList(db.CatPaises.ToList(), "nbPais", "nbPais");
//                    if (AlumnoViewModel.Alumnos.Direcciones == null)
//                    {
//                        AlumnoViewModel.Alumnos.Direcciones = new Direcciones();
//                    }
//                    var numCodigoPostal = AlumnoViewModel.Alumnos.Direcciones.numCodigoPostal;
//                    var nbEstado = AlumnoViewModel.Alumnos.Direcciones.nbEstado;
//                    var nbCiudad = (from c in db.CatCodigosPostales where c.numCodigoPostal == numCodigoPostal select new { nbCiudad = c.nbMunicipio, nbEstado = c.nbEstado }).Distinct().ToList();
//                    var nbColonia = (from c in db.CatCodigosPostales where c.numCodigoPostal == numCodigoPostal select new { nbCiudad = c.nbMunicipio, nbEstado = c.nbEstado, nbColonia = c.nbAsentamiento }).Distinct().ToList();
//                    if (!string.IsNullOrEmpty(nbEstado))
//                    {
//                        nbCiudad = nbCiudad.Where(e => e.nbEstado == nbEstado).ToList();
//                    }
//                    if (!string.IsNullOrEmpty(nbEstado))
//                    {
//                        nbColonia = nbColonia.Where(e => e.nbEstado == nbEstado).ToList();
//                    }

//                    ViewBag.idCiudad = new SelectList(nbCiudad, "nbCiudad", "nbCiudad");
//                    ViewBag.idColonia = new SelectList(nbColonia, "nbColonia", "nbColonia");
//                    ViewBag.idAlumno = tg;
//                }
//                else
//                {
//                    String fecha_Registro = Request.QueryString.Get(0);
//                    byte[] decryted = Convert.FromBase64String(fecha_Registro);
//                    result = System.Text.Encoding.Unicode.GetString(decryted);
//                    DateTime dateTimeUrl = DateTime.Parse(result);
//                    DateTime dateTimeNow = DateTime.Parse(DateTime.Now.ToString());
//                    TimeSpan diferencia = dateTimeNow - dateTimeUrl;

//                    if (diferencia.Hours < 24 && diferencia.Days < 1)
//                    {

//                        AlumnoViewModel.Alumnos = Alumno;
//                        AlumnoViewModel.MediosContactos = Alumno.Personas.MediosContactos.Where(x => x.idTipoMedioContacto == 2).ToList();
//                        AlumnoViewModel.MediosContactos1 = Alumno.Personas.MediosContactos.Where(x => x.idTipoMedioContacto == 1).ToList();
//                        ViewBag.idEstado = new SelectList(db.CatEstados.ToList(), "nbEstado", "nbEstado");
//                        ViewBag.idPais = new SelectList(db.CatPaises.ToList(), "nbPais", "nbPais");
//                        if (AlumnoViewModel.Alumnos.Direcciones == null)
//                        {
//                            AlumnoViewModel.Alumnos.Direcciones = new Direcciones();
//                        }
//                        var numCodigoPostal = AlumnoViewModel.Alumnos.Direcciones.numCodigoPostal;
//                        var nbEstado = AlumnoViewModel.Alumnos.Direcciones.nbEstado;
//                        var nbCiudad = (from c in db.CatCodigosPostales where c.numCodigoPostal == numCodigoPostal select new { nbCiudad = c.nbMunicipio, nbEstado = c.nbEstado }).Distinct().ToList();
//                        var nbColonia = (from c in db.CatCodigosPostales where c.numCodigoPostal == numCodigoPostal select new { nbCiudad = c.nbMunicipio, nbEstado = c.nbEstado, nbColonia = c.nbAsentamiento }).Distinct().ToList();
//                        if (!string.IsNullOrEmpty(nbEstado))
//                        {
//                            nbCiudad = nbCiudad.Where(e => e.nbEstado == nbEstado).ToList();
//                        }
//                        if (!string.IsNullOrEmpty(nbEstado))
//                        {
//                            nbColonia = nbColonia.Where(e => e.nbEstado == nbEstado).ToList();
//                        }

//                        ViewBag.idCiudad = new SelectList(nbCiudad, "nbCiudad", "nbCiudad");
//                        ViewBag.idColonia = new SelectList(nbColonia, "nbColonia", "nbColonia");
//                        ViewBag.idAlumno = tg;

//                        return View(AlumnoViewModel);
//                    }
//                    else
//                    {
//                        return View("Mensaje");
//                    }   
//                }
//                return View(AlumnoViewModel);
//            }
//            catch
//            {
//                return View("Mensaje");
//            }
            
//        }
//        public ActionResult Redireccion()
//        {
//            return View("Redireccion");
//        }
//        public ActionResult SaveDatosContacto(AlumnoViewModel AlumnoViewModel)
//        {
//            bool success = false;
//            using (var transaction = db.Database.BeginTransaction())
//            {
//                try
//                {
//                    var Alumno = db.Alumnos.Find(AlumnoViewModel.Alumnos.idAlumno);

//                    if (Alumno.Direcciones == null)
//                    {
//                        Alumno.Direcciones = new Direcciones();
//                    }
//                    Alumno.Direcciones.desDireccion = AlumnoViewModel.Alumnos.Direcciones.desDireccion;
//                    Alumno.Direcciones.numCodigoPostal = AlumnoViewModel.Alumnos.Direcciones.numCodigoPostal;
//                    Alumno.Direcciones.nbCiudad = AlumnoViewModel.Alumnos.Direcciones.nbCiudad;
//                    Alumno.Direcciones.nbEstado = AlumnoViewModel.Alumnos.Direcciones.nbEstado;
//                    Alumno.Direcciones.nbPais = AlumnoViewModel.Alumnos.Direcciones.nbPais;
//                    Alumno.Direcciones.nbColonia = AlumnoViewModel.Alumnos.Direcciones.nbColonia;

//                    //eliminamos todos los registros
//                    db.MediosContactos.RemoveRange(Alumno.Personas.MediosContactos);

//                    foreach (var item in AlumnoViewModel.MediosContactos)
//                    {
//                        MediosContactos md = new MediosContactos();
//                        md.idPersona = Alumno.Personas.idPersona;
//                        md.idTipoMedioContacto = 2;
//                        md.desMedioContacto = item.desMedioContacto;
//                        md.tpFormato = item.tpFormato;
//                        md.numExtension = item.numExtension;

//                        Alumno.Personas.MediosContactos.Add(md);
//                    }

//                    foreach (var item in AlumnoViewModel.MediosContactos1)
//                    {
//                        MediosContactos md1 = new MediosContactos();
//                        md1.idPersona = Alumno.Personas.idPersona;
//                        md1.idTipoMedioContacto = 1;
//                        md1.desMedioContacto = item.desMedioContacto;
//                        md1.tpFormato = item.tpFormato;

//                        Alumno.Personas.MediosContactos.Add(md1);
//                    }

//                    db.Entry(Alumno).State = EntityState.Modified;


//                    if (db.SaveChanges() > 0)
//                    {
//                        transaction.Commit();
//                        success = true;

//                    }
//                }
//                catch (Exception ex)
//                {
//                    transaction.Rollback();
//                    ViewBag.ResultMessage = "Error occured, records rolledback." + ex;
//                }
//            }
//            return Json(new { success = success }, JsonRequestBehavior.AllowGet);
//        }
//        //-------- formacion academica -----------------
//        public ActionResult _TablaFormacionAcademica(long? idAlumno, int? page)
//        {
//            int pageSize = 2;
//            int pageNumber = (page ?? 1);
//            var ListaFormacionAcademica = db.FormacionesAcademicas.Where(x => x.idAlumno == idAlumno).OrderBy(x=> x.NivelEstudios.desNivelEstudio).ToList().ToPagedList(pageNumber, pageSize);
//            ViewBag.idAlumno = idAlumno;
//            return PartialView("_TablaFormacionacademica", ListaFormacionAcademica);

//        }
//        public ActionResult DeleteFormacionAcademica(long idFormacionAcademica)
//        {
//            bool success = false;
//            var FormacionAcademica = db.FormacionesAcademicas.Find(idFormacionAcademica);
//            try
//            {
//                db.Entry(FormacionAcademica).State = EntityState.Deleted;
//                db.SaveChanges();
//                success = true;
//            }
//            catch (Exception exp)
//            {
//                ViewBag.ResultMessage = "Error occured." + exp;
//            }
//            return Json(new { success = success }, JsonRequestBehavior.AllowGet);
//        }
//        public ActionResult SaveFormacionAcademica(FormacionesAcademicas FormacionesAcademica, long idAlumno)
//        {
//            bool success = false;
//            string mensajefound = "";
//            try
//            {
//                /*codigo*/
//                var found = db.FormacionesAcademicas.FirstOrDefault(E => E.idNivelEstudio == FormacionesAcademica.idNivelEstudio && E.idLicenciatura == FormacionesAcademica.idLicenciatura && E.idAlumno == idAlumno && E.idFormacionAcademica != FormacionesAcademica.idFormacionAcademica);
//                if (found != null)
//                {
//                    mensajefound = "Ya tienes registrada una formación academica con esos datos!!";

//                }
//                else
//                {
//                    FormacionesAcademicas FormacionAcademicas = new FormacionesAcademicas();
//                    if (FormacionesAcademica.idFormacionAcademica == 0)
//                    {
//                        FormacionAcademicas.idAlumno = FormacionesAcademica.idAlumno;
//                        FormacionAcademicas.feInicio = FormacionesAcademica.feInicio;
//                        FormacionAcademicas.feTerminacion = FormacionesAcademica.feTerminacion;
//                        FormacionAcademicas.idNivelEstudio = FormacionesAcademica.idNivelEstudio;
//                        FormacionAcademicas.idLicenciatura = FormacionesAcademica.idLicenciatura;
//                        FormacionAcademicas.idColegio = FormacionesAcademica.idColegio;
//                        if (FormacionesAcademica.desComentarios == null)
//                        {
//                            FormacionesAcademica.desComentarios = "N/A";
//                        }
//                        else
//                        {
//                            FormacionesAcademica.desComentarios = FormacionesAcademica.desComentarios;
//                        }
//                        db.FormacionesAcademicas.Add(FormacionesAcademica);
//                    }
//                    else
//                    {
//                        FormacionAcademicas = db.FormacionesAcademicas.Find(FormacionesAcademica.idFormacionAcademica);
//                        FormacionAcademicas.idAlumno = FormacionesAcademica.idAlumno;
//                        FormacionAcademicas.feInicio = FormacionesAcademica.feInicio;
//                        FormacionAcademicas.feTerminacion = FormacionesAcademica.feTerminacion;
//                        FormacionAcademicas.idNivelEstudio = FormacionesAcademica.idNivelEstudio;
//                        FormacionAcademicas.idLicenciatura = FormacionesAcademica.idLicenciatura;
//                        FormacionAcademicas.idColegio = FormacionesAcademica.idColegio;


//                        if (FormacionAcademicas.desComentarios == null)
//                        {
//                            FormacionAcademicas.desComentarios = "N/A";
//                        }
//                        else
//                        {
//                            FormacionAcademicas.desComentarios = FormacionesAcademica.desComentarios;
//                        }
//                        db.Entry(FormacionAcademicas).State = EntityState.Modified;
//                    }
//                }

//                if (db.SaveChanges() > 0)
//                {
//                    success = true;
//                }
//            }
//            catch (Exception ex)
//            {
//                ViewBag.ResultMessage = "Error occured, records rolledback." + ex;
//            }

//            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);

//        }
//        public ActionResult _FormularioFormacionAcademica(long? idFormacionAcademica)
//        {
//            FormacionesAcademicas FormacionesAcademicas = new FormacionesAcademicas();
//            if (idFormacionAcademica != null)
//            {
//                FormacionesAcademicas = db.FormacionesAcademicas.Find(idFormacionAcademica);
//                ViewBag.idNivelEstudio = new SelectList(db.NivelEstudios.OrderBy(x => x.desNivelEstudio).ToList(), "idNivelEstudio", "desNivelEstudio", FormacionesAcademicas.idNivelEstudio);
//                ViewBag.idLicenciatura = new SelectList(db.Licenciaturas.OrderBy(x => x.nbLicenciatura).ToList(), "idLicenciatura", "nbLicenciatura", FormacionesAcademicas.idLicenciatura);
//                ViewBag.idColegio = new SelectList(db.Colegios.OrderBy(x => x.nbColegio).ToList(), "idColegio", "nbColegio", FormacionesAcademicas.idColegio);
//            }
//            else
//            {
//                ViewBag.idNivelEstudio = new SelectList(db.NivelEstudios.OrderBy(x=> x.desNivelEstudio).ToList(), "idNivelEstudio", "desNivelEstudio");
//                ViewBag.idLicenciatura = new SelectList(db.Licenciaturas.OrderBy(x=> x.nbLicenciatura).ToList(), "idLicenciatura", "nbLicenciatura");
//                ViewBag.idColegio = new SelectList(db.Colegios.OrderBy(x=> x.nbColegio).ToList(), "idColegio", "nbColegio");
//            }

//            //ViewBag.idTipoPuesto = new SelectList(db.CatTiposPuestos.ToList(), "descripcion", "descripcion");

//            return PartialView("_FormularioFormacionAcademica", FormacionesAcademicas);
//        }

//        //------------- formacion complementaria --------------------
//        public ActionResult _TablaFormacionComplementaria(long? idAlumno, int? page)
//        {

            

//            int pageSize = 2;
//            int pageNumber = (page ?? 1);
//            var ListaFormacionComplementaria = db.FormacionesComplementarias.Where(x => x.idAlumno == idAlumno).OrderBy(x=> x.CatTiposFormacionesComplementarias.desTipoFormacionComplementaria.ToUpper()).ToList().ToPagedList(pageNumber, pageSize);
//            ViewBag.idAlumno = idAlumno;
//            return PartialView("_TablaFormacionesComplementarias", ListaFormacionComplementaria);

//        }
//        public ActionResult DeleteFormacionComplementaria(long idFormacionComplementaria)
//        {
//            bool success = false;
//            var FormacionComplementaria = db.FormacionesComplementarias.Find(idFormacionComplementaria);
//            try
//            {
//                db.Entry(FormacionComplementaria).State = EntityState.Deleted;
//                db.SaveChanges();
//                success = true;
//            }
//            catch (Exception exp)
//            {
//                ViewBag.ResultMessage = "Error occured." + exp;
//            }
//            return Json(new { success = success }, JsonRequestBehavior.AllowGet);
//        }
//        public ActionResult SaveFormacionComplementaria(FormacionesComplementarias FormacionesComplementarias, long idAlumno)
//        {
//            bool success = false;
//            string mensajefound = "";
//            try
//            {
//                /*codigo*/
//                var found = db.FormacionesComplementarias.FirstOrDefault(E => E.nbFormacionComplementaria == FormacionesComplementarias.nbFormacionComplementaria && E.idAlumno == idAlumno && E.idFormacionComplementaria != FormacionesComplementarias.idFormacionComplementaria);
//                if (found != null)
//                {
//                    mensajefound = "Ya tienes registrada una formación complementaria con esos datos!!";

//                }
//                else
//                {
//                    FormacionesComplementarias formacionComplementaria = new FormacionesComplementarias();
//                    if (FormacionesComplementarias.idFormacionComplementaria == 0)
//                    {
//                        formacionComplementaria.idAlumno = FormacionesComplementarias.idAlumno;
//                        formacionComplementaria.nbFormacionComplementaria = FormacionesComplementarias.nbFormacionComplementaria;
//                        formacionComplementaria.feInicio = FormacionesComplementarias.feInicio;
//                        formacionComplementaria.feTerminacion = FormacionesComplementarias.feTerminacion;
//                        formacionComplementaria.idTipoFormacionComplementaria = FormacionesComplementarias.idTipoFormacionComplementaria;
//                        formacionComplementaria.numHoras = FormacionesComplementarias.numHoras;
//                        formacionComplementaria.idColegio = FormacionesComplementarias.idColegio;
//                        if (FormacionesComplementarias.desComentarios == null)
//                        {
//                            FormacionesComplementarias.desComentarios = "N/A";
//                        }
//                        else
//                        {
//                            FormacionesComplementarias.desComentarios = FormacionesComplementarias.desComentarios;
//                        }
//                        db.FormacionesComplementarias.Add(FormacionesComplementarias);
//                    }
//                    else
//                    {
//                        formacionComplementaria = db.FormacionesComplementarias.Find(FormacionesComplementarias.idFormacionComplementaria);
//                        formacionComplementaria.idAlumno = FormacionesComplementarias.idAlumno;
//                        formacionComplementaria.nbFormacionComplementaria = FormacionesComplementarias.nbFormacionComplementaria;
//                        formacionComplementaria.feInicio = FormacionesComplementarias.feInicio;
//                        formacionComplementaria.feTerminacion = FormacionesComplementarias.feTerminacion;
//                        formacionComplementaria.idTipoFormacionComplementaria = FormacionesComplementarias.idTipoFormacionComplementaria;
//                        formacionComplementaria.numHoras = FormacionesComplementarias.numHoras;
//                        formacionComplementaria.idColegio = FormacionesComplementarias.idColegio;


//                        if (formacionComplementaria.desComentarios == null)
//                        {
//                            formacionComplementaria.desComentarios = "N/A";
//                        }
//                        else
//                        {
//                            formacionComplementaria.desComentarios = FormacionesComplementarias.desComentarios;
//                        }
//                        db.Entry(formacionComplementaria).State = EntityState.Modified;
//                    }
//                }

//                if (db.SaveChanges() > 0)
//                {
//                    success = true;
//                }
//            }
//            catch (Exception ex)
//            {
//                ViewBag.ResultMessage = "Error occured, records rolledback." + ex;
//            }

//            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);

//        }
//        public ActionResult _FormularioFormacionComplementaria(long? idFormacionComplementaria)
//        {
//            FormacionesComplementarias FormacionesComplementarias = new FormacionesComplementarias();
//            if (idFormacionComplementaria != null)
//            {
//                FormacionesComplementarias = db.FormacionesComplementarias.Find(idFormacionComplementaria);
//                ViewBag.idTipoFormacionComplementaria = new SelectList(db.CatTiposFormacionesComplementarias.OrderBy(x => x.desTipoFormacionComplementaria).ToList(), "idTipoFormacionComplementaria", "desTipoFormacionComplementaria", FormacionesComplementarias.idTipoFormacionComplementaria);                
//                ViewBag.idColegio = new SelectList(db.Colegios.OrderBy(x => x.nbColegio).ToList(), "idColegio", "nbColegio", FormacionesComplementarias.idColegio);
//            }
//            else
//            {
//                var lista = db.CatTiposFormacionesComplementarias.OrderBy(x=> x.desTipoFormacionComplementaria).ToList();

//                ViewBag.idTipoFormacionComplementaria = new SelectList(lista, "idTipoFormacionComplementaria", "desTipoFormacionComplementaria");                
//                ViewBag.idColegio = new SelectList(db.Colegios.OrderBy(x=>x.nbColegio).ToList(), "idColegio", "nbColegio");
//            }

//            //ViewBag.idTipoPuesto = new SelectList(db.CatTiposPuestos.ToList(), "descripcion", "descripcion");

//            return PartialView("_FormularioFormacionComplementaria", FormacionesComplementarias);
//        }

//        //---------------- idiomas ---------------------------
//        public ActionResult _TablaIdiomas(long? idAlumno, int? page)
//        {



//            int pageSize = 2;
//            int pageNumber = (page ?? 1);
//            var ListaIdiomas = db.Idiomas.Where(x => x.idAlumno == idAlumno).ToList().ToPagedList(pageNumber, pageSize);
//            ViewBag.idAlumno = idAlumno;
//            return PartialView("_TablaIdiomas", ListaIdiomas);

//        }
//        public ActionResult SaveIdioma(Idiomas Idioma, long? idAlumno)
//        {
//            bool success = false;
//            string mensajefound = "";
//            try
//            {
//                var ListaIdiomas = db.Idiomas.ToList();
//                var found = ListaIdiomas.FirstOrDefault(i => helper.RemoveDiacritics(i.nbIdioma.Trim().ToUpper()) == helper.RemoveDiacritics(Idioma.nbIdioma.Trim().ToUpper()) && i.idAlumno == Idioma.idAlumno && i.idIdioma != Idioma.idIdioma);
//                if (found != null)
//                {
//                    mensajefound = "¡Ya ha registrado un idioma con ese nombre!";

//                }
//                else
//                {
//                    if (Idioma.idIdioma == 0)
//                    {
//                        Idiomas Idiomas = new Idiomas();
//                        Idiomas.nbIdioma = Idioma.nbIdioma;
//                        Idiomas.idAlumno = Idioma.idAlumno;
//                        Idiomas.idNivelLectura = Idioma.idNivelLectura;
//                        Idiomas.idNivelEscritura = Idioma.idNivelEscritura;
//                        Idiomas.idNivelConversacion = Idioma.idNivelConversacion;
//                        if (Idioma.desComentarios == null)
//                        {
//                            Idiomas.desComentarios = "N/A";
//                        }
//                        else
//                        {
//                            Idiomas.desComentarios = Idioma.desComentarios;
//                        }
//                        db.Idiomas.Add(Idiomas);
//                    }
//                    else
//                    {
//                        Idiomas Idiomas = db.Idiomas.Find(Idioma.idIdioma);
//                        Idiomas.nbIdioma = Idioma.nbIdioma;
//                        Idiomas.idAlumno = Idioma.idAlumno;
//                        Idiomas.idNivelLectura = Idioma.idNivelLectura;
//                        Idiomas.idNivelEscritura = Idioma.idNivelEscritura;
//                        Idiomas.idNivelConversacion = Idioma.idNivelConversacion;
//                        if (Idioma.desComentarios == null)
//                        {
//                            Idiomas.desComentarios = "N/A";
//                        }
//                        else
//                        {
//                            Idiomas.desComentarios = Idioma.desComentarios;
//                        }
//                        db.Entry(Idiomas).State = EntityState.Modified;
//                    }
//                }

//                if (db.SaveChanges() > 0)
//                {
//                    success = true;
//                }
//            }
//            catch (Exception ex)
//            {
//                ViewBag.ResultMessage = "Error occured, records rolledback." + ex;
//            }

//            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);

//        }
//        public ActionResult _FormularioIdioma(long? idIdioma)
//        {
//            Idiomas Idiomas = new Idiomas();
//            if (idIdioma != null)
//            {
//                Idiomas = db.Idiomas.Find(idIdioma);
//            }
//            ViewBag.Idiomas = new SelectList(db.CatIdiomas.OrderBy(x => x.nbIdioma).ToList(), "nbIdioma", "nbIdioma");
//            ViewBag.idNivelConocimiento = new SelectList(db.NivelesConocimientos.ToList(), "idNivelConocimiento", "nbNivelConocimiento");
//            return PartialView("_FormularioIdioma", Idiomas);
//        }
//        public ActionResult DeleteIdioma(long idIdioma)
//        {
//            bool success = false;
//            var Idioma = db.Idiomas.Find(idIdioma);
//            try
//            {
//                db.Entry(Idioma).State = EntityState.Deleted;
//                db.SaveChanges();
//                success = true;
//            }
//            catch (Exception exp)
//            {
//                ViewBag.ResultMessage = "Error occured." + exp;
//            }
//            return Json(new { success = success }, JsonRequestBehavior.AllowGet);
//        }

//        //------------ conocimientos ------------------------
//        public ActionResult _TablaConocimientosInformaticos(long? idAlumno, int? page)
//        {
//            int pageSize = 2;
//            int pageNumber = (page ?? 1);
//            var ListaConocimientosInformaticos = db.ConocimientosInformaticos.Where(x => x.idAlumno == idAlumno).ToList().ToPagedList(pageNumber, pageSize);
//            ViewBag.idAlumno = idAlumno;
//            return PartialView("_TablaConocimientosInformaticos", ListaConocimientosInformaticos);

//        }
//        public ActionResult SaveConocimientoInformatico(ConocimientosInformaticos ConocimientoInformatico, long? idAlumno)
//        {
//            bool success = false;
//            string mensajefound = "";
//            try
//            {
//                var ListaConocimientos = db.ConocimientosInformaticos.ToList();
//                var found = ListaConocimientos.FirstOrDefault(co => helper.RemoveDiacritics(co.tpSoftware.Trim().ToUpper()) == helper.RemoveDiacritics(ConocimientoInformatico.tpSoftware.Trim().ToUpper()) && co.idAlumno == ConocimientoInformatico.idAlumno && co.idConocimientoInformatico != ConocimientoInformatico.idConocimientoInformatico);
//                if (found != null)
//                {
//                    mensajefound = "¡Ya ha registrado un software con ese nombre!";

//                }
//                else
//                {
//                    ConocimientosInformaticos ConocimientosInformaticos = new ConocimientosInformaticos();
//                    if (ConocimientoInformatico.idConocimientoInformatico == 0)
//                    {
//                        ConocimientosInformaticos.idAlumno = ConocimientoInformatico.idAlumno;
//                        ConocimientosInformaticos.tpSoftware = ConocimientoInformatico.tpSoftware;
//                        ConocimientosInformaticos.idNivelConocimiento = ConocimientoInformatico.idNivelConocimiento;
//                        ConocimientosInformaticos.numAniosExperiencia = ConocimientoInformatico.numAniosExperiencia;
//                        if (ConocimientoInformatico.desComentarios == null)
//                        {
//                            ConocimientosInformaticos.desComentarios = "N/A";
//                        }
//                        else
//                        {
//                            ConocimientosInformaticos.desComentarios = ConocimientoInformatico.desComentarios;
//                        }
//                        db.ConocimientosInformaticos.Add(ConocimientosInformaticos);
//                    }
//                    else
//                    {
//                        ConocimientosInformaticos = db.ConocimientosInformaticos.Find(ConocimientoInformatico.idConocimientoInformatico);
//                        ConocimientosInformaticos.tpSoftware = ConocimientoInformatico.tpSoftware;
//                        ConocimientosInformaticos.idNivelConocimiento = ConocimientoInformatico.idNivelConocimiento;
//                        ConocimientosInformaticos.numAniosExperiencia = ConocimientoInformatico.numAniosExperiencia;
//                        if (ConocimientoInformatico.desComentarios == null)
//                        {
//                            ConocimientosInformaticos.desComentarios = "N/A";
//                        }
//                        else
//                        {
//                            ConocimientosInformaticos.desComentarios = ConocimientoInformatico.desComentarios;
//                        }
//                        db.Entry(ConocimientosInformaticos).State = EntityState.Modified;
//                    }
//                }
//                if (db.SaveChanges() > 0)
//                {
//                    success = true;
//                }
//            }
//            catch (Exception ex)
//            {
//                ViewBag.ResultMessage = "Error occured, records rolledback." + ex;
//            }

//            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);

//        }
//        public ActionResult _FormularioConocimientoInformatico(long? idConocimiento)
//        {
//            ConocimientosInformaticos ConocimientosInformaticos = new ConocimientosInformaticos();
//            if (idConocimiento != null)
//            {
//                ConocimientosInformaticos = db.ConocimientosInformaticos.Find(idConocimiento);
//                ViewBag.idNivelConocimiento = new SelectList(db.NivelesConocimientos.ToList(), "idNivelConocimiento", "nbNivelConocimiento", ConocimientosInformaticos.idConocimientoInformatico);
//            }
//            else
//            {
//                ViewBag.idNivelConocimiento = new SelectList(db.NivelesConocimientos.ToList(), "idNivelConocimiento", "nbNivelConocimiento");
//            }
//            return PartialView("_FormularioConocimientoInformatico", ConocimientosInformaticos);
//        }
//        public ActionResult DeleteConocimientoInformatico(long idConocimiento)
//        {
//            bool success = false;
//            var ConocimientosInformaticos = db.ConocimientosInformaticos.Find(idConocimiento);
//            try
//            {
//                db.Entry(ConocimientosInformaticos).State = EntityState.Deleted;
//                db.SaveChanges();
//                success = true;
//            }
//            catch (Exception exp)
//            {
//                ViewBag.ResultMessage = "Error occured." + exp;
//            }
//            return Json(new { success = success }, JsonRequestBehavior.AllowGet);
//        }

//        //---------------------- datos personales--------------
//        public ActionResult _FormularioDatosPersonales(long? tg)
//        {
//            Alumnos alumnos = new Alumnos();

//            Personas Persona = new Personas();
            
//            if (tg != null)
//            {

//                ViewBag.edit = 1;
//                alumnos = db.Alumnos.Find(tg);
//                ViewBag.idEstadoCivil = new SelectList(db.CatEstadosCiviles.ToList(), "idEstadoCivil", "nbEstadoCivil", alumnos.idEstadoCivil);
//                ViewBag.idReligion = new SelectList(db.CatReligion.ToList(), "idReligion", "nbReligion", alumnos.idReligion);
//                ViewBag.idGenero = new SelectList(db.CatGeneros.ToList(), "idGenero", "nbGenero", alumnos.Personas.idGenero);
//                ViewBag.idTipoSangre = new SelectList(db.CatTiposSangre.ToList(), "idTipoSangre", "nbTipoSangre", alumnos.idTipoSangre);               
               
//            }
//            else
//            {
//                ViewBag.idEstadoCivil = new SelectList(db.CatEstadosCiviles.ToList(), "idEstadoCivil", "nbEstadoCivil");
//                ViewBag.idReligion = new SelectList(db.CatReligion.ToList(), "idReligion", "nbReligion");
//                ViewBag.idGenero = new SelectList(db.CatGeneros.ToList(), "idGenero", "nbGenero");
//                ViewBag.tpEgresado = alumnos.tpEgresado;
//                ViewBag.fgDonarSangre = alumnos.fgDonarSangre;
                
//            }


//            return PartialView(alumnos);

           
//        }
//        public ActionResult SaveAlumno(Alumnos alumno)
//        {
            
            


//            bool success = false;
//            var serializerCat = new JavaScriptSerializer();
//            using (var transaction = db.Database.BeginTransaction())
//            {
//                try
//                {
//                    Alumnos Alumno = db.Alumnos.Find(alumno.idAlumno);
//                    Alumno.Personas.nbCompleto = alumno.Personas.nbCompleto;
//                    Alumno.Personas.idGenero = alumno.Personas.idGenero;
//                    Alumno.Personas.idTipoPersona = 1;
//                    if(alumno.Personas.pathFoto == Alumno.Personas.pathFoto )

//                    {
//                        Alumno.Personas.pathFoto = alumno.Personas.pathFoto;
//                    }
//                    else
//                    {
//                        var pathCat = serializerCat.Deserialize<string>(alumno.Personas.pathFoto);
//                        Alumno.Personas.pathFoto = "~/Upload/Egresados/" + pathCat;
//                    }                    
//                    Alumno.idReligion = alumno.idReligion;
//                    Alumno.idEstadoCivil = alumno.idEstadoCivil;
//                    Alumno.idTipoSangre = alumno.idTipoSangre;
//                    Alumno.tpEgresado = alumno.tpEgresado;
//                    Alumno.feActualizacion = DateTime.Today;
//                    Alumno.fgDonarSangre = alumno.fgDonarSangre;
//                    Alumno.fgFallecido = alumno.fgFallecido;
//                    Alumno.fgLicencia = alumno.fgLicencia;
//                    Alumno.fgTrabajaActualmente = alumno.fgTrabajaActualmente;

//                    db.Entry(Alumno).State = EntityState.Modified;

//                    if (db.SaveChanges() > 0)
//                    {
//                        transaction.Commit();
//                        success = true;

//                    }
//                }
//                catch(Exception ex)
//                {
//                    transaction.Rollback();
//                    ViewBag.ResultMessage = "Error occured, records rolledback." + ex;
//                }
//            }

//                return Json(new { success = success }, JsonRequestBehavior.AllowGet);
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
//                    savedFileName = Path.Combine(Server.MapPath("~/Upload/Egresados/"), completeName);
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
//        public ActionResult EnviarSolicitud(List<int> idAlumno)
//        {
//            bool success = false;
//            string msj = string.Empty;
//            Alumnos alum = new Alumnos();
//            string fecha;
//            foreach (var alumnno in idAlumno)
//            {
               
              
//                try
//                {
//                   var alumn = db.Alumnos.FirstOrDefault(x => x.idAlumno.Equals(alumnno));
//                    string path = ConfigurationManager.AppSettings["path"].ToString();
//                    byte[] encryted = System.Text.Encoding.Unicode.GetBytes(DateTime.Now.ToString());
//                    fecha = Convert.ToBase64String(encryted);


//                    //ruta = Url + path + "/Recuperar/Confirmar/?op2=" + fecha + "&op1=" + usuario.Up_IdUsuarioPersona;
//                    Correos Correos = new Correos();
//                    Correos.desAsunto = "Actualización de información";
//                    Correos.desTitulo = "Actualización de información personal";
//                    Correos.nbReceptor = alumn.Personas.MediosContactos.FirstOrDefault(x=> x.idTipoMedioContacto==1).desMedioContacto; 
//                    Correos.pathLogo = "http://anahuacmayab.mx/ctifimpes/img/logo-universidad-blanco.png";
//                    Correos.idEstatus = 1;
//                    Correos.pathLink = path + "/AlumnoView/EditarAlumno/?idA2=" + fecha + "&tg=" + alumnno;
//                    Correos.desColorCuerpo = "#9e9e9e";
//                    Correos.desColorHeader = "#ff9800";
//                    Correos.desColorTitulo = "#ffb74d";
//                    Correos.desMensaje = "Por medio de la presente se le informa a usted: " + alumn.Personas.nbCompleto + " que se requiere la actualización de su información personal, accediendo al enlace que se presenta en este correo, adicionalmente se le informa que el enlace tendrá una vigencia de 24 horas por lo que se le sugiere acceder lo antes posible. Por su atención gracias.";
//                    db.Correos.Add(Correos);
//                    //db.SaveChanges();
                   

//                    if (db.SaveChanges() > 0)
//                    {
//                        success = true;
//                        msj = "Correo enviado con éxito";
                        
//                    }
//                    else
//                    {
//                        msj = "Error al enviar el correo, Intente nuevamente o contacte al Administrador";
//                    }
//                }
//                catch (Exception e)
//                {
//                    Console.WriteLine(e.Message.ToString());
//                    msj = "Error al enviar el correo, Contacte al Administrador";
//                }
//            }

//            return Json(new { success = success, msj }, JsonRequestBehavior.AllowGet);
//        }
//        public ActionResult _TablaExperienciasProfesionales(long? idAlumno, int? page)
//        {
//            int pageSize = 2;
//            int pageNumber = (page ?? 1);
//            var ListaExperienciaProfesional = db.ExperienciasProfesionales.Where(x => x.idAlumno == idAlumno).ToList().ToPagedList(pageNumber, pageSize);
//            ViewBag.idAlumno = idAlumno;
//            return PartialView("_TablaExperienciasProfesionales", ListaExperienciaProfesional);
//        }
//        public ActionResult SaveExperienciaProfesional(ExperienciasProfesionales ExperienciaProfesional, long idAlumno)
//        {
//            bool success = false;
//            string mensajefound = "";
//            try
//            {
//                /*codigo*/
//                var found = db.ExperienciasProfesionales.FirstOrDefault(E => E.nbEmpresa == ExperienciaProfesional.nbEmpresa && E.nbPuesto == ExperienciaProfesional.nbPuesto && E.idAlumno == idAlumno && E.idExperienciaProfesional != ExperienciaProfesional.idExperienciaProfesional);
//                if (found != null)
//                {
//                    mensajefound = "¡Ya ha registrado una experiencia profesional con ese nombre!";

//                }
//                else
//                {
//                    ExperienciasProfesionales ExperienciasProfesionales = new ExperienciasProfesionales();
//                    if (ExperienciaProfesional.idExperienciaProfesional == 0)
//                    {
//                        ExperienciasProfesionales.idAlumno = ExperienciaProfesional.idAlumno;
//                        ExperienciasProfesionales.feInicioLaboral = ExperienciaProfesional.feInicioLaboral;
//                        ExperienciasProfesionales.feFinalLaboral = ExperienciaProfesional.feFinalLaboral;
//                        ExperienciasProfesionales.nbEmpresa = ExperienciaProfesional.nbEmpresa;
//                        ExperienciasProfesionales.nbPuesto = ExperienciaProfesional.nbPuesto;
//                        ExperienciasProfesionales.numPersonalCargo = ExperienciaProfesional.numPersonalCargo;
//                        ExperienciasProfesionales.tpPuesto = ExperienciaProfesional.tpPuesto;
//                        ExperienciasProfesionales.idArea = ExperienciaProfesional.idArea;
//                        ExperienciasProfesionales.idSector = ExperienciaProfesional.idSector;
//                        ExperienciasProfesionales.decSalario = ExperienciaProfesional.decSalario;
//                        ExperienciasProfesionales.idTipoEmpresa = ExperienciaProfesional.idTipoEmpresa;
//                        if (ExperienciaProfesional.desComentario == null)
//                        {
//                            ExperienciasProfesionales.desComentario = "N/A";
//                        }
//                        else
//                        {
//                            ExperienciasProfesionales.desComentario = ExperienciaProfesional.desComentario;
//                        }
//                        db.ExperienciasProfesionales.Add(ExperienciasProfesionales);
//                    }
//                    else
//                    {
//                        ExperienciasProfesionales = db.ExperienciasProfesionales.Find(ExperienciaProfesional.idExperienciaProfesional);
//                        ExperienciasProfesionales.idAlumno = ExperienciaProfesional.idAlumno;
//                        ExperienciasProfesionales.feInicioLaboral = ExperienciaProfesional.feInicioLaboral;
//                        ExperienciasProfesionales.feFinalLaboral = ExperienciaProfesional.feFinalLaboral;
//                        ExperienciasProfesionales.nbEmpresa = ExperienciaProfesional.nbEmpresa;
//                        ExperienciasProfesionales.nbPuesto = ExperienciaProfesional.nbPuesto;
//                        ExperienciasProfesionales.numPersonalCargo = ExperienciaProfesional.numPersonalCargo;
//                        ExperienciasProfesionales.tpPuesto = ExperienciaProfesional.tpPuesto;
//                        ExperienciasProfesionales.idArea = ExperienciaProfesional.idArea;
//                        ExperienciasProfesionales.idSector = ExperienciaProfesional.idSector;
//                        ExperienciasProfesionales.idTipoEmpresa = ExperienciaProfesional.idTipoEmpresa;
//                        ExperienciasProfesionales.decSalario = ExperienciaProfesional.decSalario;
//                        if (ExperienciaProfesional.desComentario == null)
//                        {
//                            ExperienciasProfesionales.desComentario = "N/A";
//                        }
//                        else
//                        {
//                            ExperienciasProfesionales.desComentario = ExperienciaProfesional.desComentario;
//                        }
//                        db.Entry(ExperienciasProfesionales).State = EntityState.Modified;
//                    }
//                }

//                if (db.SaveChanges() > 0)
//                {
//                    success = true;
//                }
//            }
//            catch (Exception ex)
//            {
//                ViewBag.ResultMessage = "Error occured, records rolledback." + ex;
//            }

//            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);

//        }
//        public ActionResult _FormularioExperienciaProfesional(long? idExperienciaProfesional)
//        {
//            ExperienciasProfesionales ExperienciasProfesionales = new ExperienciasProfesionales();
//            if (idExperienciaProfesional != null)
//            {
//                ExperienciasProfesionales = db.ExperienciasProfesionales.Find(idExperienciaProfesional);
//                ViewBag.idArea = new SelectList(db.CatAreas.OrderBy(x=>x.nbArea).ToList(), "idArea", "nbArea", ExperienciasProfesionales.idArea);
//                ViewBag.idSector = new SelectList(db.CatSectores.OrderBy(x=>x.nbSector).ToList(), "idSector", "nbSector", ExperienciasProfesionales.idSector);
//                ViewBag.idTipoEmpresa = new SelectList(db.CatTipoEmpresa.OrderBy(x=>x.nbTipoEmpresa).ToList(), "idTipoEmpresa", "nbTipoEmpresa", ExperienciasProfesionales.idTipoEmpresa);
//            }
//            else
//            {
//                ViewBag.idArea = new SelectList(db.CatAreas.ToList(), "idArea", "nbArea");
//                ViewBag.idSector = new SelectList(db.CatSectores.ToList(), "idSector", "nbSector");
//                ViewBag.idTipoEmpresa = new SelectList(db.CatTipoEmpresa.ToList(), "idTipoEmpresa", "nbTipoEmpresa");
//            }

//            //ViewBag.idTipoPuesto = new SelectList(db.CatTiposPuestos.ToList(), "descripcion", "descripcion");

//            return PartialView("_FormularioExperienciaProfesional", ExperienciasProfesionales);
//        }
//        public ActionResult DeleteExperienciaProfesional(long idExperienciaProfesional)
//        {
//            bool success = false;
//            var ExperienciaProfesional = db.ExperienciasProfesionales.Find(idExperienciaProfesional);
//            try
//            {
//                db.Entry(ExperienciaProfesional).State = EntityState.Deleted;
//                db.SaveChanges();
//                success = true;
//            }
//            catch (Exception exp)
//            {
//                ViewBag.ResultMessage = "Error occured." + exp;
//            }
//            return Json(new { success = success }, JsonRequestBehavior.AllowGet);
//        }
//        public ActionResult _TablaAnexos(long? idAlumno, int? page)
//        {
//            int pageSize = 2;
//            int pageNumber = (page ?? 1);
//            var ListaAnexos = db.Anexos.Where(x => x.idAlumno == idAlumno).ToList().ToPagedList(pageNumber, pageSize);
//            ViewBag.idAlumno = idAlumno;
//            return PartialView("_TablaAnexos", ListaAnexos);
//        }
//        public ActionResult SaveAnexo(Anexos Anexo, long idAlumno)
//        {
//            bool success = false;
//            string mensajefound = "";
//            try
//            {
//                var ListaAnexos = db.Anexos.ToList();
//                var found = ListaAnexos.FirstOrDefault(a => helper.RemoveDiacritics(a.nbTitulo.Trim().ToUpper()) == helper.RemoveDiacritics(Anexo.nbTitulo.Trim().ToUpper()) && a.tpDocumento == Anexo.tpDocumento && a.idAlumno == idAlumno && a.idAnexo != Anexo.idAnexo);
//                if (found != null)
//                {
//                    mensajefound = "¡Ya ha registrado un documento con ese título y el mismo formato!!!";

//                }
//                else
//                {
//                    Anexos Anexos = new Anexos();
//                    if (Anexo.idAnexo == 0)
//                    {
//                        Anexos.idAlumno = idAlumno;
//                        Anexos.nbTitulo = Anexo.nbTitulo;
//                        Anexos.tpDocumento = Anexo.tpDocumento;
//                        Anexos.desObservaciones = Anexo.desObservaciones;
//                        Anexos.pathDocumento = Session["RutaArchivo"].ToString();
//                        db.Anexos.Add(Anexos);
//                    }
//                    else
//                    {
//                        Anexos = db.Anexos.Find(Anexo.idAnexo);
//                        Anexos.idAlumno = idAlumno;
//                        Anexos.nbTitulo = Anexo.nbTitulo;
//                        Anexos.tpDocumento = Anexo.tpDocumento;
//                        Anexos.desObservaciones = Anexo.desObservaciones;
//                        if (Anexo.pathDocumento == null)
//                        {
//                            Anexos.pathDocumento = Session["RutaArchivo"].ToString();
//                        }
//                        db.Entry(Anexos).State = EntityState.Modified;
//                    }
//                }
//                if (db.SaveChanges() > 0)
//                {
//                    Session["RutaArchivo"] = null;
//                    success = true;
//                }
//            }
//            catch (Exception ex)
//            {
//                ViewBag.ResultMessage = "Error occured, records rolledback." + ex;
//            }

//            return Json(new { success = success, mensajefound }, JsonRequestBehavior.AllowGet);

//        }
//        public ActionResult subirArchivo(int? band)
//        {
//            bool success = false;
//            if (band == 1)
//            {
//                success = true;
//            }
//            foreach (string file in Request.Files)
//            {

//                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
//                string name = Path.GetRandomFileName();
//                string extension = Path.GetExtension(Path.GetFileName(hpf.FileName));
//                string completeName = name + extension;
//                string savedFileName = Path.Combine(Server.MapPath("~/Upload/Egresados/"), completeName);
//                hpf.SaveAs(savedFileName);

//                var path = "~/Upload/Egresados/" + name + extension;
//                Session["RutaArchivo"] = path;
//                success = true;
//            }

//            return Json(new { success = success }, JsonRequestBehavior.AllowGet);
//        }
//        public ActionResult _FormularioAnexo(long? idAnexo)
//        {
//            Anexos Anexos = new Anexos();
//            if (idAnexo != null)
//            {
//                Anexos = db.Anexos.Find(idAnexo);
//                string archivo = Anexos.pathDocumento;

//                int leghttxt = archivo.Length;
//                if (leghttxt < 20)
//                {
//                    ViewBag.RutaArchivo = "";
//                }
//                else
//                {
//                    ViewBag.RutaArchivo = archivo.Remove(0, 19);
//                }

//                ViewBag.bandera = 1;
//            }
//            return PartialView("_FormularioAnexo", Anexos);
//        }
//        public ActionResult DeleteAnexo(long idAnexo)
//        {
//            bool success = false;
//            var Anexos = db.Anexos.Find(idAnexo);
//            try
//            {
//                var path = Anexos.pathDocumento;
//                if (System.IO.File.Exists(Server.MapPath(path)))
//                {
//                    try
//                    {
//                        System.IO.File.Delete(Server.MapPath(path));
//                    }
//                    catch (System.IO.IOException e)
//                    {
//                        Console.WriteLine(e.Message);
//                    }

//                }

//                db.Entry(Anexos).State = EntityState.Deleted;
//                if (db.SaveChanges() > 0)
//                {
//                    success = true;
//                }

//            }
//            catch (Exception exp)
//            {
//                ViewBag.ResultMessage = "Error occured." + exp;
//            }
//            return Json(new { success = success }, JsonRequestBehavior.AllowGet);
//        }
//        public ActionResult DescargarArchivoAnexo(long? id)
//        {
//            var Anexos = db.Anexos.Find(id);
//            var path = Anexos.pathDocumento;
//            var ext = Anexos.tpDocumento;
//            var titulo = Anexos.nbTitulo;
//            return File(Url.Content(path), System.Net.Mime.MediaTypeNames.Application.Octet, titulo + "." + ext);
//        }

//    }
//}