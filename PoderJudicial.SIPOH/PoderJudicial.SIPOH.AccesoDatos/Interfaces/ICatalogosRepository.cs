
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
        List<Distrito> ObtenerDistritos(int idCircuito);
        List<Juzgado> ObtenerJuzgados(int idCircuito, TipoJuzgado tipoJuzgado);
        List<Juzgado> ObtenerSalas(TipoJuzgado tipoJuzgado);
        List<Anexo> ObtenerAnexosEjecucion(string tipo);
    }
}
