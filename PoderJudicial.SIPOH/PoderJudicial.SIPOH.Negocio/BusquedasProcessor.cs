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
       

        /// <summary>
        /// [Metodo constructor:Interface Injection Method]
        /// </summary>
        /// <param name="CatalogRepositorio"></param>
        /// <param name="EjecucionRepositorio"></param>
        /// <param name="ExpedienteRepositorio"></param>
        public BusquedasProcessor(ICatalogosRepository CatalogRepositorio, IEjecucionRepository EjecucionRepositorio, IExpedienteRepository ExpedienteRepositorio)
        {
            this.catalogoRepositorio = CatalogRepositorio;
            this.ejecucionRepositorio = EjecucionRepositorio;
            this.expedienteRepositorio = ExpedienteRepositorio;
        }

        //[Private Method]
        //Se instancian mis objetos:
        private readonly ICatalogosRepository catalogoRepositorio;
        private readonly IEjecucionRepository ejecucionRepositorio;
        private readonly IExpedienteRepository expedienteRepositorio;


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

        /// <summary>
        /// Valida la busqueda  por medio de NUC
        /// </summary>
        /// <param name="nuc"></param>
        /// <param name="idJuzgado"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Valida busqueda por medio de las partes relacionadas
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="apellidoPaterno"></param>
        /// <param name="apellidoMaterno"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Valida la busqueda por Solicitante
        /// </summary>
        /// <param name="idSolicitante"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Valida busqueda por Sentenciado|Beneficiario
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="apellidoPaterno"></param>
        /// <param name="apellidoMaterno"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// Valida busqueda por medio del numero de causa
        /// </summary>
        /// <param name="numeroCausa"></param>
        /// <param name="idJuzgado"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Valida la consulta por Distrito
        /// </summary>
        /// <param name="idDistrito"></param>
        /// <returns></returns>
        public List<Distrito> ObtenerDistritoPorCircuito(int idDistrito)
        {
            List<Distrito> distritoCircuito = catalogoRepositorio.ObtenerDistritos(idDistrito);
           
            if (catalogoRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "La consulta no genero ningun resultado";
                string messajelogger = ejecucionRepositorio.MensajeError;
            }
            if (catalogoRepositorio.Estatus == Estatus.SIN_RESULTADO)
            {
                Mensaje = "Ocurrio un error interno no controlado, consulte a soporte";
            }
            return distritoCircuito;
        }
        
        /// <summary>
        /// Valida peticion de obtencion de juzgados por distrito
        /// </summary>
        /// <param name="idCircuito"></param>
        /// <param name="tipoJuzgado"></param>
        /// <returns></returns>
        public List<Juzgado> ObtenerJuzgadosAcusatorios(int idCircuito)
        {
            List<Juzgado> juzgadosCiruito = catalogoRepositorio.ObtenerJuzgados(idCircuito, TipoJuzgado.ACUSATORIO);
           
            if (catalogoRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "La consulta no genero ningun resultado";
                string messajelogger = catalogoRepositorio.MensajeError;
            }
            if (catalogoRepositorio.Estatus == Estatus.SIN_RESULTADO)
            {
                Mensaje = "Ocurrio un error interno no controlado, consulte a soporte";
            }
            return juzgadosCiruito;

        }

        public List<Solicitante> ObtenerSolicitanteEjecucion()
        {
            List<Solicitante> solicitanteEjecucion = catalogoRepositorio.ObtenerSolicitantes();
            
            if(catalogoRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "La consulta no genero ningun resultado";
                string messajelogger = catalogoRepositorio.MensajeError;
            }
            if(catalogoRepositorio.Estatus==Estatus.SIN_RESULTADO)
            {
                Mensaje = "Ocurrio un error interno no controlado, consulte a soporte";
            }
            return solicitanteEjecucion;
        }
        #endregion
    }
}
