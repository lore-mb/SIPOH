using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;

using System.Collections.Generic;

namespace PoderJudicial.SIPOH.Negocio.Interfaces
{
    public interface IInicialesProcessor
    {
        string Mensaje { get; set; }
        List<Juzgado> RecuperaJuzgado(int idFiltro, TipoJuzgado tipoJuzgado);
        List<Juzgado> RecuperaSala(TipoJuzgado tipoJuzgado);
        List<Distrito> RecuperaDistrito(int idCircuito);
        List<Expediente> RecuperaExpedientes(int idJuzgado, string numeroExpediente, TipoExpediente expediente);
    }
}
