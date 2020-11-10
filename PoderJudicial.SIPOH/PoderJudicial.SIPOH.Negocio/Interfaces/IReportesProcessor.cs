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
        List<Reporte> RegistrosReportePorRangoFecha(TipoReporteRangoFecha TipoReporte, string FechaInicial, string FechaFinal, int IdJuzgado);
        List<Reporte> RegistrosReportePorDia(TipoReporteDia TipoReporte, string FechaHoy, int IdJuzgado);
        #endregion
    }
}
