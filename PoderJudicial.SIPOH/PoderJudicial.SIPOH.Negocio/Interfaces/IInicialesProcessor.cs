using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;

using System.Collections.Generic;

namespace PoderJudicial.SIPOH.Negocio.Interfaces
{
    public interface IInicialesProcessor
    {
        string Mensaje { get; set; }
        Expediente ObtieneCausaPorMedioDelJuzgadoNumeroCausaNUC(int idJuzgado, string numeroExpediente, TipoNumeroExpediente expediente);
        List<Ejecucion> ObtieneSentenciadosBeneficiariosPorMedioDelNombre(string nombre, string apellidoPaterno, string apellidoMaterno, int idCircuito);
        int? CreaRegistroInicialDeEjecucion(Ejecucion ejecucion, List<Toca> tocas, List<Anexo> anexos, List<string> amparos, List<int> causas, int circuito);
        bool ObtieneInformacionGeneralInicialDeEjecucion(int folio, ref Ejecucion ejecucion, ref List<Expediente> causas, ref List<Toca> tocas, ref List<string> amparos, ref List<Anexo> anexos, ref List<Relacionadas> relacionadas);
    }
}
