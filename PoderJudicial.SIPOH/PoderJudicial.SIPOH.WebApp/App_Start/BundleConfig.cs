using System.Web;
using System.Web.Optimization;

namespace PoderJudicial.SIPOH.WebApp
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Estilos Bootstrap
            bundles.Add(new StyleBundle("~/Content/Master/Bootstrap").Include(
                "~/Content/Master/Bootstrap/bootstrap.min.css"));

            // Estilos DataTables
            bundles.Add(new StyleBundle("~/Content/Master/DataTables").Include(
                "~/Content/Master/DataTables/dataTables.min.css"));

            // Estilos FontAwesome
            bundles.Add(new StyleBundle("~/Content/Master/FontAwesome").Include(
                "~/Content/Master/FontAwesome/font-awesome.min.css"));

            // Fuentes del Sitio
            bundles.Add(new StyleBundle("~/Content/Master/FontSite").Include(
                "~/Content/Master/FontSite/css?family=Poppins:300,400,700"));

            // Estilos Personalizados
            bundles.Add(new StyleBundle("~/Content/Master/Customize").Include(
                "~/Content/Master/Customize/style.default.css",
                "~/Content/Master/Customize/custom.css",
                "~/Content/Master/Customize/all.css"));

            // Scripts Jquery
            bundles.Add(new ScriptBundle("~/Scripts/Master/Jquery").Include(
              "~/Scripts/Master/Jquery/jquery-3.5.1.min.js"));

            // Scripts InputMask
            bundles.Add(new ScriptBundle("~/Scripts/Master/InptuMask").Include(
                "~/Scripts/Master/InptuMask/jquery.inputmask.js"));

            // Scripts Bootstrap
            bundles.Add(new ScriptBundle("~/Scripts/Master/Bootstrap").Include(
                "~/Scripts/Master/Bootstrap/bootstrap.min.js"));

            // Scripts DataTables
            bundles.Add(new ScriptBundle("~/Scripts/Master/DataTables").Include(
                "~/Scripts/Master/DataTables/dataTables.min.js"));

            // Scripts Front
            bundles.Add(new ScriptBundle("~/Scripts/Master/Front").Include(
                "~/Scripts/Master/Customize/front.js"));

            // Scripts Submodulo Iniciales
            bundles.Add(new ScriptBundle("~/Scripts/Modules/Ejecucion/Iniciales").Include(
                "~/Scripts/Modules/Ejecucion/Iniciales/Iniciales.js",
                "~/Scripts/Modules/Ejecucion/Iniciales/Iniciales2.js"
                ));

            // Scripts Submodulo Iniciales
            bundles.Add(new ScriptBundle("~/Scripts/Modules/Ejecucion/Busquedas").Include(
                "~/Scripts/Modules/Ejecucion/Busquedas/Busquedas.js"));

            // Script Cuenta User
            bundles.Add(new ScriptBundle("~/Scripts/Modules/Cuenta").Include(
                "~/Scripts/Modules/Cuenta/LogIn.js"));

            // SweetAlerts
            bundles.Add(new ScriptBundle("~/Scripts/Master/SweetAlert").Include(
                "~/Scripts/Master/SweetAlert/sweetalert.min.js"));
            
            //BootBox
            bundles.Add(new ScriptBundle("~/Scripts/Master/BootBox").Include(
            "~/Scripts/Master/BootBox/bootbox.min.js",
            "~/Scripts/Master/BootBox/popper.min.js"));

        }
    }
}
