using PoderJudicial.SIPOH.AccesoDatos.Interfaces;
using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Negocio.Interfaces;
using PoderJudicial.SIPOH.Entidades.Enum;
using PoderJudicial.SIPOH.Negocio;
using System.Collections.Generic;

namespace PoderJudicial.SIPOH.Negocio
{
    public class ReportesProcessor : IReportesProcessor
    {
        #region Propiedades 
        public string Mensaje { get; set; }
        #endregion

        #region Inyecccion

        private readonly ICatalogosRepository catalogosRepository;
        private readonly IEjecucionRepository ejecucionRepository;
        private readonly IExpedienteRepository expedienteRepository;

        public ReportesProcessor(ICatalogosRepository catalogosRepository, IEjecucionRepository ejecucionRepository, IExpedienteRepository expedienteRepository) {
            this.catalogosRepository = catalogosRepository;
            this.ejecucionRepository = ejecucionRepository;
            this.expedienteRepository = expedienteRepository;
        }

        #endregion

        #region Metodos

        public List<Juzgado> ObtenerJuzgadoPorCircuito(int IdCircuito)
        {
            List<Juzgado> Lista = catalogosRepository.ObtenerJuzgadoEjecucionPorCircuito(IdCircuito);

            if (catalogosRepository.Estatus == Estatus.SIN_RESULTADO)
            {
                Mensaje = "No hay respuesta para la solicitud";
            }
            else if (catalogosRepository.Estatus == Estatus.ERROR) {
                Mensaje = "Error, la consulta no ha generado ningun resultado";
                string MensajeLogger = catalogosRepository.MensajeError;
            }
            return Lista;
        }
        #endregion

    }
}
