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
        public void agregarPostEjecucion() {

            EjecucionRepository contenidoRepositorio;
            contenidoRepositorio = new EjecucionRepository(Cnx);
            
            PostEjecucion contenidoPostEjecucion;
            contenidoPostEjecucion = new PostEjecucion();
            contenidoPostEjecucion.IdEjecucion = 78;
            contenidoPostEjecucion.Promovente = "MARCO ALBERTO";
            contenidoPostEjecucion.FechaIngreso = "20-12-20202";
            contenidoPostEjecucion.IdUser = 1;

            int? idEjecucion = contenidoRepositorio.GuardarPostEjecucion(contenidoPostEjecucion);
        }

    }
}
