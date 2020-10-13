using AutoMapper;
using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;
using PoderJudicial.SIPOH.Negocio.Interfaces;
using PoderJudicial.SIPOH.WebApp.Helpers;
using PoderJudicial.SIPOH.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoderJudicial.SIPOH.WebApp.Controllers
{
    public class BusquedasController : BaseController
    {
        /// <summary>
        /// [PROPIEDA 1:Se realiza inyeccion de dependencias y creo mi objeto]
        /// </summary>
        private readonly IBusquedasProcessor busquedaProcessor;
        private readonly IMapper mapper;

        /// <summary>
        /// [Metodo CONSTRUCTOR 2:de inyeccion en mi Interfaz y asigno mi objeto a mi clase]
        /// </summary>
        /// <param name="busquedaProcessor"></param>
        public BusquedasController(IBusquedasProcessor busquedaProcessor, IMapper mapper)
        {
            this.busquedaProcessor = busquedaProcessor;
            this.mapper = mapper;
        }

        #region Metodos Publicos 
        // GET: Busquedas metodo para mandar a llamar con ajax
        public ActionResult BusquedaNumeroEjecucion()
        {
            //metodo que retorna la lista de distritos
            List<Distrito> listaDistrito = busquedaProcessor.ObtenerDistritoPorCircuito(Usuario.IdCircuito);
            List<Juzgado> listaJuzgados = busquedaProcessor.ObtenerJuzgadosAcusatoriosPorCircuito(Usuario.IdCircuito);
            List<Solicitante> listaSolicitante = busquedaProcessor.ObtenerSolicitanteEjecucion();

            //Metodo que retorna el select list a partir de una lista
            ViewBag.DistritoPorCircuito = ViewHelper.CreateSelectList(listaDistrito, "IdDistrito", "Nombre");
            ViewBag.JuzgadosAcusatorios = ViewHelper.CreateSelectList(listaJuzgados, "IdJuzgado", "Nombre");
            ViewBag.Solicitante = ViewHelper.CreateSelectList(listaSolicitante, "IdSolicitante", "Descripcion");
            return View();
        }

        /// <summary>
        /// Validacion de respuesta a la consulta por partes de la causa
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="apellidoPaterno"></param>
        /// <param name="apellidoMaterno"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BusquedaPartesCausa(string nombre, string apellidoPaterno, string apellidoMaterno)
        {
            try
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
                    //Se genera DTO con la informacion necesaria para la solicitud Ajax
                    List<EjecucionDTO> busquedaPartesCausaDTO = mapper.Map<List<Ejecucion>, List<EjecucionDTO>>(busquedaPartesCausa);

                    if (busquedaPartesCausaDTO.Count > 0)
                    {
                        Respuesta.Estatus = EstatusRespuestaJSON.OK;
                        Respuesta.Data = new { busquedaPartes = busquedaPartesCausaDTO };
                    }
                    else
                    {
                        Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                        Respuesta.Data = new object();
                    }
                }

                Respuesta.Data = busquedaProcessor.Mensaje;
                return Json(Respuesta, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Mensaje = ex.Message;
                Respuesta.Data = null;

                return Json(Respuesta, JsonRequestBehavior.AllowGet);
            }

        }

        /// <summary>
        /// Validacion de respuesta  a la consulta por Sentenciado|Beneficiario
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="apellidoPaterno"></param>
        /// <param name="apellidoMaterno"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BusquedaPorBeneficiario(string nombre, string apellidoPaterno, string apellidoMaterno)
        {
            try
            {
                List<Ejecucion> busquedaBeneficiario = busquedaProcessor.ObtenerEjecucionSentenciadoBeneficiario(nombre, apellidoPaterno, apellidoMaterno);
               
                if (busquedaBeneficiario == null)
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                    Respuesta.Data = null;
                    Respuesta.Data = busquedaProcessor.Mensaje;
                }
                else
                {
                    if (busquedaBeneficiario.Count > 0)
                    {
                        Respuesta.Estatus = EstatusRespuestaJSON.OK;
                        Respuesta.Data = new { busquedaBeneficiario };
                        Respuesta.Data = busquedaProcessor.Mensaje;
                    }
                    else
                    {
                        Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                        Respuesta.Data = new object();
                        Respuesta.Data = busquedaProcessor.Mensaje;
                    }
                }

                return Json(Respuesta, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Mensaje = ex.Message;
                Respuesta.Data = null;

                return Json(Respuesta, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Validacion de respuesta a consulta por numero de causa
        /// </summary>
        /// <param name="numCausa"></param>
        /// <param name="idJuzgado"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BusquedaPorNumeroCausa(string numCausa, int idJuzgado)
        {
            try
            {
                List<Ejecucion> busquedaNumCausa = busquedaProcessor.ObtenerEjeucionPorNumeroCausa(numCausa, idJuzgado);
                if (busquedaNumCausa == null)
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                    Respuesta.Data = null;
                    Respuesta.Data = busquedaProcessor.Mensaje;
                }
                else
                {
                    if (busquedaNumCausa.Count > 0)
                    {
                        Respuesta.Estatus = EstatusRespuestaJSON.OK;
                        Respuesta.Data = new { busquedaNumCausa };
                        Respuesta.Data = busquedaProcessor.Mensaje;
                    }
                    else
                    {
                        Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                        Respuesta.Data = new object();
                        Respuesta.Data = busquedaProcessor.Mensaje;
                    }
                }
                return Json(Respuesta, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Mensaje = ex.Message;
                Respuesta.Data = null;
                Respuesta.Data = busquedaProcessor.Mensaje;
                return Json(Respuesta, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Validacion de respuesta a consulta por NUC
        /// </summary>
        /// <param name="NUC"></param>
        /// <param name="idJuzgado"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BusquedaNUC(String NUC, int idJuzgado)
        {
            try
            {
                List<Ejecucion> busquedaNUC = busquedaProcessor.ObtenerEjecucionPorNUC(NUC, idJuzgado);
                if (busquedaNUC == null)
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                    Respuesta.Data = null;
                    Respuesta.Data = busquedaProcessor.Mensaje;
                }
                else
                {
                    if (busquedaNUC.Count > 0)
                    {
                        Respuesta.Estatus = EstatusRespuestaJSON.OK;
                        Respuesta.Data = new { busquedaNUC };
                        Respuesta.Data = busquedaProcessor.Mensaje;
                    }
                    else
                    {
                        Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                        Respuesta.Data = new object();
                        Respuesta.Data = busquedaProcessor.Mensaje;
                    }
                }
                return Json(Respuesta, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Mensaje = ex.Message;
                Respuesta.Data = null;
                Respuesta.Data = busquedaProcessor.Mensaje;

                return Json(Respuesta, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Validacion de respuesta a consulta por Solictante
        /// </summary>
        /// <param name="idSolicitante"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BusquedaPorSolicitante(int idSolicitante)
        {
            List<Ejecucion> busquedaSolicitante = busquedaProcessor.ObtenerEjecucionPorSolicitante(idSolicitante);

            if (busquedaSolicitante == null)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Data = null;
                Respuesta.Data = busquedaProcessor.Mensaje;
            }
            else
            {
                if (busquedaSolicitante.Count > 0)
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.OK;
                    Respuesta.Data = new { busquedaSolicitante };
                    Respuesta.Data = busquedaProcessor.Mensaje;
                }
                else
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                    Respuesta.Data = new object();
                    Respuesta.Data = busquedaProcessor.Mensaje;
                }
            }
            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Validacion de respuesta a consulta  por DEtalle del solicitante
        /// </summary>
        /// <param name="detalleSolicitante"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BusquedaPorDetalleSolicitante(string detalleSolicitante) 
        {
            try
            {
                List<Ejecucion> busquedaDetalleSolicitante = busquedaProcessor.ObtenerEjecucionPorDetalleSolicitante(detalleSolicitante);
                if (busquedaDetalleSolicitante == null)
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                    Respuesta.Data = null;
                    Respuesta.Data = busquedaProcessor.Mensaje;
                }
                else
                {
                    if (busquedaDetalleSolicitante.Count > 0)
                    {
                        Respuesta.Estatus = EstatusRespuestaJSON.OK;
                        Respuesta.Data = new { busquedaDetalleSolicitante };
                        Respuesta.Data = busquedaProcessor.Mensaje;
                    }
                    else
                    {
                        Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                        Respuesta.Data = new object();
                        Respuesta.Data = busquedaProcessor.Mensaje;
                    }
                }
                return Json(Respuesta, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Mensaje = ex.Message;
                Respuesta.Data = null;
                Respuesta.Data = busquedaProcessor.Mensaje;

                return Json(Respuesta, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ObtenerJuzgadosPorDistrito(int idDistrito) 
        {
            List<Juzgado> juzgados = busquedaProcessor.ObtenerJuzgadosPorDistritos(idDistrito);

            if (juzgados == null)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Data = null;
                Respuesta.Data = busquedaProcessor.Mensaje;
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
                    Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                    Respuesta.Data = new object();
                    Respuesta.Data = busquedaProcessor.Mensaje;
                }
            }

            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }
    }
    #endregion
}