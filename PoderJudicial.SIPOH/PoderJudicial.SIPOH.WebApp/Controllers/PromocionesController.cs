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

        public ActionResult CrearPromocion()
        {
            List<Anexo> ListarAnexosEjecucion = promocionesProcessor.ObtenerAnexosEjecucion("A");
            ViewBag.AnexoEjec = ListarAnexosEjecucion != null ? ListarAnexosEjecucion : new List<Anexo>(); 

            List<Juzgado> ListaJuzgadosPick = promocionesProcessor.ObtenerJuzgadoEjecucionPorCircuito(Usuario.IdCircuito);
            ViewBag.JuzgadoCircuito = ListaJuzgadosPick != null ? ListaJuzgadosPick : new List<Juzgado>();

            return View();
        }

        [HttpGet]
        public ActionResult ObtenerJuzgadoEjecucionPorCircuito(int idcircuito)
        {
            try
            {
                List<Juzgado> ListaJuzgados = promocionesProcessor.ObtenerJuzgadoEjecucionPorCircuito(idcircuito);

                ValidarJuzgado(ListaJuzgados);

                Respuesta.Mensaje = promocionesProcessor.Mensaje;

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
        public ActionResult ObtenerEjecucionPorJuzgado(int Juzgado, string NoEjecucion)
        {
            try
            {
                List<Ejecucion> ListaInformacion = promocionesProcessor.ObtenerEjecucionPorJuzgado(Juzgado, NoEjecucion);

                if (ListaInformacion == null)
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                    Respuesta.Data = null;
                }
                else
                {
                    if (ListaInformacion.Count > 0)
                    {
                        Respuesta.Estatus = EstatusRespuestaJSON.OK;
                        Respuesta.Data = new { ListaInformacion };
                    }
                    else
                    {
                        Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                        Respuesta.Data = new object();
                    }
                }
                Respuesta.Mensaje = promocionesProcessor.Mensaje;
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
        public ActionResult ObtenerExpedientesPorEjecucion(int idEjecucion) 
        {
            try
            {
                List<Expediente> ObtenerEPE = promocionesProcessor.ObtenerExpedientesRelacionadoEjecucion(idEjecucion);

                if (ObtenerEPE == null)
                {
                    Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                    Respuesta.Data = null;
                }
                else
                {
                    if (ObtenerEPE.Count > 0)
                    {
                        Respuesta.Estatus = EstatusRespuestaJSON.OK;
                        Respuesta.Data = new { ObtenerEPE };
                    }
                    else
                    {
                        Respuesta.Estatus = EstatusRespuestaJSON.SIN_RESPUESTA;
                        Respuesta.Data = null;
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

        [HttpPost]
        public ActionResult GuardarAnexosPostEjecucion(EjecucionPosteriorModelView Promociones)
        {
            try
            {
                // Parametros PostEjecucion & Mappeado
                EjecucionPosterior parametrosPost = mapper.Map<EjecucionPosteriorModelView, EjecucionPosterior>(Promociones);

                parametrosPost.IdUser = Usuario.Id;
                parametrosPost.IdEjecucionPosterior = 0;

                // Parametros Anexos & Mapeado
                List<Anexo> parametrosAnexos = mapper.Map<List<AnexosModelView>,List<Anexo>>(Promociones.Anexos);

                // Asignamos Parametros a nuestro metodo
                int? IdEjecucion = promocionesProcessor.GuardarPostEjecucion(parametrosPost, parametrosAnexos);

                if (IdEjecucion == null) 
                {

                    Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                    Respuesta.Mensaje = promocionesProcessor.Mensaje;
                    Respuesta.Data = null;

                } 
                else if (IdEjecucion != null) 
                {
                    // Generacion de parametros cifrados
                    string Link = ViewHelper.EncodedActionLink("Detalle", "Promociones", new { IdEjecucion = IdEjecucion });
                    Respuesta.Estatus = EstatusRespuestaJSON.OK;
                    Respuesta.Mensaje = promocionesProcessor.Mensaje;
                    Respuesta.Data = new { Url = Link };
                }
                System.Threading.Thread.Sleep(2000);
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
        [EncriptarParametroFilter]
        public ActionResult Detalle(int IdEjecucion) {
            try
            {
                //// Este modelo se manda a la vista
                //DetalleEjecucionModelView ModeloEjecucion = new DetalleEjecucionModelView();
                EjecucionPosteriorModelView ModeloEjecucionPosterior = new EjecucionPosteriorModelView();

                // Objetos como referencia al proccesor
                EjecucionPosterior ejecucion = new EjecucionPosterior();
                List<Anexo> anexos = new List<Anexo>();
                List<Relacionadas> ListRelacionadas = new List<Relacionadas>();

                // Acceder al metodo en proccesor y pasar Obtetos como referencia
                bool RespuestaMetodo = promocionesProcessor.InformacionRegistroPromocion(IdEjecucion, ref ejecucion, ref anexos, ref ListRelacionadas);

                if (ejecucion != null) {
                    //ModeloEjecucionPosterior = mapper.Map<EjecucionPosterior, DetalleEjecucionModelView>(ejecucion);
                    ModeloEjecucionPosterior = mapper.Map<EjecucionPosterior, EjecucionPosteriorModelView>(ejecucion);
                    ModeloEjecucionPosterior.Anexos = mapper.Map<List<Anexo>, List<AnexosModelView>>(anexos);
                }
              
                ViewBag.Ejecucion = ListRelacionadas.Contains(Relacionadas.EJECUCION);
                ViewBag.Anexos = ListRelacionadas.Contains(Relacionadas.ANEXOS);
                ViewBag.RespuestaMetodo = RespuestaMetodo;
                ViewBag.Mensaje = promocionesProcessor.Mensaje;

                return View(ModeloEjecucionPosterior);
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