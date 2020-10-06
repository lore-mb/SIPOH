
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

        public List<Juzgado> ObtenerJuzgadoEjecucionPorCircuito(int idcircuito)
        {
            List<Juzgado> JuzgadoEjecucion = catalogosRepositorio.ObtenerJuzgadoEjecucionPorCircuito(idcircuito);
            if (catalogosRepositorio.Estatus == Estatus.SIN_RESULTADO)
            {
                Mensaje = "La consulta no genero ningun resultado.";
            }

            if (catalogosRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = ("Ocurrio un error interno ono controlado, consulte a soporte");
                string mesajeLogger = catalogosRepositorio.MensajeError;
            }

            return JuzgadoEjecucion;
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

        public List<Expediente> ObtenerExpedientesPorEjecucion(int idEjecucion)
        {
            List<Expediente> ExpedienteListado = expedienteRepositorio.ObtenerExpedientesPorEjecucion(idEjecucion);
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

        public List<Anexo> ObtenerAnexosEjecucion(string tipo) {
            List<Anexo> ListarExpedientes = catalogosRepositorio.ObtenerAnexosEjecucion(tipo);
            if (catalogosRepositorio.Estatus == Estatus.SIN_RESULTADO) {
                Mensaje = ("La consulta no ha generado ningun resultado");
            }
            
            if (catalogosRepositorio.Estatus == Estatus.ERROR) {
                Mensaje = ("Ocurrio un error interno, contacte a soporte");
                string messageLoger = expedienteRepositorio.MensajeError;
            }
            return ListarExpedientes;
        }

        public Expediente ObtenerExpedienteEjecucionCausa(int idExpediente)
        {
            throw new System.NotImplementedException();
        }

        public int? GuardarPostEjecucion(PostEjecucion postEjecucion) {

            int? idEjecucion = ejecucionRepositorio.GuardarPostEjecucion(postEjecucion);

            if (catalogosRepositorio.Estatus == Estatus.OK) {
                Mensaje = "Se generó con exito el registro.";    
            }
            else if (catalogosRepositorio.Estatus == Estatus.ERROR) {
                Mensaje = " Ocurrio un error al intentar generera el registro.";
                string MensajeLogger = catalogosRepositorio.MensajeError;
            }
            return idEjecucion;
        }
    }
}
