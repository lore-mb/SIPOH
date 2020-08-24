using PoderJudicial.SIPOH.AccesoDatos.Enum;
using PoderJudicial.SIPOH.Entidades;
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
        List<Distrito> ObtenerDistritoPorCircuito(int idCircuito, ref Resultado resultado);
        List<Juzgado> ObtenerJuzgadosAcusatorio(int idCircuito, ref Resultado resultado);
        List<Juzgado> ObtenerJuzgadosTradicional(int idDistrito, ref Resultado resultado);
    }
}
