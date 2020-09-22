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

        #region Public Methods

        // GET: Promociones
        public ActionResult CrearPromocion()
        {
            List<Juzgado> ListaJuzgadosPick = promocionesProcessor.ObtenerJuzgadoEjecucionPorCircuito(Usuario.IdCircuito);
            ViewBag.JuzgadoCircuit = ListaJuzgadosPick != null ? ListaJuzgadosPick : new List<Juzgado>();
            return View();
        }

        [HttpGet]
        public ActionResult ObtenerJuzgadoEjecucionPorCircuito(int idcircuito)
        {
            List<Juzgado> ListaJuzgados = promocionesProcessor.ObtenerJuzgadoEjecucionPorCircuito(idcircuito);
            // Validacion Formulario
            ValidarJuzgado(ListaJuzgados);
            Respuesta.Mensaje = promocionesProcessor.Mensaje;
            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerEjecucionPorJuzgado(int Juzgado, string NoEjecucion)
        {
            List<Ejecucion> ListaInformacion = promocionesProcessor.ObtenerEjecucionPorJuzgado(Juzgado, NoEjecucion);

            if (ListaInformacion == null)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Data = null;
            }
            else {
                if (ListaInformacion.Count > 0)
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.OK;
                    Respuesta.Data = new { ListaInformacion };
                }
                else {
                    Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                    Respuesta.Data = new object();
                }
            } 
            Respuesta.Mensaje = promocionesProcessor.Mensaje;
            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerExpedienteEjecucionCausa(int idExpediente)
        {
            Expediente ExpedienteCRE = promocionesProcessor.ObtenerExpedienteEjecucionCausa(idExpediente);

            if (ExpedienteCRE == null)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Data = null;
            }
            else
            {
                if (ExpedienteCRE.IdExpediente == default)
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                    Respuesta.Data = null;
                }
                else
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.OK;
                    Respuesta.Data = ExpedienteCRE;
                }
            }
            Respuesta.Mensaje = promocionesProcessor.Mensaje;
            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Private method

        private void ValidarJuzgado(List<Juzgado> ListaJuzgados) {
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