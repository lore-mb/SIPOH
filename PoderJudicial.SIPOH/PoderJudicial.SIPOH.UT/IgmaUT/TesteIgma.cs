using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoderJudicial.SIPOH.AccesoDatos;
using PoderJudicial.SIPOH.AccesoDatos.Conexion;
using PoderJudicial.SIPOH.Entidades;

namespace PoderJudicial.SIPOH.UT.IgmaUT
{
    [TestClass]
    public class TesteIgma
    {

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
            ejecucion.IdSolicitante = 3;
            ejecucion.DetalleSolicitante = "ESTE REGISTRO SE CREA DESDE LA UT DE WEB APP";
            ejecucion.IdSolicitud = 1;
            ejecucion.OtraSolicita = "ESTA ES OTRA SOLICITUD";
            ejecucion.NombreBeneficiario = "IGMAR";
            ejecucion.ApellidoPBeneficiario = "HERNANDEZ";
            ejecucion.ApellidoMBeneficiario = "MARTINEZ";
            ejecucion.Interno = "S";
            ejecucion.IdUsuario = 22;

            List<int> causas = new List<int>() { 404, 405, 406, 407 };
            List<Toca> tocas = new List<Toca>()
            {
                new Toca(){ IdJuzgado = 4, NumeroDeToca = "0001/2020" },
                new Toca(){ IdJuzgado = 5, NumeroDeToca = "0002/2020" },
                new Toca(){ IdJuzgado = 4, NumeroDeToca = "0003/2020" }
            };

            List<string> amparos = new List<string>() { "ASDF", "QWER", "ZXCV", "FGHJ" };

            List<Anexo> anexos = new List<Anexo>()
            {
               new Anexo(){ IdAnexo = 1, Cantidad = 3},
               new Anexo(){ IdAnexo = 3, Cantidad = 4},
               new Anexo(){ IdAnexo = 4, Cantidad = 8}
            };

            int? idEjecucion = repo.CreaEjecucion(ejecucion, causas, tocas, amparos, anexos, null, true);
        }

    }
}
