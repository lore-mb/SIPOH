using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoderJudicial.SIPOH.AccesoDatos;
using PoderJudicial.SIPOH.AccesoDatos.Conexion;
using PoderJudicial.SIPOH.AccesoDatos.Enum;
using PoderJudicial.SIPOH.Entidades;

namespace PoderJudicial.SIPOH.UT.AlbertoUT
{
    [TestClass]
    public class UTCatalogos
    {
        [TestMethod]
        public void ObtenerCatalogosParaOptions()
        {
            ServerConnection cnx = ServerConnection.GetConnection();
            CatalogosRepository repo = new CatalogosRepository(cnx);

            Resultado res = Resultado.SIN_RESULTADO;

            //TRADICIONAL
            //Recuperar Distritos por circuto
            List<Distrito> distritos1 = repo.ObtenerDistritoPorCircuito(1, ref res);
            List<Distrito> distritos2 = repo.ObtenerDistritoPorCircuito(2, ref res);
            List<Distrito> distritos3 = repo.ObtenerDistritoPorCircuito(3, ref res);
            List<Distrito> distritos4 = repo.ObtenerDistritoPorCircuito(4, ref res);
            List<Distrito> distritos5 = repo.ObtenerDistritoPorCircuito(5, ref res);

            //Recuperar Juzgados por Ditrito Traducional
            foreach (Distrito distrito in distritos1) 
            {
                List<Juzgado> juzgados1 = repo.ObtenerJuzgadosTradicional(distrito.IdDistrito, ref res);
            }

            //ACUSATORIO
            //Juzgados por Circuito acusatorio
            List<Juzgado> juzgadosC1 = repo.ObtenerJuzgadosAcusatorio(1, ref res);
            List<Juzgado> juzgadosC2 = repo.ObtenerJuzgadosAcusatorio(2, ref res);
            List<Juzgado> juzgadosC3 = repo.ObtenerJuzgadosAcusatorio(3, ref res);
            List<Juzgado> juzgadosC4 = repo.ObtenerJuzgadosAcusatorio(4, ref res);
            List<Juzgado> juzgadosC5 = repo.ObtenerJuzgadosAcusatorio(5, ref res);
        }
    }
}
