using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoderJudicial.SIPOH.Entidades
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreIdentificador { get; set; }
        public string Rol { get; set; }
        public string NombreCircuito { get; set; }
        public int IdCircuito { get; set; }
        public string NombreDistrito { set; get; }
        public int IdDistrito { set; get; }
        public string NombreJuzgado { set; get; }
        public int IdJuzgado { set; get; }
        public bool Activo { set; get; }
    }
}
