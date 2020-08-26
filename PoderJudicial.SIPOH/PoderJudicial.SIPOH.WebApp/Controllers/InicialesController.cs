using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;
using PoderJudicial.SIPOH.Negocio.Interfaces;
using PoderJudicial.SIPOH.WebApp.Helpers;

using System.Collections.Generic;
using System.Web.Mvc;

namespace PoderJudicial.SIPOH.WebApp.Controllers
{
    public class InicialesController : BaseController
    {
        private readonly IInicialesProcessor inicialesProcessor;
    
        public InicialesController(IInicialesProcessor inicialesProcessor) 
        {
            this.inicialesProcessor = inicialesProcessor;
        }
        // GET: Iniciales
        public ActionResult Iniciales()
        {
            return View();
        }

        #region Metodos Publicos del Controlador
        [HttpGet]
        public ActionResult ObtenerCircuito() 
        {
            Respuesta.Data = new { Value = Usuario.IdCircuito, Text = Usuario.NombreCircuito };
            Respuesta.Estatus = EstatusRespuestaJSON.OK;
            Respuesta.Mensaje = Usuario.NombreCircuito;

            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerJuzgadoAcusatorio(int idCircuito)
        {
            List<Juzgado> juzgados = inicialesProcessor.RecuperaJuzgado(idCircuito, TipoJuzgado.ACUSATORIO);

            ValidaJuzgados(juzgados);
            Respuesta.Mensaje = inicialesProcessor.Mensaje;
   
            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerJuzgadoTradicional(int idDistrito)
        {
            List<Juzgado> juzgados = inicialesProcessor.RecuperaJuzgado(idDistrito, TipoJuzgado.TRADICIONAL);

            ValidaJuzgados(juzgados);
            Respuesta.Mensaje = inicialesProcessor.Mensaje;
            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerSalaAcusatorio() 
        {
            List<Juzgado> juzgados = inicialesProcessor.RecuperaSala(TipoJuzgado.ACUSATORIO);

            ValidaJuzgados(juzgados);
            Respuesta.Mensaje = inicialesProcessor.Mensaje;

            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerSalaTradicional()
        {
            List<Juzgado> juzgados = inicialesProcessor.RecuperaSala(TipoJuzgado.ACUSATORIO);

            ValidaJuzgados(juzgados);
            Respuesta.Mensaje = inicialesProcessor.Mensaje;

            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerDistritosPorCircuito(int idCircuito)
        {
            List<Distrito> distritos = inicialesProcessor.RecuperaDistrito(idCircuito);

            if (distritos == null)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Data = null;
            }
            else
            {

                if (distritos.Count > 0)
                {
                    var lista = ViewHelper.Options(distritos, "IdDistrito", "Nombre");
                    Respuesta.Estatus = EstatusRespuestaJSON.OK;
                    Respuesta.Data = lista;
                }
                else
                {
                    Respuesta.Data = new object();
                    Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                }
            }

            Respuesta.Mensaje = inicialesProcessor.Mensaje;
            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerExpedientePorNUC(int idJuzgado, string nuc) 
        {
            List<Expediente> expedientes = inicialesProcessor.RecuperaExpedientes(idJuzgado, nuc, TipoExpediente.NUC);

            ValidaExpedientes(expedientes);
            Respuesta.Mensaje = inicialesProcessor.Mensaje;

            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerExpedientePorCausa(int idJuzgado, string numeroCausa)
        {
            List<Expediente> expedientes = inicialesProcessor.RecuperaExpedientes(idJuzgado, numeroCausa, TipoExpediente.CAUSA);
            
            ValidaExpedientes(expedientes);
            Respuesta.Mensaje = inicialesProcessor.Mensaje;

            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Metodos Privados del Controlador
        private void ValidaJuzgados(List<Juzgado> juzgados) 
        {
            if (juzgados == null)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Data = null;
            }
            else
            {
                if (juzgados.Count > 0)
                {
                    var lista = ViewHelper.Options(juzgados, "IdJuzgado", "Nombre");
                    Respuesta.Estatus = EstatusRespuestaJSON.OK;
                    Respuesta.Data = lista;
                }
                else
                {
                    Respuesta.Data = new object();
                    Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                }
            }
        }

        private void ValidaExpedientes(List<Expediente> expedientes) 
        {
            if (expedientes == null)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Data = null;
            }
            else
            {
                if (expedientes.Count > 0)
                {
                    var lista = expedientes;
                    Respuesta.Estatus = EstatusRespuestaJSON.OK;
                    Respuesta.Data = lista;
                }
                else
                {
                    Respuesta.Data = new object();
                    Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                }
            }
        }
        #endregion
    }
}