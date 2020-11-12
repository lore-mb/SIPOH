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
        bool? ValidaAsignacionManualDeNumeroEjecucion(int idJuzgado, string numeroDeEjecucion);
        bool? ValidaExistenciaDeExpedientePorJuzgadoMasNumeroCausa(int idJuzgado, string numeroDeCausa, string nuc = null);
    }
}
