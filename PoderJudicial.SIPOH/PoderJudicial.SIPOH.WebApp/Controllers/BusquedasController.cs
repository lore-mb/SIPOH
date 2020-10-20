using AutoMapper;
using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Negocio.Interfaces;
using PoderJudicial.SIPOH.WebApp.Helpers;
using PoderJudicial.SIPOH.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Mvc;

namespace PoderJudicial.SIPOH.WebApp.Controllers
{
    public class BusquedasController : BaseController
    {
        private readonly IBusquedasProcessor busquedaProcessor;
        private readonly IMapper mapper;

        /// <summary>
        /// Metodo Constructor del Controlador Busquedas
        /// </summary>
        /// <param name="busquedaProcessor">Objeto que contiene funcionalidad para el proceso de Busqueda</param>
        /// <param name="mapper">Mapeador de objetos</param>
        public BusquedasController(IBusquedasProcessor busquedaProcessor, IMapper mapper)
        {
            this.busquedaProcessor = busquedaProcessor;
            this.mapper = mapper;
        }

        #region Metodos Publicos 
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
        /// Metodo del controlador para obtener los registros de ejecucion por media de una parte de la causa relacionada
        /// </summary>
        /// <param name="nombre">Nombre de la parte</param>
        /// <param name="apellidoPaterno">Apellido paterno de la parte</param>
        /// <param name="apellidoMaterno">Apellido materno de la parte</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BusquedaPorPartesCausa(string nombre, string apellidoPaterno, string apellidoMaterno)
        {
            try
            {
                List<Ejecucion> numerosDeEjecucion = busquedaProcessor.ObtenerEjecucionPorPartesCausa(nombre, apellidoPaterno, apellidoMaterno, Usuario.IdCircuito);

                if (numerosDeEjecucion == null)
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                    Respuesta.Data = null;      
                }
                else
                {
                    //Se genera DTO con la informacion necesaria para la solicitud Ajax
                    List<EjecucionDTO> numerosDeEjecucionDTO = mapper.Map<List<Ejecucion>, List<EjecucionDTO>>(numerosDeEjecucion);

                    if (numerosDeEjecucionDTO.Count > 0)
                    {
                        Respuesta.Estatus = EstatusRespuestaJSON.OK;
                        Respuesta.Data = new { busquedaNumerosEjecucionPartes = numerosDeEjecucionDTO };
                    }
                    else
                    {
                        Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                        Respuesta.Data = new object();
                    }
                }

                Respuesta.Mensaje = busquedaProcessor.Mensaje;

                Thread.Sleep(500);
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
        ///  Metodo del controlador para obtener los registros de ejecucion por medio de un beneficiario
        /// </summary>
        /// <param name="nombre">Nombre del beneficiario</param>
        /// <param name="apellidoPaterno">Apellido paterno del beneficiario</param>
        /// <param name="apellidoMaterno">Apellido materno del beneficiario</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BusquedaPorBeneficiario(string nombre, string apellidoPaterno, string apellidoMaterno)
        {
            try
            {
                List<Ejecucion> numerosDeEjecucion = busquedaProcessor.ObtenerEjecucionSentenciadoBeneficiario(nombre, apellidoPaterno, apellidoMaterno, Usuario.IdCircuito);
               
                if (numerosDeEjecucion == null)
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                    Respuesta.Data = null;
               
                }
                else
                {
                    //Se genera DTO con la informacion necesaria para la solicitud Ajax
                    List<EjecucionDTO> numerosDeEjecucionDTO = mapper.Map<List<Ejecucion>, List<EjecucionDTO>>(numerosDeEjecucion);

                    if (numerosDeEjecucionDTO.Count > 0)
                    {
                        Respuesta.Estatus = EstatusRespuestaJSON.OK;
                        Respuesta.Data = new { busquedaNumerosEjecucion = numerosDeEjecucionDTO };
                  
                    }
                    else
                    {
                        Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                        Respuesta.Data = new object();      
                    }
                }

                Respuesta.Mensaje = busquedaProcessor.Mensaje;

                Thread.Sleep(500);
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
        /// Metodo del controlador para obtener los registros de ejecucion por medio de una causa relacionada a la ejecucion
        /// </summary>
        /// <param name="numCausa">Numero de Causa relacionada a la ejecucion</param>
        /// <param name="idJuzgado">Id del juzgado de la Causa relacionada a la ejecucion</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BusquedaPorNumeroCausa(string numCausa, int idJuzgado)
        {
            try
            {
                List<Ejecucion> numerosDeEjecucion = busquedaProcessor.ObtenerEjecucionPorNumeroCausa(numCausa, idJuzgado, Usuario.IdCircuito);

                if (numerosDeEjecucion == null)
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                    Respuesta.Data = null;
                
                }
                else
                {
                    List<EjecucionDTO> numerosDeEjecucionDTO = mapper.Map<List<Ejecucion>, List<EjecucionDTO>>(numerosDeEjecucion);

                    if (numerosDeEjecucionDTO.Count > 0)
                    {
                        Respuesta.Estatus = EstatusRespuestaJSON.OK;
                        Respuesta.Data = new { busquedaNumerosEjecucion = numerosDeEjecucionDTO };
                    }
                    else
                    {
                        Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                        Respuesta.Data = new object();
                    }
                }

                Respuesta.Mensaje = busquedaProcessor.Mensaje;

                Thread.Sleep(500);
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
        /// Metodo del controlador para obtener los registros de ejecucion por medio del Nuc de la causa relacionada a la ejecucion
        /// </summary>
        /// <param name="NUC">NUC de la causa relacionada a la ejecucion</param>
        /// <param name="idJuzgado"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BusquedaPorNUC(string nuc, int idJuzgado)
        {
            try
            {
                List<Ejecucion> numerosDeEjecucion = busquedaProcessor.ObtenerEjecucionPorNUC(nuc, idJuzgado);

                if (numerosDeEjecucion == null)
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                    Respuesta.Data = null;
                }
                else
                {
                    List<EjecucionDTO> numerosDeEjecucionDTO = mapper.Map<List<Ejecucion>, List<EjecucionDTO>>(numerosDeEjecucion);

                    if (numerosDeEjecucionDTO.Count > 0)
                    {
                        Respuesta.Estatus = EstatusRespuestaJSON.OK;
                        Respuesta.Data = new { busquedaNumerosEjecucion = numerosDeEjecucionDTO };

                    }
                    else
                    {
                        Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                        Respuesta.Data = new object();
                    }
                }

                Respuesta.Mensaje = busquedaProcessor.Mensaje;

                Thread.Sleep(500);
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
            }
            else
            {
                if (busquedaSolicitante.Count > 0)
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.OK;
                    Respuesta.Data = new { busquedaNumerosEjecucion=busquedaSolicitante };
                }
                else
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                    Respuesta.Data = new object();
                }
            }

            Respuesta.Mensaje = busquedaProcessor.Mensaje;

            Thread.Sleep(500);
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
                }
                else
                {
                    if (busquedaDetalleSolicitante.Count > 0)
                    {
                        Respuesta.Estatus = EstatusRespuestaJSON.OK;
                        Respuesta.Data = new { busquedaNumerosEjecucion=busquedaDetalleSolicitante };
                    }
                    else
                    {
                        Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                        Respuesta.Data = new object();
                    }
                }

                Respuesta.Mensaje = busquedaProcessor.Mensaje;

                Thread.Sleep(500);
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

        [HttpGet]
        public ActionResult ObtenerJuzgadosPorDistrito(int idDistrito) 
        {
            List<Juzgado> juzgados = busquedaProcessor.ObtenerJuzgadosPorDistritos(idDistrito);

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
                    Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                    Respuesta.Data = new object();
                }
            }

            Respuesta.Mensaje = busquedaProcessor.Mensaje;
            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }
       
        [HttpGet]
        public ActionResult ObtenerCausasRelacionadasEjecucion(int idEjecucion) 
        {
            try
            {
                //Defino mi lista y creo mi objeto---- - accedo a mi objeto general(inyeccion)
                List<Expediente> expedientesEjecucion = busquedaProcessor.ObtenerExpedientesPorEjecucion(idEjecucion);

                if (expedientesEjecucion == null)
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                    Respuesta.Data = null;
                }
                else
                {
                    //Se genera DTO con la informacion necesaria para la solicitud Ajax
                    List<ExpedienteDTO> causasEjecucion = mapper.Map<List<Expediente>, List<ExpedienteDTO>>(expedientesEjecucion);

                    if (causasEjecucion.Count > 0)
                    {
                        Respuesta.Estatus = EstatusRespuestaJSON.OK;
                        Respuesta.Data = new { CausasEjecucion = causasEjecucion };
                    }
                    else
                    {
                        Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                        Respuesta.Data = new object();
                    }
                }

                Respuesta.Mensaje = busquedaProcessor.Mensaje;

                Thread.Sleep(500);
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
    }
    #endregion
}