
using PoderJudicial.SIPOH.AccesoDatos.Interfaces;
using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;
using PoderJudicial.SIPOH.Negocio.Interfaces;

using System.Collections.Generic;

namespace PoderJudicial.SIPOH.Negocio
{
    public class InicialesProcessor : IInicialesProcessor
    {
        //Atributos publicos de proceso
        public string Mensaje { set; get; }

        //Atributos privados del proceso
        private readonly ICatalogosRepository catalogosRepositorio;
        private readonly IExpedienteRepository expedienteRepositorio;

        //Metodo Contructor del proceso, se le inyecta la interfaz CuentaRepositoru
        public InicialesProcessor(ICatalogosRepository catalogosRepositorio, IExpedienteRepository expedienteRepositorio)
        {
            this.catalogosRepositorio = catalogosRepositorio;
            this.expedienteRepositorio = expedienteRepositorio;
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
            List<Juzgado> juzgados = catalogosRepositorio.ObtenerJuzgados(id, tipoJuzgado);

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

        public List<Expediente> RecuperaExpedientes(int idJuzgado, string numeroExpediente, TipoExpediente expediente)
        {
            List<Expediente> expedientes = expedienteRepositorio.ObtenerExpedientes(idJuzgado, numeroExpediente, expediente);

            if(expedienteRepositorio.Estatus == Estatus.SIN_RESULTADO)
                Mensaje = "La consulta no genero ningun resultado";

            else if (expedienteRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "Ocurrio un error interno no controlado, consulte a soporte";
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
    }
}
