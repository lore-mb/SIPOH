using PoderJudicial.SIPOH.AccesoDatos.Interfaces;
using PoderJudicial.SIPOH.Entidades.Enum;
using PoderJudicial.SIPOH.Negocio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoderJudicial.SIPOH.Negocio
{
    public class ConsignacionesHistoricasProcessor : IConsignacionesHistoricasProcessor
    {
        public string Mensaje { get; set; }

        //Atributos privados del proceso
        private readonly IExpedienteRepository expedienteRepositorio;

        public ConsignacionesHistoricasProcessor(IExpedienteRepository expedienteRepositorio) 
        {
            this.expedienteRepositorio = expedienteRepositorio;
        }

        /// <summary>
        /// Valida que el nuc sea nulo o contenga valor, depediento de su valor solicita acceso a datos el metodo correspondiente para
        /// la consulta
        /// </summary>
        /// <param name="idJuzgado">IdJuzgado de la causa a validar</param>
        /// <param name="numeroDeCausa">Numero de causa de la causa a validar</param>
        /// <param name="nuc">Nuc de la causa a validar</param>
        /// <returns>Boleano que indica verdadero si existe el registro solicitado en la base de datos</returns>
        public bool? ValidaExistenciaDeCausaPorJuzgadoMasNumeroDeCausaNUC(int idJuzgado, string numeroDeCausa, string nuc = null)
        {
            //Si el nuc es nulo, toma la validacion de expediente sin NUC
            if (nuc == null)
            expedienteRepositorio.ExisteExpediente(idJuzgado, numeroDeCausa);

            //Si el nuc contiene valor, toma la validacion de expediente con NUC y Causa
            else 
            expedienteRepositorio.ExisteExpediente(idJuzgado, numeroDeCausa, nuc);
            
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
    }
}
