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
    public class InventarioUnidadesViewController : Controller
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
        public ActionResult SaveEmpresa(inventario_unidades Empresas)
        {
            //Empresa empresas = new Empresa();
            var serializerCat = new JavaScriptSerializer();
            bool success = false;
            string mensajefound = "";
            //linea pendiente de revision

            var found = db.inventario_unidades.FirstOrDefault(x => x.numero_economico_unidad == Empresas.numero_economico_unidad && x.inv_unidad_ID != Empresas.inv_unidad_ID);
            //var found = db.Empresa.FirstOrDefault(x => x.Em_Descripcion == Empresas.Em_Descripcion && x.Em_Cve_Empresa != Empresas.Em_Cve_Empresa);

            if (string.IsNullOrWhiteSpace(Empresas.numero_economico_unidad))
            {
                mensajefound = "¡no puede estar en blanco el Numero de economico de la unidad!";
            }
            else
            {


                if (found != null)
                {
                    mensajefound = "¡Ya existe una unidad que coincide con el numero economico ingresado!";

                }
                else
                {
                    if (Empresas.inv_unidad_ID == 0)
                    {
                        try
                        {
                            inventario_unidades empre = new inventario_unidades();
                            empre.tipo_caja = Empresas.tipo_caja;
                            empre.marca_chasis = Empresas.marca_chasis;
                            empre.tipo_de_caja = Empresas.tipo_de_caja;
                            empre.modelo_capacidad = Empresas.modelo_capacidad;
                            empre.anio_chasis = Empresas.anio_chasis;
                            empre.numero_motor = Empresas.numero_motor;
                            empre.numero_serie_chasis = Empresas.numero_serie_chasis;
                            empre.anio_adaptacion = Empresas.anio_adaptacion;
                            empre.numero_serie_adaptacion = Empresas.numero_serie_adaptacion;
                            //empre.numero_economico_unidad = "N " + Empresas.numero_economico_unidad;
                            empre.numero_economico_unidad = Empresas.numero_economico_unidad;
                            empre.sistema_levante = Empresas.sistema_levante;
                            empre.duplicado_llaves = Empresas.duplicado_llaves;
                            empre.placas_nuevas = Empresas.placas_nuevas;
                            empre.empresa_chasis = Empresas.empresa_chasis;
                            empre.empresa_adaptación = Empresas.empresa_adaptación;
                            empre.empresa_gps = Empresas.empresa_gps;
                            empre.imei_gps = Empresas.imei_gps;
                            empre.ceco = Empresas.ceco;
                            empre.compania_aseguradora = Empresas.compania_aseguradora;
                            empre.poliza = Empresas.poliza;
                            empre.inciso = Empresas.inciso;
                            empre.inicio_vigencia = Empresas.inicio_vigencia;
                            empre.fin_vigencia = Empresas.fin_vigencia;
                            empre.numero_gasomatic = Empresas.numero_gasomatic;
                            empre.permiso_estatal = Empresas.permiso_estatal;

                            //datos definidos por el sistema
                            empre.estatus_ID = 1;
                            empre.usuario_app_ID = null;
                            empre.created_time_actuales = DateTime.Now;
                            empre.Em_Cve_Empresa = Empresas.Em_Cve_Empresa;

                            //definidos temporalmente
                            empre.url_pdf_poliza = "ffff";
                            empre.type_poliza = "gggg";
                            empre.url_pdf_tarjeta = "zzzz";
                            empre.type_tarjeta = "yyyy";


                            db.inventario_unidades.Add(empre);
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
                            inventario_unidades Empre = db.inventario_unidades.Find(Empresas.inv_unidad_ID);
                            Empre.tipo_caja = Empresas.tipo_caja;
                            Empre.marca_chasis = Empresas.marca_chasis;
                            Empre.tipo_de_caja = Empresas.tipo_de_caja;
                            Empre.modelo_capacidad = Empresas.modelo_capacidad;
                            Empre.anio_chasis = Empresas.anio_chasis;
                            Empre.numero_motor = Empresas.numero_motor;
                            Empre.numero_serie_chasis = Empresas.numero_serie_chasis;
                            Empre.anio_adaptacion = Empresas.anio_adaptacion;
                            Empre.numero_serie_adaptacion = Empresas.numero_serie_adaptacion;
                            //empre.numero_economico_unidad = "N " + Empresas.numero_economico_unidad;
                            Empre.numero_economico_unidad = Empresas.numero_economico_unidad;
                            Empre.sistema_levante = Empresas.sistema_levante;
                            Empre.duplicado_llaves = Empresas.duplicado_llaves;
                            Empre.placas_nuevas = Empresas.placas_nuevas;
                            Empre.empresa_chasis = Empresas.empresa_chasis;
                            Empre.empresa_adaptación = Empresas.empresa_adaptación;
                            Empre.empresa_gps = Empresas.empresa_gps;
                            Empre.imei_gps = Empresas.imei_gps;
                            Empre.ceco = Empresas.ceco;
                            Empre.compania_aseguradora = Empresas.compania_aseguradora;
                            Empre.poliza = Empresas.poliza;
                            Empre.inciso = Empresas.inciso;
                            Empre.inicio_vigencia = Empresas.inicio_vigencia;
                            Empre.fin_vigencia = Empresas.fin_vigencia;
                            Empre.numero_gasomatic = Empresas.numero_gasomatic;
                            Empre.permiso_estatal = Empresas.permiso_estatal;
                            Empre.Em_Cve_Empresa = Empresas.Em_Cve_Empresa;

                            //definidos temporalmente
                            Empre.url_pdf_poliza = "ffff";
                            Empre.type_poliza = "gggg";
                            Empre.url_pdf_tarjeta = "zzzz";
                            Empre.type_tarjeta = "yyyy";


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

        public ActionResult _FormularioEmpresa(long? inv_unidad_ID)
        {  //codigo para agregar y editar usuarios
            //UsuariosPersonas Personas = new UsuariosPersonas();
            inventario_unidades Empresas = new inventario_unidades();
           // Personas Persona = new Personas();

            if (inv_unidad_ID != null)
            {
                ViewBag.edit = 1;
                Empresas = db.inventario_unidades.Find(inv_unidad_ID);
                //añadir controlador de listas

                List<SelectListItem> items5 = new List<SelectListItem>();
                items5.Add(new SelectListItem { Value = "COMPACTADOR", Text = "COMPACTADOR" });
                items5.Add(new SelectListItem { Value = "COMPACTADOR ROLL-OFF", Text = "COMPACTADOR ROLL-OFF" });
                items5.Add(new SelectListItem { Value = "COMPACTADOR SUPER RABON", Text = "COMPACTADOR SUPER RABON" });
                items5.Add(new SelectListItem { Value = "RECOLECTOR TIPO VOLTEO HIDRAULICO", Text = "RECOLECTOR TIPO VOLTEO HIDRAULICO" });
                items5.Add(new SelectListItem { Value = "ROLL-OFF", Text = "ROLL-OFF" });
                items5.Add(new SelectListItem { Value = "VOLQUETE", Text = "VOLQUETE" });

                if (Empresas.tipo_caja != null)
                {
                    ViewBag.tipo_unidad = new SelectList(items5, "Value", "Text", Empresas.tipo_caja);
                }
                else
                {
                    ViewBag.tipo_unidad = new SelectList(items5, "Value", "Text");
                }


                List<SelectListItem> items1 = new List<SelectListItem>();
                items1.Add(new SelectListItem { Value = "INTERNATIONAL", Text = "INTERNATIONAL" });
                items1.Add(new SelectListItem { Value = "HINO", Text = "HINO" });

                if(Empresas.marca_chasis != null){
                    ViewBag.region_ID = new SelectList(items1, "Value", "Text", Empresas.marca_chasis);
                }
                else
                {
                    ViewBag.region_ID = new SelectList(items1, "Value", "Text");
                }


                List<SelectListItem> items4 = new List<SelectListItem>();
                items4.Add(new SelectListItem { Value = "CAJA DE VOLTEO", Text = "CAJA DE VOLTEO" });
                items4.Add(new SelectListItem { Value = "CUMMINS-MCNILIUS", Text = "CUMMINS-MCNILIUS" });
                items4.Add(new SelectListItem { Value = "MCNILIUS", Text = "MCNILIUS" });
                items4.Add(new SelectListItem { Value = "REPSA", Text = "REPSA" });
                items4.Add(new SelectListItem { Value = "ROLL OFF", Text = "ROLL OFF" });
                //ViewBag.region_ID = new SelectList(items1, "Value", "Text", Empresas.region);

                if (Empresas.tipo_de_caja != null)
                {
                    ViewBag.tipocaja = new SelectList(items4, "Value", "Text", Empresas.tipo_de_caja);
                }
                else
                {
                    ViewBag.tipocaja = new SelectList(items4, "Value", "Text");
                }


                List<SelectListItem> items3 = new List<SelectListItem>();
                items3.Add(new SelectListItem { Value = "CHASIS CABINA 4300-210 DURASTAR 11T V6 STD CON COMPACTADOR", Text = "CHASIS CABINA 4300-210 DURASTAR 11T V6 STD CON COMPACTADOR" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA 4300-215 11T V6 STD 2 D/T SA SE FM SB", Text = "CHASIS CABINA 4300-215 11T V6 STD 2 D/T SA SE FM SB" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA 4300-215 DURASTAR 11T V6 STD CON COMPACTADOR", Text = "CHASIS CABINA 4300-215 DURASTAR 11T V6 STD CON COMPACTADOR" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA DHV607 27T V6 IMP STD 2ABS CON CAJA ROLL OFF", Text = "CHASIS CABINA DHV607 27T V6 IMP STD 2ABS CON CAJA ROLL OFF" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA DHV607 27T V6 IMP STD 2ABS CON CAJA VOLTEO REPSA (Volquete)", Text = "CHASIS CABINA DHV607 27T V6 IMP STD 2ABS CON CAJA VOLTEO REPSA (Volquete)" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA INTERNATIONAL DURASTAR 4300 MODELO 2014", Text = "CHASIS CABINA INTERNATIONAL DURASTAR 4300 MODELO 2014" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA INTERNATIONAL DURASTAR 4300 MODELO 2016", Text = "CHASIS CABINA INTERNATIONAL DURASTAR 4300 MODELO 2016" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA INTERNATIONAL DURASTAR MAXFORCED MODELO 4400-260", Text = "CHASIS CABINA INTERNATIONAL DURASTAR MAXFORCED MODELO 4400-260" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA INTERNATIONAL MAXXFORCE-9 TURBO MODELO 4400-310", Text = "CHASIS CABINA INTERNATIONAL MAXXFORCE-9 TURBO MODELO 4400-310" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA INTERNATIONAL MODELO 4400-300", Text = "CHASIS CABINA INTERNATIONAL MODELO 4400-300" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA INTERNATIONAL NUEVO MODELO 4300-210 DURASTAR", Text = "CHASIS CABINA INTERNATIONAL NUEVO MODELO 4300-210 DURASTAR" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA INTERNATIONAL NUEVO MODELO 4400-310 DURASTAR", Text = "CHASIS CABINA INTERNATIONAL NUEVO MODELO 4400-310 DURASTAR" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA INTERNATIONAL NUEVO MODELO CITYSTAR CLASE 5 ", Text = "CHASIS CABINA INTERNATIONAL NUEVO MODELO CITYSTAR CLASE 5 " });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA INTERNATIONAL NUEVO, MODELO 4400-310 DURASTAR, CAJA 14 MTRS", Text = "CHASIS CABINA INTERNATIONAL NUEVO, MODELO 4400-310 DURASTAR, CAJA 14 MTRS" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA MV CUMMINS ISB EURO V 6CIL DIESEL", Text = "CHASIS CABINA MV CUMMINS ISB EURO V 6CIL DIESEL" });
                items3.Add(new SelectListItem { Value = "EQ INTER-NAVIS 4300-210 VT466 5 VEL CHASIS STD MAS 7.5 Y HASTA 14 T", Text = "EQ INTER-NAVIS 4300-210 VT466 5 VEL CHASIS STD MAS 7.5 Y HASTA 14 T" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA MARCA HINO SERIE 300, TIPO CAB OVER ENGINE ESTANDAR 3 PASAJEROS MODELO 616 SEMI", Text = "CHASIS CABINA MARCA HINO SERIE 300, TIPO CAB OVER ENGINE ESTANDAR 3 PASAJEROS MODELO 616 SEMI" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA MARCA HINO SERIE 500, modelo 1018G 2022 EURO 5 TIPO CAB OVER ENGINE ESTÁNDAR, 3 PASAJE", Text = "CHASIS CABINA MARCA HINO SERIE 500, modelo 1018G 2022 EURO 5 TIPO CAB OVER ENGINE ESTÁNDAR, 3 PASAJE" });
                //agregados el 27-06-2023
                items3.Add(new SelectListItem { Value = "CHASIS CABINA NUEVO MARCA HINO SERIE 500, TIPO CAB OVER ENGINE EXTENDIDA 3 PASAJEROS AÑO 2024", Text = "CHASIS CABINA NUEVO MARCA HINO SERIE 500, TIPO CAB OVER ENGINE EXTENDIDA 3 PASAJEROS AÑO 2024" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA NUEVO MARCA HINO SERIE 500, TIPO CAB OVER ENGINE EXTENDIDA 3 PASAJEROS AÑO 2023", Text = "CHASIS CABINA NUEVO MARCA HINO SERIE 500, TIPO CAB OVER ENGINE EXTENDIDA 3 PASAJEROS AÑO 2023" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA INTERNATIONAL NUEVO, MODELO MV 4X2 AÑO MODELO 2024 CUMMINS ISB V, DE 6 CILINDROS DIESE", Text = "CHASIS CABINA INTERNATIONAL NUEVO, MODELO MV 4X2 AÑO MODELO 2024 CUMMINS ISB V, DE 6 CILINDROS DIESE" });


                if (Empresas.modelo_capacidad != null){
                    ViewBag.capacidad = new SelectList(items3, "Value", "Text", Empresas.modelo_capacidad);
                }
                else
                {
                    ViewBag.capacidad = new SelectList(items3, "Value", "Text");
                }


                List<SelectListItem> items6 = new List<SelectListItem>();
                items6.Add(new SelectListItem { Value = "SI", Text = "SI" });
                items6.Add(new SelectListItem { Value = "NO", Text = "NO" });
              
                if (Empresas.sistema_levante != null)
                {
                    ViewBag.levante = new SelectList(items6, "Value", "Text", Empresas.sistema_levante);
                }
                else
                {
                    ViewBag.levante = new SelectList(items6, "Value", "Text");
                }


                List<SelectListItem> items7 = new List<SelectListItem>();
                items7.Add(new SelectListItem { Value = "Si", Text = "Si" });
                items7.Add(new SelectListItem { Value = "No", Text = "No" });
               
                if (Empresas.duplicado_llaves != null)
                {
                    ViewBag.duplicado = new SelectList(items7, "Value", "Text",Empresas.duplicado_llaves);
                }
                else
                {
                    ViewBag.duplicado = new SelectList(items7, "Value", "Text");
                }

                List<SelectListItem> items8 = new List<SelectListItem>();
                items8.Add(new SelectListItem { Value = "ARREND ACTINVER", Text = "ARREND ACTINVER" });
                items8.Add(new SelectListItem { Value = "ARREND BEPENSA LEASING", Text = "ARREND BEPENSA LEASING" });
                items8.Add(new SelectListItem { Value = "ARSESA", Text = "ARSESA" });
                items8.Add(new SelectListItem { Value = "ATLAS", Text = "ATLAS" });
                items8.Add(new SelectListItem { Value = "ATLAS LEASEBACK", Text = "ATLAS LEASEBACK" });
                items8.Add(new SelectListItem { Value = "FINBE/SANA", Text = "FINBE/SANA" });
                items8.Add(new SelectListItem { Value = "SANA", Text = "SANA" });
                items8.Add(new SelectListItem { Value = "SANA OK", Text = "SANA OK" });


                if (Empresas.empresa_chasis != null)
                {
                    ViewBag.empre_chasis = new SelectList(items8, "Value", "Text",Empresas.empresa_chasis);
                }
                else
                {
                    ViewBag.empre_chasis = new SelectList(items8, "Value", "Text");
                }

                List<SelectListItem> items9 = new List<SelectListItem>();
                items9.Add(new SelectListItem { Value = "ARREND ACTINVER", Text = "ARREND ACTINVER" });
                items9.Add(new SelectListItem { Value = "ARREND BEPENSA LEASING", Text = "ARREND BEPENSA LEASING" });
                items9.Add(new SelectListItem { Value = "ARSESA", Text = "ARSESA" });
                items9.Add(new SelectListItem { Value = "ATLAS", Text = "ATLAS" });
                items9.Add(new SelectListItem { Value = "ATLAS LEASEBACK", Text = "ATLAS LEASEBACK" });
                items9.Add(new SelectListItem { Value = "FINBE/SANA", Text = "FINBE/SANA" });
                items9.Add(new SelectListItem { Value = "SANA", Text = "SANA" });
                items9.Add(new SelectListItem { Value = "SANA OK", Text = "SANA OK" });
                
                
                if (Empresas.empresa_adaptación != null)
                {
                    ViewBag.adaptacion = new SelectList(items9, "Value", "Text", Empresas.empresa_adaptación);
                }
                else
                {
                    ViewBag.adaptacion = new SelectList(items9, "Value", "Text");
                }

                List<SelectListItem> items10 = new List<SelectListItem>();
                items10.Add(new SelectListItem { Value = "AT&T", Text = "AT&T" });
                items10.Add(new SelectListItem { Value = "N/A", Text = "N/A" });
              
                if (Empresas.empresa_gps!= null)
                {
                    ViewBag.gps_empr = new SelectList(items10, "Value", "Text", Empresas.empresa_gps);
                }
                else
                {
                    ViewBag.gps_empr = new SelectList(items10, "Value", "Text");
                }

                List<SelectListItem> items11 = new List<SelectListItem>();
                items11.Add(new SelectListItem { Value = "CTS CORBASE", Text = "CTS CORBASE" });
                items11.Add(new SelectListItem { Value = "CTS DOMICILIAR", Text = "CTS DOMICILIAR" });
                items11.Add(new SelectListItem { Value = "CTS COMERCIAL", Text = "CTS COMERCIAL" });
                items11.Add(new SelectListItem { Value = "CTS KANASIN", Text = "CTS KANASIN" });
                items11.Add(new SelectListItem { Value = "CTS OXKUTZCAB", Text = "CTS OXKUTZCAB" });
                items11.Add(new SelectListItem { Value = "CTS RELLENO SANITARIO", Text = "CTS RELLENO SANITARIO" });
                items11.Add(new SelectListItem { Value = "GASTOS COMERCIAL", Text = "GASTOS COMERCIAL" });
                items11.Add(new SelectListItem { Value = "GTS CORPORATIVOS", Text = "GTS CORPORATIVOS" });
                items11.Add(new SelectListItem { Value = "GASTOS ADMON", Text = "GASTOS ADMON" });
                items11.Add(new SelectListItem { Value = "GASTOS OPERACION", Text = "GASTOS OPERACION" });
                items11.Add(new SelectListItem { Value = "GASTOS ADMON KANASIN", Text = "GASTOS ADMON KANASIN" });
                items11.Add(new SelectListItem { Value = "CTS DOMICILIAR CORBASE", Text = "CTS DOMICILIAR CORBASE" });
                items11.Add(new SelectListItem { Value = "GASTOS ADMON CORBASE", Text = "GASTOS ADMON CORBASE" });
                items11.Add(new SelectListItem { Value = "CTS UMAN", Text = "CTS UMAN" });
                

                if (Empresas.ceco != null)
                {
                    ViewBag.costo = new SelectList(items11, "Value", "Text", Empresas.ceco);
                }
                else
                {
                    ViewBag.costo = new SelectList(items11, "Value", "Text");
                }


                List<SelectListItem> items12 = new List<SelectListItem>();
                items12.Add(new SelectListItem { Value = "BANORTE SEGUROS", Text = "BANORTE SEGUROS" });
                items12.Add(new SelectListItem { Value = "CHUUB SEGUROS", Text = "CHUUB SEGUROS" });
                items12.Add(new SelectListItem { Value = "N/A", Text = "N/A" });
                //ViewBag.aseguradora = new SelectList(items12, "Value", "Text");

                if (Empresas.compania_aseguradora != null)
                {
                    ViewBag.aseguradora = new SelectList(items12, "Value", "Text", Empresas.compania_aseguradora);
                }
                else
                {
                    ViewBag.aseguradora = new SelectList(items12, "Value", "Text");
                }


                List<SelectListItem> items2 = new List<SelectListItem>();
                items2.Add(new SelectListItem { Value = "SI" , Text = "SI" });
                items2.Add(new SelectListItem{ Value = "NO", Text = "NO" });
                
                if (Empresas.permiso_estatal != null)
                {
                    ViewBag.adendum = new SelectList(items2,"Value","Text", Empresas.permiso_estatal);
                }
                else
                {
                    ViewBag.adendum = new SelectList(items2, "Value", "Text");
                }


                if (Empresas.Em_Cve_Empresa != 0)
                {
                    ViewBag.lista_empresa = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion", Empresas.Em_Cve_Empresa);
                }
                else
                {
                    ViewBag.lista_empresa = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion");
                }


            }

            else
            {
                List<SelectListItem> items1 = new List<SelectListItem>();
                items1.Add(new SelectListItem { Value = "INTERNATIONAL", Text = "INTERNATIONAL" });
                items1.Add(new SelectListItem { Value = "HINO", Text = "HINO" });
                ViewBag.region_ID = new SelectList(items1, "Value", "Text");


                List<SelectListItem> items2 = new List<SelectListItem>();
                items2.Add(new SelectListItem { Value = "SI", Text = "SI" });
                items2.Add(new SelectListItem { Value = "NO", Text = "NO" });
                ViewBag.adendum = new SelectList(items2, "Value", "Text");
                

                List<SelectListItem> items3 = new List<SelectListItem>();
                items3.Add(new SelectListItem { Value = "CHASIS CABINA 4300-210 DURASTAR 11T V6 STD CON COMPACTADOR", Text = "CHASIS CABINA 4300-210 DURASTAR 11T V6 STD CON COMPACTADOR" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA 4300-215 11T V6 STD 2 D/T SA SE FM SB", Text = "CHASIS CABINA 4300-215 11T V6 STD 2 D/T SA SE FM SB" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA 4300-215 DURASTAR 11T V6 STD CON COMPACTADOR", Text = "CHASIS CABINA 4300-215 DURASTAR 11T V6 STD CON COMPACTADOR" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA DHV607 27T V6 IMP STD 2ABS CON CAJA ROLL OFF", Text = "CHASIS CABINA DHV607 27T V6 IMP STD 2ABS CON CAJA ROLL OFF" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA DHV607 27T V6 IMP STD 2ABS CON CAJA VOLTEO REPSA (Volquete)", Text = "CHASIS CABINA DHV607 27T V6 IMP STD 2ABS CON CAJA VOLTEO REPSA (Volquete)" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA INTERNATIONAL DURASTAR 4300 MODELO 2014", Text = "CHASIS CABINA INTERNATIONAL DURASTAR 4300 MODELO 2014" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA INTERNATIONAL DURASTAR 4300 MODELO 2016", Text = "CHASIS CABINA INTERNATIONAL DURASTAR 4300 MODELO 2016" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA INTERNATIONAL DURASTAR MAXFORCED MODELO 4400-260", Text = "CHASIS CABINA INTERNATIONAL DURASTAR MAXFORCED MODELO 4400-260" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA INTERNATIONAL MAXXFORCE-9 TURBO MODELO 4400-310", Text = "CHASIS CABINA INTERNATIONAL MAXXFORCE-9 TURBO MODELO 4400-310" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA INTERNATIONAL MODELO 4400-300", Text = "CHASIS CABINA INTERNATIONAL MODELO 4400-300" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA INTERNATIONAL NUEVO MODELO 4300-210 DURASTAR", Text = "CHASIS CABINA INTERNATIONAL NUEVO MODELO 4300-210 DURASTAR" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA INTERNATIONAL NUEVO MODELO 4400-310 DURASTAR", Text = "CHASIS CABINA INTERNATIONAL NUEVO MODELO 4400-310 DURASTAR" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA INTERNATIONAL NUEVO MODELO CITYSTAR CLASE 5 ", Text = "CHASIS CABINA INTERNATIONAL NUEVO MODELO CITYSTAR CLASE 5 " });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA INTERNATIONAL NUEVO, MODELO 4400-310 DURASTAR, CAJA 14 MTRS", Text = "CHASIS CABINA INTERNATIONAL NUEVO, MODELO 4400-310 DURASTAR, CAJA 14 MTRS" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA MV CUMMINS ISB EURO V 6CIL DIESEL", Text = "CHASIS CABINA MV CUMMINS ISB EURO V 6CIL DIESEL" });
                items3.Add(new SelectListItem { Value = "EQ INTER-NAVIS 4300-210 VT466 5 VEL CHASIS STD MAS 7.5 Y HASTA 14 T", Text = "EQ INTER-NAVIS 4300-210 VT466 5 VEL CHASIS STD MAS 7.5 Y HASTA 14 T" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA MARCA HINO SERIE 300, TIPO CAB OVER ENGINE ESTANDAR 3 PASAJEROS MODELO 616 SEMI", Text = "CHASIS CABINA MARCA HINO SERIE 300, TIPO CAB OVER ENGINE ESTANDAR 3 PASAJEROS MODELO 616 SEMI" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA MARCA HINO SERIE 500, modelo 1018G 2022 EURO 5 TIPO CAB OVER ENGINE ESTÁNDAR, 3 PASAJEROS", Text = "CHASIS CABINA MARCA HINO SERIE 500, modelo 1018G 2022 EURO 5 TIPO CAB OVER ENGINE ESTÁNDAR, 3 PASAJEROS" });
                //agregados el 27-06-2023
                items3.Add(new SelectListItem { Value = "CHASIS CABINA NUEVO MARCA HINO SERIE 500, TIPO CAB OVER ENGINE EXTENDIDA 3 PASAJEROS AÑO 2024", Text = "CHASIS CABINA NUEVO MARCA HINO SERIE 500, TIPO CAB OVER ENGINE EXTENDIDA 3 PASAJEROS AÑO 2024" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA NUEVO MARCA HINO SERIE 500, TIPO CAB OVER ENGINE EXTENDIDA 3 PASAJEROS AÑO 2023", Text = "CHASIS CABINA NUEVO MARCA HINO SERIE 500, TIPO CAB OVER ENGINE EXTENDIDA 3 PASAJEROS AÑO 2023" });
                items3.Add(new SelectListItem { Value = "CHASIS CABINA INTERNATIONAL NUEVO, MODELO MV 4X2 AÑO MODELO 2024 CUMMINS ISB V, DE 6 CILINDROS DIESE", Text = "CHASIS CABINA INTERNATIONAL NUEVO, MODELO MV 4X2 AÑO MODELO 2024 CUMMINS ISB V, DE 6 CILINDROS DIESE" });





                ViewBag.capacidad = new SelectList(items3, "Value", "Text");


                List<SelectListItem> items4 = new List<SelectListItem>();
                items4.Add(new SelectListItem { Value = "CAJA DE VOLTEO", Text = "CAJA DE VOLTEO" });
                items4.Add(new SelectListItem { Value = "CUMMINS-MCNILIUS", Text = "CUMMINS-MCNILIUS" });
                items4.Add(new SelectListItem { Value = "MCNILIUS", Text = "MCNILIUS" });
                items4.Add(new SelectListItem { Value = "REPSA", Text = "REPSA" });
                items4.Add(new SelectListItem { Value = "ROLL OFF", Text = "ROLL OFF" });
                //ViewBag.region_ID = new SelectList(items1, "Value", "Text", Empresas.region);

                 ViewBag.tipocaja = new SelectList(items4, "Value", "Text");

                List<SelectListItem> items5 = new List<SelectListItem>();
                items5.Add(new SelectListItem { Value = "COMPACTADOR", Text = "COMPACTADOR" });
                items5.Add(new SelectListItem { Value = "COMPACTADOR ROLL-OFF", Text = "COMPACTADOR ROLL-OFF" });
                items5.Add(new SelectListItem { Value = "COMPACTADOR SUPER RABON", Text = "COMPACTADOR SUPER RABON" });
                items5.Add(new SelectListItem { Value = "RECOLECTOR TIPO VOLTEO HIDRAULICO", Text = "RECOLECTOR TIPO VOLTEO HIDRAULICO" });
                items5.Add(new SelectListItem { Value = "ROLL OFF", Text = "ROLL OFF" });
                items5.Add(new SelectListItem { Value = "VOLQUETE", Text = "VOLQUETE" });
                ViewBag.tipo_unidad = new SelectList(items5, "Value", "Text");


                List<SelectListItem> items6 = new List<SelectListItem>();
                items6.Add(new SelectListItem { Value = "Si", Text = "Si" });
                items6.Add(new SelectListItem { Value = "No", Text = "No" });
                ViewBag.levante = new SelectList(items6, "Value", "Text");


                List<SelectListItem> items7 = new List<SelectListItem>();
                items7.Add(new SelectListItem { Value = "Si", Text = "Si" });
                items7.Add(new SelectListItem { Value = "No", Text = "No" });
                ViewBag.duplicado = new SelectList(items7, "Value", "Text");


                List<SelectListItem> items8 = new List<SelectListItem>();
                items8.Add(new SelectListItem { Value = "ARREND ACTINVER", Text = "ARREND ACTINVER" });
                items8.Add(new SelectListItem { Value = "ARREND BEPENSA LEASING", Text = "ARREND BEPENSA LEASING" });
                items8.Add(new SelectListItem { Value = "ARSESA", Text = "ARSESA" });
                items8.Add(new SelectListItem { Value = "ATLAS", Text = "ATLAS" });
                items8.Add(new SelectListItem { Value = "ATLAS LEASEBACK", Text = "ATLAS LEASEBACK" });
                items8.Add(new SelectListItem { Value = "FINBE/SANA", Text = "FINBE/SANA" });
                items8.Add(new SelectListItem { Value = "SANA", Text = "SANA" });
                items8.Add(new SelectListItem { Value = "SANA OK", Text = "SANA OK" });
                ViewBag.empre_chasis = new SelectList(items8, "Value", "Text");


                List<SelectListItem> items9 = new List<SelectListItem>();
                items9.Add(new SelectListItem { Value = "ARREND ACTINVER", Text = "ARREND ACTINVER" });
                items9.Add(new SelectListItem { Value = "ARREND BEPENSA LEASING", Text = "ARREND BEPENSA LEASING" });
                items9.Add(new SelectListItem { Value = "ARSESA", Text = "ARSESA" });
                items9.Add(new SelectListItem { Value = "ATLAS", Text = "ATLAS" });
                items9.Add(new SelectListItem { Value = "ATLAS LEASEBACK", Text = "ATLAS LEASEBACK" });
                items9.Add(new SelectListItem { Value = "FINBE/SANA", Text = "FINBE/SANA" });
                items9.Add(new SelectListItem { Value = "SANA", Text = "SANA" });
                items9.Add(new SelectListItem { Value = "SANA OK", Text = "SANA OK" });
                ViewBag.adaptacion = new SelectList(items9, "Value", "Text");

                List<SelectListItem> items10 = new List<SelectListItem>();
                items10.Add(new SelectListItem { Value = "AT&T", Text = "AT&T" });
                items10.Add(new SelectListItem { Value = "N/A", Text = "N/A" });
                ViewBag.gps_empr = new SelectList(items10, "Value", "Text");


                List<SelectListItem> items11 = new List<SelectListItem>();
                items11.Add(new SelectListItem { Value = "CTS CORBASE", Text = "CTS CORBASE" });
                items11.Add(new SelectListItem { Value = "CTS DOMICILIAR", Text = "CTS DOMICILIAR" });
                items11.Add(new SelectListItem { Value = "CTS COMERCIAL", Text = "CTS COMERCIAL" });
                items11.Add(new SelectListItem { Value = "CTS KANASIN", Text = "CTS KANASIN" });
                items11.Add(new SelectListItem { Value = "CTS OXKUTZCAB", Text = "CTS OXKUTZCAB" });
                items11.Add(new SelectListItem { Value = "CTS RELLENO SANITARIO", Text = "CTS RELLENO SANITARIO" });
                items11.Add(new SelectListItem { Value = "GASTOS COMERCIAL", Text = "GASTOS COMERCIAL" });
                items11.Add(new SelectListItem { Value = "GTS CORPORATIVOS", Text = "GTS CORPORATIVOS" });
                items11.Add(new SelectListItem { Value = "GASTOS ADMON", Text = "GASTOS ADMON" });
                items11.Add(new SelectListItem { Value = "GASTOS OPERACION", Text = "GASTOS OPERACION" });
                items11.Add(new SelectListItem { Value = "GASTOS ADMON KANASIN", Text = "GASTOS ADMON KANASIN" });
                items11.Add(new SelectListItem { Value = "CTS DOMICILIAR CORBASE", Text = "CTS DOMICILIAR CORBASE" });
                items11.Add(new SelectListItem { Value = "GASTOS ADMON CORBASE", Text = "GASTOS ADMON CORBASE" });
                items11.Add(new SelectListItem { Value = "CTS UMAN", Text = "CTS UMAN" });
                ViewBag.costo = new SelectList(items11, "Value", "Text");


                List<SelectListItem> items12 = new List<SelectListItem>();
                items12.Add(new SelectListItem { Value = "BANORTE SEGUROS", Text = "BANORTE SEGUROS" });
                items12.Add(new SelectListItem { Value = "CHUUB SEGUROS", Text = "CHUUB SEGUROS" });
                items12.Add(new SelectListItem { Value = "N/A", Text = "N/A" });
                ViewBag.aseguradora = new SelectList(items12, "Value", "Text");


                ViewBag.lista_empresa = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion");
            }

            return PartialView(Empresas);
        }

        //metodo listo
        public ActionResult _TablaLineas(int? page, string filtro)
        {
            const int pageSize = 10;
            int pageNumber = (page ?? 1);


            if (string.IsNullOrEmpty(filtro))
            {


                //var lista = db.inventario_lineas.Where(x=> x.estatus_ID == 2).ToList();

                var lista = db.inventario_unidades.OrderByDescending(x=> x.inv_unidad_ID).ToList();
                //var lista = db.inventario_lineas.ToList();
                ViewBag.filtro = filtro;
                return PartialView(lista.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                var lista = db.inventario_unidades.Where(x => x.numero_economico_unidad.ToUpper().Contains(filtro.ToUpper().Trim())
                   || x.tipo_caja.ToUpper().Contains(filtro.ToUpper().Trim()) || x.marca_chasis.ToUpper().Contains(filtro.ToUpper().Trim())
                   || x.tipo_de_caja.ToUpper().Contains(filtro.ToUpper().Trim()) || x.modelo_capacidad.ToUpper().Contains(filtro.ToUpper().Trim())
                   || x.anio_chasis.ToUpper().Contains(filtro.ToUpper().Trim()) || x.numero_motor.ToUpper().Contains(filtro.ToUpper().Trim())
                   || x.numero_serie_chasis.ToUpper().Contains(filtro.ToUpper().Trim())).ToList();

                ViewBag.filtro = filtro;

                return PartialView(lista.ToPagedList(pageNumber, pageSize));
            }


        }

        //metodo casi listo
        public ActionResult DeleteUsuario(long? inv_unidad_ID)
        {
            bool success = false; ;
            string mensajefound = "";

            try
            {
                //inventario_lineas lin = db.inventario_lineas.Find(inv_linea_ID);
                inventario_unidades lin = db.inventario_unidades.Find(inv_unidad_ID);

                if (lin.estatus_ID != 4)
                {
                    lin.estatus_ID = 4;
                }
                else
                {
                    lin.estatus_ID = 1;
                }
                //lin.estatus_ID = 4;
                //Empre.Fecha_Baja = DateTime.Now;
                //Empre.Oper_Baja = Auth.Usuario.username;

                db.Entry(lin).State = EntityState.Modified;
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


        //metodo no necesario
        public JsonResult CargarLogo()
        {
            #region 'metodo cargar logo'
            bool success = false;
            string error = "";
            var savedFileNameDownload = "";
            string savedFileName = "";
            string completeName = "";
            try
            {
                foreach (string file in Request.Files)
                {

                    HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                    string name = Path.GetRandomFileName();
                    string extension = Path.GetExtension(Path.GetFileName(hpf.FileName));
                    completeName = name + extension;
                    savedFileName = Path.Combine(Server.MapPath("~/Upload/Empresa/"), completeName);
                    hpf.SaveAs(savedFileName);
                    success = true;

                }
            }
            catch (Exception ex)
            {
                error = "Archivo Invalido, Error al procesar el archivo";
            }

            return Json(new { success = success, error = error, savedFileName = completeName }, JsonRequestBehavior.AllowGet);
            #endregion
        }

        //metodo aun menos necesario
        public ActionResult EliminarLogo(string path)
        {
            #region 'metodo eliminar logo'
            bool success = false;
            try
            {
                var serializer = new JavaScriptSerializer();
                var pathh = serializer.Deserialize<string>(path);
                var rutapath = "~/Upload/Empresa/" + pathh;
                if (System.IO.File.Exists(Server.MapPath(rutapath)))
                {
                    try
                    {
                        System.IO.File.Delete(Server.MapPath(rutapath));
                        success = true;
                    }
                    catch (System.IO.IOException e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }

            }
            catch (Exception exp)
            {
                ViewBag.ResultMessage = "Error occured." + exp;
            }
            return Json(new { success = success }, JsonRequestBehavior.AllowGet);
            #endregion
        }

        public ActionResult ExportarExcel(int? creado, string filtro)
        {
            bool success = false;
            string excelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            if (creado == 1)
            {
                // Aqui se debe cambiar por "Resguardos Mobiliario"
                var ruta = "~/Upload/Temporales/Descargas/" + "Inventario unidades - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx";
                return File(Url.Content(ruta), excelContentType, "Inventario unidades - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx");
            }
            // Aqui se debe cambiar por "Resguardos Mobiliario"
            if (System.IO.File.Exists(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Inventario unidades - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx")))
            {
                System.IO.File.Delete(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Inventario unidades - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx"));
            }

            string savedFileName = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Temporales/Descargas/" + "Inventario unidades - " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx"));
            //FileStream stream = System.IO.File.Create(savedFileName);
            using (FileStream stream = new FileStream(savedFileName, FileMode.Create))
            {
                try
                {
                    List<lista_Inv_Unidades_excel> productos = new List<lista_Inv_Unidades_excel>();
                    if (filtro == null || filtro == "")
                    {
                        productos = db.Database.SqlQuery<lista_Inv_Unidades_excel>("Sp_Get_Inventario_unidades_excel").ToList();
                    }
                    else
                    {
                        productos = db.Database.SqlQuery<lista_Inv_Unidades_excel>("Sp_Get_Inventario_unidades_excel_filtro @filtro", new SqlParameter("@filtro", filtro)).ToList();
                    }

                    using (var libro = new ExcelPackage())
                    {

                        var worksheet = libro.Workbook.Worksheets.Add("Inventario unidades");
                        #region titulo para poner la razon social de la empresa
                        worksheet.Cells["C3:J3"].Merge = true;
                        worksheet.Cells["C3:J3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["C3:J3"].Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                        var cell = worksheet.Cells["C3"];
                        cell.IsRichText = true;     // Cell contains RichText rather than basic values


                        var title = cell.RichText.Add("SIRE - ");
                        title.Bold = true;
                        title.FontName = "Calibri";
                        title.Size = 15;
                        title.Color = ColorTranslator.FromHtml("#44546A");
                        #endregion
                        #region titulo para el reporte
                        worksheet.Cells["C4:J4"].Merge = true;
                        worksheet.Cells["C4:J4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells["C4:J4"].Style.VerticalAlignment = ExcelVerticalAlignment.Bottom;
                        //worksheet.Cells["D4:I4"].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                        //worksheet.Cells["D4:I4"].Style.Border.Bottom.Color.SetColor(Color.Blue);

                        var cellrs = worksheet.Cells["C4"];
                        cellrs.IsRichText = true;     // Cell contains RichText rather than basic values
                                                      //cell.Style.WrapText = true; // Required to honor new lines

                        var titlers = cellrs.RichText.Add("Inventario de unidades");
                        titlers.Bold = true;
                        titlers.FontName = "Calibri";
                        titlers.Size = 15;
                        titlers.Color = ColorTranslator.FromHtml("#44546A");
                        #endregion
                        #region llenado de la informacion
                        worksheet.Cells["B6"].LoadFromCollection(productos, PrintHeaders: true);
                        for (var col = 1; col < productos.Count + 1; col++)
                        {
                            worksheet.Column(col).AutoFit();
                        }
                        #endregion
                        #region incrutacion de imagen
                        Image logo = Image.FromFile(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Sistema/cicloAmb.png"));

                        //get the image from disk                        
                        var excelImage = worksheet.Drawings.AddPicture("logo ciclo", logo);
                        //add the image to row 20, column E
                        excelImage.From.Column = 1;
                        excelImage.From.Row = 0;
                        excelImage.SetSize(150, 80);
                        // 2x2 px space for better alignment
                        excelImage.From.ColumnOff = Pixel2MTU(2);
                        excelImage.From.RowOff = Pixel2MTU(2);

                        Image logo2 = Image.FromFile(System.Web.Hosting.HostingEnvironment.MapPath("~/Upload/Sistema/cicloAmb.png"));
                        //get the image from disk                        
                        var excelImage2 = worksheet.Drawings.AddPicture("logo empresa", logo2);
                        //add the image to row 20, column E
                        excelImage2.From.Column = 9;
                        //excelImage2.From.Column = 9;
                        excelImage2.From.Row = 0;
                        excelImage2.SetSize(150, 80);
                        // 2x2 px space for better alignment
                        excelImage2.From.ColumnOff = Pixel2MTU(2);
                        excelImage2.From.RowOff = Pixel2MTU(2);
                        #endregion

                        var tabla = worksheet.Tables.Add(new ExcelAddressBase(fromRow: 6, fromCol: 2, toRow: productos.Count + 6, toColumn: 10), "inventario");
                        tabla.ShowHeader = true;
                        tabla.TableStyle = TableStyles.Light5;
                        libro.Workbook.Properties.Company = "Ciclo ambiental";
                        libro.Workbook.Properties.Keywords = "Excel";
                        libro.SaveAs(stream);
                        success = true;

                    }
                    stream.Close();
                    stream.Dispose();
                }
                catch (Exception ex)
                {
                    stream.Close();
                    stream.Dispose();
                }
                // return File(Url.Content(ruta), excelContentType, "Resguardos laptops - " + DateTime.Now.ToShortDateString() + ".xlsx");
            }
            return Json(new { success = success });
        }

        public int Pixel2MTU(int pixels)
        {
            int mtus = pixels * 9525;
            return mtus;
        }
        //metodo listo


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