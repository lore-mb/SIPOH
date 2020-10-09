using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoderJudicial.SIPOH.AccesoDatos;
using PoderJudicial.SIPOH.AccesoDatos.Conexion;
using PoderJudicial.SIPOH.Entidades;

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
            RepositorioPostEjecucion.Promovente = "Lic. Promovente 12";
            RepositorioPostEjecucion.IdUser = 1;

            List<PostEjecucion> anexos = new List<PostEjecucion>() {
                new PostEjecucion (){  IdEjecucionPosterior = 7, IdCatAnexEjecucion = 5, OtroAnexoEjecucion = null, Cantidad = 5 },
                new PostEjecucion (){  IdEjecucionPosterior = 7, IdCatAnexEjecucion = 8, OtroAnexoEjecucion = "Este es otro anexo 3", Cantidad = 8 }
            };

            RepositorioEjecucion.GuardarPostEjecucion(RepositorioPostEjecucion, anexos);
        }

    }
}
