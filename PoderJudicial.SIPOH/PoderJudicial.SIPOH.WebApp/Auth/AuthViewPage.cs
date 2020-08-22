using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace PoderJudicial.SIPOH.WebApp.Auth
{
    public abstract class AuthViewPage<TModel> : WebViewPage<TModel>
    {
        protected AppUsuario Usuario
        {
            get
            {
                return new AppUsuario(this.User as ClaimsPrincipal);
            }
        }
    }

    public abstract class AuthViewPage : AuthViewPage<dynamic>
    {

    }
}