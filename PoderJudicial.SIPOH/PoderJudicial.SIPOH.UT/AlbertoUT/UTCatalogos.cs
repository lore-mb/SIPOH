using System.Collections.Generic;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoderJudicial.SIPOH.AccesoDatos;
using PoderJudicial.SIPOH.AccesoDatos.Conexion;
using PoderJudicial.SIPOH.AccesoDatos.Interfaces;
using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;
using PoderJudicial.SIPOH.Negocio;
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

            List<Anexo> anexos = repo.ConsultaAnexos("A");
            List<Anexo> anexo2 = repo.ConsultaAnexos("T");
        }

        [TestMethod]
        public void ObtenerBeneficiarios()
        {
            EjecucionRepository repo = new EjecucionRepository(cnx);
            string nombre = "Alberto";
            string apellidop = "Trejo";
            string apellidom = string.Empty;

            List<Ejecucion> ejecucion = repo.ConsultaEjecuciones(ParteCausaBeneficiario.BENEFICIARIO, nombre, apellidop, apellidom, 1);
        }

        [TestMethod]
        public void ObtenerCatalogosParaOptionsSalas()
        {

            CatalogosRepository repo = new CatalogosRepository(cnx);

            List<Juzgado> juzgadosC1 = repo.ConsultaJuzgados(TipoSistema.ACUSATORIO);
            List<Juzgado> juzgadosC2 = repo.ConsultaJuzgados(TipoSistema.TRADICIONAL);
        }

        [TestMethod]
        public void ObtenerCatalogosParaOptions()
        {
            CatalogosRepository repo = new CatalogosRepository(cnx);

            //TRADICIONAL
            //Recuperar Distritos por circuto
            List<Distrito> distritos1 = repo.ConsultaDistritos(1);
            List<Distrito> distritos2 = repo.ConsultaDistritos(2);
            List<Distrito> distritos3 = repo.ConsultaDistritos(3);
            List<Distrito> distritos4 = repo.ConsultaDistritos(4);
            List<Distrito> distritos5 = repo.ConsultaDistritos(5);

            //Recuperar Juzgados por Ditrito Traducional
            foreach (Distrito distrito in distritos1)
            {
                List<Juzgado> juzgados1 = repo.ConsultaJuzgados(TipoSistema.TRADICIONAL, distrito.IdDistrito);
            }

            //ACUSATORIO
            //Juzgados por Circuito acusatorio
            List<Juzgado> juzgadosC1 = repo.ConsultaJuzgados(TipoSistema.ACUSATORIO, 1);
            List<Juzgado> juzgadosC2 = repo.ConsultaJuzgados(TipoSistema.ACUSATORIO, 2);
            List<Juzgado> juzgadosC3 = repo.ConsultaJuzgados(TipoSistema.ACUSATORIO, 3);
            List<Juzgado> juzgadosC4 = repo.ConsultaJuzgados(TipoSistema.ACUSATORIO, 4);
            List<Juzgado> juzgadosC5 = repo.ConsultaJuzgados(TipoSistema.ACUSATORIO, 5);
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
            List<Toca> tocas = new List<Toca>();

            List<string> amparos = new List<string>();

            List<Anexo> anexos = new List<Anexo>()
            {
                new Anexo(){ IdAnexo = 3, Cantidad = 2, Descripcion = null},
                new Anexo(){ IdAnexo = 8, Cantidad =3, Descripcion="ESTE ES OTRO ANEXO UT"}
            };

            int? idEjecucion = repo.CreaEjecucion(ejecucion, causas, tocas, amparos, anexos, null, true);           
        }

        [TestMethod]
        public void SolicitantesSolicitud()
        {
            CatalogosRepository repo = new CatalogosRepository(cnx);
            List<Solicitud> solicitud = repo.ConsultaSolicitudes();
            List<Solicitante> solicitantes = repo.ConsultaSolicitantes();
        }


        [TestMethod]
        public void EjecucionPorFolio()
        {
            EjecucionRepository repo = new EjecucionRepository(cnx);

            Ejecucion test1 = repo.ConsultaEjecucion(4);
            Ejecucion test2 = repo.ConsultaEjecucion(6);
            Ejecucion test3 = repo.ConsultaEjecucion(999);
        }

        [TestMethod]
        public void ExpedientePorFolio()
        {
            ExpedienteRepository repo = new ExpedienteRepository(cnx);
            List<Expediente> expedientes = repo.ConsultaCausas(86);

            CatalogosRepository repoCatalogos = new CatalogosRepository(cnx);
            List<Toca> tocas = repoCatalogos.ConsultaTocas(81);
            List<string> amparos = repoCatalogos.ConsultaAmparos(78);
            List<Anexo> anexos = repoCatalogos.ConsultaAnexos(1, Instancia.INICIAL);
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


        [TestMethod]
        public void PruebaDeRangos()
        {
            IExpedienteRepository expedienteRepositorio = new ExpedienteRepository(cnx);
            IEjecucionRepository ejecucionRepositorio = new EjecucionRepository(cnx);

            ConsignacionesHistoricasProcessor repoCatalogos = new ConsignacionesHistoricasProcessor(expedienteRepositorio, ejecucionRepositorio);
            string mensaje = string.Empty;

            bool? existe0 = repoCatalogos.ValidaAsignacionManualDeNumeroDeEjecucion(223, "0091/2013");
            mensaje = repoCatalogos.Mensaje;

            bool? existe22 = repoCatalogos.ValidaAsignacionManualDeNumeroDeEjecucion(223, "0091/2019");
            mensaje = repoCatalogos.Mensaje;

            bool? existe6 = repoCatalogos.ValidaAsignacionManualDeNumeroDeEjecucion(223, "0091/2020");
            mensaje = repoCatalogos.Mensaje;

            bool? existe1 = repoCatalogos.ValidaAsignacionManualDeNumeroDeEjecucion(223, "0110/2020");
            mensaje = repoCatalogos.Mensaje;
            
            bool? existe2 = repoCatalogos.ValidaAsignacionManualDeNumeroDeEjecucion(223, "0114/2020");
            mensaje = repoCatalogos.Mensaje;
            
            bool? existe3 = repoCatalogos.ValidaAsignacionManualDeNumeroDeEjecucion(223, "0118/2020");
            mensaje = repoCatalogos.Mensaje;
            
            bool? existe4 = repoCatalogos.ValidaAsignacionManualDeNumeroDeEjecucion(223, "0120/2020");
            mensaje = repoCatalogos.Mensaje;
            
            bool? existe5 = repoCatalogos.ValidaAsignacionManualDeNumeroDeEjecucion(223, "0121/2020");
            mensaje = repoCatalogos.Mensaje;

            bool? existe7 = repoCatalogos.ValidaAsignacionManualDeNumeroDeEjecucion(223, "0121/2021");
            mensaje = repoCatalogos.Mensaje;

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
