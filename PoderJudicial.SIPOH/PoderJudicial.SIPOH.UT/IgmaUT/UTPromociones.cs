using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoderJudicial.SIPOH.AccesoDatos;
using PoderJudicial.SIPOH.AccesoDatos.Conexion;
using PoderJudicial.SIPOH.Entidades;
using System.Collections.Generic;

namespace PoderJudicial.SIPOH.UT.IgmaUT
{
    [TestClass]
    public class UTPromociones
    {
        ServerConnection Cnx = null;

        [TestInitialize]
        public void TestConexion() {
            Cnx = ServerConnection.GetConnection();
        }
        [TestMethod]
        public void JuzgadosEjecucionPorCircuito() {
            CatalogosRepository test1 = new CatalogosRepository(Cnx);
            List<Juzgado> distrito1 = test1.ObtenerJuzgadoEjecucionPorCircuito(1);
        }

        [TestMethod]
        public void ConsultaEjecucionPorJuzgado() {
            EjecucionRepository test2 = new EjecucionRepository(Cnx);
            Ejecucion ejec = new Ejecucion();
            ejec.IdJuzgado = 223;
            ejec.NumeroEjecucion = "0002/2020";
        }

        [TestMethod]
        public void ObtenerExpedienteEjecucionCausa() {
            ExpedienteRepository test3 = new ExpedienteRepository(Cnx);
            Expediente expe = new Expediente();
            expe.IdExpediente = 3911;
        }

        [TestMethod]
        public void GuardarPostEjecucion()
        {

            EjecucionRepository RepositorioEjecucion;
            RepositorioEjecucion = new EjecucionRepository(Cnx);

            PostEjecucion RepositorioPostEjecucion;
            RepositorioPostEjecucion = new PostEjecucion();
            RepositorioPostEjecucion.IdEjecucion = 78;
            RepositorioPostEjecucion.Promovente = "Lic. PROMO 21";
            RepositorioPostEjecucion.IdUser = 1;
            RepositorioPostEjecucion.IdEjecucionPosterior = 0;

            List<Anexo> anexos = new List<Anexo>() {
                new Anexo (){ IdAnexo = 8, Descripcion = "Este es otro anexo 15", Cantidad = 8 }
            };

            int? GuardarPostEjecucion = RepositorioEjecucion.GuardarPostEjecucion(RepositorioPostEjecucion, anexos);

        }

        // ** Consulta Registro ** //

        [TestMethod]
        public void ConsultarEjecucion ()
        {
            EjecucionRepository new1 = new EjecucionRepository(Cnx);
            Ejecucion ejecucion = new1.ObtenerEjecucionPorFolio(89);
        }

        [TestMethod]
        public void ConsultarAnexos() {
            CatalogosRepository catalogos = new CatalogosRepository(Cnx);

            List<Anexo> anexo = catalogos.ObtenerAnexosPorEjecucion(89); 

        }

    }
}
