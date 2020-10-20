using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Negocio.Interfaces;
using PoderJudicial.SIPOH.WebApp.Helpers;
using PoderJudicial.SIPOH.WebApp.Models;
using PoderJudicial.SIPOH.Entidades.Enum;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;


namespace PoderJudicial.SIPOH.WebApp.Controllers
{
    public class PromocionesController : BaseController
    {
        private readonly IPromocionesProcessor promocionesProcessor;
        private readonly IMapper mapper;

        public PromocionesController(IPromocionesProcessor promocionesProcessor, IMapper mapper)
        {
            this.promocionesProcessor = promocionesProcessor;
            this.mapper = mapper;
        }

        #region Public Methods

        #region [MAIN] Return View & 
        public ActionResult CrearPromocion()
        {
            List<Anexo> ListarAnexosEjecucion = promocionesProcessor.ObtenerAnexosEjecucion("A");
            ViewBag.AnexoEjec = ListarAnexosEjecucion != null ? ListarAnexosEjecucion : new List<Anexo>(); 

            List<Juzgado> ListaJuzgadosPick = promocionesProcessor.ObtenerJuzgadoEjecucionPorCircuito(Usuario.IdCircuito);
            ViewBag.JuzgadoCircuito = ListaJuzgadosPick != null ? ListaJuzgadosPick : new List<Juzgado>();

            return View();
        }
        #endregion

        #region [OBTENER] Juzgados de ejecucion por circutios
        [HttpGet]
        public ActionResult ObtenerJuzgadoEjecucionPorCircuito(int idcircuito)
        {
            List<Juzgado> ListaJuzgados = promocionesProcessor.ObtenerJuzgadoEjecucionPorCircuito(idcircuito);
            ValidarJuzgado(ListaJuzgados);
            Respuesta.Mensaje = promocionesProcessor.Mensaje;
            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region [OBTENER] Datos Generales por Juzgado
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

        #endregion

        #region [OBTENER] Expedientes por numero de ejecución

        [HttpGet]
        public ActionResult ObtenerExpedientesPorEjecucion(int idEjecucion) {
            List<Expediente> ObtenerEPE = promocionesProcessor.ObtenerExpedientesPorEjecucion(idEjecucion);
            if (ObtenerEPE == null)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Data = null;
            }
            else {
                if (ObtenerEPE.Count > 0) {
                    Respuesta.Estatus = EstatusRespuestaJSON.OK;
                    Respuesta.Data = new { ObtenerEPE };
                } else {
                    Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                    Respuesta.Data = null;
                }
            }
            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region [OBTENER] Expedientes de ejecucion por causa
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

        #region [GUARDAR] Anexos-Ejecución
        [HttpPost]
        public ActionResult GuardarAnexosPostEjecucion(PromocionModelView Promociones)
        {
            try
            {
                // Parametros PostEjecucion & Mappeado
                PostEjecucion parametrosPost = mapper.Map<PromocionModelView, PostEjecucion>(Promociones);

                parametrosPost.IdUser = Usuario.Id;
                parametrosPost.IdEjecucionPosterior = 0;

                // Parametros Anexos & Mapeado
                List<Anexo> parametrosAnexos = mapper.Map<List<AnexosModelView>,List<Anexo>>(Promociones.Anexos);

                // Asignamos Parametros a nuestro metodo
                int? IdEjecucion = promocionesProcessor.GuardarPostEjecucion(parametrosPost, parametrosAnexos);

                if (IdEjecucion == null) {

                    Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                    Respuesta.Mensaje = promocionesProcessor.Mensaje;
                    Respuesta.Data = null;

                } else if (IdEjecucion != null) {

                    // Generacion de parametros cifrados
                    string Link = ViewHelper.EncodedActionLink("Detalle", "Promociones", new { IdEjecucion = IdEjecucion });
                    Respuesta.Estatus = EstatusRespuestaJSON.OK;
                    Respuesta.Mensaje = promocionesProcessor.Mensaje;
                    Respuesta.Data = new { Url = Link };
                }
                System.Threading.Thread.Sleep(2000);
                return Json(Respuesta, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Mensaje = ex.Message;
                Respuesta.Data = null;
                return Json(Respuesta, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region [CONSULTAR] Detalles de registro a promociones
        [HttpGet]
        [EncriptarParametroFilter]
        public ActionResult Detalle(int IdEjecucion) {
            try
            {
                // Este modelo se manda a la vista
                EjecucionModelView ModeloEjecucion = new EjecucionModelView();

                // Objetos como referencia al proccesor
                Ejecucion ejecucion = new Ejecucion();
                List<Anexo> anexos = new List<Anexo>();
                List<Relacionadas> ListRelacionadas = new List<Relacionadas>();

                // Acceder al metodo en proccesor y pasar Obtetos como referencia
                bool RespuestaMetodo = promocionesProcessor.InformacionRegistroPromocion(IdEjecucion, ref ejecucion, ref anexos, ref ListRelacionadas);

                if (ejecucion != null) {
                    ModeloEjecucion = mapper.Map<Ejecucion, EjecucionModelView>(ejecucion);
                    ModeloEjecucion.Anexos = mapper.Map<List<Anexo>, List<AnexosModelView>>(anexos);
                }

                
                ViewBag.Ejecucion = ListRelacionadas.Contains(Relacionadas.EJECUCION);
                ViewBag.Anexos = ListRelacionadas.Contains(Relacionadas.ANEXOS);
                ViewBag.RespuestaMetodo = RespuestaMetodo;
                ViewBag.Mensaje = promocionesProcessor.Mensaje;

                return View(ModeloEjecucion);
            }
            catch (Exception ex)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Mensaje = ex.Message;
                Respuesta.Data = null;

                return Json(Respuesta, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion

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