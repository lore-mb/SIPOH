using PoderJudicial.SIPOH.AccesoDatos.Interfaces;
using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoderJudicial.SIPOH.Negocio.Interfaces
{
    public class CatalogosProcessor : ICatalogosProcessor
    {
        //Atributos publicos de proceso
        public string Mensaje { set; get; }

        //Atributos privados del proceso
        private readonly ICatalogosRepository catalogosRepositorio;

        public CatalogosProcessor(ICatalogosRepository catalogosRepositorio) 
        {
            this.catalogosRepositorio = catalogosRepositorio;
        }

        //Solo validacion
        public List<Distrito> ObtieneDistritosPorCircuito(int idCircuito)
        {
            List<Distrito> distritos = catalogosRepositorio.ConsultaDistritos(idCircuito);

            if (catalogosRepositorio.Estatus == Estatus.SIN_RESULTADO)
                Mensaje = "La consulta no generó ningún resultado";

            if (catalogosRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrio un error interno no controlado de acceso a datos";
                string mensajeLogger = catalogosRepositorio.MensajeError;
                //Logica para ILogger
            }
            return distritos;
        }

        //Solo validacion
        public List<Juzgado> ObtieneJuzgadosPorTipoSistema(int idCircuitoDistrito, TipoSistema tipoJuzgado)
        {
            List<Juzgado> juzgados = catalogosRepositorio.ConsultaJuzgados(tipoJuzgado, idCircuitoDistrito);

            if (catalogosRepositorio.Estatus == Estatus.SIN_RESULTADO)
                Mensaje = "La consulta no generó ningún resultado";

            else if (catalogosRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrio un error interno no controlado de acceso a datos";
                string mensajeLogger = catalogosRepositorio.MensajeError;
                //Logica para ILogger
            }
            return juzgados;
        }

        //Solo validacion
        public List<Juzgado> ObtieneSalasPorTipoSistema(TipoSistema tipoJuzgado)
        {
            List<Juzgado> juzgados = catalogosRepositorio.ConsultaJuzgados(tipoJuzgado);

            if (catalogosRepositorio.Estatus == Estatus.SIN_RESULTADO)
                Mensaje = "La consulta no generó ningún resultado";

            else if (catalogosRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrio un error interno no controlado de acceso a datos";
                string mensajeLogger = catalogosRepositorio.MensajeError;
                //Logica para ILogger
            }
            return juzgados;
        }

        //Solo validacion
        public List<Anexo> ObtieneAnexosPorTipo(string tipo)
        {
            List<Anexo> anexos = catalogosRepositorio.ConsultaAnexos(tipo);

            if (catalogosRepositorio.Estatus == Estatus.SIN_RESULTADO)
                Mensaje = "La consulta no generó ningún resultado";

            else if (catalogosRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrio un error interno no controlado de acceso a datos";
                string mensajeLogger = catalogosRepositorio.MensajeError;
                //Logica para ILogger
            }
            return anexos;
        }

        //Solo validacion
        public List<Solicitante> ObtieneSolicitantes()
        {
            List<Solicitante> solicitantes = catalogosRepositorio.ConsultaSolicitantes();

            if (catalogosRepositorio.Estatus == Estatus.SIN_RESULTADO)
                Mensaje = "La consulta no generó ningún resultado";

            else if (catalogosRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrio un error interno no controlado de acceso a datos";
                string mensajeLogger = catalogosRepositorio.MensajeError;
                //Logica para ILogger
            }
            return solicitantes;
        }

        //Solo validacion
        public List<Solicitud> ObtieneSolicitudes()
        {
            List<Solicitud> solcitudes = catalogosRepositorio.ConsultaSolicitudes();

            if (catalogosRepositorio.Estatus == Estatus.SIN_RESULTADO)
                Mensaje = "La consulta no generó ningún resultado";

            else if (catalogosRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrio un error interno no controlado de acceso a datos";
                string mensajeLogger = catalogosRepositorio.MensajeError;
                //Logica para ILogger
            }
            return solcitudes;
        }

        /// <summary>
        /// Valida peticion de obtencion de juzgados por distrito
        /// </summary>
        /// <param name="idCircuito"></param>
        /// <param name="tipoJuzgado"></param>
        /// <returns></returns>
        public List<Juzgado> ObtieneJuzgadosAcusatoriosPorCircuito(int idCircuito)
        {
            List<Juzgado> juzgadosCiruito = catalogosRepositorio.ConsultaJuzgados(TipoSistema.ACUSATORIO, idCircuito);

            if (catalogosRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "La consulta no generó ningún resultado";
                string messajelogger = catalogosRepositorio.MensajeError;
            }

            if (catalogosRepositorio.Estatus == Estatus.SIN_RESULTADO)
            {
                Mensaje = "Ocurrio un error interno no controlado de acceso a datos";
            }

            return juzgadosCiruito;
        }

        public List<Juzgado> ObtieneJuzgadosPorDistrito(int idDistrito)
        {
            List<Juzgado> juzgados = catalogosRepositorio.ConsultaJuzgados(idDistrito);

            if (catalogosRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "La consulta no generó ningún resultado";
                string messajelogger = catalogosRepositorio.MensajeError;
            }

            if (catalogosRepositorio.Estatus == Estatus.SIN_RESULTADO)
            {
                Mensaje = "Ocurrio un error interno no controlado de acceso a datos";
            }

            return juzgados;
        }

        public List<Juzgado> ObtieneJuzgadosPorCircuito(int IdCircuito)
        {
            List<Juzgado> JuzgadoEjecucion = catalogosRepositorio.ConsultaJuzgados(IdCircuito, TipoJuzgado.EJECUCION);

            if (catalogosRepositorio.Estatus == Estatus.SIN_RESULTADO)
            {
                Mensaje = "La búsqueda no obtuvo ningún resultado.";
            }

            if (catalogosRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrio un error interno no controlado de acceso a datos";
                string InfoMensajeLogger = catalogosRepositorio.MensajeError;
            }

            return JuzgadoEjecucion;
        }

        public List<Delito> ObtieneDelitosDelImputado()
        {
            List<Delito> delitos = catalogosRepositorio.ConsultaDelitos();

            if (catalogosRepositorio.Estatus == Estatus.SIN_RESULTADO)
            Mensaje = "La consulta no generó ningún resultado";

            else if (catalogosRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrio un error interno no controlado de acceso a datos";
                string mensajeLogger = catalogosRepositorio.MensajeError;
                //Logica para ILogger
            }
            return delitos;
        }
    }
}
