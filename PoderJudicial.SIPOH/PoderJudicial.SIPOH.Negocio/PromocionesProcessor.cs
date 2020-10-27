
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
            List<Juzgado> JuzgadoEjecucion = catalogosRepositorio.ConsultaJuzgados(idcircuito, TipoJuzgado.EJECUCION);
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
            List<Ejecucion> EjecucionPorCircuito = ejecucionRepositorio.ConsultaEjecuciones(TipoNumeroExpediente.EJECUCION, NoEjecucion, Juzgado);
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

        public List<Anexo> ObtenerAnexosEjecucion(string tipo) {
            List<Anexo> ListarExpedientes = catalogosRepositorio.ConsultaAnexos(tipo);
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

        public int? GuardarPostEjecucion(PostEjecucion postEjecucion, List<Anexo> anexos) {

            int? IdEjecucion = ejecucionRepositorio.GuardarPostEjecucion(postEjecucion, anexos);

            if (ejecucionRepositorio.Estatus == Estatus.OK) {
                Mensaje = "El registro con numero de ejecucion " + "<b>" + IdEjecucion + "</b>" + " se ha ejecutado correctamente";
            } else if (ejecucionRepositorio.Estatus == Estatus.ERROR) {
                Mensaje = "Ocurrio un problema al ejecutar el registro";
                string mensajeLogger = catalogosRepositorio.MensajeError;
            }
            return IdEjecucion;
        }

        public bool InformacionRegistroPromocion(int FolioEjecucion, ref Ejecucion ejecucion, ref List<Anexo> anexo, ref List<Relacionadas> relacionadas) {

            // Consulta informacion relacionada a PostEjecucion

            ejecucion = ejecucionRepositorio.ObtenerEjecucionPromocionPorFolio(FolioEjecucion);

            if (ejecucionRepositorio.Estatus == Estatus.SIN_RESULTADO)
            {
                relacionadas.Add(Relacionadas.EJECUCION);
                Mensaje = "No hay respuesta para la consulta del folio " + "<b>" + FolioEjecucion + "</b>" + " registrado previamente, consulte con soporte.";
                return false;
            }
            
            if (ejecucionRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "ERROR al obtener los detalles del numero del folio " + "<b>" + FolioEjecucion +"</b>" + " .";
                string MensajeLogger = ejecucionRepositorio.MensajeError;
                return false;
            }
            
            else if (ejecucionRepositorio.Estatus == Estatus.OK) 
            {
                anexo = catalogosRepositorio.ConsultarAnexosPorEjecucionPosterior(FolioEjecucion);
                if (catalogosRepositorio.Estatus == Estatus.ERROR) {
                    Mensaje = "Hubo un error al consultar la informacion";
                    relacionadas.Add(Relacionadas.ANEXOS);
                    string MensajeLogger = catalogosRepositorio.MensajeError;
                }
            
            }

            if (relacionadas.Count > 0)
            {
                Mensaje = "Problema";
                return false;
            }
            else
                return true;
        
        }

    }
}
