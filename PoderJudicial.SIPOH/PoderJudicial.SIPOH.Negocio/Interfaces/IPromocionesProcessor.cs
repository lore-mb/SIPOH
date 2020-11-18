using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;
using System.Collections.Generic;

namespace PoderJudicial.SIPOH.Negocio.Interfaces
{
    public interface IPromocionesProcessor
    {
        string Mensaje { get; set; }
        List<Ejecucion> ObtenerEjecucionPorJuzgado(int idJuzgado, string noEjecucion);
        List<Expediente> ObtenerExpedientesRelacionadoEjecucion(int idExpediente);
        int? RegistrarEjecucionPosterior(EjecucionPosterior ejecucionPosterior, List<Anexo> anexos);
        bool ObtenerInformacionRegistroEjecucionPosterior(int folioEjecucion, ref EjecucionPosterior ejecucion, ref List<Anexo> anexo, ref List<Relacionadas> relacionada);
    }
}
