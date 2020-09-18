using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Negocio.Interfaces;
using PoderJudicial.SIPOH.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoderJudicial.SIPOH.WebApp.Controllers
{
    public class PromocionesController : BaseController
    {
        private readonly IPromocionesProcessor promocionesProcessor;
        public PromocionesController(IPromocionesProcessor promocionesProcessor)
        {
            this.promocionesProcessor = promocionesProcessor;
        }

        // GET: Promociones
        public ActionResult CrearPromocion()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ObtenerJuzgadoEjecucionPorCircuito(int idcircuito) {
            List<Juzgado> ListaJuzgados = promocionesProcessor.ObtenerJuzgadoEjecucionPorCircuito(idcircuito);
            // Validacion Formulario
            ValidarJuzgado(ListaJuzgados);
            Respuesta.Mensaje = promocionesProcessor.Mensaje;
            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerEjecucionPorJuzgado(int Juzgado, string NoEjecución) {
            List<Ejecucion> ListaInformacion = promocionesProcessor.ObtenerEjecucionPorJuzgado(Juzgado, NoEjecución);
            Respuesta.Mensaje = promocionesProcessor.Mensaje;
            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerExpedienteEjecucionCausa(int idExpediente) {
            Expediente ExpedienteCRE = promocionesProcessor.ObtenerExpedienteEjecucionCausa(idExpediente);
            Respuesta.Mensaje = promocionesProcessor.Mensaje;
            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }

        #region Private method

        public void ValidarJuzgado(List<Juzgado> ListaJuzgados) {
            if (ListaJuzgados == null)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Data = null;
            }
            else {

                if (ListaJuzgados.Count > 0)
                {
                    var ListadoJuzgados = ViewHelper.Options(ListaJuzgados, "IdJuzgado", "Nombre");
                    Respuesta.Estatus = EstatusRespuestaJSON.OK;
                    Respuesta.Data = ListadoJuzgados;
                }
                else {
                    Respuesta.Data = new object();
                    Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                }
            
            }
        
        }

        #endregion

    }
}