using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;
using System.Collections.Generic;

namespace PoderJudicial.SIPOH.Negocio.Interfaces
{
    public interface IReportesProcessor
    {
        #region Propiedades
        string Mensaje { get; set; }
        #endregion

        #region Metodos
        List<Juzgado> ObtenerJuzgadoPorCircuito(int IdCircuito);
        List<EjecucionCausa> ListaInicialesPromocionesPorDia(Instancia tipoReporte, string fechaHoy, int idJuzgado);
        List<EjecucionCausa> ListaInicialesPromocionesPorRangoFecha(Instancia tipoReporte, string fechaInicial, string fechaFinal, int idJuzgado);
        #endregion
    }
}
