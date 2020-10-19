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
        List<Ejecucion> ObtenerSentenciadoBeneficiario(string nombre, string apellidoPaterno, string apellidoMaterno);
        int? CrearEjecucion(Ejecucion ejecucion, List<int> causas, List<Expediente> tocas, List<string> amparos, List<Anexo> anexos, int? idJuzgado, bool circuitoPachuca);
        List<Ejecucion> ObtenerEjecucionPorJuzgado(int Juzgado, string NoEjecucion);
        Ejecucion ObtenerEjecucionPorFolio(int folio);
        Ejecucion ObtenerEjecucionPromocionPorFolio(int folio);
        List<Ejecucion> ObtenerEjecucionPorPartesCausa(string nombre, string apellidoPaterno, string apellidoMaterno);
        List<Ejecucion> ObtenerEjecucionPorNumeroCausa(string numeroCausa, int idJuzgado);
        List<Ejecucion> ObtenerEjecucionPorDetalleSolicitante(string detalleSolicitante);
        List<Ejecucion> ObtenerEjecucionPorNUC(string nuc, int idJuzgado);
        List<Ejecucion> ObtenerEjecucionPorSolicitante(int idSolicitante);
        int? GuardarPostEjecucion(PostEjecucion postEjecucion, List<Anexo> anexos);
    }
}
