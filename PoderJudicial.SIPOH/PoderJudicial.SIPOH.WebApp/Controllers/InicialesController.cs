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

        #region Metodos Publicos del Controlador
        public ActionResult CrearInicial()
        { 
            List<Juzgado> juzgadosAcusatorios = inicialesProcessor.RecuperaJuzgado(Usuario.IdCircuito, TipoJuzgado.ACUSATORIO);
            List<Distrito> distritos = inicialesProcessor.RecuperaDistrito(Usuario.IdCircuito);
            List<Juzgado> salasAcusatorio = inicialesProcessor.RecuperaSala(TipoJuzgado.ACUSATORIO);
            List<Juzgado> salasTradicional = inicialesProcessor.RecuperaSala(TipoJuzgado.TRADICIONAL);
            List<Anexo> anexosEjecucion = inicialesProcessor.RecuperaAnexos("A");

            //Parametros al View Bag
            ViewBag.IdCircuito = Usuario.IdCircuito;
            ViewBag.JuzgadosAcusatorios = juzgadosAcusatorios != null ? juzgadosAcusatorios : new List<Juzgado>();
            ViewBag.DistritosPorCircuito = distritos != null ? distritos : new List<Distrito>();
            ViewBag.SalasAcusatorio = salasAcusatorio != null ? salasAcusatorio : new List<Juzgado>();
            ViewBag.SalasTradicional = salasTradicional != null ? salasTradicional : new List<Juzgado>();
            ViewBag.AnexosInicales = anexosEjecucion != null ? anexosEjecucion : new List<Anexo>();

            return View();
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

        [HttpPost]
        public ActionResult CrearEjecucion(DetalleEjecucionModelView modelo) 
        {
            Respuesta.Estatus = EstatusRespuestaJSON.OK;
            Respuesta.Mensaje = "Se creo la ejecucion";
            System.Threading.Thread.Sleep(2000);

            Respuesta.Data = new { Folio = 100 };
            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Detalle(int folio) 
        {
            DetalleEjecucionModelView modelo = new DetalleEjecucionModelView();
            modelo.JuzgadoEjecucion = "JUZGADO PRIMERO DE EJECUCION DEL SISTEMA PROCESAL PENAL ACUSATORIO ORAL";
            modelo.NombreBeneficiario = "ESTHER";
            modelo.ApellidoPaternoBeneficiario = "ROMERO";
            modelo.ApellidoMaternoBeneficiario = "PARDO";
            modelo.Folio = folio;
            modelo.NumeroExpediente = "0310/2020";
            modelo.Causas = new List<CausasModelView>();
            modelo.Tocas = new List<TocasModelView>();
            modelo.Amparos = new List<string>();
            modelo.Anexos = new List<AnexosModelView>();
            modelo.SentenciadoInterno = false;

            ViewBag.miEjemplo = "HOLA MUNDO";
            ViewBag.miEjemploII = "HOLA MUNDO II";

            return View(modelo);        
        }

        [HttpGet]
        public ActionResult GenerarSello()
        {
            Respuesta.Estatus = EstatusRespuestaJSON.OK;
            Respuesta.Mensaje = "Se creo la ejecucion";
            System.Threading.Thread.Sleep(2000);

            SelloModelView model = new SelloModelView();
            model.JuzgadoEjecucion = "JUZGADO PRIMERO DE EJECUCION DEL SISTEMA PROCESAL PENAL ACUSATORIO ORAL";
            model.NumeroExpediente = "0310/2020";
            model.Folio = 4606;

            Respuesta.VistaRender = RenderViewToString("_Sello", model);
            Respuesta.Data = null;

            return Json(Respuesta, JsonRequestBehavior.AllowGet);
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