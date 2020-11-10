using PoderJudicial.SIPOH.AccesoDatos.Interfaces;
using PoderJudicial.SIPOH.Entidades.Enum;
using PoderJudicial.SIPOH.Negocio.Interfaces;
using System;

namespace PoderJudicial.SIPOH.Negocio
{
    public class ConsignacionesHistoricasProcessor : IConsignacionesHistoricasProcessor
    {
        public string Mensaje { get; set; }

        //Atributos privados del proceso
        private readonly IExpedienteRepository expedienteRepositorio;
        private readonly IEjecucionRepository ejecucionRepositorio;

        public ConsignacionesHistoricasProcessor(IExpedienteRepository expedienteRepositorio, IEjecucionRepository ejecucionRepositorio) 
        {
            this.expedienteRepositorio = expedienteRepositorio;
            this.ejecucionRepositorio = ejecucionRepositorio;
        }

        /// <summary>
        /// Valida que el nuc sea nulo o contenga valor, depediento de su valor solicita acceso a datos el metodo correspondiente para
        /// la consulta
        /// </summary>
        /// <param name="idJuzgado">IdJuzgado de la causa a validar</param>
        /// <param name="numeroDeCausa">Numero de causa de la causa a validar</param>
        /// <param name="nuc">Nuc de la causa a validar</param>
        /// <returns>Boleano que indica verdadero si existe el registro solicitado en la base de datos</returns>
        public bool? ValidaExistenciaDeExpedientePorJuzgadoMasNumeroCausa(int idJuzgado, string numeroDeCausa, string nuc = null)
        {
            //Si el nuc es nulo, toma la validacion de expediente sin NUC
            if (nuc == null)
            expedienteRepositorio.ValidaNumeroCausa(idJuzgado, numeroDeCausa);

            //Si el nuc contiene valor, toma la validacion de expediente con NUC y Causa
            else 
            expedienteRepositorio.ValidaNumeroCausa(idJuzgado, numeroDeCausa, nuc);
            
            //Validacion del acceso a datos
            if (expedienteRepositorio.Estatus == Estatus.SIN_RESULTADO)
            {
                Mensaje = "La consulta no genero ningun resultado";
                return false;
            }

            else if (expedienteRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrio un error interno de acceso a datos no controlado, intente nuevamente o consulte a soporte";
                string mensajeLogger = expedienteRepositorio.MensajeError;
                return null;
                //Logica para ILogger
            }

            return true;
        }

        /// <summary>
        /// Valida que el numero de ejecucion asignado no exista dentro del juzgado seleccionado, a si mismo no sea mayor al rango de numero por año 
        /// </summary>
        /// <param name="idJuzgado">Id de Juzgado de Ejecucion</param>
        /// <param name="numeroDeEjecucion">Numero de Ejecucion</param>
        /// <returns></returns>
        public bool? ValidaAsignacionManualDeNumeroEjecucion(int idJuzgado, string numeroEjecucion)
        {
            //Metodo del repositorio que valida que el numero de ejecucion exista ya en la base de datos
            ejecucionRepositorio.ValidaNumeroEjecucion(idJuzgado, numeroEjecucion);

            if (ejecucionRepositorio.Estatus == Estatus.OK) 
            {
                Mensaje = string.Format("El numero de Ejecucion {0} asignado ya existe en el juzgado seleccionado", numeroEjecucion);
                return false;
            }
            else if (ejecucionRepositorio.Estatus == Estatus.ERROR) 
            {
                Mensaje = "Ocurrio un error interno de acceso a datos no controlado, intente nuevamente o consulte a soporte";
                string mensajeLogger = ejecucionRepositorio.MensajeError;
                //Logica para ILogger
                return null;
            }
            else
            {
                //Obtiene los consecutivos del numero ejecucion 
                int numeroConsecutivo = Convert.ToInt32(numeroEjecucion.Split('/')[0]);
                string anio = Convert.ToString(numeroEjecucion.Split('/')[1]);

                ejecucionRepositorio.ConsultaRengoDeNumeroEjecucion(idJuzgado, anio, out string numeroEjecucionMinimo, out string numeroEjecucionMaximo);

                if (ejecucionRepositorio.Estatus == Estatus.ERROR)
                {
                    Mensaje = "Ocurrio un error interno de acceso a datos no controlado, intente nuevamente o consulte a soporte";
                    string mensajeLogger = ejecucionRepositorio.MensajeError;
                    //Logica para ILogger
                    return null;
                }
                else if (ejecucionRepositorio.Estatus == Estatus.OK)
                {
                    //Obtiene los consecutivos del rango de numeros de ejecucion por el año
                    int numeroConsecutivoMinimo = Convert.ToInt32(numeroEjecucionMinimo.Split('/')[0]);
                    int numeroConsecutivoMaximo = Convert.ToInt32(numeroEjecucionMaximo.Split('/')[0]);

                    if (numeroConsecutivo < numeroConsecutivoMinimo)
                    {
                        Mensaje = string.Format("El Número de Ejecución {0} ingresado se encuentra disponible para el registro de Consignacion Historica", numeroEjecucion);
                        return true;
                    }

                    if (numeroConsecutivo > numeroConsecutivoMaximo)
                    {
                        Mensaje = string.Format("El consecutivo del Número de Ejecución {0} no puede ser mayor o igual al ultimo Numero de Ejecucion generado", numeroEjecucion);
                        return false;
                    }

                    Mensaje = string.Format("El consecutivo del Número de Ejecución {0} ingresado ya se encuentra asignado al juzgado actualmente", numeroEjecucion);
                    return false;
                }
                else 
                {
                    int anioNumero = Convert.ToInt32(numeroEjecucion.Split('/')[1]);
                    int anioActual = DateTime.Now.Year;

                    if (anioNumero > anioActual) 
                    {
                        Mensaje = string.Format("El año del Número de Ejecución {0} ingresado no puede ser mayor al año actual", numeroEjecucion);
                        return false;
                    }

                    Mensaje = string.Format("El Número de Ejecución {0} ingresado se encuentra disponible para el registro de consignacion historica", numeroEjecucion);
                    return true;
                }
            }       
        }
    }
}
