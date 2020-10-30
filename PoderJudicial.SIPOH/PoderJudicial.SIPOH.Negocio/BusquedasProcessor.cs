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
        //[Private Method]
        //Se instancian mis objetos:
        private readonly IEjecucionRepository ejecucionRepositorio;
        private readonly IExpedienteRepository expedienteRepositorio;

        /// <summary>
        /// [Metodo constructor:Interface Injection Method]
        /// </summary>
        /// <param name="CatalogRepositorio"></param>
        /// <param name="EjecucionRepositorio"></param>
        /// <param name="ExpedienteRepositorio"></param>
        public BusquedasProcessor(IEjecucionRepository ejecucionRepositorio, IExpedienteRepository expedienteRepositorio)
        {
            this.ejecucionRepositorio = ejecucionRepositorio;
            this.expedienteRepositorio = expedienteRepositorio;
        }

        #region Metodos Publicos
        /// <summary>
        /// [Public Method]
        /// </summary>
        public string Mensaje { get; set; }

        /// <summary>
        /// Valida la Busqueda por DetalledelSolictante
        /// </summary>
        /// <param name="detalleSolicitante"></param>
        /// <returns></returns>
        public List<Ejecucion>ObtieneEjecucionesPorDetalleSolicitante(string detalleSolicitante, int idCircuito)
        {
            List<Ejecucion> DetalleSolicitante = ejecucionRepositorio.ConsultaEjecuciones(detalleSolicitante, idCircuito);
           
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

        /// <summary>
        /// Valida la busqueda  por medio de NUC
        /// </summary>
        /// <param name="nuc"></param>
        /// <param name="idJuzgado"></param>
        /// <returns></returns>
        public List<Ejecucion> ObtieneEjecucionesPorNUC(string nuc, int idJuzgado)
        {
            List<Ejecucion> NUC = ejecucionRepositorio.ConsultaEjecuciones(TipoNumeroExpediente.NUC, nuc, idJuzgado);
           
            if (ejecucionRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrio un error interno no controlado, consulte a soporte";
                string messajelogger = ejecucionRepositorio.MensajeError;
            }
            
            if (ejecucionRepositorio.Estatus == Estatus.SIN_RESULTADO)
            {
                Mensaje = "La consulta no genero ningun resultado";
            }
            
            return NUC;
        }

        /// <summary>
        /// Valida busqueda por medio de las partes relacionadas
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="apellidoPaterno"></param>
        /// <param name="apellidoMaterno"></param>
        /// <returns></returns>
        public List<Ejecucion> ObtieneEjecucionesPorNombreDePartesCausa(string nombre, string apellidoPaterno, string apellidoMaterno, int idCircuito)
        {
            List<Ejecucion> PartesCausa = ejecucionRepositorio.ConsultaEjecuciones(ParteCausaBeneficiario.PARTE, nombre, apellidoPaterno, apellidoMaterno, idCircuito);
          
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

        /// <summary>
        /// Valida la busqueda por Solicitante
        /// </summary>
        /// <param name="idSolicitante"></param>
        /// <returns></returns>
        public List<Ejecucion> ObtieneEjecucionesPorSolicitante(int idSolicitante, int idCircuito)
        {
            List<Ejecucion> Solicitante = ejecucionRepositorio.ConsultaEjecuciones(idSolicitante, idCircuito);
            
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

        /// <summary>
        /// Valida busqueda por Sentenciado|Beneficiario
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="apellidoPaterno"></param>
        /// <param name="apellidoMaterno"></param>
        /// <returns></returns>
        public List<Ejecucion> ObtieneEjecucionesPorNombreDeSentenciadoBeneficiario(string nombre, string apellidoPaterno, string apellidoMaterno, int idCircuito)
        {
            List<Ejecucion> Beneficiario = ejecucionRepositorio.ConsultaEjecuciones(ParteCausaBeneficiario.BENEFICIARIO, nombre, apellidoPaterno, apellidoMaterno, idCircuito);
            
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
        
        /// <summary>
        /// Valida busqueda por medio del numero de causa
        /// </summary>
        /// <param name="numeroCausa"></param>
        /// <param name="idJuzgado"></param>
        /// <returns></returns>
        public List<Ejecucion> ObtieneEjecucionesPorNumeroCausa(string numeroCausa, int idJuzgado)
        {
            List<Ejecucion> numCausa = ejecucionRepositorio.ConsultaEjecuciones(TipoNumeroExpediente.CAUSA, numeroCausa, idJuzgado);
          
            if (ejecucionRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrio un error interno no controlado, consulte a soporte";
                string messajelogger = ejecucionRepositorio.MensajeError;
            }
            
            if (ejecucionRepositorio.Estatus == Estatus.SIN_RESULTADO)
            {
                Mensaje = "La consulta no genero ningun resultado";
            }
           
            return numCausa;
        }
 
        public List<Expediente> ObtieneEjecionesPorIdEjecucion(int idEjecucion)
        {
            List<Expediente> ExpedienteListado = expedienteRepositorio.ConsultaExpedientes(idEjecucion);

            if (expedienteRepositorio.Estatus == Estatus.SIN_RESULTADO)
            {
                Mensaje = ("La consulta no ha generado ningun resultado");
            }

            if (expedienteRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = ("Ocurrio un error interno, consultar con soporte");
                string messageLogger = expedienteRepositorio.MensajeError;
            }
            return ExpedienteListado;
        }
        #endregion
    }
}
