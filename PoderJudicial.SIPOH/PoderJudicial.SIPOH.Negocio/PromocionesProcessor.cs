
using PoderJudicial.SIPOH.AccesoDatos.Interfaces;
using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;
using PoderJudicial.SIPOH.Negocio.Interfaces;
using System.Collections.Generic;
namespace PoderJudicial.SIPOH.Negocio
{
    public class PromocionesProcessor : IPromocionesProcessor
    {
        // [Public method]
        public string Mensaje { get; set; }

        // [Private method]
        private readonly ICatalogosRepository catalogosRepositorio;
        private readonly IEjecucionRepository ejecucionRepositorio;
        private readonly IExpedienteRepository expedienteRepositorio;

        // [Interface injection method]
        public PromocionesProcessor(ICatalogosRepository catalogosRepositorio, IEjecucionRepository ejecucionRepositorio, IExpedienteRepository expedienteRepositorio) {
            this.catalogosRepositorio = catalogosRepositorio;
            this.ejecucionRepositorio = ejecucionRepositorio;
            this.expedienteRepositorio = expedienteRepositorio;
        }

        public List<Ejecucion> ObtenerEjecucionPorJuzgado(int Juzgado, string NoEjecucion)
        {
            List<Ejecucion> EjecucionPorCircuito = ejecucionRepositorio.ObtenerEjecucionPorJuzgado(Juzgado, NoEjecucion);
            if (ejecucionRepositorio.Estatus == Estatus.SIN_RESULTADO) {
                Mensaje = ("La consulta no generó ningun resultado");
            }

            if (ejecucionRepositorio.Estatus == Estatus.ERROR) {
                Mensaje = ("Ocurrio un error interno, contacte con soporte");
                string messajeLogger = ejecucionRepositorio.MensajeError;
            }

            return EjecucionPorCircuito;
        }

        public Expediente ObtenerExpedienteEjecucionCausa(int idExpediente)
        {
            Expediente expedienteCRE = expedienteRepositorio.ObtenerExpedienteEjecucionCausa(idExpediente);

            if (expedienteRepositorio.Estatus == Estatus.SIN_RESULTADO) 
            {
                Mensaje = ("La consulta no genero ningun resultado");
            }

            if (expedienteRepositorio.Estatus == Estatus.ERROR) {
                Mensaje = ("Ocurrio un error interno, consulte con soporte");
            }

            return expedienteCRE;
        }

        public List<Juzgado> ObtenerJuzgadoEjecucionPorCircuito(int idcircuito)
        {
            List<Juzgado> JuzgadoEjecucion = catalogosRepositorio.ObtenerJuzgadoEjecucionPorCircuito(idcircuito);
            if (catalogosRepositorio.Estatus == Estatus.SIN_RESULTADO) {
                Mensaje = "La consulta no genero ningun resultado.";
            }

            if (catalogosRepositorio.Estatus == Estatus.ERROR) {
                Mensaje = ("Ocurrio un error interno ono controlado, consulte a soporte");
                string mesajeLogger = catalogosRepositorio.MensajeError;
            }

            return JuzgadoEjecucion;
        }
    }
}
