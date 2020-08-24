using PoderJudicial.SIPOH.WebApp.Auth;
using PoderJudicial.SIPOH.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PoderJudicial.SIPOH.WebApp.Controllers
{
    public class BaseController : Controller
    {
        protected AppUsuario Usuario
        {
            get
            {
                return new AppUsuario(User as ClaimsPrincipal);
            }
        }

        protected RespuestaJson Respuesta 
        {
            get 
            {
                return new RespuestaJson();
            }
        }

        protected string RenderViewToString(string viewName, object model = null)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        protected string Encrypt(string cadena)
        {
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(cadena);
            return Convert.ToBase64String(encryted);
        }

        protected string DeCrypt(string cadena)
        {
            byte[] decryted = Convert.FromBase64String(cadena);
            return System.Text.Encoding.Unicode.GetString(decryted);
        }

        protected string SHA1HashStringForUTF8String(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);

            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            return HexStringFromBytes(hashBytes);
        }

        protected static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }

        protected static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            char[] a = s.ToLower().ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
    }
}