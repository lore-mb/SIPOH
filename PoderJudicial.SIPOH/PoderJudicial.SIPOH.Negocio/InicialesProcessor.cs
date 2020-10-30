
using PoderJudicial.SIPOH.AccesoDatos.Interfaces;
using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;
using PoderJudicial.SIPOH.Negocio.Interfaces;

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PoderJudicial.SIPOH.Negocio
{
    public class InicialesProcessor : IInicialesProcessor
    {
        //Atributos publicos de proceso
        public string Mensaje { set; get; }

        //Atributos privados del proceso
        private readonly ICatalogosRepository catalogosRepositorio;
        private readonly IExpedienteRepository expedienteRepositorio;
        private readonly IEjecucionRepository ejecucionRepository;

        //Metodo Constructor del proceso, se le inyecta la interfaz CuentaRepositoru
        public InicialesProcessor(ICatalogosRepository catalogosRepositorio, IExpedienteRepository expedienteRepositorio, IEjecucionRepository ejecucionRepository)
        {
            this.catalogosRepositorio = catalogosRepositorio;
            this.expedienteRepositorio = expedienteRepositorio;
            this.ejecucionRepository = ejecucionRepository;
        }

        public Expediente ObtieneCausaPorMedioDelJuzgadoNumeroCausaNUC(int idJuzgado, string numeroExpediente, TipoNumeroExpediente expediente)
        {
            Expediente expedientes = expedienteRepositorio.ConsultaExpediente(idJuzgado, numeroExpediente, expediente);

            if (expedienteRepositorio.Estatus == Estatus.SIN_RESULTADO)
                Mensaje = "La consulta no genero ningun resultado";

            else if (expedienteRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrio un error al consultar la informacion solicitada";
                string mensajeLogger = expedienteRepositorio.MensajeError;
                //Logica para ILogger
            }
            return expedientes;
        }
     
        public List<Ejecucion> ObtieneSentenciadosBeneficiariosPorMedioDelNombre(string nombre, string apellidoPaterno, string apellidoMaterno, int idCircuito)
        {
            List<Ejecucion> beneficiarios = ejecucionRepository.ConsultaEjecuciones(ParteCausaBeneficiario.BENEFICIARIO, nombre, apellidoPaterno, apellidoMaterno, idCircuito);

            if (ejecucionRepository.Estatus == Estatus.SIN_RESULTADO)
                Mensaje = "La consulta no genero ningun resultado";

            else if (ejecucionRepository.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrio un error interno no controlado por el sistema, al intentar consultar los beneficiarios coincidentes";
                string mensajeLogger = ejecucionRepository.MensajeError;
                //Logica para ILogger
            }
            return beneficiarios;
        }

        public int? CreaRegistroInicialDeEjecucion(Ejecucion ejecucion, List<Toca> tocas, List<Anexo> anexos, List<string> amparos, List<int> causas, int circuito)
        {
            int? idUnidad = null;
            bool esCircuitoPachuca = true;

            List<Juzgado> juzgadoEjecucion = catalogosRepositorio.ConsultaJuzgados(circuito, TipoJuzgado.EJECUCION);
            
            if (juzgadoEjecucion != null && circuito != 1) 
            {
                idUnidad = juzgadoEjecucion.Select(x => x.IdJuzgado).FirstOrDefault();
                esCircuitoPachuca = false;
            }

            int? idEjecucion = ejecucionRepository.CreaEjecucion(ejecucion, causas, tocas, amparos, anexos, idUnidad, esCircuitoPachuca);

            if (catalogosRepositorio.Estatus == Estatus.OK)
            Mensaje = "La inserción de datos fue correcta, folio de ejecucion generado : " + idEjecucion;

            else if (catalogosRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrio un error al intentar generar el registro de Ejecución";
                string mensajeLogger = catalogosRepositorio.MensajeError;
                
                //Logica para ILogger
            }
             
            return idEjecucion;
        }

        public bool ObtieneInformacionGeneralInicialDeEjecucion(int folio, ref Ejecucion ejecucion, ref List<Expediente> causas, ref List<Toca> tocas, ref List<string> amparos, ref List<Anexo> anexos, ref List<Relacionadas> relaciones)
        {
            //Recupera la informacion de la ejecucion
            ejecucion = ejecucionRepository.ConsultaEjecucion(folio);

            if (ejecucionRepository.Estatus == Estatus.SIN_RESULTADO)
            {
                relaciones.Add(Relacionadas.EJECUCION);
                Mensaje = "No es posible generar el detalle, el numero de folio <b>" + folio + "</b> no existe en la base de datos, consulte a soporte.";
                return false;
            }

            if (ejecucionRepository.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrio un error al tratar de consultar el registro de ejecución con numero de folio <b>" + folio + "</b>";
                string mensajeLogger = ejecucionRepository.MensajeError;
                return false;
            }
            else if (ejecucionRepository.Estatus == Estatus.OK) 
            {
                causas = expedienteRepositorio.ConsultaExpedientes(folio);
                if (expedienteRepositorio.Estatus == Estatus.ERROR)
                {
                   relaciones.Add(Relacionadas.CAUSAS);
                   string mensajeLogger = expedienteRepositorio.MensajeError;             
                }

                tocas = catalogosRepositorio.ConsultaTocas(folio);
                if (catalogosRepositorio.Estatus == Estatus.ERROR)
                {
                   relaciones.Add(Relacionadas.TOCAS);
                   string mensajeLogger = catalogosRepositorio.MensajeError;            
                }

                amparos = catalogosRepositorio.ConsultaAmparos(folio);
                if (catalogosRepositorio.Estatus == Estatus.ERROR)
                {
                   relaciones.Add(Relacionadas.AMPAROS);
                   string mensajeLogger = catalogosRepositorio.MensajeError;                
                }

                anexos = catalogosRepositorio.ConsultaAnexos(folio, Instancia.INICIAL);   
                if (catalogosRepositorio.Estatus == Estatus.ERROR)
                {
                   relaciones.Add(Relacionadas.ANEXOS);
                   string mensajeLogger = catalogosRepositorio.MensajeError;             
                }            
            }

            if (relaciones.Count > 0)
            {
                Mensaje = "Ocurrio un error al tratar de consultar la información relacionada al registro de ejecución con numero <b>" + ejecucion.NumeroEjecucion +"</b>";
                return false;
            }
            else
            return true;
        }
    }
}
