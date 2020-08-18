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
        public string Correo { get; set; }
        public string Contrasenia { get; set; }
        public string Nombre { get; set; }
        public string NickName { get; set; }
        public int IdCircuito { set; get; }
        public bool Activo { set; get; }
    }
}
