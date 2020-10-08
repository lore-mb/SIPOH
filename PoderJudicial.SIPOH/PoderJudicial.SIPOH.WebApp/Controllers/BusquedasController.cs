using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoderJudicial.SIPOH.WebApp.Controllers
{
    public class BusquedasController : BaseController
    {
        // GET: Busquedas metodo para mandar a llamar con ajax
        public ActionResult BusquedaNumeroEjecucion()
        {
            //Parametros al View Bag PickList
            ViewBag.IdCircuito = Usuario.IdCircuito;
            ViewBag.JuzgadosAcusatorios = new SelectList(new List<Ejecucion>());
            ViewBag.DistritosPorCircuito = new SelectList(new List<Ejecucion>());
            ViewBag.SalasAcusatorio = new SelectList(new List<Ejecucion>());
            ViewBag.SalasTradicional = new SelectList(new List<Ejecucion>());
            ViewBag.AnexosInicales = new SelectList(new List<Ejecucion>());
            ViewBag.Solicitudes = new SelectList(new List<Ejecucion>());
            ViewBag.Solicitantes = new SelectList(new List<Ejecucion>());

            //Campos Banderas para validacio de "OTROS" de PickList
            ViewBag.IdOtroAnexos = 10;
            ViewBag.IdOtroSolicitud = 10;
            ViewBag.IdOtroSolicitante = 10;
            ViewBag.SalasAcusatorioTotal = 10;

            return View();
        }
    }
}