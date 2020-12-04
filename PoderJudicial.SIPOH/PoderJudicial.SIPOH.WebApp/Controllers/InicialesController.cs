using AutoMapper;
using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;
using PoderJudicial.SIPOH.Negocio.Interfaces;
using PoderJudicial.SIPOH.WebApp.Helpers;
using PoderJudicial.SIPOH.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace PoderJudicial.SIPOH.WebApp.Controllers
{
    public class InicialesController : BaseController
    {
        private readonly IInicialesProcessor inicialesProcessor;
        private readonly ICatalogosProcessor catalogosProcessor;
        private readonly IMapper mapper;

        public InicialesController(IInicialesProcessor inicialesProcessor, ICatalogosProcessor catalogosProcessor, IMapper mapper) 
        {
            
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            var estaAutenticado = identity.Identity.IsAuthenticated;
            
            if (!estaAutenticado)
            {
                RedirectToAction("index", "home");
            }

            this.inicialesProcessor = inicialesProcessor;
            this.catalogosProcessor = catalogosProcessor;
            this.mapper = mapper;
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        #region Metodos Publicos del Controlador
        public ActionResult CrearInicial()
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

                ViewBag.EsConsignacionHistorica = false;

                return View();
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

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
                Respuesta.Mensaje = "Ocurrio un error interno no controlado por el sistema";

                return Json(Respuesta, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ObtenerDistritosPorCircuito(int idCircuito)
        {
            try
            {
                List<Distrito> distritos = catalogosProcessor.ObtieneDistritosPorCircuito(idCircuito);
  
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
            catch (Exception ex)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Mensaje = "Ocurrio un error interno no controlado por el sistema";

                return Json(Respuesta, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ObtenerExpedientePorNUC(int idJuzgado, string nuc) 
        {
            try
            {
                Expediente expedientes = inicialesProcessor.ObtieneCausaPorJuzgadoMasTipoNumeroExpediente(idJuzgado, nuc, TipoNumeroExpediente.NUC);

                ValidaExpedientes(expedientes);
                Respuesta.Mensaje = inicialesProcessor.Mensaje;

                return Json(Respuesta, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Mensaje = "Ocurrio un error interno no controlado por el sistema";

                return Json(Respuesta, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ObtenerExpedientePorCausa(int idJuzgado, string numeroCausa)
        {
            try
            {
                Expediente expedientes = inicialesProcessor.ObtieneCausaPorJuzgadoMasTipoNumeroExpediente(idJuzgado, numeroCausa, TipoNumeroExpediente.CAUSA);
            
                ValidaExpedientes(expedientes);
                Respuesta.Mensaje = inicialesProcessor.Mensaje;

                return Json(Respuesta, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Mensaje = "Ocurrio un error interno no controlado por el sistema";

                return Json(Respuesta, JsonRequestBehavior.AllowGet);
            }
        }
   
        [HttpGet]
        public ActionResult ConsultarSentenciadoBeneficiario(string nombreBene, string apellidoPaternoBene, string apellidoMaternoBene) 
        {
            try
            {
                List<Ejecucion> beneficiarios = inicialesProcessor.ObtieneSentenciadosBeneficiariosPorNombre(nombreBene, apellidoPaternoBene, apellidoMaternoBene, Usuario.IdCircuito);
            
                if (beneficiarios == null)
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                    Respuesta.Data = null;
                }
                else
                {
                   //Se genera DTO con la informacion necesaria para la solicitud Ajax
                   List<BeneficiarioDTO> beneficiariosDTO = mapper.Map<List<Ejecucion>, List<BeneficiarioDTO>>(beneficiarios);

                   if (beneficiariosDTO.Count > 0)
                   {
                      Respuesta.Estatus = EstatusRespuestaJSON.OK;
                      Respuesta.Data = new { beneficiarios = beneficiariosDTO, total = beneficiariosDTO.Count };
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
            catch (Exception ex)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Mensaje = "Ocurrio un error interno no controlado por el sistema";

                return Json(Respuesta, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult CrearEjecucion(DetalleEjecucionModelView modelo) 
        {
            try
            {
                //Se mapea la informacion a Ejecucion
                Ejecucion ejecucion = mapper.Map<DetalleEjecucionModelView, Ejecucion>(modelo);
            
                //Id del usuario logeado
                ejecucion.IdUsuario = Usuario.Id;

                List<Toca> tocas = modelo.Tocas != null ? modelo.Tocas : new List<Toca>();
                List<Anexo> anexos = mapper.Map<List<AnexosModelView>, List<Anexo>>(modelo.Anexos);
                List<int> causas = modelo.IdExpedientes.Select(x => x.IdExpediente).ToList();
                List<string> amparos = modelo.Amparos != null ? modelo.Amparos : new List<string>();

                int? folio = inicialesProcessor.CreaRegistroDeEjecucion(ejecucion, tocas, anexos, amparos, causas, Usuario.IdCircuito);
 
                if (folio == null) 
                {
                   Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                   Respuesta.Mensaje = inicialesProcessor.Mensaje;
                }

                if (folio != null)
                { 
                   //Genera los parametro encriptados
                   string url = ViewHelper.EncodedActionLink("Detalle", "Iniciales", new { Folio = folio });

                   Respuesta.Estatus = EstatusRespuestaJSON.OK;
                   Respuesta.Mensaje = inicialesProcessor.Mensaje;
                   Respuesta.Data = new { Url = url };
                }
   
                return Json(Respuesta, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Mensaje = "Ocurrio un error interno no controlado por el sistema";

                return Json(Respuesta, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [EncriptarParametroFilter]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Detalle(int folio)
        {
            try
            {
                //Creacion del modelo que se enviara a la vista
                DetalleEjecucionModelView modelo = new DetalleEjecucionModelView();

                //Objetos para pasar como referencia
                Ejecucion inicial = new Ejecucion();
                List<Expediente> causas = new List<Expediente>();
                List<Toca> tocas = new List<Toca>();
                List<string> amparos = new List<string>();
                List<Anexo> anexos = new List<Anexo>();
                List<Relacionadas> entidad = new List<Relacionadas>();

                //Metodo que consulta a la bd la informacion relacionada a la ejecucion
                bool fueCorrectoElProceso = inicialesProcessor.ObtieneInformacionGeneralDeEjecucion(folio, ref inicial, ref causas, ref tocas, ref amparos, ref anexos, ref entidad);

                if (inicial != null)
                {
                    modelo = mapper.Map<Ejecucion, DetalleEjecucionModelView>(inicial);
                    modelo.IdExpedientes = mapper.Map<List<Expediente>, List<CausasModelView>>(causas);
                    modelo.Tocas = tocas == null ? new List<Toca>() : tocas;
                    modelo.Amparos = amparos == null ? new List<string>() : amparos;
                    modelo.Anexos = mapper.Map<List<Anexo>, List<AnexosModelView>>(anexos);
                }

                ViewBag.Ejecucion = entidad.Contains(Relacionadas.EJECUCION);
                ViewBag.Tocas = entidad.Contains(Relacionadas.TOCAS);
                ViewBag.Amparos = entidad.Contains(Relacionadas.AMPAROS);
                ViewBag.Causas = entidad.Contains(Relacionadas.CAUSAS);
                ViewBag.Anexos = entidad.Contains(Relacionadas.ANEXOS);
                ViewBag.fueCorrectoElProceso = fueCorrectoElProceso;
                ViewBag.Mensaje = inicialesProcessor.Mensaje;

                return View(modelo);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
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

        private void ValidaExpedientes(Expediente expediente) 
        {
            if (expediente == null)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Data = null;
            }
            else
            {
                //Se genera DTO con la informacion necesaria para la solicitud Ajax
                ExpedienteDTO causasDTO = mapper.Map<Expediente, ExpedienteDTO>(expediente);

                if (causasDTO.IdExpediente == default)
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                    Respuesta.Data = null;
                }
                else
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.OK;
                    Respuesta.Data = causasDTO;
                }
            }
        }
        #endregion
    }
}