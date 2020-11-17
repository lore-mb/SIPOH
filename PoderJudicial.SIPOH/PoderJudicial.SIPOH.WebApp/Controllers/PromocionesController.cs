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
        #region Inyección de dependencias y mapeado de clases.
        private readonly IPromocionesProcessor promocionesProcessor;
        private readonly ICatalogosProcessor catalogosProcessor;
        private readonly IMapper mapper;

        public PromocionesController(IPromocionesProcessor promocionesProcessor, ICatalogosProcessor catalogosProcessor, IMapper mapper)
        {
            this.promocionesProcessor = promocionesProcessor;
            this.catalogosProcessor = catalogosProcessor;
            this.mapper = mapper;
        }
        #endregion

        #region Metodos Publicos.

        public ActionResult CrearPromocion()
        {
            List<Anexo> ListarAnexosEjecucion = catalogosProcessor.ObtieneAnexosPorTipo("A");
            ViewBag.AnexoEjec = ListarAnexosEjecucion != null ? ListarAnexosEjecucion : new List<Anexo>();

            List<Juzgado> ListaJuzgadosPick = catalogosProcessor.ObtieneJuzgadosPorCircuito(Usuario.IdCircuito);
            ViewBag.JuzgadoCircuito = ListaJuzgadosPick != null ? ListaJuzgadosPick : new List<Juzgado>();

            return View();
        }

        [HttpGet]
        public ActionResult ObtenerEjecucionPorJuzgado(int juzgado, string noEjecucion)
        {
            try
            {
                List<Ejecucion> ListaInformacion = promocionesProcessor.ObtenerEjecucionPorJuzgado(juzgado, noEjecucion);

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
                List<Anexo> parametrosAnexos = mapper.Map<List<AnexosModelView>, List<Anexo>>(Promociones.Anexos);

                // Asignamos Parametros a nuestro metodo
                int? IdEjecucion = promocionesProcessor.RegistrarEjecucionPosterior(parametrosPost, parametrosAnexos);

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
        public ActionResult Detalle(int IdEjecucion)
        {
            try
            {
                //// Este modelo se manda a la vista
                EjecucionPosteriorModelView ModeloEjecucionPosterior = new EjecucionPosteriorModelView();

                // Objetos como referencia al proccesor
                EjecucionPosterior ejecucion = new EjecucionPosterior();
                List<Anexo> anexos = new List<Anexo>();
                List<Relacionadas> ListRelacionadas = new List<Relacionadas>();

                // Acceder al metodo en proccesor y pasar Obtetos como referencia
                bool RespuestaMetodo = promocionesProcessor.ObtenerInformacionRegistroEjecucionPosterior(IdEjecucion, ref ejecucion, ref anexos, ref ListRelacionadas);

                if (ejecucion != null)
                {
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

        #region Metodos Privados.
        private void ValidarJuzgado(List<Juzgado> ListaJuzgados)
        {
            if (ListaJuzgados == null)
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Data = null;
            }
            else
            {
                if (ListaJuzgados.Count > 0)
                {
                    var ListadoJuzgados = ViewHelper.Options(ListaJuzgados, "IdJuzgado", "Nombre");
                    Respuesta.Estatus = EstatusRespuestaJSON.OK;
                    Respuesta.Data = ListadoJuzgados;
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