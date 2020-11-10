using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoderJudicial.SIPOH.AccesoDatos.Interfaces
{
    public interface IEjecucionRepository
    {
        string MensajeError { get; set; }
        Estatus Estatus { get; set; }
        List<Ejecucion> ConsultaEjecuciones(ParteCausaBeneficiario parteCausaBeneficiario, string nombre, string apellidoPaterno, string apellidoMaterno, int idCircuito);
        List<Ejecucion> ConsultaEjecuciones(TipoNumeroExpediente tipoNumeroExpediente, string numeroExpediente, int idJuzgado);
        List<Ejecucion> ConsultaEjecuciones(string detalleSolicitante, int idCircuito);
        List<Ejecucion> ConsultaEjecuciones(int idSolicitante, int idCircuito);
        Ejecucion ConsultaEjecucion(int idEjecucion);
        EjecucionPosterior ConsultaEjecucionPosterior(int IdEjecucionPosterior);
        void ConsultaRengoDeNumeroEjecucion(int idJuzgado, string anio, out string numeroEjecucionMin, out string numeroEjecucionMax);
        void ValidaNumeroEjecucion(int idJuzgadoEjecucion, string numeroEjecucion);
        int ? CreaEjecucion(Ejecucion ejecucion, List<int> causas, List<Toca> tocas, List<string> amparos, List<Anexo> anexos, int? idJuzgado, bool circuitoPachuca); 
        int ? CreaEjecucionPosterior(EjecucionPosterior ejecucionPosterior, List<Anexo> anexos);
        EjecucionPosterior ConsultarRegistroEjecucionPosterior(int IdEjecucionPosterior);
        List<Reporte> GenerarReporteRangoFecha(TipoReporteRangoFecha TipoReporte, string FechaInicial, string FechaFinal, int IdJuzgado);
        List<Reporte> GenerarReportePorDia(TipoReporteDia TipoReporte, string FechaHoy, int IdJuzgado);
    }
}
