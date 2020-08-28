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
        public ActionResult Busquedas()
        {
            return View();
        }
    }
}