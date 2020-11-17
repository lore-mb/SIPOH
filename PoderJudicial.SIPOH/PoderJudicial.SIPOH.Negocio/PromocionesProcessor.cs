using PoderJudicial.SIPOH.AccesoDatos.Interfaces;
using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;
using PoderJudicial.SIPOH.Negocio.Interfaces;
using System.Collections.Generic;
namespace PoderJudicial.SIPOH.Negocio
{
    public class PromocionesProcessor : IPromocionesProcessor
    {
        public string Mensaje { get; set; }

        private readonly ICatalogosRepository catalogosRepositorio;
        private readonly IEjecucionRepository ejecucionRepositorio;
        private readonly IExpedienteRepository expedienteRepositorio;

        public PromocionesProcessor(ICatalogosRepository catalogosRepositorio, IEjecucionRepository ejecucionRepositorio, IExpedienteRepository expedienteRepositorio)
        {
            this.catalogosRepositorio = catalogosRepositorio;
            this.ejecucionRepositorio = ejecucionRepositorio;
            this.expedienteRepositorio = expedienteRepositorio;
        }

        public List<Ejecucion> ObtenerEjecucionPorJuzgado(int idJuzgado, string noEjecucion)
        {
            List<Ejecucion> EjecucionPorCircuito = ejecucionRepositorio.ConsultaEjecuciones(TipoNumeroExpediente.EJECUCION, noEjecucion, idJuzgado);

            if (ejecucionRepositorio.Estatus == Estatus.SIN_RESULTADO)
            {
                Mensaje = "La búsqueda no obtuvo ningún resultado.";
            }

            if (ejecucionRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrió un error no controlado por el sistema, por favor contacte a soporte técnico.";
                string InfoMensajeLogger = ejecucionRepositorio.MensajeError;
            }

            return EjecucionPorCircuito;
        }

        public List<Expediente> ObtenerExpedientesRelacionadoEjecucion(int idExpediente)
        {
            List<Expediente> ExpedienteListado = expedienteRepositorio.ConsultaCausas(idExpediente);

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

        public int? RegistrarEjecucionPosterior(EjecucionPosterior ejecucionPosterior, List<Anexo> anexos)
        {
            int? IdEjecucion = ejecucionRepositorio.CreaEjecucionPosterior(ejecucionPosterior, anexos);

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

        public bool ObtenerInformacionRegistroEjecucionPosterior(int folioEjecucion, ref EjecucionPosterior ejecucion, ref List<Anexo> anexo, ref List<Relacionadas> relacionada)
        {

            ejecucion = ejecucionRepositorio.ConsultaEjecucionPosterior(folioEjecucion);

            if (ejecucionRepositorio.Estatus == Estatus.SIN_RESULTADO)
            {
                relacionada.Add(Relacionadas.EJECUCION);
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
                anexo = catalogosRepositorio.ConsultaAnexos(folioEjecucion, Instancia.PROMOCION);

                if (catalogosRepositorio.Estatus == Estatus.ERROR)
                {
                    Mensaje = "Ocurrió un error no controlado por el sistema, por favor contacte a soporte técnico.";
                    relacionada.Add(Relacionadas.ANEXOS);
                    string InfoMensajeLogger = catalogosRepositorio.MensajeError;
                }
            }

            if (relacionada.Count > 0)
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
