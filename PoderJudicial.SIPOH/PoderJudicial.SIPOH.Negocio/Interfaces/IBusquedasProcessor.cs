using PoderJudicial.SIPOH.AccesoDatos;
using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoderJudicial.SIPOH.Negocio.Interfaces
{
    public interface IBusquedasProcessor 
    {
        string Mensaje { get; set; }
        List<Ejecucion> ObtieneEjecucionesPorNombreDePartesCausa(string nombre, string apellidoPaterno, string apellidoMaterno, int idCircuito);
        List<Ejecucion> ObtieneEjecucionesPorNombreDeSentenciadoBeneficiario(string nombre,string apellidoPaterno, string apellidoMaterno, int idCircuito);
        List<Ejecucion> ObtieneEjecucionesPorNumeroCausaMasIdJuzgado(string numeroCausa, int idJuzgado);
        List<Ejecucion> ObtieneEjecucionesPorNUCMasIdJuzgado(string nuc, int idJuzgado);
        List<Ejecucion> ObtieneEjecucionesPorDetalleSolicitante(string detalleSolicitante, int idCircuito);
        List<Ejecucion> ObtieneEjecucionesPorIdSolicitante(int idSolicitante, int idCircuito);
        List<Expediente> ObtieneEjecionesPorIdEjecucion(int idEjecucion);
    }
}
