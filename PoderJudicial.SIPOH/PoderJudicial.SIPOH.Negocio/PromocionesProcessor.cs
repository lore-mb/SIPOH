
using PoderJudicial.SIPOH.AccesoDatos.Interfaces;
using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;
using PoderJudicial.SIPOH.Negocio.Interfaces;
using System.Collections.Generic;
namespace PoderJudicial.SIPOH.Negocio
{
    public class PromocionesProcessor : IPromocionesProcessor
    {
        // [Public Method]
        public string Mensaje { get; set; }

        // [Private Method] 
        private readonly ICatalogosRepository catalogosRepositorio;
        private readonly IEjecucionRepository ejecucionRepositorio; 
        private readonly IExpedienteRepository expedienteRepositorio;

        // [Interface Injection Method]
        public PromocionesProcessor(ICatalogosRepository catalogosRepositorio, IEjecucionRepository ejecucionRepositorio, IExpedienteRepository expedienteRepositorio) {
            this.catalogosRepositorio = catalogosRepositorio;
            this.ejecucionRepositorio = ejecucionRepositorio;
            this.expedienteRepositorio = expedienteRepositorio;
        }

        public List<Ejecucion> ObtenerEjecucionPorJuzgado(int Juzgado, string NoEjecucion)
        {
            List<Ejecucion> EjecucionPorCircuito = ejecucionRepositorio.ConsultaEjecuciones(TipoNumeroExpediente.EJECUCION, NoEjecucion, Juzgado);

            if (ejecucionRepositorio.Estatus == Estatus.SIN_RESULTADO) {
                Mensaje = "La búsqueda no obtuvo ningún resultado.";
            }

            if (ejecucionRepositorio.Estatus == Estatus.ERROR) {
                Mensaje = "Ocurrió un error no controlado por el sistema, por favor contacte a soporte técnico.";
                string InfoMensajeLogger = ejecucionRepositorio.MensajeError;
            }

            return EjecucionPorCircuito;
        }

        public List<Expediente> ObtenerExpedientesRelacionadoEjecucion(int IdExpediente)
        {
            List<Expediente> ExpedienteListado = expedienteRepositorio.ConsultaExpedientes(IdExpediente);

            if (expedienteRepositorio.Estatus == Estatus.SIN_RESULTADO)
            {
                Mensaje = ("La búsqueda no obtuvo ningún resultado.");
            }

            if (expedienteRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrió un error no controlado por el sistema, por favor contacte a soporte técnico.";
                string InfoMensajeLogger = expedienteRepositorio.MensajeError;
            }
            return ExpedienteListado;
        }

        public int? GuardarPostEjecucion(EjecucionPosterior PostEjecucion, List<Anexo> Anexos) 
        {
            int? IdEjecucion = ejecucionRepositorio.CreaEjecucionPosterior(PostEjecucion, Anexos);

            if (ejecucionRepositorio.Estatus == Estatus.OK) 
            {
                Mensaje = "El registro con numero de ejecucion " + "<b>" + IdEjecucion + "</b>" + " se ha ejecutado correctamente";
            } 
            else if (ejecucionRepositorio.Estatus == Estatus.ERROR) 
            {
                Mensaje = "Ocurrio un problema al ejecutar el registro";
                string InfoMensajeLogger = catalogosRepositorio.MensajeError;
            }

            return IdEjecucion;
        }

        public bool InformacionRegistroPromocion(int FolioEjecucion, ref EjecucionPosterior Ejecucion, ref List<Anexo> Anexo, ref List<Relacionadas> Relacionada) {

            // Consulta informacion relacionada a PostEjecucion

            Ejecucion = ejecucionRepositorio.ConsultarRegistroEjecucionPosterior(FolioEjecucion);

            if (ejecucionRepositorio.Estatus == Estatus.SIN_RESULTADO)
            {
                Relacionada.Add(Relacionadas.EJECUCION);
                Mensaje = "La búsqueda no obtuvo ningún resultado.";
                return false;
            }
            
            if (ejecucionRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrió un error no controlado por el sistema, por favor contacte a soporte técnico.";
                string InfoMensajeLogger = ejecucionRepositorio.MensajeError;
                return false;
            }
            
            else if (ejecucionRepositorio.Estatus == Estatus.OK) 
            {
                Anexo = catalogosRepositorio.ConsultaAnexos(FolioEjecucion, Instancia.PROMOCION);

                if (catalogosRepositorio.Estatus == Estatus.ERROR) {
                    Mensaje = "Ocurrió un error no controlado por el sistema, por favor contacte a soporte técnico.";
                    Relacionada.Add(Relacionadas.ANEXOS);
                    string InfoMensajeLogger = catalogosRepositorio.MensajeError;
                }
            }

            if (Relacionada.Count > 0)
            {
                Mensaje = "Problema";
                return false;
            }
            else
            {
                return true;
            }

        }

    }
}
