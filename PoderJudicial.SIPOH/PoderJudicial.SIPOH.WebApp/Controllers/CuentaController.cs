using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.WebApp.Models;

namespace PoderJudicial.SIPOH.WebApp.Controllers
{
    public class CuentaController : BaseController
    {
        [AllowAnonymous]
        // GET: Cuenta
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

            Usuario usuario = new Usuario();

            if (usuario != null)
            {
                FirmaUsuario(usuario);
                return Redirect(GetRedirectUrl(model.ReturnUrl));
            }

            // user authN failed
            ModelState.AddModelError("", "Ocurrio un Error");
            return View(model);
        }

        private void FirmaUsuario(Usuario usuario)
        {
            var identidad = new ClaimsIdentity(new[]
                {
                   new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                   new Claim(ClaimTypes.Name, usuario.NickName),
                   new Claim(ClaimTypes.Email, usuario.Correo),
                   new Claim(ClaimTypes.Locality, usuario.IdCircuito.ToString())
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
    }
}