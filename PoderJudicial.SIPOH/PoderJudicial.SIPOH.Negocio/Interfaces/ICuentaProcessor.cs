using PoderJudicial.SIPOH.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoderJudicial.SIPOH.Negocio
{
    public interface ICuentaProcessor
    {
        string Mensaje { get; set; }
        Usuario ValidarLogInUsuario(string email, string password);
    }
}
