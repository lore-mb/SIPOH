using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;

using System.Collections.Generic;

namespace PoderJudicial.SIPOH.Negocio.Interfaces
{
    public interface IPromocionesProcessor {
        string Mensaje { get; set; }

        List<Juzgado> ObtenerJuzgadoEjecucionPorCircuito(int idcircuito);
        List<Ejecucion> ObtenerEjecucionPorJuzgado(int Juzgado, string NoEjecucion);
        List<Expediente> ObtenerExpedientesPorEjecucion(int idEjecucion);
        Expediente ObtenerExpedienteEjecucionCausa(int idExpediente);
        List<Anexo> ObtenerAnexosEjecucion(string tipo);
    }
}
