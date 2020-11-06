using AutoMapper;
using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;
using PoderJudicial.SIPOH.Negocio.Interfaces;
using PoderJudicial.SIPOH.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PoderJudicial.SIPOH.WebApp.Controllers
{
    public class ConsignacionesHistoricasController : BaseController
    {
        private readonly IConsignacionesHistoricasProcessor consignacionesProcessor;
        private readonly ICatalogosProcessor catalogosProcessor;
        private readonly IMapper mapper;

        public ConsignacionesHistoricasController(ICatalogosProcessor catalogosProcessor, IConsignacionesHistoricasProcessor consignacionesProcessor, IMapper mapper)
        {
            this.consignacionesProcessor = consignacionesProcessor;
            this.catalogosProcessor = catalogosProcessor;
            this.mapper = mapper;
        }

        // GET: ConsignacionesHistoricas
        public ActionResult CrearConsignacionHistorica()
        {
            try
            {
                List<Juzgado> juzgadosAcusatorios = catalogosProcessor.ObtieneJuzgadosPorTipoSistema(Usuario.IdCircuito, TipoSistema.ACUSATORIO);
                List<Distrito> distritos = catalogosProcessor.ObtieneDistritosPorCircuito(Usuario.IdCircuito);


                //Parametros al View Bag PickList
                ViewBag.IdCircuito = Usuario.IdCircuito;
                ViewBag.JuzgadosAcusatorios = ViewHelper.CreateSelectList(juzgadosAcusatorios, "IdJuzgado", "Nombre");
                ViewBag.DistritosPorCircuito = ViewHelper.CreateSelectList(distritos, "IdDistrito", "Nombre");

                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        /// <summary>
        /// Metodo del Controlador que retorna un listado de tipo Option, referente a los juzgados relacionados a un distrito
        /// </summary>
        /// <param name="idDistrito">Id del Distrito</param>
        /// <returns>Lista de Tipo Option</returns>
        [HttpGet]
        public ActionResult ObtenerJuzgadoTradicional(int idDistrito)
        {
            try
            {
                List<Juzgado> juzgados = catalogosProcessor.ObtieneJuzgadosPorTipoSistema(idDistrito, TipoSistema.TRADICIONAL);

                ValidaJuzgados(juzgados);
                Respuesta.Mensaje = catalogosProcessor.Mensaje;

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
        /// Valida que exista un registro de cuasa por medio del idJuzgado y su numero de causa
        /// </summary>
        /// <param name="idJuzgado">IdJuzgado asigando a la causa</param>
        /// <param name="numeroDeCausa">Numero de Causa</param>
        /// <returns>Retorna JSON Respuesta</returns>
        [HttpGet]
        public ActionResult ValidaCausaEnJuzgadoPorNumeroCausa(int idJuzgado, string numeroDeCausa)
        {
            try
            {
                bool? existe = consignacionesProcessor.ValidaExistenciaDeCausaPorJuzgadoMasNumeroDeCausaNUC(idJuzgado, numeroDeCausa);

                if (existe == null)
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                }
                else 
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.OK;
                }

                Respuesta.Data = existe;
                Respuesta.Mensaje = consignacionesProcessor.Mensaje;

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
        /// Valida que exista un registro de cuasa por medio del idJuzgado y su numero unico de caso
        /// </summary>
        /// <param name="idJuzgado">IdJuzgado asigando a la causa</param>
        /// <param name="numeroDeCausa">Numero unico de caso de la causa</param>
        /// <returns>Retorna JSON Respuesta</returns>
        [HttpGet]
        public ActionResult ValidaCausaEnJuzgadoPorNumeroNUC(int idJuzgado, string numeroCausa, string nuc)
        {
            try
            {
                bool? existe = consignacionesProcessor.ValidaExistenciaDeCausaPorJuzgadoMasNumeroDeCausaNUC(idJuzgado, numeroCausa, nuc);

                if (existe.Value)
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.OK;
                }
                else
                {
                    Respuesta.Estatus = !existe.Value ? EstatusRespuestaJSON.SIN_RESPUESTA : EstatusRespuestaJSON.ERROR;
                }

                Respuesta.Data = existe;
                Respuesta.Mensaje = consignacionesProcessor.Mensaje;

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
    }
}