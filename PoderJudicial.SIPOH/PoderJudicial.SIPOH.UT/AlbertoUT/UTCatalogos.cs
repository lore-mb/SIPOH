using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoderJudicial.SIPOH.AccesoDatos;
using PoderJudicial.SIPOH.AccesoDatos.Conexion;
using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;

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

            Estatus res = Estatus.SIN_RESULTADO;

            //TRADICIONAL
            //Recuperar Distritos por circuto
            List<Distrito> distritos1 = repo.ObtenerDistritos(1);
            List<Distrito> distritos2 = repo.ObtenerDistritos(2);
            List<Distrito> distritos3 = repo.ObtenerDistritos(3);
            List<Distrito> distritos4 = repo.ObtenerDistritos(4);
            List<Distrito> distritos5 = repo.ObtenerDistritos(5);

            //Recuperar Juzgados por Ditrito Traducional
            foreach (Distrito distrito in distritos1) 
            {
                List<Juzgado> juzgados1 = repo.ObtenerJuzgados(distrito.IdDistrito, TipoJuzgado.TRADICIONAL);
            }

            //ACUSATORIO
            //Juzgados por Circuito acusatorio
            List<Juzgado> juzgadosC1 = repo.ObtenerJuzgados(1, TipoJuzgado.ACUSATORIO);
            List<Juzgado> juzgadosC2 = repo.ObtenerJuzgados(2, TipoJuzgado.ACUSATORIO);
            List<Juzgado> juzgadosC3 = repo.ObtenerJuzgados(3, TipoJuzgado.ACUSATORIO);
            List<Juzgado> juzgadosC4 = repo.ObtenerJuzgados(4, TipoJuzgado.ACUSATORIO);
            List<Juzgado> juzgadosC5 = repo.ObtenerJuzgados(5, TipoJuzgado.ACUSATORIO);
        }
    }
}
