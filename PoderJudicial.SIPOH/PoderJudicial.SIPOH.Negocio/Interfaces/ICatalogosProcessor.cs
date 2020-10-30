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
        List<Juzgado> ObtieneJuzgadosPorMedioDelTipoSistema(int idCircuitoDistrito, TipoSistema tipoJuzgado);
        List<Juzgado> ObtieneSalasPorMedioDelTipoSistema(TipoSistema tipoJuzgado);
        List<Distrito> ObtieneDistritosPorMedioDelCircuito(int idCircuito);
        List<Anexo> ObtieneAnexosPorTipo(string tipo);
        List<Solicitante> ObtieneSolicitantes();
        List<Solicitud> ObtieneSolicitudes();
        List<Juzgado> ObtieneJuzgadosAcusatoriosPorCircuito(int idCircuito);
        List<Juzgado> ObtieneJuzgadosPorDistrito(int idDistrito);
    }
}
