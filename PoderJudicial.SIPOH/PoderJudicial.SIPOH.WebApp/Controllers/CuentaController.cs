using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Negocio;
using PoderJudicial.SIPOH.WebApp.Models;

namespace PoderJudicial.SIPOH.WebApp.Controllers
{
    public class CuentaController : BaseController
    {
        //Atributos privados del controlador cuenta
        private readonly ICuentaProcessor processor;


        //Metodo constructor del controlador cuenta, se le inyecta la interfaz para el proceso de cuentas
        public CuentaController(ICuentaProcessor processor)
        {
            this.processor = processor;
        }

        #region Metodos publicos del Controlador
        [AllowAnonymous]
        [HttpGet]
        public ActionResult LogIn(string returnUrl)
        {
            var loginModelView = new LogInModelView
            {
                ReturnUrl = returnUrl
            };

            return View(loginModelView);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult LogIn(LogInModelView model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Usuario usuario = processor.ValidarLogInUsuario(model.Usuario, model.Password);

            if (usuario != null)
            {
                FirmaUsuario(usuario);
                return Redirect(GetRedirectUrl(model.ReturnUrl));
            }

            // user authN failed
            ModelState.AddModelError("", "Ocurrio un Error");
            return View(model);
        }
        #endregion

        #region Metodos privados del Controlador
        private void FirmaUsuario(Usuario usuario)
        {
            var identidad = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Sid, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Locality, usuario.IdJuzgado.ToString()),
                new Claim(ClaimTypes.StreetAddress, usuario.NombreJuzgado),
                new Claim(ClaimTypes.SerialNumber, usuario.IdDistrito.ToString()),
                new Claim(ClaimTypes.StreetAddress, usuario.IdCircuito.ToString()),
                new Claim(ClaimTypes.Role, usuario.Rol)
            }, "SipohAppCookie");

            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignIn(identidad);
        }

        private string GetRedirectUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return Url.Action("index", "home");
            }

            return returnUrl;
        }

        #endregion
    }
}