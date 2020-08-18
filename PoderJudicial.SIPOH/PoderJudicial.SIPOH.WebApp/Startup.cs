using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoderJudicial.SIPOH.WebApp
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "SipohAppCookie",

                // Va Cambiar en cuanto se tenga Vista para el inicio de sesion        
                LoginPath = new PathString("/Home/Index"),

                // Tiempo en exipirar la cookie para la sesion
                ExpireTimeSpan = TimeSpan.FromMinutes(30.0)
            });
        }
    }
}