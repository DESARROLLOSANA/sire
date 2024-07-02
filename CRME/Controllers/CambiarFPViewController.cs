using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;
using CRME.Models;
using PagedList;
using System.Web.Script.Serialization;
using System.IO;

namespace CRME.Controllers
{
    public class CambiarFPViewController : Controller
    {

        private SIRE_Context db = new SIRE_Context();
        // GET: CambiarFPView
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _Formulario(long? idUsuario)
        {
            //codigo para agregar y editar usuarios
            //UsuariosPersonas Personas = new UsuariosPersonas();
            cat_sistemas Usuarios = new cat_sistemas();
            //Personas Persona = new Personas();

            if (idUsuario != null)
            {

                ViewBag.edit = 1;
                Usuarios = db.cat_sistemas.Find(idUsuario);

                Usuarios.password = "QWERTY123*";
                ViewBag.idEstatus = new SelectList(db.Cat_Estatus.ToList(), "Id", "Estatus", Usuarios.estatus_ID);
                ViewBag.idPerfil = new SelectList(db.cat_perfiles.ToList(), "perfil_ID", "perfil", Usuarios.perfil_ID);
                ViewBag.puesto = new SelectList(db.Puestos.ToList(), "Pu_Cve_Puesto", "Pu_Descripcion", Usuarios.Pu_Cve_Puesto);
                ViewBag.departamento = new SelectList(db.Departamentos.ToList(), "Dp_Cve_Departamento", "Dp_Descripcion", Usuarios.Dp_Cve_Departamento);
                ViewBag.empresa = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion", Usuarios.Em_Cve_Empresa);
                ViewBag.sucursal = new SelectList(db.Sucursal.ToList(), "Sc_Cve_Sucursal", "Sc_Descripcion", Usuarios.Sc_Cve_Sucursal);
                //ViewBag.idGenero = new SelectList(db.CatGeneros.ToList(), "idGenero", "nbGenero", Usuarios.Personas.idGenero);
            }
            else
            {
                ViewBag.idEstatus = new SelectList(db.Cat_Estatus.ToList(), "Id", "Estatus");
                ViewBag.idPerfil = new SelectList(db.cat_perfiles.ToList(), "perfil_ID", "perfil");
                ViewBag.empresa = new SelectList(db.Empresa.ToList(), "Em_Cve_Empresa", "Em_Descripcion");
                ViewBag.departamento = new SelectList("", "Dp_Cve_Departamento", "Dp_Descripcion");
                ViewBag.puesto = new SelectList("", "Pu_Cve_Puesto", "Pu_Descripcion");
                ViewBag.sucursal = new SelectList("", "Sc_Cve_Sucursal", "Sc_Descripcion");
            }


            return PartialView(Usuarios);
        }


    }
}