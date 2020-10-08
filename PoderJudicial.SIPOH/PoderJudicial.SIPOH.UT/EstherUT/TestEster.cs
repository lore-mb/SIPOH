using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoderJudicial.SIPOH.AccesoDatos;
using PoderJudicial.SIPOH.AccesoDatos.Conexion;
using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;

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

            //Creo mi objeto
            EjecucionRepository PruebaEjecucionBusqueda = new EjecucionRepository(cnx);

            string nombre = "IGNACIO";
            string apellidoP = "";
            string apellidoM = "";
            List<Ejecucion> ListaPartesEjecucion = PruebaEjecucionBusqueda.ObtenerEjecucionPorPartesCausa(nombre, apellidoP, apellidoM);


            string numeroCausa = "0001/2015";
            int idjuzgado = 204;
            List<Ejecucion> ListaNumeroCausa = PruebaEjecucionBusqueda.ObtenerEjecucionPorNumeroCausa(numeroCausa, idjuzgado);


            string detalleSolicitante = "ESTE ES UN DETALLE";
            List<Ejecucion> ListaDetalleSolicitante = PruebaEjecucionBusqueda.ObtenerEjecucionPorDetalleSolicitante(detalleSolicitante);

            string nuc="13-2017-001300";
            int idJuzgado = 0;
            List<Ejecucion> ListaNUC = PruebaEjecucionBusqueda.ObtenerEjecucionPorNUC(nuc,idJuzgado);

            string mensaje = "null";
            int solicitante = 0;
            List<Ejecucion> ListaSolicitante = PruebaEjecucionBusqueda.ObtenerEjecucionPorSolicitante(solicitante);
            

            //validacion del status
            //creo mi objeto de tipo enum y asigno el valor de estatus a atributo estado de peticion, notese que Estatus(contiene los distintos status de prueba)
            Estatus estadodepeticion = PruebaEjecucionBusqueda.Estatus;

            //valida si el estado de mi peticion es igual a error envia mensaje accediendo a la propiedad MensajeError mediante el objeto
            if (estadodepeticion == Estatus.ERROR)
                mensaje = PruebaEjecucionBusqueda.MensajeError;

            //si es diferente de error enviara estatus en ok
            else if
                (estadodepeticion == Estatus.OK)
                mensaje = "bien";
            else if
                (estadodepeticion == Estatus.SIN_RESULTADO)
                mensaje = "ningun resultado";
        }

    }
}
