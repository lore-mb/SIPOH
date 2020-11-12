using AutoMapper;
using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;
using PoderJudicial.SIPOH.Negocio.Interfaces;
using PoderJudicial.SIPOH.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PoderJudicial.SIPOH.WebApp.Controllers
{
    public class ConsignacionesHistoricasController : BaseController
    {
        private readonly IConsignacionesHistoricasProcessor consignacionesProcessor;
        private readonly ICatalogosProcessor catalogosProcessor;
        private readonly IMapper mapper;

        /// <summary>
        /// Constructor del controlador, se inicializan objectos que son inyeccion de dependencias
        /// </summary>
        /// <param name="catalogosProcessor">Objeto que contiene la funcioanlidad para catalogos</param>
        /// <param name="consignacionesProcessor">Objeto que contiene la funcionalidad para Procesos</param>
        /// <param name="mapper">Objeto que contiene la funcioanlidad para el mapeo de objetos</param>
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
                //Obtencion de Datos para PinckList al cargado de la vista
                List<Juzgado> juzgadosAcusatorios = catalogosProcessor.ObtieneJuzgadosPorTipoSistema(Usuario.IdCircuito, TipoSistema.ACUSATORIO);
                List<Distrito> distritos = catalogosProcessor.ObtieneDistritosPorCircuito(Usuario.IdCircuito);
                List<Juzgado> salasAcusatorio = catalogosProcessor.ObtieneSalasPorTipoSistema(TipoSistema.ACUSATORIO);
                List<Juzgado> salasTradicional = catalogosProcessor.ObtieneSalasPorTipoSistema(TipoSistema.TRADICIONAL);
                List<Anexo> anexosEjecucion = catalogosProcessor.ObtieneAnexosPorTipo("A");
                List<Solicitud> solicitudes = catalogosProcessor.ObtieneSolicitudes();
                List<Solicitante> solicitantes = catalogosProcessor.ObtieneSolicitantes();

                //Obtiene los Ids del tipo "OTRO" para la validacion de Pick List
                int idOtroAnexos = anexosEjecucion.Where(x => x.Tipo == "O").Select(x => x.IdAnexo).FirstOrDefault();
                int idOtroSolicitud = solicitudes.Where(x => x.Tipo == "O").Select(x => x.IdSolicitud).FirstOrDefault();
                int idOtroSolicitante = solicitantes.Where(x => x.Tipo == "O").Select(x => x.IdSolicitante).FirstOrDefault();

                //Parametros al View Bag PickList
                ViewBag.IdCircuito = Usuario.IdCircuito;
                ViewBag.JuzgadosAcusatorios = ViewHelper.CreateSelectList(juzgadosAcusatorios, "IdJuzgado", "Nombre");
                ViewBag.DistritosPorCircuito = ViewHelper.CreateSelectList(distritos, "IdDistrito", "Nombre");
                ViewBag.SalasAcusatorio = ViewHelper.CreateSelectList(salasAcusatorio, "IdJuzgado", "Nombre");
                ViewBag.SalasTradicional = ViewHelper.CreateSelectList(salasTradicional, "IdJuzgado", "Nombre");
                ViewBag.AnexosInicales = ViewHelper.CreateSelectList(anexosEjecucion, "IdAnexo", "Descripcion");
                ViewBag.Solicitudes = ViewHelper.CreateSelectList(solicitudes, "IdSolicitud", "Descripcion");
                ViewBag.Solicitantes = ViewHelper.CreateSelectList(solicitantes, "IdSolicitante", "Descripcion");

                //Campos Banderas para validacio de "OTROS" de PickList
                ViewBag.IdOtroAnexos = idOtroAnexos;
                ViewBag.IdOtroSolicitud = idOtroSolicitud;
                ViewBag.IdOtroSolicitante = idOtroSolicitante;
                ViewBag.SalasAcusatorioTotal = salasAcusatorio != null ? salasAcusatorio.Count() : 0;
                ViewBag.EsConsignacionHistorica = true;

                return View();
            }
            catch (Exception ex)
            {
                return View("Error");
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
                Respuesta.Mensaje = "Ocurrio un error interno no controlado por el sistema, intente de nuevo o consulte a soporte";

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
        public ActionResult ValidaCausaEnJuzgadoPorNumeroNUC(int idJuzgado, string numeroDeCausa, string nuc)
        {
            try
            {
                bool? existe = consignacionesProcessor.ValidaExistenciaDeCausaEnJuzgado(idJuzgado, numeroDeCausa, nuc);

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
                Respuesta.Mensaje = "Ocurrio un error interno no controlado por el sistema, intente de nuevo o consulte a soporte";

                return Json(Respuesta, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Meotodo que vailida los juszgados obtenidos y genera un objeto de tipo respuesta
        /// </summary>
        /// <param name="juzgados">Lista de juzgados</param>
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