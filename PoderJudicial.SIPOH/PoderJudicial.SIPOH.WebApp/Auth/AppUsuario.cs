using System;
using System.Security.Claims;

namespace PoderJudicial.SIPOH.WebApp.Auth
{  
    public class AppUsuario : ClaimsPrincipal
    {
        public string Name
        {
            get
            {
                return FindFirst(ClaimTypes.Name).Value;
            }
        }

        public string Email
        {
            get
            {
                return FindFirst(ClaimTypes.Email).Value;
            }
        }

        public int Id
        {
            get
            {
                return Convert.ToInt32(FindFirst(ClaimTypes.NameIdentifier).Value);
            }
        }

        public int IdCircuito
        {
            get
            {
                return Convert.ToInt32(FindFirst(ClaimTypes.Locality).Value);
            }
        }

        public AppUsuario(ClaimsPrincipal principal) : base(principal)
        {


        }
    }
}