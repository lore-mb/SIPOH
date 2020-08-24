using System;
using System.Security.Claims;

namespace PoderJudicial.SIPOH.WebApp.Auth
{  
    public class AppUsuario : ClaimsPrincipal
    {
        public int Id
        {
            get
            {
                return Convert.ToInt32(FindFirst(ClaimTypes.Sid).Value);
            }
        }

        public string Nombre
        {
            get
            {
                return FindFirst(ClaimTypes.Name).Value;
            }
        }

        public string NombreIdentificador
        {
            get
            {
                return FindFirst(ClaimTypes.NameIdentifier).Value;
            }
        }


        public int IdJuzgado
        {
            get
            {
                return Convert.ToInt32(FindFirst(ClaimTypes.Locality).Value);
            }
        }

        public string NombreJuzgado
        {
            get
            {
                return FindFirst(ClaimTypes.StreetAddress).Value;
            }
        }

        public int IdDistrito
        {
            get
            {
                return Convert.ToInt32(FindFirst(ClaimTypes.SerialNumber).Value);
            }
        }

        public string NombreDistrito
        {
            get
            {
                return FindFirst(ClaimTypes.PostalCode).Value;
            }
        }

        public int IdCircuito
        {
            get
            {
                return Convert.ToInt32(FindFirst(ClaimTypes.Country).Value);
            }
        }

        public string NombreCircuito
        {
            get
            {
                return FindFirst(ClaimTypes.GivenName).Value;
            }
        }

        public string Rol
        {
            get
            {
                return FindFirst(ClaimTypes.Role).Value;
            }
        }

        public AppUsuario(ClaimsPrincipal principal) : base(principal)
        {


        }
    }
}