using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Negocio.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoderJudicial.SIPOH.Negocio.Interfaces
{
    public interface IInicialesProcessor
    {
        string Mensaje { get; set; }
        List<Juzgado> RecuperaJuzgado(int idFiltro, TipoJuzgado tipoJuzgado);
        List<Distrito> RecuperaDistrito(int idCircuito);
    }
}
