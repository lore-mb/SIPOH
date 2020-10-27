
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
        List<Distrito> ConsultaDistritos(int idCircuito);
        List<Juzgado> ConsultaJuzgados(TipoSistema sistema, int idCircuitoDistrito);
        List<Juzgado> ConsultaJuzgados(TipoSistema sistema);
        List<Juzgado> ConsultaJuzgados(int idCircuitoDistrito, TipoJuzgado ? tipoJuzgado = null);
        List<Solicitud> ConsultaSolicitudes();
        List<Solicitante> ConsultaSolicitantes();
        List<Toca> ConsultaTocas(int idEjecucion);
        List<string> ConsultaAmparos(int idEjecucion);
        List<Anexo> ConsultaAnexos(string tipo);

        //Fucionar metodo, crear un Enum para diferenciar entre Inicial y Promocion
        List<Anexo> ConsultaAnexos(int idEjecucion);
        List<Anexo> ConsultarAnexosPorEjecucionPosterior(int IdEjecucionPosterior);
    }
}
