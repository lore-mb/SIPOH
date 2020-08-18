using PoderJudicial.SIPOH.Negocio.Interfaces;
using System.Web.Mvc;

namespace PoderJudicial.SIPOH.WebApp.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IPruebaProcessor processor;

        public HomeController(IPruebaProcessor processor) 
        {
            this.processor = processor;
        }

        public ActionResult Index()
        {
            processor.PruebaProcessorInterface();
            return View();
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}