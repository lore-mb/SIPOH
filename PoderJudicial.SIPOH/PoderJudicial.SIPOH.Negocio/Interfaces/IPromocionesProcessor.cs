using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;

using System.Collections.Generic;

namespace PoderJudicial.SIPOH.Negocio.Interfaces
{
    public interface IPromocionesProcessor {
        string Mensaje { get; set; }
        List<Ejecucion> ObtenerEjecucionPorJuzgado(int Juzgado, string NoEjecucion);
        List<Expediente> ObtenerExpedientesRelacionadoEjecucion(int IdExpediente);
        int? GuardarPostEjecucion(EjecucionPosterior PostEjecucion, List<Anexo> Anexos);
        bool InformacionRegistroPromocion(int FolioEjecucion, ref EjecucionPosterior Ejecucion, ref List<Anexo> Anexo, ref List<Relacionadas> Relacionada);
    }
}
