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
            //-------- STILOS CSS SITE    -------- //
            //-------------------------------------//
            bundles.Add(new StyleBundle("~/Content/Style_Datatables").Include(
                "~/Content/Style_Datatables.min.css"
                ));

            bundles.Add(new StyleBundle("~/Content/Style_DateTimePicker").Include(
                "~/Content/Style_DateTimePicker.min.css"
                ));

            bundles.Add(new StyleBundle("~/Content/StyleSite").Include(
               "~/Content/Style_Bootstrap.css",
               "~/Content/Style_CapaPersonalizada.css"
               ));

            bundles.Add(new StyleBundle("~/Content/FontAwesome").Include(
               "~/Content/FontAwesome/all.min.css"
               ));

            bundles.Add(new StyleBundle("~/Content/IcoMoon").Include(
               "~/Content/IcoMoon/style.css"
               ));


            //-------------------------------------//
            //------- SCRIPTS LIBRERIAS   -------- //
            //-------------------------------------//

            /* BootBox */
            bundles.Add(new ScriptBundle("~/Scripts/Lib_Bootbox").Include(
                "~/Scripts/Lib_Bootbox.min.js",
                "~/Scripts/Lib_BootboxPopper.min.js"
                ));

            /* Bootstrap */
            bundles.Add(new ScriptBundle("~/Scripts/Lib_Bootstrap.min").Include(
                "~/Scripts/Lib_Bootstrap.min.js"
                ));

            /* DataTable */
            bundles.Add(new ScriptBundle("~/Scripts/Lib_DataTables.min").Include(
                "~/Scripts/Lib_DataTables.min.js"
                ));

            /* DateTimePicker */
            bundles.Add(new ScriptBundle("~/Scripts/Libs_DateTimePicker").Include(
                "~/Scripts/Lib_Moment.min.js",
                "~/Scripts/Lib_Traslate_Es-mx.js",
                "~/Scripts/Lib_DateTimePicker.min.js"
                ));

            /* InputMask */
            bundles.Add(new ScriptBundle("~/Scripts/Lib_Inputmask").Include(
                "~/Scripts/Lib_Inputmask.js"
                ));

            /* JQuery */
            bundles.Add(new ScriptBundle("~/Scripts/Lib_JQuery.min").Include(
                "~/Scripts/Lib_JQuery.min.js"
                ));

            /* JQuery Scrollbar */
            bundles.Add(new ScriptBundle("~/Scripts/Lib_JQueryScrollbarMin").Include(
                "~/Scripts/Lib_JQueryScrollbarMin.js"
                ));

            /* JQuery UI Touch*/
            bundles.Add(new ScriptBundle("~/Scripts/Lib_UITouch").Include(
                "~/Scripts/Lib_JQueryUI.js",
                "~/Scripts/Lib_JQueryUITouchPunch.js"
                ));

            /* Notify */
            bundles.Add(new ScriptBundle("~/Scripts/Lib_BootstrapNotify.min").Include(
                "~/Scripts/Lib_BootstrapNotify.min.js"
                ));

            /* Popper */
            bundles.Add(new ScriptBundle("~/Scripts/Lib_Popper").Include(
                "~/Scripts/Lib_Popper.min.js"
                ));

            /* Ready */
            bundles.Add(new ScriptBundle("~/Scripts/Lib_Ready.min").Include(
                "~/Scripts/Lib_Ready.min.js"
                ));

            /* WebFont */
            bundles.Add(new ScriptBundle("~/Scripts/Lib_WebFont").Include(
                "~/Scripts/Lib_WebFont.min.js"
                ));


            //-------------------------------------//
            //------ SCRIPTS MODULO CUENTA  ------ //
            //-------------------------------------//
            bundles.Add(new ScriptBundle("~/Scripts/Cuenta").Include(
                "~/Scripts/LogIn.js"
                ));

            bundles.Add(new ScriptBundle("~/Scripts/LoggedIn").Include(
                "~/Scripts/LoggedIn.js"
                ));

            //-------------------------------------//
            //----- SCRIPTS MODULO EJECUCION ----- //
            //-------------------------------------//

            /* INICIALES */
            bundles.Add(new ScriptBundle("~/Scripts/EjecucionIniciales").Include(
                "~/Scripts/EjecucionIniciales.js"
                ));

            bundles.Add(new ScriptBundle("~/Scripts/EjecucionInicialesDetalle").Include(
                "~/Scripts/EjecucionInicialesDetalle.js"
                 ));            
            
            bundles.Add(new ScriptBundle("~/Scripts/EjecucionInicialesSello").Include(
                "~/Scripts/EjecucionInicialesSello.js"
                 ));

            /* CONSIGNACIONE HISTORICAS */
            bundles.Add(new ScriptBundle("~/Scripts/EjecucionConsignaciones").Include(
                "~/Scripts/EjecucionConsignacionesHistoricas.js"
                ));

            /* BUSQUEDAS */
            bundles.Add(new ScriptBundle("~/Scripts/EjecucionBusquedas").Include(
                "~/Scripts/EjecucionBusquedas.js"
                ));

            /* PROMOCIONES */
            bundles.Add(new ScriptBundle("~/Scripts/EjecucionPromociones").Include(
                "~/Scripts/EjecucionPromociones.js"
                )); 
            
            bundles.Add(new ScriptBundle("~/Scripts/EjecucionPromocionesDetalle").Include(
                "~/Scripts/EjecucionPromocionesDetalle.js"
                ));

            /* REPORTES */
            bundles.Add(new ScriptBundle("~/Scripts/EjecucionReportes").Include(
                "~/Scripts/EjecucionReportes.js"
                ));

        }
    }
}
