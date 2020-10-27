
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

        public List<Distrito> RecuperaDistrito(int idCircuito)
        {
            List<Distrito> distritos = catalogosRepositorio.ObtenerDistritos(idCircuito);
   
            if (catalogosRepositorio.Estatus == Estatus.SIN_RESULTADO)
                Mensaje = "La consulta no genero ningun resultado";

            if (catalogosRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrio un error interno no controlado, consulte a soporte";
                string mensajeLogger = catalogosRepositorio.MensajeError;
                //Logica para ILogger
            }
            
            return distritos;
        }

        public List<Juzgado> RecuperaJuzgado(int id, TipoJuzgado tipoJuzgado)
        {
            List<Juzgado> juzgados = catalogosRepositorio.ObtenerJuzgadosAcusatorioTradicional(id, tipoJuzgado);

            if (catalogosRepositorio.Estatus == Estatus.SIN_RESULTADO)
                Mensaje = "La consulta no genero ningun resultado";

            else if (catalogosRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrio un error interno no controlado, consulte a soporte";
                string mensajeLogger = catalogosRepositorio.MensajeError;
                //Logica para ILogger
            }           
            return juzgados;
        }

        public Expediente RecuperaExpedientes(int idJuzgado, string numeroExpediente, TipoNumeroExpediente expediente)
        {
            Expediente expedientes = expedienteRepositorio.ObtenerExpedientes(idJuzgado, numeroExpediente, expediente);

            if (expedienteRepositorio.Estatus == Estatus.SIN_RESULTADO)
                Mensaje = "La consulta no genero ningun resultado";

            else if (expedienteRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrio un error al consultar la informacion solicitada";
                string mensajeLogger = catalogosRepositorio.MensajeError;
                //Logica para ILogger
            }
            return expedientes;
        }

        public List<Juzgado> RecuperaSala(TipoJuzgado tipoJuzgado)
        {
            List<Juzgado> juzgados = catalogosRepositorio.ObtenerSalas(tipoJuzgado);

            if (catalogosRepositorio.Estatus == Estatus.SIN_RESULTADO)
                Mensaje = "La consulta no genero ningun resultado";

            else if (catalogosRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrio un error interno no controlado, consulte a soporte";
                string mensajeLogger = catalogosRepositorio.MensajeError;
                //Logica para ILogger
            }
            return juzgados;
        }

        public List<Ejecucion> RecuperaSentenciadoBeneficiario(string nombre, string apellidoPaterno, string apellidoMaterno, int idCircuito)
        {
            List<Ejecucion> beneficiarios = ejecucionRepository.ConsultaEjecuciones(ParteCausaBeneficiario.BENEFICIARIO, nombre, apellidoPaterno, apellidoMaterno, idCircuito);

            if (catalogosRepositorio.Estatus == Estatus.SIN_RESULTADO)
                Mensaje = "La consulta no genero ningun resultado";

            else if (catalogosRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrio un error interno no controlado por el sistema, al intentar consultar los beneficiarios coincidentes";
                string mensajeLogger = catalogosRepositorio.MensajeError;
                //Logica para ILogger
            }
            return beneficiarios;
        }

        public List<Anexo> RecuperaAnexos(string tipo) 
        {
            List<Anexo> anexos = catalogosRepositorio.ObtenerAnexosEjecucion(tipo);

            if (catalogosRepositorio.Estatus == Estatus.SIN_RESULTADO)
                Mensaje = "La consulta no genero ningun resultado";

            else if (catalogosRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrio un error interno no controlado, consulte a soporte";
                string mensajeLogger = catalogosRepositorio.MensajeError;
                //Logica para ILogger
            }
            return anexos;
        }

        public List<Solicitante> RecuperaSolicitante()
        {
            List<Solicitante> solicitantes = catalogosRepositorio.ObtenerSolicitantes();

            if (catalogosRepositorio.Estatus == Estatus.SIN_RESULTADO)
                Mensaje = "La consulta no genero ningun resultado";

            else if (catalogosRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrio un error interno no controlado, consulte a soporte";
                string mensajeLogger = catalogosRepositorio.MensajeError;
                //Logica para ILogger
            }
            return solicitantes;
        }

        public List<Solicitud> RecuperaSolicitud()
        {
            List<Solicitud> solcitudes = catalogosRepositorio.ObtenerSolicitudes();

            if (catalogosRepositorio.Estatus == Estatus.SIN_RESULTADO)
                Mensaje = "La consulta no genero ningun resultado";

            else if (catalogosRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrio un error interno no controlado, consulte a soporte";
                string mensajeLogger = catalogosRepositorio.MensajeError;
                //Logica para ILogger
            }
            return solcitudes;
        }

        public int? CrearRegistroInicialDeEjecucion(Ejecucion ejecucion, List<Toca> tocas, List<Anexo> anexos, List<string> amparos, List<int> causas, int circuito)
        {
            int? idUnidad = null;
            bool esCircuitoPachuca = true;

            List<Juzgado> juzgadoEjecucion = catalogosRepositorio.ObtenerJuzgadoEjecucionPorCircuito(circuito);
            
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

        public bool ObtenerInformacionGeneralInicialDeEjecucion(int folio, ref Ejecucion ejecucion, ref List<Expediente> causas, ref List<Toca> tocas, ref List<string> amparos, ref List<Anexo> anexos, ref List<Relacionadas> relaciones)
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
                causas = expedienteRepositorio.ObtenerExpedientesPorEjecucion(folio);
                if (expedienteRepositorio.Estatus == Estatus.ERROR)
                {
                   relaciones.Add(Relacionadas.CAUSAS);
                   string mensajeLogger = expedienteRepositorio.MensajeError;             
                }

                tocas = catalogosRepositorio.ObtenerTocasPorEjecucion(folio);
                if (catalogosRepositorio.Estatus == Estatus.ERROR)
                {
                   relaciones.Add(Relacionadas.TOCAS);
                   string mensajeLogger = catalogosRepositorio.MensajeError;            
                }

                amparos = catalogosRepositorio.ObtenerAmparosPorEjecucion(folio);
                if (catalogosRepositorio.Estatus == Estatus.ERROR)
                {
                   relaciones.Add(Relacionadas.AMPAROS);
                   string mensajeLogger = catalogosRepositorio.MensajeError;                
                }

                anexos = catalogosRepositorio.ObtenerAnexosPorEjecucion(folio);   
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
