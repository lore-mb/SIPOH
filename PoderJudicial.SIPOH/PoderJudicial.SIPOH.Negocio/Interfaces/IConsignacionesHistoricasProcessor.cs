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
        bool ? ValidaExistenciaDeCausaPorJuzgadoMasNumeroDeCausaNUC(int idJuzgado, string numeroExpediente, string nuc = null);
    }
}
