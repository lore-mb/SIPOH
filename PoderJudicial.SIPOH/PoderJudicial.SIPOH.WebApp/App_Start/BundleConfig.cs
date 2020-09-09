using System.Web;
using System.Web.Optimization;

namespace PoderJudicial.SIPOH.WebApp
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            //-------------------------------------//
            // --------- CSS CORE SIPOH ---------- //
            //-------------------------------------//

            // CSS CORE para sitio
            bundles.Add(new StyleBundle("~/Content/Master/Site").Include(
                "~/Content/Master/Site/bootstrap.min.css",
                "~/Content/Master/Site/coresite.min.css"));

            bundles.Add(new ScriptBundle("~/Scripts/Master/WebFonts").Include(
                "~/Scripts/Master/WebFonts/webfont.min.js"));

            //-------------------------------------//
            // ------------- ICON PACK ----------- //
            //-------------------------------------//

            // Paqueteria IconMoon [Iconos]
            bundles.Add(new StyleBundle("~/Content/Master/Fonts/IcoMoon/").Include(
                "~/Content/Master/Fonts/IcoMoon/style.css"));

            // Paqueteria FontAwesome [Iconos]
            bundles.Add(new StyleBundle("~/Content/Master/Fonts/FontAwesome").Include(
                "~/Content/Master/Fonts/FontAwesome/all.min.css"));

            //-------------------------------------//
            // ---------- JQUERY SCRIPTS --------- //
            //-------------------------------------//

            // Script Jquery 3.5.1
            bundles.Add(new ScriptBundle("~/Scripts/Master/Jquery").Include(
              "~/Scripts/Master/Jquery/jquery-3.5.1.min.js"));

            // Jquery UI - Custom
            bundles.Add(new ScriptBundle("~/Scripts/Master/Jquery_UI_Custom").Include(
                "~/Scripts/Master/Jquery_UI_Custom/jquery-ui.min.js"));

            // Jquery UI Touch
            bundles.Add(new ScriptBundle("~/Scripts/Master/Jquery_UI_Touch").Include(
                "~/Scripts/Master/Jquery_UI_Touch/jquery.ui.touch-punch.min.js"));

            // JQuery Scrollbar
            bundles.Add(new ScriptBundle("~/Scripts/Master/Jquery_Scrollbar").Include(
                "~/Scripts/Master/Jquery_Scrollbar/jquery.scrollbar.min.js"));

            //-------------------------------------//
            // ---- BOOTSTRAP & DEPENDENCIES ----- //
            //-------------------------------------//

            // Scripts Bootstrap
            bundles.Add(new ScriptBundle("~/Scripts/Master/Bootstrap").Include(
                "~/Scripts/Master/Bootstrap/bootstrap.min.js"));

            // Scripts Popper
            bundles.Add(new ScriptBundle("~/Scripts/Master/Popper").Include(
                "~/Scripts/Master/Popper/popper.min.js"));

            //-------------------------------------//
            // ----------- DATEPICKER ------------ //
            //-------------------------------------//

            bundles.Add(new StyleBundle("~/Content/Master/DatePicker").Include(
                "~/Content/Master/DatePicker/bootstrap-datetimepicker.min.css"));

            bundles.Add(new ScriptBundle("~/Scripts/Master/DatePicker").Include(
                "~/Scripts/Master/DatePicker/bootstrap-datetimepicker.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Master/Moment").Include(
                "~/Scripts/Master/Moment/moment.min.js"));

            //-------------------------------------//
            // ------- DATATABLES CSS & JS ------- //
            //-------------------------------------//

            // Estilos DataTables
            bundles.Add(new StyleBundle("~/Content/Master/DataTables").Include(
                "~/Content/Master/DataTables/datatables.min.css"));

            // Scripts DataTables
            bundles.Add(new ScriptBundle("~/Scripts/Master/DataTables").Include(
                "~/Scripts/Master/DataTables/datatables.min.js"));

            //-------------------------------------//
            // --------- SCRIPTS ALERTS ---------- //
            //-------------------------------------//

            // Script SweetAlert
            bundles.Add(new ScriptBundle("~/Scripts/Master/SweetAlert").Include(
                "~/Scripts/Master/SweetAlert/sweetalert.min.js"));

            // Script BootBox
            bundles.Add(new ScriptBundle("~/Scripts/Master/BootBox").Include(
            "~/Scripts/Master/BootBox/bootbox.min.js",
            "~/Scripts/Master/BootBox/popper.min.js"));

            //-------------------------------------//
            // --------- SCRIPT INPUTMASK -------- //
            //-------------------------------------//

            // Scripts InputMask
            bundles.Add(new ScriptBundle("~/Scripts/Master/InptuMask").Include(
                "~/Scripts/Master/InptuMask/jquery.inputmask.js"));

            //-------------------------------------//
            // ---------- CUSTOM SCRIPTS --------- //
            //-------------------------------------//

            // Scripts Ready
            bundles.Add(new ScriptBundle("~/Scripts/Master/Ready").Include(
                "~/Scripts/Master/Ready/ready.min.js"));

            //-------------------------------------//
            // ------ SCRIPTS CUENTA USUARIO ----- //
            //-------------------------------------//

            // Script Cuenta Login
            bundles.Add(new ScriptBundle("~/Scripts/Modules/Cuenta").Include(
                "~/Scripts/Modules/Cuenta/LogIn.js"));

            //-------------------------------------//
            // ---- SCRIPTS MODULO EJECUCIÓN ----- //
            //-------------------------------------//

            // Script Ejecución - Iniciales
            bundles.Add(new ScriptBundle("~/Scripts/Modules/Ejecucion/Iniciales").Include(
                "~/Scripts/Modules/Ejecucion/Iniciales/Iniciales.js"));

            // Script Ejecución - Busquedas
            bundles.Add(new ScriptBundle("~/Scripts/Modules/Ejecucion/Busquedas").Include(
                "~/Scripts/Modules/Ejecucion/Busquedas/Busquedas.js"));

            // Script Ejecución - Reportes
            bundles.Add(new ScriptBundle("~/Scripts/Modules/Ejecucion/Reportes").Include(
                "~/Scripts/Modules/Ejecucion/Reportes/Reportes.js"));

        }
    }
}
