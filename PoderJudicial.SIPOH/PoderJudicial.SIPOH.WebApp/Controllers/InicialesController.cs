using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Negocio.Enum;
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

        [HttpGet]
        public ActionResult ObtenerCircuito() 
        {
            Respuesta.Data = new { Value = Usuario.IdCircuito, Name = Usuario.NombreCircuito };
            Respuesta.Estatus = EstatusRespuestaJSON.OK;
            Respuesta.Mensaje = Usuario.NombreCircuito;

            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerJuzgadoAcusatorio(int idCircuito)
        {
            List<Juzgado> juzgados = inicialesProcessor.RecuperaJuzgado(idCircuito, TipoJuzgado.ACUSATORIO);

            if (juzgados == null)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Data = null;
            }

            if (juzgados.Count > 0)
            {
                var lista = juzgados.Options("IdJuzgado", "Nombre");
                Respuesta.Estatus = EstatusRespuestaJSON.OK;
                Respuesta.Data = lista;
            }
            else
            {
                Respuesta.Data = new object();
                Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
            }

            Respuesta.Mensaje = inicialesProcessor.Mensaje;
            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerJuzgadoTradicional(int idDistrito)
        {
            List<Juzgado> juzgados = inicialesProcessor.RecuperaJuzgado(idDistrito, TipoJuzgado.TRADICIONAL);

            if (juzgados == null)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Data = null;
            }

            if (juzgados.Count > 0)
            {
                var lista = juzgados.Options("IdJuzgado", "Nombre");
                Respuesta.Estatus = EstatusRespuestaJSON.OK;
                Respuesta.Data = lista;
            }
            else
            {
                Respuesta.Data = new object();
                Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
            }

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

            if (distritos.Count > 0)
            {
                var lista = distritos.Options("IdDistrito", "Nombre");
                Respuesta.Estatus = EstatusRespuestaJSON.OK;
                Respuesta.Data = lista;
            }
            else
            {
                Respuesta.Data = new object();
                Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
            }

            Respuesta.Mensaje = inicialesProcessor.Mensaje;
            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }
    }
}