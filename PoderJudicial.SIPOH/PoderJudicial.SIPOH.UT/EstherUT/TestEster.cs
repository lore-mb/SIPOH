using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoderJudicial.SIPOH.AccesoDatos;
using PoderJudicial.SIPOH.AccesoDatos.Conexion;
using PoderJudicial.SIPOH.Entidades;

namespace PoderJudicial.SIPOH.UT.EstherUT
{
    [TestClass]
    public class TestEster
    {
        [TestMethod]
        public void TestMethod1()
        {
            var Est = 2;
        }
        ServerConnection cnx = null;

        [TestInitialize]
        public void GeneraConexion()
        {
            cnx = ServerConnection.GetConnection();
        }

        [TestMethod]
        public void CrearEjecucion()
        {
            EjecucionRepository repo = new EjecucionRepository(cnx);
            Ejecucion ejecucion = new Ejecucion();
            ejecucion.IdSolicitante = 1;
            ejecucion.DetalleSolicitante = "ESTE REGISTRO SE CREA DESDE LA UT DE WEB APP";
            ejecucion.IdSolicitud = 2;
            ejecucion.OtraSolicita = "ESTA ES OTRA SOLICITUD";
            ejecucion.NombreBeneficiario = "ESTHER";
            ejecucion.ApellidoPBeneficiario = "VAZQUEZ";
            ejecucion.ApellidoMBeneficiario = "VAZQUEZ";
            ejecucion.Interno = "S";
            ejecucion.IdUsuario = 22;

            List<int> causas = new List<int>() { 404, 405, 406, 407 };
            List<Expediente> tocas = new List<Expediente>()
            {
                new Expediente(){ IdJuzgado = 4, NumeroDeToca = "0001/2020" },
                new Expediente(){ IdJuzgado = 5, NumeroDeToca = "0002/2020" },
                new Expediente(){ IdJuzgado = 4, NumeroDeToca = "0003/2020" }
            };

            List<string> amparos = new List<string>() { "ASDF", "QWER", "ZXCV", "FGHJ" };

            List<Anexo> anexos = new List<Anexo>()
            {
               new Anexo(){ IdAnexo = 1, Cantidad = 3},
               new Anexo(){ IdAnexo = 3, Cantidad = 4},
               new Anexo(){ IdAnexo = 4, Cantidad = 8}
            };

            int? idEjecucion = repo.CrearEjecucion(ejecucion, causas, tocas, amparos, anexos, null, true);
        }

        [TestMethod]
        public void ObtenerPartesEjecucion()
        {
            EjecucionRepository PruebaEjecucionBusqueda = new EjecucionRepository(cnx);

            string nombre = "IGNACIO";
            string apellidoP = "";
            string apellidoM = "";


            List<Ejecucion> ListaPartesEjecucion = PruebaEjecucionBusqueda.ObtenerEjecucionPorPartesCausa(nombre, apellidoP, apellidoM);

        }

    }
}
