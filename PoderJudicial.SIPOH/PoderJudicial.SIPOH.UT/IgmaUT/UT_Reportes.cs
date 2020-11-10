using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoderJudicial.SIPOH.AccesoDatos;
using PoderJudicial.SIPOH.AccesoDatos.Conexion;
using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;
using System.Collections.Generic;

namespace PoderJudicial.SIPOH.UT.IgmaUT
{
    [TestClass]
    public class UT_Reportes
    {
        ServerConnection Connection = null;

        [TestInitialize]
        public void ConexionPrueba() {
            Connection = ServerConnection.GetConnection();
        }

        [TestMethod]
        public void ObtenerJuzadosPorCircuito() {
            CatalogosRepository catalogo = new CatalogosRepository(Connection);
            List<Juzgado> ListaJuzgados = catalogo.ConsultaJuzgados(1, TipoJuzgado.EJECUCION);
        }


        [TestMethod]
        public void ConsultaInicialPorRangoFechas() 
        {
            EjecucionRepository TestEjecucionRepo = new EjecucionRepository(Connection);
            List<Reporte> Registros = TestEjecucionRepo.GenerarReporteRangoFecha(TipoReporteRangoFecha.INICIAL, "2020-01-01", "2020-10-29" , 224);
        }

        [TestMethod]

        public void ConsultarInicialPorDia() 
        {
            EjecucionRepository TestEjecucionRepo = new EjecucionRepository(Connection);
            List<Reporte> Registros = TestEjecucionRepo.GenerarReportePorDia(TipoReporteDia.INICIAL, "2020-10-16", 223);
        }

    }
}
