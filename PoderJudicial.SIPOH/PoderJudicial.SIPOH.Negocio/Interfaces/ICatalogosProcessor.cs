using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoderJudicial.SIPOH.Negocio.Interfaces
{
    public interface ICatalogosProcessor
    {
        string Mensaje { get; set; }
        List<Juzgado> ObtieneJuzgadosPorTipoSistema(int idCircuitoDistrito, TipoSistema tipoJuzgado);
        List<Juzgado> ObtieneSalasPorTipoSistema(TipoSistema tipoJuzgado);
        List<Juzgado> ObtieneJuzgadosPorCircuito(int IdCircuito);
        List<Distrito> ObtieneDistritosPorCircuito(int idCircuito);
        List<Anexo> ObtieneAnexosPorTipo(string tipo);
        List<Solicitante> ObtieneSolicitantes();
        List<Solicitud> ObtieneSolicitudes();
        List<Juzgado> ObtieneJuzgadosAcusatoriosPorCircuito(int idCircuito);
        List<Juzgado> ObtieneJuzgadosPorDistrito(int idDistrito);
        List<Delito> ObtieneDelitosDelImputado();
    }
}
