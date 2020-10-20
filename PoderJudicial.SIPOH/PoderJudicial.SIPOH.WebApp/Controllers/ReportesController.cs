using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Negocio.Interfaces;
using PoderJudicial.SIPOH.WebApp.Helpers;
using PoderJudicial.SIPOH.WebApp.Models;
using PoderJudicial.SIPOH.Entidades.Enum;
using System.Collections.Generic;
using System.Web.Mvc;
using System;

namespace PoderJudicial.SIPOH.WebApp.Controllers
{
    public class ReportesController : BaseController
    {

        #region Inyección Dependencias 
        public readonly IReportesProcessor reportesProcessor;

        public ReportesController(IReportesProcessor reportesProcessor)
        {
            this.reportesProcessor = reportesProcessor;
        }
        #endregion

        #region Retorna Vista Reportes
        public ActionResult Reportes()
        {
            List<Juzgado> ComboListaJuzgadosIniciales = reportesProcessor.ObtenerJuzgadoPorCircuito(Usuario.IdCircuito);
            List<Juzgado> ComboListaJuzgadosPromociones = reportesProcessor.ObtenerJuzgadoPorCircuito(Usuario.IdCircuito);
            ViewBag.ListaCircuitoJuzgadoIniciales = ComboListaJuzgadosIniciales != null ? ComboListaJuzgadosIniciales : new List<Juzgado>();
            ViewBag.ListaCircuitoJuzgadoPromociones = ComboListaJuzgadosPromociones != null ? ComboListaJuzgadosPromociones : new List<Juzgado>();
            return View();
        }
        #endregion

        [HttpGet]
        public ActionResult ListaJuzgadoPorCircuito(int IdCircuito)
        {
            try
            {
                List<Juzgado> ListaJuzgados = reportesProcessor.ObtenerJuzgadoPorCircuito(IdCircuito);
                ValidarJuzgado(ListaJuzgados);
                Respuesta.Mensaje = reportesProcessor.Mensaje;
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

        #region Metodos Privados

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