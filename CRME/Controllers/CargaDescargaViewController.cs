//using OfficeOpenXml;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Mvc;
//using System.Web.Script.Serialization;
//using CRME.Models;
//using CRME.Helpers;
//using System.Data.Entity.Validation;
//using OfficeOpenXml.Style;
//using System.Drawing;
//using System.Data.Entity;
//using PagedList;

//namespace CRME.Controllers
//{
//    public class CargaDescargaViewController : Controller
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
//        public async Task<ActionResult> CargarEgresados()
//        {
//            //if (!User.Identity.IsAuthenticated)
//            //{
//            //    return RedirectToAction("Index", "AccesoView");
//            //}
//            bool success = false;
//            string mensaje = "";
//            JsonResult Resp = await UploadEgresados();
//            JavaScriptSerializer ser = new JavaScriptSerializer();
//            ResponseObjectVM Respuestas = ser.Deserialize<ResponseObjectVM>(ser.Serialize(Resp.Data));
//            success = Respuestas.success;
//            mensaje = Respuestas.mensaje;
//            return Json(new { success = success, mensaje });
//        }
//        public async Task<JsonResult> UploadEgresados()
//        {
//            bool success = false;
//            string mensaje = "";
//            string msj = "";
//            await Task.Run(() =>
//            {

//            string savedFileNameDownload = "";
//            string nombreArchivo = "Egresados";
//            string nombreArchivoTemp = "EgresadosTemp";
//            var codigosPostales = db.CatCodigosPostales.ToList();
//            FileStream stream = null;

//            try
//            {
//                foreach (string file in Request.Files)
//                {
//                    if (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Egresados/" + nombreArchivo + ".xlsx")))
//                    {
//                        System.IO.File.Delete(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Egresados/" + nombreArchivo + ".xlsx"));
//                    }
//                    if (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Egresados/" + nombreArchivoTemp + ".xlsx")))
//                    {
//                        System.IO.File.Delete(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Egresados/" + nombreArchivoTemp + ".xlsx"));

//                    }

//                    HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
//                    string savedFileName = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Egresados/"), nombreArchivo + Path.GetExtension(Path.GetFileName(hpf.FileName)));
//                    hpf.SaveAs(savedFileName);
//                    using (stream = System.IO.File.Open(savedFileName, FileMode.Open, FileAccess.ReadWrite))
//                    {
//                        using (ExcelPackage excel = new ExcelPackage())
//                        {
//                            excel.Load(stream);

//                            foreach (var ws in excel.Workbook.Worksheets.ToList())
//                            {
//                                var start = ws.Dimension.Start;
//                                var end = ws.Dimension.End;

//                                if (end.Column>=94)
//                                {
//                                        for (int row = start.Row; row <= end.Row; row++)
//                                        {
//                                            try
//                                            {
//                                                //RelAlumnoMatricula
//                                                var noMatricula = string.Empty;

//                                                //Alumnos
//                                                var tpEstadoCivil = string.Empty;
//                                                var feNacimiento = (DateTime?)null;
//                                                var nbReligion = string.Empty;
//                                                var fgTrabajaActualmente = string.Empty;
//                                                var tpEgresado = string.Empty;
//                                                var tpSangre = string.Empty;

//                                                //Personas
//                                                var nbCompleto = string.Empty;
//                                                var tpGenero = string.Empty;

//                                                //Direcciones
//                                                var desDireccion = string.Empty;
//                                                var numCodigoPostal = string.Empty;
//                                                var nbPais = string.Empty;
//                                                var nbEstado = string.Empty;
//                                                var nbCiudad = string.Empty;
//                                                var nbColonia = string.Empty;

//                                                //MediosContacto
//                                                var desTelPersonal = string.Empty;
//                                                var desTelOficina = string.Empty;
//                                                var desTelCelular = string.Empty;

//                                                var desCorreo1 = string.Empty;
//                                                var desCorreo2 = string.Empty;

//                                                //Formación Academica
//                                                var licenciatura = string.Empty;
//                                                var nbColegio = string.Empty;
//                                                var nivelColegio = string.Empty;

//                                                //ExperienciaProfesional
//                                                var nbEmpresa = string.Empty;
//                                                var nbPuesto = string.Empty;
//                                                var tpPuesto = string.Empty;
//                                                var Area = string.Empty;
//                                                var Sector = string.Empty;
//                                                var tpEmpresa = string.Empty;


//                                                var periodoEgreso = string.Empty;
//                                                DateTime fecha = new DateTime();

//                                                for (int col = start.Column; col <= end.Column; col++)
//                                                {
//                                                    object cellValue = new object();

//                                                    if (col == 1)
//                                                    {
//                                                        cellValue = ws.Cells[row, col].Text.Trim();
//                                                        noMatricula = cellValue.ToString();
//                                                    }
//                                                    else if (col == 2)
//                                                    {
//                                                        cellValue = ws.Cells[row, col].Text.Trim();
//                                                        nbCompleto = cellValue.ToString();
//                                                    }

//                                                    else if (col == 5)
//                                                    {
//                                                        cellValue = ws.Cells[row, col].Text.Trim();
//                                                        periodoEgreso = cellValue.ToString();
//                                                    }
//                                                    else if (col == 7)
//                                                    {
//                                                        cellValue = ws.Cells[row, col].Text.Trim();
//                                                        licenciatura = cellValue.ToString();

//                                                    }
//                                                    else if (col == 13)
//                                                    {
//                                                        cellValue = ws.Cells[row, col].Text.Trim();
//                                                        nivelColegio = cellValue.ToString();

//                                                    }
//                                                    else if (col == 14)
//                                                    {
//                                                        cellValue = ws.Cells[row, col].Text.Trim();
//                                                        nbColegio = cellValue.ToString();

//                                                    }
//                                                    else if (col == 17)
//                                                    {
//                                                        cellValue = ws.Cells[row, col].Text.Trim();
//                                                        bool result = DateTime.TryParse(ws.Cells[row, col].Text, out fecha);
//                                                        if (result == true)
//                                                        {
//                                                            feNacimiento = Convert.ToDateTime(cellValue);
//                                                        }

//                                                    }
//                                                    else if (col == 18)
//                                                    {
//                                                        cellValue = ws.Cells[row, col].Text.Trim();
//                                                        tpGenero = cellValue.ToString();
//                                                    }
//                                                    else if (col == 19)
//                                                    {
//                                                        cellValue = ws.Cells[row, col].Text.Trim();
//                                                        tpEstadoCivil = cellValue.ToString();
//                                                    }
//                                                    else if (col == 20)
//                                                    {
//                                                        cellValue = ws.Cells[row, col].Text.Trim();
//                                                        nbReligion = cellValue.ToString();
//                                                    }
//                                                    else if (col == 25)
//                                                    {
//                                                        cellValue = ws.Cells[row, col].Text.Trim();
//                                                        tpEgresado = cellValue.ToString();
//                                                    }
//                                                    else if (col == 33)
//                                                    {
//                                                        cellValue = ws.Cells[row, col].Text.Trim();
//                                                        desCorreo1 = cellValue.ToString();
//                                                    }
//                                                    else if (col == 34)
//                                                    {
//                                                        cellValue = ws.Cells[row, col].Text.Trim();
//                                                        desCorreo2 = cellValue.ToString();
//                                                    }
//                                                    else if (col == 39)
//                                                    {
//                                                        //Trabajo
//                                                        cellValue = ws.Cells[row, col].Text.Trim();
//                                                        fgTrabajaActualmente = cellValue.ToString();
//                                                    }
//                                                    else if (col == 45)
//                                                    {
//                                                        //tel ofi
//                                                        cellValue = ws.Cells[row, col].Text.Trim();
//                                                        desTelOficina = cellValue.ToString();
//                                                    }
//                                                    else if (col == 46)
//                                                    {
//                                                        //tel cel
//                                                        cellValue = ws.Cells[row, col].Text.Trim();
//                                                        desTelCelular = cellValue.ToString();
//                                                    }
//                                                    else if (col == 47)
//                                                    {
//                                                        //tel parti
//                                                        cellValue = ws.Cells[row, col].Text.Trim();
//                                                        desTelPersonal = cellValue.ToString();
//                                                    }
//                                                    else if (col == 49)
//                                                    {

//                                                        cellValue = ws.Cells[row, col].Text.Trim();
//                                                        desDireccion = cellValue.ToString();
//                                                    }
//                                                    else if (col == 51)
//                                                    {

//                                                        cellValue = ws.Cells[row, col].Text.Trim();
//                                                        nbColonia = cellValue.ToString();
//                                                    }
//                                                    else if (col == 52)
//                                                    {

//                                                        cellValue = ws.Cells[row, col].Text.Trim();
//                                                        nbCiudad = cellValue.ToString();
//                                                    }
//                                                    else if (col == 53)
//                                                    {

//                                                        cellValue = ws.Cells[row, col].Text.Trim();
//                                                        nbEstado = cellValue.ToString();
//                                                    }
//                                                    else if (col == 54)
//                                                    {

//                                                        cellValue = ws.Cells[row, col].Text.Trim();
//                                                        nbPais = cellValue.ToString();
//                                                    }
//                                                    else if (col == 55)
//                                                    {

//                                                        cellValue = ws.Cells[row, col].Text.Trim();
//                                                        numCodigoPostal = cellValue.ToString();
//                                                    }
//                                                    else if (col == 57)
//                                                    {

//                                                        cellValue = ws.Cells[row, col].Text.Trim();
//                                                        nbEmpresa = cellValue.ToString();
//                                                    }
//                                                    else if (col == 58)
//                                                    {

//                                                        cellValue = ws.Cells[row, col].Text.Trim();
//                                                        Area = cellValue.ToString();
//                                                    }
//                                                    else if (col == 59)
//                                                    {

//                                                        cellValue = ws.Cells[row, col].Text.Trim();
//                                                        Sector = cellValue.ToString();
//                                                    }
//                                                    else if (col == 60)
//                                                    {

//                                                        cellValue = ws.Cells[row, col].Text.Trim();
//                                                        tpEmpresa = cellValue.ToString();
//                                                    }
//                                                    else if (col == 61)
//                                                    {

//                                                        cellValue = ws.Cells[row, col].Text.Trim();
//                                                        tpPuesto = cellValue.ToString();
//                                                    }
//                                                    else if (col == 62)
//                                                    {

//                                                        cellValue = ws.Cells[row, col].Text.Trim();
//                                                        nbPuesto = cellValue.ToString();
//                                                    }
//                                                    else if (col == 74)
//                                                    {

//                                                        cellValue = ws.Cells[row, col].Text.Trim();
//                                                        tpSangre = cellValue.ToString();
//                                                    }

//                                                }

//                                                if (row > 1)
//                                                {

//                                                    if (noMatricula != "" && nbCompleto != "")
//                                                    {
//                                                        object cellValue = new object();

//                                                        var Alumnos = db.Alumnos.FirstOrDefault(x => x.Personas.nbCompleto.Trim().ToUpper() == nbCompleto.Trim().ToUpper());
//                                                        Alumnos Alumno = new Alumnos();
//                                                        Direcciones Direcciones = new Direcciones();
//                                                        Personas Persona = new Personas();
//                                                        FormacionesAcademicas FormacionAcademica = new FormacionesAcademicas();
//                                                        ExperienciasProfesionales ExperienciaProfesional = new ExperienciasProfesionales();
//                                                        var Licenciaturasdb = db.Licenciaturas.ToList();
//                                                        var Lic = new Licenciaturas();
//                                                        if (licenciatura != "")
//                                                        {
//                                                            Lic = Licenciaturasdb.FirstOrDefault(x => x.nbLicenciatura.Trim() != null && helper.RemoveDiacritics(x.nbLicenciatura.Trim().ToLower()) == helper.RemoveDiacritics(licenciatura.Trim().ToLower()));
//                                                        }

//                                                        if (Alumnos == null)
//                                                        {
//                                                            //Personas
//                                                            Persona.nbCompleto = nbCompleto;
//                                                            Persona.idTipoPersona = 2;
//                                                            var idGenero = (tpGenero != "") ? db.CatGeneros.FirstOrDefault(x => x.nbGenero.Trim() == tpGenero.Trim()).idGenero : 3;
//                                                            Persona.idGenero = idGenero;
//                                                            db.Personas.Add(Persona);
//                                                            nbEstado = nbEstado.Replace("DISTRITO FEDERAL", "Ciudad de México").Replace("Distrito Federal", "Ciudad de México");
//                                                            //Direcciones
//                                                            if (nbCiudad != string.Empty && nbEstado != string.Empty)
//                                                            {
//                                                                var CatCodigosPostales = codigosPostales.Where(x => helper.RemoveDiacritics(x.nbCiudad.Trim().ToUpper()) == helper.RemoveDiacritics(nbCiudad.Trim().ToUpper()) && helper.RemoveDiacritics(x.nbEstado.Trim().ToUpper()) == helper.RemoveDiacritics(nbEstado.Trim().ToUpper()));
//                                                                if (CatCodigosPostales.Count() > 0)
//                                                                {
//                                                                    Direcciones.numCodigoPostal = CatCodigosPostales.FirstOrDefault().numCodigoPostal;
//                                                                    Direcciones.nbCiudad = CatCodigosPostales.FirstOrDefault().nbCiudad;
//                                                                    Direcciones.nbEstado = CatCodigosPostales.FirstOrDefault().nbEstado;
//                                                                    Direcciones.nbColonia = CatCodigosPostales.FirstOrDefault().nbAsentamiento;
//                                                                }
//                                                                else
//                                                                {
//                                                                    Direcciones.numCodigoPostal = "-1";
//                                                                    Direcciones.nbCiudad = nbCiudad;
//                                                                    Direcciones.nbEstado = nbEstado;
//                                                                    Direcciones.nbColonia = (nbColonia != "" ? nbColonia : "Sin especificar");
//                                                                }
//                                                            }
//                                                            else if (nbCiudad == string.Empty && nbEstado != string.Empty)
//                                                            {
//                                                                var CatCodigosPostales = codigosPostales.Where(x => helper.RemoveDiacritics(x.nbEstado.Trim().ToUpper()) == helper.RemoveDiacritics(nbEstado.Trim().ToUpper()));
//                                                                if (CatCodigosPostales.Count() > 0)
//                                                                {
//                                                                    Direcciones.numCodigoPostal = CatCodigosPostales.FirstOrDefault().numCodigoPostal;
//                                                                    Direcciones.nbCiudad = CatCodigosPostales.FirstOrDefault().nbCiudad;
//                                                                    Direcciones.nbEstado = CatCodigosPostales.FirstOrDefault().nbEstado;
//                                                                    Direcciones.nbColonia = CatCodigosPostales.FirstOrDefault().nbAsentamiento;
//                                                                }
//                                                                else
//                                                                {
//                                                                    Direcciones.numCodigoPostal = "-1";
//                                                                    Direcciones.nbCiudad = "Sin especificar";
//                                                                    Direcciones.nbEstado = nbEstado;
//                                                                    Direcciones.nbColonia = (nbColonia != "" ? nbColonia : "Sin especificar");
//                                                                }

//                                                            }
//                                                            else if (nbCiudad != string.Empty && nbEstado == string.Empty)
//                                                            {
//                                                                var CatCodigosPostales = codigosPostales.Where(x => helper.RemoveDiacritics(x.nbCiudad.Trim().ToUpper()) == helper.RemoveDiacritics(nbEstado.Trim().ToUpper()));
//                                                                if (CatCodigosPostales.Count() > 0)
//                                                                {
//                                                                    Direcciones.numCodigoPostal = CatCodigosPostales.FirstOrDefault().numCodigoPostal;
//                                                                    Direcciones.nbCiudad = CatCodigosPostales.FirstOrDefault().nbCiudad;
//                                                                    Direcciones.nbEstado = CatCodigosPostales.FirstOrDefault().nbEstado;
//                                                                    Direcciones.nbColonia = CatCodigosPostales.FirstOrDefault().nbAsentamiento;
//                                                                }
//                                                                else
//                                                                {
//                                                                    Direcciones.numCodigoPostal = "-1";
//                                                                    Direcciones.nbCiudad = nbCiudad;
//                                                                    Direcciones.nbEstado = "Sin especificar";
//                                                                    Direcciones.nbColonia = (nbColonia != "" ? nbColonia : "Sin especificar");
//                                                                }

//                                                            }
//                                                            else if (nbCiudad == string.Empty && nbEstado == string.Empty)
//                                                            {
//                                                                Direcciones.numCodigoPostal = "-1";
//                                                                Direcciones.nbCiudad = "Sin especificar";
//                                                                Direcciones.nbEstado = "Sin especificar";
//                                                                Direcciones.nbColonia = "Sin especificar";
//                                                            }
//                                                            Direcciones.desDireccion = (desDireccion != string.Empty ? desDireccion : "Sin especificar");
//                                                            Direcciones.nbPais = "México";
//                                                            db.Direcciones.Add(Direcciones);

//                                                            //Alumno
//                                                            Alumno.idPersona = Persona.idPersona;
//                                                            Alumno.idDireccion = Direcciones.idDireccion;
//                                                            var idEstadoCivil = (tpEstadoCivil != "") ? db.CatEstadosCiviles.FirstOrDefault(x => x.nbEstadoCivil.Trim() == tpEstadoCivil).idEstadoCivil : 4;
//                                                            Alumno.idEstadoCivil = idEstadoCivil;
//                                                            Alumno.feNacimiento = (feNacimiento != null) ? feNacimiento : new DateTime();
//                                                            Alumno.feActualizacion = DateTime.Now;
//                                                            nbReligion = nbReligion.Replace("No Declarado", "");
//                                                            var idReligion = (nbReligion != "") ? db.CatReligion.FirstOrDefault(x => x.nbReligion.Trim() == nbReligion.Trim()).idReligion : 12;
//                                                            Alumno.idReligion = idReligion;
//                                                            var Trabaja = (fgTrabajaActualmente != "") ? ((fgTrabajaActualmente == "S") ? true : false) : false;
//                                                            Alumno.fgTrabajaActualmente = Trabaja;
//                                                            Alumno.fgFallecido = false;
//                                                            Alumno.tpEgresado = (tpEgresado != "") ? tpEgresado : "Sin Especificar";
//                                                            tpSangre = tpSangre.Replace("OO", "O+");
//                                                            Alumno.idTipoSangre = (tpSangre != "") ? db.CatTiposSangre.FirstOrDefault(x => x.nbTipoSangre.Trim().ToUpper() == tpSangre.Trim().ToUpper()).idTipoSangre : 9;
//                                                            Alumno.fgDonarSangre = false;
//                                                            Alumno.fgLicencia = false;
//                                                            db.Alumnos.Add(Alumno);

//                                                            //Medios de Contacto Telefonos
//                                                            desTelOficina = desTelOficina.ToUpper().Replace("-", "").Replace("+", "").Replace(" ", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace("EXT", "E");
//                                                            desTelCelular = desTelCelular.ToUpper().Replace("-", "").Replace("+", "").Replace(" ", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace("EXT", "E");
//                                                            desTelPersonal = desTelPersonal.ToUpper().Replace("-", "").Replace("+", "").Replace(" ", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace("EXT", "E");
//                                                            if (desTelOficina != "")
//                                                            {
//                                                                var TelOfiExtension = desTelOficina.Split('E');
//                                                                MediosContactos md = new MediosContactos();
//                                                                md.idPersona = Persona.idPersona;
//                                                                md.idTipoMedioContacto = 2;
//                                                                md.desMedioContacto = TelOfiExtension[0];
//                                                                if (TelOfiExtension.Length > 1)
//                                                                {
//                                                                    md.numExtension = TelOfiExtension[1];
//                                                                }
//                                                                md.tpFormato = "Oficina";
//                                                                db.MediosContactos.Add(md);
//                                                            }


//                                                            if (desTelCelular != "")
//                                                            {
//                                                                var TelCelExtension = desTelCelular.Split('E');
//                                                                MediosContactos md1 = new MediosContactos();
//                                                                md1.idPersona = Persona.idPersona;
//                                                                md1.idTipoMedioContacto = 2;
//                                                                md1.desMedioContacto = TelCelExtension[0];
//                                                                if (TelCelExtension.Length > 1)
//                                                                {
//                                                                    md1.numExtension = TelCelExtension[1];
//                                                                }
//                                                                md1.tpFormato = "Móvil";
//                                                                db.MediosContactos.Add(md1);
//                                                            }

//                                                            if (desTelPersonal != "")
//                                                            {
//                                                                var TelPerExtension = desTelPersonal.Split('E');
//                                                                MediosContactos md2 = new MediosContactos();
//                                                                md2.idPersona = Persona.idPersona;
//                                                                md2.idTipoMedioContacto = 2;
//                                                                md2.desMedioContacto = TelPerExtension[0];
//                                                                if (TelPerExtension.Length > 1)
//                                                                {
//                                                                    md2.numExtension = TelPerExtension[1];
//                                                                }
//                                                                md2.tpFormato = "Personal";
//                                                                db.MediosContactos.Add(md2);
//                                                            }

//                                                            //Medios de Contacto Correos
//                                                            if (desCorreo1 != "")
//                                                            {
//                                                                MediosContactos md3 = new MediosContactos();
//                                                                md3.idPersona = Persona.idPersona;
//                                                                md3.idTipoMedioContacto = 1;
//                                                                md3.desMedioContacto = desCorreo1;
//                                                                md3.tpFormato = "Personal";
//                                                                db.MediosContactos.Add(md3);
//                                                            }

//                                                            if (desCorreo2 != "")
//                                                            {
//                                                                desCorreo2 = desCorreo2.Replace(" ", "");
//                                                                var ListaCorreos2 = desCorreo2.Split(',');
//                                                                var CountPers = 0;
//                                                                var CountCasa = 0;
//                                                                foreach (var CorreoN0 in ListaCorreos2)
//                                                                {
//                                                                    var desCorreo = string.Empty;
//                                                                    var tpCorreo = string.Empty;
//                                                                    int Length = CorreoN0.Length - 4;
//                                                                    tpCorreo = CorreoN0.Substring(0, 4);
//                                                                    desCorreo = CorreoN0.Substring(4, Length);

//                                                                    if (tpCorreo == "PERS" && CountPers < 2)
//                                                                    {
//                                                                        MediosContactos md4 = new MediosContactos();
//                                                                        md4.idPersona = Persona.idPersona;
//                                                                        md4.idTipoMedioContacto = 1;
//                                                                        md4.desMedioContacto = desCorreo.Trim();
//                                                                        md4.tpFormato = "Otro";
//                                                                        db.MediosContactos.Add(md4);
//                                                                        CountPers++;
//                                                                    }
//                                                                    else if (tpCorreo == "CASA" && CountCasa < 2)
//                                                                    {
//                                                                        MediosContactos md4 = new MediosContactos();
//                                                                        md4.idPersona = Persona.idPersona;
//                                                                        md4.idTipoMedioContacto = 1;
//                                                                        md4.desMedioContacto = desCorreo.Trim();
//                                                                        md4.tpFormato = "Otro";
//                                                                        db.MediosContactos.Add(md4);
//                                                                        CountCasa++;
//                                                                    }

//                                                                }
//                                                            }

//                                                            //Formaciones Academicas
//                                                            if (Lic != null)
//                                                            {
//                                                                FormacionAcademica.idLicenciatura = Lic.idLicenciatura;
//                                                            }
//                                                            else
//                                                            {
//                                                                Licenciaturas Licenciaturas = new Licenciaturas();
//                                                                Licenciaturas.nbLicenciatura = licenciatura;
//                                                                Licenciaturas.idNivelEstudio = 1;
//                                                                db.Licenciaturas.Add(Licenciaturas);
//                                                                FormacionAcademica.idLicenciatura = Licenciaturas.idLicenciatura;
//                                                            }
//                                                            FormacionAcademica.idNivelEstudio = 1;
//                                                            FormacionAcademica.idColegio = 1;
//                                                            FormacionAcademica.idAlumno = Alumno.idAlumno;
//                                                            db.FormacionesAcademicas.Add(FormacionAcademica);


//                                                            //Experiencias Profesionales
//                                                            if (nbEmpresa != "" && nbPuesto != "" && tpPuesto != "")
//                                                            {
//                                                                if (tpEmpresa == "E")
//                                                                {
//                                                                    tpEmpresa = "Empleado";
//                                                                }
//                                                                else if (tpEmpresa == "P")
//                                                                {
//                                                                    tpEmpresa = "Propia";
//                                                                }
//                                                                else if (tpEmpresa == "F")
//                                                                {
//                                                                    tpEmpresa = "Familiar";
//                                                                }
//                                                                ExperienciaProfesional.idAlumno = Alumno.idAlumno;
//                                                                ExperienciaProfesional.idTipoEmpresa = (tpEmpresa != "") ? db.CatTipoEmpresa.FirstOrDefault(x => x.nbTipoEmpresa.Trim() == tpEmpresa.Trim()).idTipoEmpresa : 3;
//                                                                var CatAreas = db.CatAreas.ToList();
//                                                                var idArea = CatAreas.FirstOrDefault(x => helper.RemoveDiacritics(x.nbArea.Trim().ToUpper()) == helper.RemoveDiacritics(Area));
//                                                                if (idArea != null)
//                                                                {
//                                                                    ExperienciaProfesional.idArea = idArea.idArea;
//                                                                }
//                                                                else
//                                                                {
//                                                                    CatAreas catArea = new CatAreas();
//                                                                    catArea.nbArea = Area.Trim().ToUpper();
//                                                                    db.CatAreas.Add(catArea);
//                                                                    ExperienciaProfesional.idArea = catArea.idArea;
//                                                                }

//                                                                var CatSectores = db.CatSectores.ToList();
//                                                                var idSector = CatSectores.FirstOrDefault(x => helper.RemoveDiacritics(x.nbSector.Trim().ToUpper()) == helper.RemoveDiacritics(Sector.Trim().ToUpper()));
//                                                                if (idSector != null)
//                                                                {
//                                                                    ExperienciaProfesional.idSector = idSector.idSector;
//                                                                }
//                                                                else
//                                                                {
//                                                                    CatSectores catSector = new CatSectores();
//                                                                    catSector.nbSector = Sector.Trim().ToUpper();
//                                                                    db.CatSectores.Add(catSector);
//                                                                    ExperienciaProfesional.idSector = catSector.idSector;
//                                                                }
//                                                                ExperienciaProfesional.nbEmpresa = nbEmpresa.Trim();
//                                                                ExperienciaProfesional.nbPuesto = nbPuesto.Trim();
//                                                                ExperienciaProfesional.tpPuesto = tpPuesto.Trim();
//                                                                db.ExperienciasProfesionales.Add(ExperienciaProfesional);
//                                                            }

//                                                            //RelAlumnoMatricula
//                                                            var FoundMatricula = db.RelAlumnosMatriculas.FirstOrDefault(x => x.noMatricula.ToUpper().Trim() == noMatricula.ToUpper().Trim() && x.Alumnos.Personas.nbCompleto.ToUpper().Trim() == nbCompleto.ToUpper().Trim());
//                                                            if (FoundMatricula == null)
//                                                            {
//                                                                RelAlumnosMatriculas RelAlumnosMatriculas = new RelAlumnosMatriculas();
//                                                                RelAlumnosMatriculas.idAlumno = Alumno.idAlumno;
//                                                                RelAlumnosMatriculas.noMatricula = noMatricula;
//                                                                db.RelAlumnosMatriculas.Add(RelAlumnosMatriculas);
//                                                            }

//                                                            ws.Cells[row, 1, row, 94].Style.Fill.PatternType = ExcelFillStyle.Solid;
//                                                            ws.Cells[row, 1, row, 94].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 153, 0));


//                                                        }
//                                                        else
//                                                        {
//                                                            //Personas
//                                                            Persona = db.Personas.FirstOrDefault(x => x.idPersona == Alumnos.idPersona);
//                                                            Persona.nbCompleto = nbCompleto;
//                                                            Persona.idTipoPersona = 2;
//                                                            var idGenero = (tpGenero != "") ? db.CatGeneros.FirstOrDefault(x => x.nbGenero.Trim() == tpGenero.Trim()).idGenero : 3;
//                                                            Persona.idGenero = idGenero;

//                                                            nbEstado = nbEstado.Replace("DISTRITO FEDERAL", "Ciudad de México").Replace("Distrito Federal", "Ciudad de México");
//                                                            Direcciones = db.Direcciones.FirstOrDefault(x => x.idDireccion == Alumnos.idDireccion);
//                                                            //Direcciones
//                                                            if (nbCiudad != string.Empty && nbEstado != string.Empty)
//                                                            {
//                                                                var CatCodigosPostales = codigosPostales.Where(x => helper.RemoveDiacritics(x.nbCiudad.Trim().ToUpper()) == helper.RemoveDiacritics(nbCiudad.Trim().ToUpper()) && helper.RemoveDiacritics(x.nbEstado.Trim().ToUpper()) == helper.RemoveDiacritics(nbEstado.Trim().ToUpper()));
//                                                                if (CatCodigosPostales.Count() > 0)
//                                                                {
//                                                                    Direcciones.numCodigoPostal = CatCodigosPostales.FirstOrDefault().numCodigoPostal;
//                                                                    Direcciones.nbCiudad = CatCodigosPostales.FirstOrDefault().nbCiudad;
//                                                                    Direcciones.nbEstado = CatCodigosPostales.FirstOrDefault().nbEstado;
//                                                                    Direcciones.nbColonia = CatCodigosPostales.FirstOrDefault().nbAsentamiento;
//                                                                }
//                                                                else
//                                                                {
//                                                                    Direcciones.numCodigoPostal = "-1";
//                                                                    Direcciones.nbCiudad = nbCiudad;
//                                                                    Direcciones.nbEstado = nbEstado;
//                                                                    Direcciones.nbColonia = (nbColonia != "" ? nbColonia : "Sin especificar");
//                                                                }
//                                                            }
//                                                            else if (nbCiudad == string.Empty && nbEstado != string.Empty)
//                                                            {
//                                                                var CatCodigosPostales = codigosPostales.Where(x => helper.RemoveDiacritics(x.nbEstado.Trim().ToUpper()) == helper.RemoveDiacritics(nbEstado.Trim().ToUpper()));
//                                                                if (CatCodigosPostales.Count() > 0)
//                                                                {
//                                                                    Direcciones.numCodigoPostal = CatCodigosPostales.FirstOrDefault().numCodigoPostal;
//                                                                    Direcciones.nbCiudad = CatCodigosPostales.FirstOrDefault().nbCiudad;
//                                                                    Direcciones.nbEstado = CatCodigosPostales.FirstOrDefault().nbEstado;
//                                                                    Direcciones.nbColonia = CatCodigosPostales.FirstOrDefault().nbAsentamiento;
//                                                                }
//                                                                else
//                                                                {
//                                                                    Direcciones.numCodigoPostal = "-1";
//                                                                    Direcciones.nbCiudad = "Sin especificar";
//                                                                    Direcciones.nbEstado = nbEstado;
//                                                                    Direcciones.nbColonia = (nbColonia != "" ? nbColonia : "Sin especificar");
//                                                                }

//                                                            }
//                                                            else if (nbCiudad != string.Empty && nbEstado == string.Empty)
//                                                            {
//                                                                var CatCodigosPostales = codigosPostales.Where(x => helper.RemoveDiacritics(x.nbCiudad.Trim().ToUpper()) == helper.RemoveDiacritics(nbEstado.Trim().ToUpper()));
//                                                                if (CatCodigosPostales.Count() > 0)
//                                                                {
//                                                                    Direcciones.numCodigoPostal = CatCodigosPostales.FirstOrDefault().numCodigoPostal;
//                                                                    Direcciones.nbCiudad = CatCodigosPostales.FirstOrDefault().nbCiudad;
//                                                                    Direcciones.nbEstado = CatCodigosPostales.FirstOrDefault().nbEstado;
//                                                                    Direcciones.nbColonia = CatCodigosPostales.FirstOrDefault().nbAsentamiento;
//                                                                }
//                                                                else
//                                                                {
//                                                                    Direcciones.numCodigoPostal = "-1";
//                                                                    Direcciones.nbCiudad = nbCiudad;
//                                                                    Direcciones.nbEstado = "Sin especificar";
//                                                                    Direcciones.nbColonia = (nbColonia != "" ? nbColonia : "Sin especificar");
//                                                                }

//                                                            }
//                                                            else if (nbCiudad == string.Empty && nbEstado == string.Empty)
//                                                            {
//                                                                Direcciones.numCodigoPostal = "-1";
//                                                                Direcciones.nbCiudad = "Sin especificar";
//                                                                Direcciones.nbEstado = "Sin especificar";
//                                                                Direcciones.nbColonia = "Sin especificar";
//                                                            }
//                                                            Direcciones.desDireccion = (desDireccion != string.Empty ? desDireccion : "Sin especificar");
//                                                            Direcciones.nbPais = "México";

//                                                            //Alumno
//                                                            Alumnos.idPersona = Persona.idPersona;
//                                                            Alumnos.idDireccion = Direcciones.idDireccion;
//                                                            var idEstadoCivil = (tpEstadoCivil != "") ? db.CatEstadosCiviles.FirstOrDefault(x => x.nbEstadoCivil.Trim() == tpEstadoCivil).idEstadoCivil : 4;
//                                                            Alumnos.idEstadoCivil = idEstadoCivil;
//                                                            Alumnos.feNacimiento = (feNacimiento != null) ? feNacimiento : new DateTime();
//                                                            Alumnos.feActualizacion = DateTime.Now;
//                                                            nbReligion = nbReligion.Replace("No Declarado", "");
//                                                            var idReligion = (nbReligion != "") ? db.CatReligion.FirstOrDefault(x => x.nbReligion.Trim() == nbReligion.Trim()).idReligion : 12;
//                                                            Alumnos.idReligion = idReligion;
//                                                            var Trabaja = (fgTrabajaActualmente != "") ? ((fgTrabajaActualmente == "S") ? true : false) : false;
//                                                            Alumnos.fgTrabajaActualmente = Trabaja;
//                                                            Alumnos.fgFallecido = false;
//                                                            Alumnos.tpEgresado = (tpEgresado != "") ? tpEgresado : "Sin Especificar";
//                                                            tpSangre = tpSangre.Replace("OO", "O+");
//                                                            Alumnos.idTipoSangre = (tpSangre != "") ? db.CatTiposSangre.FirstOrDefault(x => x.nbTipoSangre.Trim().ToUpper() == tpSangre.Trim().ToUpper()).idTipoSangre : 9;
//                                                            Alumnos.fgDonarSangre = false;
//                                                            Alumnos.fgLicencia = false;


//                                                            //eliminamos todos los registros
//                                                            db.MediosContactos.RemoveRange(Persona.MediosContactos);
//                                                            //Medios de Contacto Telefonos
//                                                            desTelOficina = desTelOficina.ToUpper().Replace("-", "").Replace("+", "").Replace(" ", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace("EXT", "E");
//                                                            desTelCelular = desTelCelular.ToUpper().Replace("-", "").Replace("+", "").Replace(" ", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace("EXT", "E");
//                                                            desTelPersonal = desTelPersonal.ToUpper().Replace("-", "").Replace("+", "").Replace(" ", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace("EXT", "E");
//                                                            if (desTelOficina != "")
//                                                            {
//                                                                var TelOfiExtension = desTelOficina.Split('E');
//                                                                MediosContactos md = new MediosContactos();
//                                                                md.idPersona = Persona.idPersona;
//                                                                md.idTipoMedioContacto = 2;
//                                                                md.desMedioContacto = TelOfiExtension[0];
//                                                                if (TelOfiExtension.Length > 1)
//                                                                {
//                                                                    md.numExtension = TelOfiExtension[1];
//                                                                }
//                                                                md.tpFormato = "Oficina";
//                                                                db.MediosContactos.Add(md);
//                                                            }


//                                                            if (desTelCelular != "")
//                                                            {
//                                                                var TelCelExtension = desTelCelular.Split('E');
//                                                                MediosContactos md1 = new MediosContactos();
//                                                                md1.idPersona = Persona.idPersona;
//                                                                md1.idTipoMedioContacto = 2;
//                                                                md1.desMedioContacto = TelCelExtension[0];
//                                                                if (TelCelExtension.Length > 1)
//                                                                {
//                                                                    md1.numExtension = TelCelExtension[1];
//                                                                }
//                                                                md1.tpFormato = "Móvil";
//                                                                db.MediosContactos.Add(md1);
//                                                            }

//                                                            if (desTelPersonal != "")
//                                                            {
//                                                                var TelPerExtension = desTelPersonal.Split('E');
//                                                                MediosContactos md2 = new MediosContactos();
//                                                                md2.idPersona = Persona.idPersona;
//                                                                md2.idTipoMedioContacto = 2;
//                                                                md2.desMedioContacto = TelPerExtension[0];
//                                                                if (TelPerExtension.Length > 1)
//                                                                {
//                                                                    md2.numExtension = TelPerExtension[1];
//                                                                }
//                                                                md2.tpFormato = "Personal";
//                                                                db.MediosContactos.Add(md2);
//                                                            }

//                                                            //Medios de Contacto Correos
//                                                            if (desCorreo1 != "")
//                                                            {
//                                                                MediosContactos md3 = new MediosContactos();
//                                                                md3.idPersona = Persona.idPersona;
//                                                                md3.idTipoMedioContacto = 1;
//                                                                md3.desMedioContacto = desCorreo1;
//                                                                md3.tpFormato = "Personal";
//                                                                db.MediosContactos.Add(md3);
//                                                            }

//                                                            if (desCorreo2 != "")
//                                                            {
//                                                                desCorreo2 = desCorreo2.Replace(" ", "");
//                                                                var ListaCorreos2 = desCorreo2.Split(',');
//                                                                var CountPers = 0;
//                                                                var CountCasa = 0;
//                                                                foreach (var CorreoN0 in ListaCorreos2)
//                                                                {
//                                                                    var desCorreo = string.Empty;
//                                                                    var tpCorreo = string.Empty;
//                                                                    var Length = CorreoN0.Length - 4;
//                                                                    tpCorreo = CorreoN0.Substring(0, 4);
//                                                                    desCorreo = CorreoN0.Substring(4, Length);

//                                                                    if (tpCorreo == "PERS" && CountPers < 2)
//                                                                    {
//                                                                        MediosContactos md4 = new MediosContactos();
//                                                                        md4.idPersona = Persona.idPersona;
//                                                                        md4.idTipoMedioContacto = 1;
//                                                                        md4.desMedioContacto = desCorreo.Trim();
//                                                                        md4.tpFormato = "Personal";
//                                                                        db.MediosContactos.Add(md4);
//                                                                        CountPers++;
//                                                                    }
//                                                                    else if (tpCorreo == "CASA" && CountCasa < 2)
//                                                                    {
//                                                                        MediosContactos md4 = new MediosContactos();
//                                                                        md4.idPersona = Persona.idPersona;
//                                                                        md4.idTipoMedioContacto = 1;
//                                                                        md4.desMedioContacto = desCorreo.Trim();
//                                                                        md4.tpFormato = "Otro";
//                                                                        db.MediosContactos.Add(md4);
//                                                                        CountCasa++;
//                                                                    }

//                                                                }
//                                                            }

//                                                            //Formaciones Academicas
//                                                            FormacionAcademica = db.FormacionesAcademicas.FirstOrDefault(x => x.idAlumno == Alumnos.idAlumno);
//                                                            if (FormacionAcademica == null)
//                                                            {
//                                                                if (Lic != null)
//                                                                {
//                                                                    FormacionAcademica.idLicenciatura = Lic.idLicenciatura;
//                                                                }
//                                                                else
//                                                                {
//                                                                    Licenciaturas Licenciaturas = new Licenciaturas();
//                                                                    Licenciaturas.nbLicenciatura = licenciatura;
//                                                                    Licenciaturas.idNivelEstudio = 1;
//                                                                    db.Licenciaturas.Add(Licenciaturas);
//                                                                    FormacionAcademica.idLicenciatura = Licenciaturas.idLicenciatura;
//                                                                }
//                                                                FormacionAcademica.idNivelEstudio = 1;
//                                                                FormacionAcademica.idColegio = 1;
//                                                                FormacionAcademica.idAlumno = Alumnos.idAlumno;
//                                                                db.FormacionesAcademicas.Add(FormacionAcademica);
//                                                            }



//                                                            //Experiencias Profesionales

//                                                            ExperienciaProfesional = db.ExperienciasProfesionales.FirstOrDefault(x => x.idAlumno == Alumnos.idAlumno);
//                                                            if (ExperienciaProfesional == null && nbEmpresa != "")
//                                                            {
//                                                                if (tpEmpresa == "E")
//                                                                {
//                                                                    tpEmpresa = "Empleado";
//                                                                }
//                                                                else if (tpEmpresa == "P")
//                                                                {
//                                                                    tpEmpresa = "Propia";
//                                                                }
//                                                                else if (tpEmpresa == "F")
//                                                                {
//                                                                    tpEmpresa = "Familiar";
//                                                                }
//                                                                ExperienciaProfesional.idAlumno = Alumnos.idAlumno;
//                                                                ExperienciaProfesional.idTipoEmpresa = (tpEmpresa != "") ? db.CatTipoEmpresa.FirstOrDefault(x => x.nbTipoEmpresa.Trim() == tpEmpresa.Trim()).idTipoEmpresa : 3;
//                                                                var CatAreas = db.CatAreas.ToList();
//                                                                var idArea = CatAreas.FirstOrDefault(x => helper.RemoveDiacritics(x.nbArea.Trim().ToUpper()) == helper.RemoveDiacritics(Area));
//                                                                if (idArea != null)
//                                                                {
//                                                                    ExperienciaProfesional.idArea = idArea.idArea;
//                                                                }
//                                                                else
//                                                                {
//                                                                    CatAreas catArea = new CatAreas();
//                                                                    catArea.nbArea = Area.Trim().ToUpper();
//                                                                    db.CatAreas.Add(catArea);
//                                                                    ExperienciaProfesional.idArea = catArea.idArea;
//                                                                }

//                                                                var CatSectores = db.CatSectores.ToList();
//                                                                var idSector = CatSectores.FirstOrDefault(x => helper.RemoveDiacritics(x.nbSector.Trim().ToUpper()) == helper.RemoveDiacritics(Sector));
//                                                                if (idSector != null)
//                                                                {
//                                                                    ExperienciaProfesional.idSector = idSector.idSector;
//                                                                }
//                                                                else
//                                                                {
//                                                                    CatSectores catSector = new CatSectores();
//                                                                    catSector.nbSector = Sector.Trim().ToUpper();
//                                                                    db.CatSectores.Add(catSector);
//                                                                    ExperienciaProfesional.idSector = catSector.idSector;
//                                                                }

//                                                                ExperienciaProfesional.nbEmpresa = nbEmpresa.Trim();
//                                                                ExperienciaProfesional.nbPuesto = nbPuesto.Trim();
//                                                                ExperienciaProfesional.tpPuesto = tpPuesto.Trim();
//                                                                db.ExperienciasProfesionales.Add(ExperienciaProfesional);
//                                                            }


//                                                            //RelAlumnoMatricula
//                                                            var FoundMatricula = db.RelAlumnosMatriculas.FirstOrDefault(x => x.noMatricula.ToUpper().Trim() == noMatricula.ToUpper().Trim() && x.Alumnos.Personas.nbCompleto.ToUpper().Trim() == nbCompleto.ToUpper().Trim() && x.idAlumno == Alumnos.idAlumno);
//                                                            if (FoundMatricula == null)
//                                                            {
//                                                                RelAlumnosMatriculas RelAlumnosMatriculas = new RelAlumnosMatriculas();
//                                                                RelAlumnosMatriculas.idAlumno = Alumnos.idAlumno;
//                                                                RelAlumnosMatriculas.noMatricula = noMatricula;
//                                                                db.RelAlumnosMatriculas.Add(RelAlumnosMatriculas);
//                                                            }


//                                                            db.Entry(Persona).State = EntityState.Modified;
//                                                            db.Entry(Direcciones).State = EntityState.Modified;
//                                                            db.Entry(Alumnos).State = EntityState.Modified;

//                                                            ws.Cells[row, 1, row, 94].Style.Fill.PatternType = ExcelFillStyle.Solid;
//                                                            ws.Cells[row, 1, row, 94].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(245, 184, 0));


//                                                        }


//                                                        if (db.SaveChanges() > 0)
//                                                        {
//                                                            success = true;
//                                                            mensaje = "Operación realizada con éxito.";
//                                                        }
//                                                    }
//                                                }

//                                            }
//                                            catch (Exception ex)
//                                            {
//                                                Console.WriteLine(ex);
//                                                if (stream != null)
//                                                    stream.Close();
//                                                stream.Dispose();
//                                                string lines = ex.ToString();
//                                                // Write the string to a file.
//                                                StreamWriter fileError = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Egresados/DOCError.txt"));
//                                                fileError.WriteLine(lines);
//                                                fileError.Close();

//                                                ws.Cells[row, 1, row, 94].Style.Fill.PatternType = ExcelFillStyle.Solid;
//                                                ws.Cells[row, 1, row, 94].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 0, 0));
//                                            }
//                                        }
//                                    }
//                                    else
//                                    {
//                                        msj = "Ocurrió un problema al realizar la carga, el documento no cumple el formato correcto!!";
//                                    }
                               
//                                }

//                                string savedFileNameTemp = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Egresados/" + nombreArchivoTemp + ".xlsx"));
//                                FileStream streamTemp = System.IO.File.Create(savedFileNameTemp);
//                                excel.SaveAs(streamTemp);
//                                savedFileNameDownload = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Egresados/" + nombreArchivoTemp + ".xlsx"));
//                                streamTemp.Close();
//                                streamTemp.Dispose();
//                            };
//                            stream.Close();
//                            stream.Dispose();
//                            if (success==false && msj=="")
//                            {
//                                mensaje = "Algunos Egresados no se lograrón registrar y se han marcado, revise el archivo.";
//                            }else if (success==false && msj!="")
//                            {
//                                mensaje = msj;
//                            }
                                
//                        }

//                    }
//                }
//                catch (DbEntityValidationException ex)
//                {
//                    success = false;
//                    mensaje = "Ocurrió un problema al subir el archivo. Comuníquese con soporte técnico.";
//                    Console.WriteLine(ex);
//                    if (stream != null)
//                        stream.Close();
//                    stream.Dispose();
//                }
//            });

//            return Json(new { success = success, mensaje });
//        }
//        public ActionResult DescargarPlantilla()
//        {
//            if (!User.Identity.IsAuthenticated)
//            {
//                return RedirectToAction("Index", "AccesoView");
//            }
//            var ruta = "~/Upload/Plantillas/PlantillaCargaEgresados.xlsx";
//            return File(Url.Content(ruta), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Plantilla.xlsx");
//        }
//        public ActionResult DescargarEgresados(int? creado)
//        {
//            bool success = false;
//            var Alumnos = db.Alumnos.ToList();
//            if (creado==1)
//            {
//                var ruta = "~/Upload/Temporales/Descargas/Egresados.xlsx";
//                return File(Url.Content(ruta), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Egresados.xlsx");
//            }

//            if (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/Egresados.xlsx")))
//            {
//                System.IO.File.Delete(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/Egresados.xlsx"));
//            }

//            string savedFileName = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/Egresados.xlsx"));
//            //FileStream stream = System.IO.File.Create(savedFileName);
//            using (FileStream stream = new FileStream(savedFileName, FileMode.Create))
//            {
//                try
//                {
//                    using (ExcelPackage excel = new ExcelPackage())
//                    {
//                        var count = 2;
//                        ExcelWorksheet ws = excel.Workbook.Worksheets.Add("Egresados");

//                        ws.Cells["A1"].Value = "ID";
//                        ws.Cells["B1"].Value = "Nombre completo";
//                        ws.Cells["C1"].Value = "Nombre preferido";
//                        ws.Cells["D1"].Value = "Nombre alternativo";
//                        ws.Cells["E1"].Value = "periodo egreso";
//                        ws.Cells["F1"].Value = "Periodo graduación";
//                        ws.Cells["G1"].Value = "Carrera";
//                        ws.Cells["H1"].Value = "Nivel";
//                        ws.Cells["I1"].Value = "Escuela";
//                        ws.Cells["J1"].Value = "Generación";
//                        ws.Cells["K1"].Value = "Fecha graduación esperada";
//                        ws.Cells["L1"].Value = "Año graduación";
//                        ws.Cells["M1"].Value = "Titulo";
//                        ws.Cells["N1"].Value = "Campus";
//                        ws.Cells["O1"].Value = "Código preparatoria";
//                        ws.Cells["P1"].Value = "Nombre de la preparatoria";
//                        ws.Cells["Q1"].Value = "Fecha de Nacimiento";
//                        ws.Cells["R1"].Value = "Sexo";
//                        ws.Cells["S1"].Value = "Estado Civil";
//                        ws.Cells["T1"].Value = "religión";
//                        ws.Cells["U1"].Value = "Falleció";
//                        ws.Cells["V1"].Value = "Credencial Ga";
//                        ws.Cells["W1"].Value = "Benefator";
//                        ws.Cells["X1"].Value = "Fecha trámite";
//                        ws.Cells["Y1"].Value = "Titulado/Pasante";
//                        ws.Cells["Z1"].Value = "Fecha observaciones";
//                        ws.Cells["AA1"].Value = "Usuario observaciones";
//                        ws.Cells["AB1"].Value = "Observaciones";
//                        ws.Cells["AC1"].Value = "fecha última act";
//                        ws.Cells["AD1"].Value = "Se actualizó con";
//                        ws.Cells["AE1"].Value = "Tel. actualización";
//                        ws.Cells["AF1"].Value = "Tel. fon";
//                        ws.Cells["AG1"].Value = "Correo Egpr";
//                        ws.Cells["AH1"].Value = "Otros correos";
//                        ws.Cells["AI1"].Value = "Autoriza act. por email";
//                        ws.Cells["AJ1"].Value = "Fecha próx. contacto";
//                        ws.Cells["AK1"].Value = "Hr. próx. contacto";
//                        ws.Cells["AL1"].Value = "Observaciones";
//                        ws.Cells["AM1"].Value = "¿Trabaja?";
//                        ws.Cells["AN1"].Value = "Limpiar hist.";
//                        ws.Cells["AO1"].Value = "Estudia postgrado";
//                        ws.Cells["AP1"].Value = "Realiza serv. soc.";
//                        ws.Cells["AQ1"].Value = "Ama de casa";
//                        ws.Cells["AR1"].Value = "Desempleado";
//                        ws.Cells["AS1"].Value = "Teléfono oficina";
//                        ws.Cells["AT1"].Value = "Teléfono celular";
//                        ws.Cells["AU1"].Value = "Teléfono particular";
//                        ws.Cells["AV1"].Value = "Tipo dirección";
//                        ws.Cells["AW1"].Value = "Calle";
//                        ws.Cells["AX1"].Value = "Número";
//                        ws.Cells["AY1"].Value = "Colonia";
//                        ws.Cells["AZ1"].Value = "Ciudad";
//                        ws.Cells["BA1"].Value = "Estado";
//                        ws.Cells["BB1"].Value = "País";
//                        ws.Cells["BC1"].Value = "CP";
//                        ws.Cells["BD1"].Value = "Código reparto";
//                        ws.Cells["BE1"].Value = "NOmbre de la empresa";
//                        ws.Cells["BF1"].Value = "Giro";
//                        ws.Cells["BG1"].Value = "Subgiro";
//                        ws.Cells["BH1"].Value = "Propia/Familiar/Empleado";
//                        ws.Cells["BI1"].Value = "Nivel puesto";
//                        ws.Cells["BJ1"].Value = "Descripción del  puesto";
//                        ws.Cells["BK1"].Value = "Giro del puesto";
//                        ws.Cells["BL1"].Value = "Fecha inicio";
//                        ws.Cells["BM1"].Value = "Teléfono 1";
//                        ws.Cells["BN1"].Value = "Teléfono 2";
//                        ws.Cells["BO1"].Value = "Usuario bolsa trab.";
//                        ws.Cells["BP1"].Value = "Fecha ingreso a BT";
//                        ws.Cells["BQ1"].Value = "Asociación evento";
//                        ws.Cells["BR1"].Value = "Tipo de participación";
//                        ws.Cells["BS1"].Value = "Observaciones";
//                        ws.Cells["BT1"].Value = "¿Puede donar?";
//                        ws.Cells["BU1"].Value = "¿Desea participar?";
//                        ws.Cells["BV1"].Value = "Tipo de sangre";
//                        ws.Cells["BW1"].Value = "Observaciones_3";
//                        ws.Cells["BX1"].Value = "Revista";
//                        ws.Cells["BY1"].Value = "Eventos";
//                        ws.Cells["BZ1"].Value = "Boletín egresados";
//                        ws.Cells["CA1"].Value = "Boletín carrera";
//                        ws.Cells["CB1"].Value = "Apostolados";
//                        ws.Cells["CC1"].Value = "Cred. egresado";
//                        ws.Cells["CD1"].Value = "Correspondencia";
//                        ws.Cells["CE1"].Value = "Observaciones phonaton";
//                        ws.Cells["CF1"].Value = "Vuelta";
//                        ws.Cells["CG1"].Value = "Primera llamada";
//                        ws.Cells["CH1"].Value = "Fecha próx. contacto donación";
//                        ws.Cells["CI1"].Value = "Hr. próx. contacto donación";
//                        ws.Cells["CJ1"].Value = "Fecha próx. cobro";
//                        ws.Cells["CK1"].Value = "Motivo de negación";
//                        ws.Cells["CL1"].Value = "Estatus donador";
//                        ws.Cells["CM1"].Value = "Recaudador";
//                        ws.Cells["CN1"].Value = "Fecha reg. actualizar";
//                        ws.Cells["CO1"].Value = "NUM";
//                        ws.Cells["CP1"].Value = "Nombre";
//                        ws.Cells["A1:CP1"].Style.Font.Bold = true;

//                        for (var c = 1; c <= 94; c++)
//                        {
//                            if (c == 33 || c == 34 || c == 45 || c == 46 || c == 47)
//                            {
//                                ws.Column(c).Width = 60;
//                            }
//                            else if(c==2 || c==7 || c==58 ||c==59 ||c==60 ||c==61 ||c==62)
//                            {
//                                ws.Column(c).Width = 40;
//                            }else
//                            {
//                                ws.Column(c).Width = 20;
//                            }
//                        }
//                        //ws.Column(1).Width = 20;
//                        //ws.Column(2).Width = 40;

//                        foreach (var item in Alumnos)
//                        {
//                            try
//                            {
//                                for (var col = 0; col <= 94; col++)
//                                {
                                    
//                                    string telefonos = string.Empty;
//                                    string correos = string.Empty;

//                                    item.Personas.MediosContactos.ToList().ForEach(me =>
//                                    {
//                                        if (me.idTipoMedioContacto == 1)
//                                        {
//                                            correos += (correos.Length > 0) ? ", " + me.desMedioContacto : me.desMedioContacto;
//                                        }
//                                        else
//                                        {
//                                            telefonos += (telefonos.Length > 0) ? ", " + me.desMedioContacto : me.desMedioContacto;
//                                        }
//                                    });
//                                    ws.Cells[count, 1].Value = item.RelAlumnosMatriculas.FirstOrDefault().noMatricula;
//                                    ws.Cells[count, 2].Value = item.Personas.nbCompleto;
//                                    ws.Cells[count, 7].Value = item.FormacionesAcademicas.OrderBy(x=>x.idFormacionAcademica).FirstOrDefault().Licenciaturas.nbLicenciatura;
//                                    //ws.Cells[count, 13].Value = nivelColegio;
//                                    ws.Cells[count, 14].Value = (item.FormacionesAcademicas.OrderBy(x => x.idFormacionAcademica).FirstOrDefault().idColegio==1)?"UAM":"OTRO";
//                                    ws.Cells[count, 17].Value = item.feNacimiento;
//                                    ws.Cells[count, 18].Value = item.Personas.CatGeneros.nbGenero;
//                                    ws.Cells[count, 19].Value = item.CatEstadosCiviles.nbEstadoCivil;
//                                    ws.Cells[count, 20].Value = item.CatReligion.nbReligion;
//                                    ws.Cells[count, 21].Value = (item.fgFallecido==true)?"S":"N";
//                                    ws.Cells[count, 25].Value = item.tpEgresado;
//                                    var CorreoP = string.Empty;
//                                    var telOfi = string.Empty;
//                                    var telMov = string.Empty;
//                                    var telPer = string.Empty;
//                                    if (correos != "")
//                                    {
//                                        var ListaCorreos = correos.Split(',');
//                                        if (ListaCorreos.Length>0)
//                                        {
//                                            CorreoP = ListaCorreos[0].ToString();
//                                        }
//                                    }
//                                    if (telefonos!="")
//                                    {
//                                        var ListaTelefonos = telefonos.Split(',');
//                                        for (var i= 0; i<ListaTelefonos.Length;i++)
//                                        {
//                                            if (i==0 )
//                                            {
//                                                telOfi = ListaTelefonos[i].ToString();
//                                            }
//                                            else if (i==1)
//                                            {
//                                                telMov = ListaTelefonos[i].ToString();
//                                            }
//                                            else if (i==2)
//                                            {
//                                                telPer = ListaTelefonos[i].ToString();
//                                            }
//                                        }
                                       
//                                    }
//                                    ws.Cells[count, 33].Value = CorreoP;
//                                    ws.Cells[count, 34].Value = correos;
//                                    ws.Cells[count, 39].Value = (item.fgTrabajaActualmente==true)?"S":"N";
//                                    ws.Cells[count, 45].Value = telOfi;
//                                    ws.Cells[count, 46].Value = telMov;
//                                    ws.Cells[count, 47].Value = telPer;
//                                    ws.Cells[count, 49].Value = item.Direcciones.desDireccion;
//                                    ws.Cells[count, 51].Value = item.Direcciones.nbColonia;
//                                    ws.Cells[count, 52].Value = item.Direcciones.nbCiudad;
//                                    ws.Cells[count, 53].Value = item.Direcciones.nbEstado;
//                                    ws.Cells[count, 54].Value = item.Direcciones.nbPais;
//                                    ws.Cells[count, 55].Value = item.Direcciones.numCodigoPostal;
//                                    ws.Cells[count, 57].Value = (item.ExperienciasProfesionales.OrderBy(x => x.idExperienciaProfesional).FirstOrDefault()!=null)?item.ExperienciasProfesionales.OrderBy(x => x.idExperienciaProfesional).FirstOrDefault().nbEmpresa:"";
//                                    ws.Cells[count, 58].Value = (item.ExperienciasProfesionales.OrderBy(x => x.idExperienciaProfesional).FirstOrDefault() != null) ? item.ExperienciasProfesionales.OrderBy(x => x.idExperienciaProfesional).FirstOrDefault().CatAreas.nbArea : "";
//                                    ws.Cells[count, 60].Value = (item.ExperienciasProfesionales.OrderBy(x => x.idExperienciaProfesional).FirstOrDefault() != null) ? item.ExperienciasProfesionales.OrderBy(x => x.idExperienciaProfesional).FirstOrDefault().CatTipoEmpresa.nbTipoEmpresa:"";
//                                    ws.Cells[count, 61].Value = (item.ExperienciasProfesionales.OrderBy(x => x.idExperienciaProfesional).FirstOrDefault() != null) ? item.ExperienciasProfesionales.OrderBy(x => x.idExperienciaProfesional).FirstOrDefault().tpPuesto:"";
//                                    ws.Cells[count, 62].Value = (item.ExperienciasProfesionales.OrderBy(x => x.idExperienciaProfesional).FirstOrDefault() != null) ? item.ExperienciasProfesionales.OrderBy(x => x.idExperienciaProfesional).FirstOrDefault().nbPuesto:""; 
//                                    ws.Cells[count, 72].Value = (item.fgDonarSangre == true) ? "S" : "N";
//                                    ws.Cells[count, 74].Value = item.CatTiposSangre.nbTipoSangre;

//                                }
//                                count++;
//                            }
//                            catch (Exception ex) { }
//                        }
                        
//                        excel.SaveAs(stream);
//                        success = true;
//                    }
//                    stream.Close();
//                    stream.Dispose();
//                }
//                catch (Exception)
//                {
//                    stream.Close();
//                    stream.Dispose();
//                }
//            }

//            return Json(new { success = success});
//        }
//    }
//}