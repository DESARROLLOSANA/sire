using CRME.Helpers;
using System.Web;
using System.Web.Optimization;

namespace CRME
{
    public class BundleConfig
    {
        // Para obtener más información sobre Bundles, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*")
                        .Include("~/Scripts/jquery.validate.unobtrusive*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryUnobtrusive")
               .Include("~/Scripts/jquery.unobtrusive-ajax.min.js")
           );
            //-------------------configuracion de materialize y jquery--------------------------------------

            bundles.Add(new ScriptBundle("~/bundles/Materialize").Include(
                        "~/Scripts/Aplicacion/Materialize/js/materialize.min.js"
                        , "~/Scripts/Aplicacion/site.js"));

            bundles.Add(new ScriptBundle("~/bundles/JQueryUI").Include(
                        "~/Scripts/Aplicacion/JQueryUI/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/Modal").Include(
                        "~/Scripts/Aplicacion/Modal/modal.js"));

            bundles.Add(new ScriptBundle("~/bundles/Fileinput").Include(
           "~/Scripts/Aplicacion/Fileinput/fileinput.js",
           "~/Scripts/Aplicacion/Fileinput/fileinput_locale_es.js")
           );

            bundles.Add(new ScriptBundle("~/bundles/nouislider").Include(
                        "~/Scripts/Aplicacion/nouislider/nouislider.min.js",
                        "~/Scripts/Aplicacion/nouislider/wNumb.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/Mask").Include(
            "~/Scripts/Aplicacion/Mask/jquery.mask.js",
            "~/Scripts/Aplicacion/Mask/site.mask.js")
            );
            //-------------------------------------------------------------------

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // preparado para la producción y podrá utilizar la herramienta de compilación disponible en http://modernizr.com para seleccionar solo las pruebas que necesite.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/",
            //          "~/Scripts/respond.js"));


            bundles.Add(new StyleBundle("~/Content/Materialize").Include(
                      "~/Content/Aplicacion/Materialize/css/materialize.min.css"));

            bundles.Add(new StyleBundle("~/Content/Iconos")
                .Include("~/Content/Aplicacion/Materialize/Iconos/css/materialdesignicons.min.css", new CssFixRewriteUrlTransform())
                );

            bundles.Add(new StyleBundle("~/Content/JQueryUI").Include(
                      "~/Content/Aplicacion/JQueryUI/jquery-ui.min.css"));

            bundles.Add(new StyleBundle("~/Content/Modal").Include(
                      "~/Content/Aplicacion/Modal/Modal.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/FileInput").Include(
                     "~/Content/Aplicacion/FileInput/fileinput.css"));

            bundles.Add(new StyleBundle("~/Content/nouislider").Include(
                    "~/Content/Aplicacion/nouislider/nouislider.min.css"
                    ));
            
        }
    }
}
