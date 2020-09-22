using AutoMapper;
using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;
using PoderJudicial.SIPOH.Negocio.Interfaces;
using PoderJudicial.SIPOH.WebApp.Helpers;
using PoderJudicial.SIPOH.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            List<Solicitud> solicitudes = inicialesProcessor.RecuperaSolicitud();
            List<Solicitante> solicitantes = inicialesProcessor.RecuperaSolicitante();

            //Obtiene los Ids del tipo "OTRO" para la validacion de Pick List
            int idOtroAnexos = anexosEjecucion.Where(x => x.Tipo == "O").Select(x => x.IdAnexo).FirstOrDefault();
            int idOtroSolicitud = solicitudes.Where(x => x.Tipo == "O").Select(x => x.IdSolicitud).FirstOrDefault();
            int idOtroSolicitante = solicitantes.Where(x => x.Tipo == "O").Select(x => x.IdSolicitante).FirstOrDefault();

            //Parametros al View Bag
            ViewBag.IdCircuito = Usuario.IdCircuito;
            ViewBag.JuzgadosAcusatorios = juzgadosAcusatorios != null ? juzgadosAcusatorios : new List<Juzgado>();
            ViewBag.DistritosPorCircuito = distritos != null ? distritos : new List<Distrito>();
            ViewBag.SalasAcusatorio = salasAcusatorio != null ? salasAcusatorio : new List<Juzgado>();
            ViewBag.SalasTradicional = salasTradicional != null ? salasTradicional : new List<Juzgado>();
            ViewBag.AnexosInicales = anexosEjecucion != null ? anexosEjecucion : new List<Anexo>();
            ViewBag.Solicitudes = solicitudes != null ? solicitudes : new List<Solicitud>();
            ViewBag.Solicitantes = solicitantes != null ? solicitantes : new List<Solicitante>();
            ViewBag.IdOtroAnexos = idOtroAnexos;
            ViewBag.IdOtroSolicitud = idOtroSolicitud;
            ViewBag.IdOtroSolicitante = idOtroSolicitante;

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
        public ActionResult CrearEjecucion(EjecucionModelView modelo) 
        {
            //Se mapea la informacion a Ejecucion
            Ejecucion ejecucion = mapper.Map<EjecucionModelView, Ejecucion>(modelo);
            
            //Id del usuario logeado
            ejecucion.IdUsuario = Usuario.Id;

            List<Expediente> tocas = mapper.Map<List<TocasModelView>, List<Expediente>>(modelo.Tocas);
            List<Anexo> anexos = mapper.Map<List<AnexosModelView>, List<Anexo>>(modelo.Anexos);
            List<int> causas = modelo.Causas.Select(x => x.IdCausa).ToList();
            List<string> amparos = modelo.Amparos != null ? modelo.Amparos : new List<string>();

            int ? folio = inicialesProcessor.CrearEjecucion(ejecucion, tocas, anexos, amparos, causas, Usuario.IdCircuito);

            if (folio == null) 
            {
                Respuesta.Estatus = EstatusRespuestaJSON.ERROR;
                Respuesta.Mensaje = inicialesProcessor.Mensaje;
            }

            if (folio != null) 
            {
                Respuesta.Estatus = EstatusRespuestaJSON.OK;
                Respuesta.Mensaje = inicialesProcessor.Mensaje;
                Respuesta.Data = new { Folio = folio };
            }
   
            System.Threading.Thread.Sleep(2000);
            return Json(Respuesta, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Detalle(int ejecucion)
        {
            EjecucionModelView modelo = new EjecucionModelView();
            modelo.IdJuzgado = "JUZGADO PRIMERO DE EJECUCION DEL SISTEMA PROCESAL PENAL ACUSATORIO ORAL";
            modelo.NombreBeneficiario = "ESTHER";
            modelo.ApellidoPBeneficiario = "ROMERO";
            modelo.ApellidoMBeneficiario = "PARDO";
            modelo.IdEjecucion = ejecucion;
            modelo.NumeroEjecucion = "0310/2020";
            modelo.Solicitante = "JUZGADO";
            modelo.Solicitud = "REMISIÓN PARCIAL DE LA PENA";
            modelo.DetalleSolicitante = "EL DETALLE"; 
            modelo.Causas = new List<CausasModelView> 
            { 
                new CausasModelView() 
                { 
                    Delitos = "Delto",
                    NombreJuzgado = "Juzgado",
                    CausaNuc = "0001/2020", 
                    Inculpados = "Alberto Romero", 
                    Ofendidos = "Pedro alcatraz" 
                } 
            };
            modelo.Tocas = new List<TocasModelView>() 
            {
              new TocasModelView()
              {
                 Sala = "PRIMERA SALA PENAL",
                 NumeroDeToca = "0023/2014"
              },
              new TocasModelView()
              {
                 Sala = "SALA UNITARIA PARA ADOLECENTES",
                 NumeroDeToca = "0023/2014"
              }
            };
            modelo.Amparos = new List<string>()
            {
                "232344", "12312"
            };
            modelo.Anexos = new List<AnexosModelView>()
            {
                new AnexosModelView()
                {
                    Cantidad = 2,
                    Descripcion = "COPIA CERTIFICADA DE CONSTANCIA DE EJECUCIÓN DE PENA"
                }
            };
            modelo.Interno = "SI";
            modelo.FechaEjecucion = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
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