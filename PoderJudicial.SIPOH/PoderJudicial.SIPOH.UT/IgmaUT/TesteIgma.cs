using System;
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
            ejecucion.Solicitante = "SS";
            ejecucion.DetalleSolicitante = "ESTE REGISTRO SE CREA DESDE LA UT DE WEB APP";
            ejecucion.Solicitud = "0";
            ejecucion.OtroSolicitante = "ESTA ES OTRA SOLICITUD";
            ejecucion.NombreBeneficiario = "IGMAR";
            ejecucion.ApellidoPBeneficiario = "HERNANDEZ";
            ejecucion.ApellidoMBeneficiario = "MARTINEZ";
            ejecucion.Interno = "S";
            ejecucion.IdUsuario = 22;

            int? idEjecucion = repo.CrearEjecucion(ejecucion, true, null);
        }


    }
}
