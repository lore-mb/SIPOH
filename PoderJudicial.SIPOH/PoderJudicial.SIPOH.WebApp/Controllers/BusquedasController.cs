using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;
using PoderJudicial.SIPOH.Negocio.Interfaces;
using PoderJudicial.SIPOH.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoderJudicial.SIPOH.WebApp.Controllers
{
    public class BusquedasController : BaseController
    {
        // GET: Busquedas metodo para mandar a llamar con ajax
        public ActionResult BusquedaNumeroEjecucion()
        {

            return View();
        }
        //[Se realiza inyeccion de dependencias y creo mi objeto]
        private readonly IBusquedasProcessor busquedaProcessor;

        //[Metodo de inyeccion en mi Interfaz y asigno mi objeto a mi clase]
        public BusquedasController(IBusquedasProcessor busquedaProcessor)
        {

        }

        //[Metodos Publicos]
        public ActionResult BusquedaPartesCausa(string nombre, string apellidoPaterno, string apellidoMaterno)
        {
            //Defino mi lista y creo mi objeto---- - accedo a mi objeto general(inyeccion)
            List<Ejecucion> busquedaPartesCausa = busquedaProcessor.ObtenerEjecucionPorPartesCausa(nombre, apellidoPaterno, apellidoMaterno);

            if (busquedaPartesCausa == null)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Data = null;
            }
            else
            {
                if (busquedaPartesCausa.Count > 0)
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.OK;
                    Respuesta.Data = new { busquedaPartesCausa };
                }
                else
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                    Respuesta.Data = new object();
                }
            }

            return Json(Respuesta, JsonRequestBehavior.AllowGet);

        }
        public ActionResult BusquedaPorBeneficiario(string nombre, string apellidoPaterno, string apellidoMaterno)
        {
            List<Ejecucion> busquedaBeneficiario = busquedaProcessor.ObtenerEjecucionSentenciadoBeneficiario(nombre, apellidoPaterno, apellidoMaterno);
            if (busquedaBeneficiario == null)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
            }
            else
            {
                if (busquedaBeneficiario.Count > 0)
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.OK;
                    Respuesta.Data = new { busquedaBeneficiario };
                }
                else
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                    Respuesta.Data = new object();
                }
            }

            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BusquedaPorNumeroCausa(string numCausa, int idJuzgado)
        {
            List<Ejecucion> busquedaNumCausa = busquedaProcessor.ObtenerEjeucionPorNumeroCausa(numCausa, idJuzgado);
            if (busquedaNumCausa == null)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
            }
            else
            {
                if (busquedaNumCausa.Count > 0)
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.OK;
                    Respuesta.Data = new { busquedaNumCausa };
                }
                else
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                    Respuesta.Data = new object();
                }
            }
            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BusquedaNUC(String NUC, int idJuzgado)
        {
            List<Ejecucion> busquedaNUC = busquedaProcessor.ObtenerEjecucionPorNUC(NUC, idJuzgado);
            if (busquedaNUC == null)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
            }
            else
            {
                if (busquedaNUC.Count > 0)
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.OK;
                    Respuesta.Data = new { busquedaNUC };
                }
                else
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                    Respuesta.Data = new object();
                }
            }
            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BusquedaPorSolicitante(int idSolicitante)
        {
            List<Ejecucion> busquedaSolicitante = busquedaProcessor.ObtenerEjecucionPorSolicitante(int idSolicitante);
            if (busquedaSolicitante == null)
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
            else
            {
                if (busquedaSolicitante.Count > 0)
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.OK;
                    Respuesta.Data = new { busquedaSolicitante };
                }
                else
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                    Respuesta.Data = new object();
                }
            }
            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BusquedaPorDetalleSolicitante(string detalleSolicitante) {
            List<Ejecucion> busquedaDetalleSolicitante = busquedaProcessor.ObtenerEjecucionPorDetalleSolicitante(detalleSolicitante);
            if (busquedaDetalleSolicitante == null)
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
            else
            {
                if (busquedaDetalleSolicitante.Count > 0)
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.OK;
                    Respuesta.Data = new { busquedaDetalleSolicitante };
                }
                else
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                    Respuesta.Data = new object();
                }
            }
            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }
    }
}