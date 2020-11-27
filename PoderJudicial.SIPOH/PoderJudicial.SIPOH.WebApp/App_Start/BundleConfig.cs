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
            bundles.Add(new StyleBundle("~/Content/StyleDatatables").Include(
                "~/Content/StyleDatatables.min.css"
                ));

            bundles.Add(new StyleBundle("~/Content/StyleDateTimePicker").Include(
                "~/Content/StyleDateTimePicker.min.css"
                ));

            bundles.Add(new StyleBundle("~/Content/StyleSite").Include(
               "~/Content/StyleBootstrap.css",
               "~/Content/StyleCapaPersonalizada.css"
               ));

            bundles.Add(new StyleBundle("~/Content/FontAwesome").Include(
               "~/Content/StyleFontAwesome.min.css"
               ));

            bundles.Add(new StyleBundle("~/Content/IcoMoon").Include(
               "~/Content/StyleIconMoon.css"
               ));


            //-------------------------------------//
            //------- SCRIPTS LIBRERIAS   -------- //
            //-------------------------------------//

            /* BootBox */
            bundles.Add(new ScriptBundle("~/Scripts/bootbox").Include(
                "~/Scripts/LibBootbox.min.js",
                "~/Scripts/LibBootboxPopper.min.js"
                ));

            /* Bootstrap */
            bundles.Add(new ScriptBundle("~/Scripts/bootstrap").Include(
                "~/Scripts/LibBootstrap.min.js"
                ));

            /* DataTable */
            bundles.Add(new ScriptBundle("~/Scripts/dataTables").Include(
                "~/Scripts/LibDataTables.min.js"
                ));

            /* DateTimePicker */
            bundles.Add(new ScriptBundle("~/Scripts/dateTimePicker").Include(
                "~/Scripts/LibMoment.min.js",
                "~/Scripts/LibTraslateEsmx.js",
                "~/Scripts/LibDateTimePicker.min.js"
                ));

            /* InputMask */
            bundles.Add(new ScriptBundle("~/Scripts/inputmask").Include(
                "~/Scripts/LibInputmask.js"
                ));

            /* JQuery */
            bundles.Add(new ScriptBundle("~/Scripts/jQuery").Include(
                "~/Scripts/LibJQuery.min.js"
                ));

            /* JQuery Scrollbar */
            bundles.Add(new ScriptBundle("~/Scripts/jQueryScrollbar").Include(
                "~/Scripts/LibJQueryScrollbarMin.js"
                ));

            /* JQuery UI Touch*/
            bundles.Add(new ScriptBundle("~/Scripts/uITouch").Include(
                "~/Scripts/LibJQueryUI.js",
                "~/Scripts/LibJQueryUITouchPunch.js"
                ));

            /* Notify */
            bundles.Add(new ScriptBundle("~/Scripts/bootstrapNotify").Include(
                "~/Scripts/LibBootstrapNotify.min.js"
                ));

            /* Popper */
            bundles.Add(new ScriptBundle("~/Scripts/popper").Include(
                "~/Scripts/LibPopper.min.js"
                ));

            /* Ready */
            bundles.Add(new ScriptBundle("~/Scripts/ready").Include(
                "~/Scripts/LibReady.min.js"
                ));

            /* WebFont */
            bundles.Add(new ScriptBundle("~/Scripts/webFont").Include(
                "~/Scripts/LibWebFont.min.js"
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
