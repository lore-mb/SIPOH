using System.Web;
using System.Web.Optimization;

namespace PoderJudicial.SIPOH.WebApp
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            // Estilos del sitio
            bundles.Add(new StyleBundle("~/Content/Master/Site").Include(
                "~/Content/Master/Site/bootstrap.min.css",
                "~/Content/Master/Site/azzara.min.css"));
            
            // IconMoon
            bundles.Add(new StyleBundle("~/Content/Master/Fonts/IcoMoon/").Include(
                "~/Content/Master/Fonts/IcoMoon/style.css"));

            bundles.Add(new ScriptBundle("~/Scripts/Master/WebFonts").Include(
                "~/Scripts/Master/WebFonts/webfont.min.js"));

            // Estilos  FontAwesome
            bundles.Add(new StyleBundle("~/Content/Master/Fonts/FontAwesome").Include(
                "~/Content/Master/Fonts/FontAwesome/all.min.css"));

            // Scripts Jquery
            bundles.Add(new ScriptBundle("~/Scripts/Master/Jquery").Include(
              "~/Scripts/Master/Jquery/jquery-3.5.1.min.js"));

            // Scripts Bootstrap
            bundles.Add(new ScriptBundle("~/Scripts/Master/Bootstrap").Include(
                "~/Scripts/Master/Bootstrap/bootstrap.min.js"));         

            // Scripts Popper
            bundles.Add(new ScriptBundle("~/Scripts/Master/Popper").Include(
                "~/Scripts/Master/Popper/popper.min.js"));

            // Jquery UI - Custom
            bundles.Add(new ScriptBundle("~/Scripts/Master/Jquery_UI_Custom").Include(
                "~/Scripts/Master/Jquery_UI_Custom/jquery-ui.min.js"));

            // Jquery UI Touch
            bundles.Add(new ScriptBundle("~/Scripts/Master/Jquery_UI_Touch").Include(
                "~/Scripts/Master/Jquery_UI_Touch/jquery.ui.touch-punch.min.js"));

            // JQuery Scrollbar
            bundles.Add(new ScriptBundle("~/Scripts/Master/Jquery_Scrollbar").Include(
                "~/Scripts/Master/Jquery_Scrollbar/jquery.scrollbar.min.js"));

            // Scripts Ready
            bundles.Add(new ScriptBundle("~/Scripts/Master/Ready").Include(
                "~/Scripts/Master/Ready/ready.min.js"));

            // Estilos DataTables
            bundles.Add(new StyleBundle("~/Content/Master/DataTables").Include(
                "~/Content/Master/DataTables/datatables.min.css"));

            // Scripts DataTables
            bundles.Add(new ScriptBundle("~/Scripts/Master/DataTables").Include(
                "~/Scripts/Master/DataTables/datatables.min.js"));

            // Scripts InputMask
            bundles.Add(new ScriptBundle("~/Scripts/Master/InptuMask").Include(
                "~/Scripts/Master/InptuMask/jquery.inputmask.js"));

            // Scripts Bootstrap
            bundles.Add(new ScriptBundle("~/Scripts/Master/Bootstrap").Include(
                "~/Scripts/Master/Bootstrap/bootstrap.min.js"));



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
            
            // BootBox
            bundles.Add(new ScriptBundle("~/Scripts/Master/BootBox").Include(
            "~/Scripts/Master/BootBox/bootbox.min.js",
            "~/Scripts/Master/BootBox/popper.min.js"));

            // Script Reportes
            bundles.Add(new ScriptBundle("~/Scripts/Modules/Ejecucion/Reportes").Include(
                "~/Scripts/Modules/Ejecucion/Reportes/Reportes.js"));


        }
    }
}
