using PoderJudicial.SIPOH.AccesoDatos.Enum;
using PoderJudicial.SIPOH.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoderJudicial.SIPOH.AccesoDatos.Interfaces
{
    public interface ICuentaRepository
    {
        string MensajeError { get; set; }
        Usuario LogIn(string email, string password, ref Resultado res);
    }
}
