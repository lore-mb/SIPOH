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

        public bool ? ValidaExistenciaDeCausaPorJuzgadoMasNumeroDeCausaNUC(int idJuzgado, string numeroExpediente, string nuc = null)
        {
            if (nuc == null)
            {
                expedienteRepositorio.ConsultaTotalExpedientes(idJuzgado, numeroExpediente);
            }
            else 
            {
                expedienteRepositorio.ConsultaTotalExpedientes(idJuzgado, numeroExpediente, nuc);
            }

            if (expedienteRepositorio.Estatus == Estatus.SIN_RESULTADO)
            {
                Mensaje = "La consulta no genero ningun resultado";
                return false;
            }

            else if (expedienteRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrio un error al consultar la informacion solicitada";
                string mensajeLogger = expedienteRepositorio.MensajeError;
                return null;
                //Logica para ILogger
            }

            return true;
        }
    }
}
