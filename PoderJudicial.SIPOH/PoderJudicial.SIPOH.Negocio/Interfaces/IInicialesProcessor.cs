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
        Expediente RecuperaExpedientes(int idJuzgado, string numeroExpediente, TipoExpediente expediente);
        List<Ejecucion> RecuperaSentenciadoBeneficiario(string nombre, string apellidoPaterno, string apellidoMaterno);
        List<Anexo> RecuperaAnexos(string tipo);
        List<Solicitante> RecuperaSolicitante();
        List<Solicitud> RecuperaSolicitud();
        int? CrearEjecucion(Ejecucion ejecucion, List<Expediente> tocas, List<Anexo> anexos, List<string> amparos, List<int> causas, int circuito);
    }
}
