using PoderJudicial.SIPOH.AccesoDatos.Interfaces;
using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;
using PoderJudicial.SIPOH.Negocio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoderJudicial.SIPOH.Negocio
{
    public class BusquedasProcessor : IBusquedasProcessor
    {
        //[Public Method]
        public string Mensaje { get; set; }


        //[Private Method]
                                            //Se crean mis objetos:
        private readonly ICatalogosRepository catalogRepositorio;
        private readonly IEjecucionRepository ejecucionRepositorio;
        private readonly IExpedienteRepository expedienteRepositorio;

        
        //[Interface Injection Method]
        public BusquedasProcessor(ICatalogosRepository CatalogRepositorio, IEjecucionRepository EjecucionRepositorio, IExpedienteRepository ExpedienteRepositorio)
        {

        }

        public List<Ejecucion>ObtenerEjecucionPorDetalleSolicitante(string detalleSolicitante)
        {
            List<Ejecucion> DetalleSolicitante = ejecucionRepositorio.ObtenerEjecucionPorDetalleSolicitante(detalleSolicitante);
            if (ejecucionRepositorio.Estatus == Estatus.SIN_RESULTADO)
            {
                Mensaje = "La consulta no genero ningun resultado";
            }
            if(ejecucionRepositorio.Estatus== Estatus.ERROR)
            {
                Mensaje = "Ocurrio un error interno no controlado, consulte a soporte";
                string messajelogger = ejecucionRepositorio.MensajeError;
            }
            return DetalleSolicitante;
        }

        public List<Ejecucion> ObtenerEjecucionPorNUC(string nuc, int idJuzgado)
        {
            List<Ejecucion> NUC = ejecucionRepositorio.ObtenerEjecucionPorNUC(nuc, idJuzgado);
            if (ejecucionRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "La consulta no genero ningun resultado";
                string messajelogger = ejecucionRepositorio.MensajeError;
            }
            if (ejecucionRepositorio.Estatus == Estatus.SIN_RESULTADO)
            {
                Mensaje = "Ocurrio un error interno no controlado, consulte a soporte";
            }
            return NUC;
        }

        public List<Ejecucion> ObtenerEjecucionPorPartesCausa(string nombre, string apellidoPaterno, string apellidoMaterno)
        {
            List<Ejecucion> PartesCausa = ejecucionRepositorio.ObtenerEjecucionPorPartesCausa(nombre, apellidoPaterno, apellidoMaterno);
            if (ejecucionRepositorio.Estatus == Estatus.SIN_RESULTADO)
            {
                Mensaje = "La consulta no genero ningun resultado";
            }
            if (ejecucionRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrio un error interno no controlado, consulte a soporte";
                string messajelogger = ejecucionRepositorio.MensajeError;
            }
            return PartesCausa;
        }

        public List<Ejecucion> ObtenerEjecucionPorSolicitante(int idSolicitante)
        {
            List<Ejecucion> Solicitante = ejecucionRepositorio.ObtenerEjecucionPorSolicitante(idSolicitante);
            if(ejecucionRepositorio.Estatus== Estatus.SIN_RESULTADO)
            {
                Mensaje = "La consulta no genero ningun resultado";
            }
            if (ejecucionRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrio un error interno no controlado, consulte a soporte";
                string messajelogger = ejecucionRepositorio.MensajeError;
            }
            return Solicitante;
        }

        public List<Ejecucion> ObtenerEjecucionSentenciadoBeneficiario(string nombre, string apellidoPaterno, string apellidoMaterno)
        {
            List<Ejecucion> Beneficiario = ejecucionRepositorio.ObtenerSentenciadoBeneficiario(nombre, apellidoPaterno,apellidoMaterno);
            if (ejecucionRepositorio.Estatus == Estatus.SIN_RESULTADO)
            {
                Mensaje = "La consulta no genero ningun resultado";
            }
            if (ejecucionRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrio un error interno no controlado, consulte a soporte";
                string messajelogger = ejecucionRepositorio.MensajeError;
            }
            return Beneficiario;
        }

        public List<Ejecucion> ObtenerEjeucionPorNumeroCausa(string numeroCausa, int idJuzgado)
        {
            List<Ejecucion> numCausa = ejecucionRepositorio.ObtenerEjecucionPorNumeroCausa(numeroCausa ,idJuzgado);
            if (ejecucionRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "La consulta no genero ningun resultado";
                string messajelogger = ejecucionRepositorio.MensajeError;
            }
            if (ejecucionRepositorio.Estatus == Estatus.SIN_RESULTADO)
            {
                Mensaje = "Ocurrio un error interno no controlado, consulte a soporte";
            }
            return numCausa;
        }
    }
}
