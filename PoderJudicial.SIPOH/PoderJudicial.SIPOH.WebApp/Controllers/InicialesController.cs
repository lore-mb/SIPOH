using AutoMapper;
using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;
using PoderJudicial.SIPOH.Negocio.Interfaces;
using PoderJudicial.SIPOH.WebApp.Helpers;
using PoderJudicial.SIPOH.WebApp.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PoderJudicial.SIPOH.WebApp.Controllers
{
    public class InicialesController : BaseController
    {
        private readonly IInicialesProcessor inicialesProcessor;
        private readonly IMapper mapper;


        public InicialesController(IInicialesProcessor inicialesProcessor, IMapper mapper) 
        {
            this.inicialesProcessor = inicialesProcessor;
            this.mapper = mapper;
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
            Expediente expedientes = inicialesProcessor.RecuperaExpedientes(idJuzgado, nuc, TipoExpediente.NUC);

            ValidaExpedientes(expedientes);
            Respuesta.Mensaje = inicialesProcessor.Mensaje;

            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ObtenerExpedientePorCausa(int idJuzgado, string numeroCausa)
        {
            Expediente expedientes = inicialesProcessor.RecuperaExpedientes(idJuzgado, numeroCausa, TipoExpediente.CAUSA);
            
            ValidaExpedientes(expedientes);
            Respuesta.Mensaje = inicialesProcessor.Mensaje;

            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpGet]
        public ActionResult ConsultarSentenciadoBeneficiario(string nombreBene, string apellidoPaternoBene, string apellidoMaternoBene) 
        {
            List<Ejecucion> beneficiarios = inicialesProcessor.RecuperaSentenciadoBeneficiario(nombreBene, apellidoPaternoBene, apellidoMaternoBene);
            
            //Se genera DTO con la informacion necesaria para la solicitud Ajax
            List<BeneficiarioDTO> beneficiariosDTO = mapper.Map<List<Ejecucion>, List<BeneficiarioDTO>>(beneficiarios);

            if (beneficiariosDTO == null)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Data = null;
            }
            else
            {
                if (beneficiariosDTO.Count > 0)
                {
                    var lista = beneficiariosDTO;
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

        private void ValidaExpedientes(Expediente expediente) 
        {
            if (expediente == null)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Data = null;
            }
            else
            {
                if (expediente.IdExpediente == default)
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                    Respuesta.Data = null;
                }
                else
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.OK;
                    Respuesta.Data = expediente;
                }
            }
        }
        #endregion
    }
}