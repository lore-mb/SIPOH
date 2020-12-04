using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoderJudicial.SIPOH.Negocio.Interfaces
{
    public interface IConsignacionesHistoricasProcessor
    {
        string Mensaje { get; set; }
        bool? ValidaAsignacionManualDeNumeroDeEjecucion(int idJuzgado, string numeroDeEjecucion);
        bool? ValidaExistenciaDeCausaEnJuzgado(int idJuzgado, string numeroDeCausa, string nuc);
        int? CreaRegistroDeEjecucionHistorica(Ejecucion ejecucion);
    }
}
