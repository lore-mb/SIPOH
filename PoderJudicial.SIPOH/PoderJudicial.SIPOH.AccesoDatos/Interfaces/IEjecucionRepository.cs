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
        int ? CreaEjecucion(Ejecucion ejecucion, List<int> causas, List<Toca> tocas, List<string> amparos, List<Anexo> anexos, int? idJuzgado, bool circuitoPachuca); 
        int ? GuardarPostEjecucion(PostEjecucion postEjecucion, List<Anexo> anexos);
        Ejecucion ObtenerEjecucionPromocionPorFolio(int folioEjecucion);
    }
}
