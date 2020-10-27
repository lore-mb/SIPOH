
using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoderJudicial.SIPOH.AccesoDatos.Interfaces
{
    public interface ICatalogosRepository
    {
        string MensajeError { get; set; }
        Estatus Estatus { get; set; }
        List<Distrito> ObtenerDistritos(int idCircuito);
        List<Juzgado> ObtenerJuzgadosAcusatorioTradicional(int idCircuitoDistrito, TipoJuzgado tipoJuzgado);
        List<Juzgado> ObtenerJuzgadosPorDistrito(int idDistrito);
        List<Juzgado> ObtenerSalas(TipoJuzgado tipoJuzgado);
        List<Anexo> ObtenerAnexosEjecucion(string tipo);
        List<Juzgado> ObtenerJuzgadoEjecucionPorCircuito(int idcircuito);
        List<Solicitud> ObtenerSolicitudes();
        List<Solicitante> ObtenerSolicitantes();
        List<Toca> ObtenerTocasPorEjecucion(int idEjecucion);
        List<string> ObtenerAmparosPorEjecucion(int idEjecucion);
        List<Anexo> ObtenerAnexosPorEjecucion(int idEjecucion);
        List<Anexo> ConsultarAnexosPorEjecucionPosterior(int IdEjecucionPosterior);
    }
}
