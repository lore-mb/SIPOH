
using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;

namespace PoderJudicial.SIPOH.AccesoDatos.Interfaces
{
    public interface ICuentaRepository
    {
        string MensajeError { get; set; }
        Estatus Estatus { set; get; }
        Usuario LogIn(string email, string password);
    }
}
