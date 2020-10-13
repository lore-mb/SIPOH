using System.Collections.Generic;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoderJudicial.SIPOH.AccesoDatos;
using PoderJudicial.SIPOH.AccesoDatos.Conexion;
using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;
using PoderJudicial.SIPOH.WebApp.Helpers;

namespace PoderJudicial.SIPOH.UT.AlbertoUT
{
    [TestClass]
    public class UTCatalogos
    {
        ServerConnection cnx = null;
        [TestInitialize]
        public void GeneraConexion()
        {
            cnx = ServerConnection.GetConnection();
        }

        [TestMethod]
        public void ObtenerAnexos()
        {
            CatalogosRepository repo = new CatalogosRepository(cnx);

            List<Anexo> anexos = repo.ObtenerAnexosEjecucion("A");
            List<Anexo> anexo2 = repo.ObtenerAnexosEjecucion("T");
        }

        [TestMethod]
        public void ObtenerBeneficiarios()
        {
            EjecucionRepository repo = new EjecucionRepository(cnx);
            string nombre = "Alberto";
            string apellidop = "Trejo";
            string apellidom = string.Empty;

            List<Ejecucion> ejecucion = repo.ObtenerSentenciadoBeneficiario(nombre, apellidop, apellidom);
        }

        [TestMethod]
        public void ObtenerCatalogosParaOptionsSalas()
        {

            CatalogosRepository repo = new CatalogosRepository(cnx);

            List<Juzgado> juzgadosC1 = repo.ObtenerSalas(TipoJuzgado.ACUSATORIO);
            List<Juzgado> juzgadosC2 = repo.ObtenerSalas(TipoJuzgado.TRADICIONAL);
        }

        [TestMethod]
        public void ObtenerCatalogosParaOptions()
        {
            CatalogosRepository repo = new CatalogosRepository(cnx);

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
                List<Juzgado> juzgados1 = repo.ObtenerJuzgadosAcusatorioTradicional(distrito.IdDistrito, TipoJuzgado.TRADICIONAL);
            }

            //ACUSATORIO
            //Juzgados por Circuito acusatorio
            List<Juzgado> juzgadosC1 = repo.ObtenerJuzgadosAcusatorioTradicional(1, TipoJuzgado.ACUSATORIO);
            List<Juzgado> juzgadosC2 = repo.ObtenerJuzgadosAcusatorioTradicional(2, TipoJuzgado.ACUSATORIO);
            List<Juzgado> juzgadosC3 = repo.ObtenerJuzgadosAcusatorioTradicional(3, TipoJuzgado.ACUSATORIO);
            List<Juzgado> juzgadosC4 = repo.ObtenerJuzgadosAcusatorioTradicional(4, TipoJuzgado.ACUSATORIO);
            List<Juzgado> juzgadosC5 = repo.ObtenerJuzgadosAcusatorioTradicional(5, TipoJuzgado.ACUSATORIO);
        }

        [TestMethod]
        public void CrearEjecucion()
        {
            EjecucionRepository repo = new EjecucionRepository(cnx);
            Ejecucion ejecucion = new Ejecucion();
            ejecucion.IdSolicitante = 3;
            ejecucion.DetalleSolicitante = "REGISTRO SIN ANEXOS, NI AMPAROS, NI TOCAS";
            ejecucion.IdSolicitud = 1;
            ejecucion.OtraSolicita = null;
            ejecucion.NombreBeneficiario = "ALBERTO";
            ejecucion.ApellidoPBeneficiario = "ROMERO";
            ejecucion.ApellidoMBeneficiario = "PARDO";
            ejecucion.Interno = "S";
            ejecucion.IdUsuario = 22;

            List<int> causas = new List<int>() { 456, 457, 458, 459};
            List<Expediente> tocas = new List<Expediente>();

            List<string> amparos = new List<string>();

            List<Anexo> anexos = new List<Anexo>()
            {
                new Anexo(){ IdAnexo = 3, Cantidad = 2, Descripcion = null},
                new Anexo(){ IdAnexo = 8, Cantidad =3, Descripcion="ESTE ES OTRO ANEXO UT"}
            };

            int? idEjecucion = repo.CrearEjecucion(ejecucion, causas, tocas, amparos, anexos, null, true);           
        }

        [TestMethod]
        public void SolicitantesSolicitud()
        {
            CatalogosRepository repo = new CatalogosRepository(cnx);
            List<Solicitud> solicitud = repo.ObtenerSolicitudes();
            List<Solicitante> solicitantes = repo.ObtenerSolicitantes();
        }


        [TestMethod]
        public void EjecucionPorFolio()
        {
            EjecucionRepository repo = new EjecucionRepository(cnx);

            Ejecucion test1 = repo.ObtenerEjecucionPorFolio(4);
            Ejecucion test2 = repo.ObtenerEjecucionPorFolio(6);
            Ejecucion test3 = repo.ObtenerEjecucionPorFolio(999);
        }

        [TestMethod]
        public void ExpedientePorFolio()
        {
            ExpedienteRepository repo = new ExpedienteRepository(cnx);
            List<Expediente> expedientes = repo.ObtenerExpedientesPorEjecucion(86);

            CatalogosRepository repoCatalogos = new CatalogosRepository(cnx);
            List<Expediente> tocas = repoCatalogos.ObtenerTocasPorEjecucion(81);
            List<string> amparos = repoCatalogos.ObtenerAmparosPorEjecucion(78);
            List<Anexo> anexos = repoCatalogos.ObtenerAnexosPorEjecucion(87);
        }

        [TestMethod]
        public void PruebaParaEncriptadoDesencriptado()
        {
            List<string> lista = new List<string>();
            List<string> descripta = new List<string>();

            for (int c = 1; c <= 1000000; c++) 
            {
                string urlencriptada = ViewHelper.Encrypt("folio=" + c);
                lista.Add(urlencriptada);

                string descript = ViewHelper.Decrypt(urlencriptada);
                descripta.Add(descript);
            }
        }


        [TestMethod]
        public void PreubaRef()
        {
            int miValor = 5;
            int resultado = pasoParametroSinReferencia(miValor);

            //int resultado2 = pasoParametroConReferencia(ref miValor);

        }

        public int pasoParametroSinReferencia(int valor) 
        {
             valor = valor + 3;
             return valor;
        }

        public int pasoParametroConReferencia(ref int valor)
        {
            valor = valor + 3;
            return valor;
        }
    }
}
