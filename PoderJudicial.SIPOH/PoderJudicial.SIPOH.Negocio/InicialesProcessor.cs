using PoderJudicial.SIPOH.AccesoDatos.Enum;
using PoderJudicial.SIPOH.AccesoDatos.Interfaces;
using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Negocio.Enum;
using PoderJudicial.SIPOH.Negocio.Interfaces;

using System.Collections.Generic;

namespace PoderJudicial.SIPOH.Negocio
{
    public class InicialesProcessor : IInicialesProcessor
    {
        //Atributos publicos de proceso
        public string Mensaje { set; get; }

        //Atributos privados del proceso
        private Resultado resultado;
        private readonly ICatalogosRepository repositorioCatalogos;

        //Metodo Contructor del proceso, se le inyecta la interfaz CuentaRepositoru
        public InicialesProcessor(ICatalogosRepository repositorioCatalogos)
        {
            this.repositorioCatalogos = repositorioCatalogos;
        }

        public List<Distrito> RecuperaDistrito(int idCircuito)
        {
            List<Distrito> distritos = repositorioCatalogos.ObtenerDistritoPorCircuito(idCircuito, ref resultado);

            if (resultado != Resultado.OK) 
            {
                if (resultado == Resultado.SIN_RESULTADO)
                    Mensaje = "La consulta no genero ningun resultado";

                else if (resultado == Resultado.ERROR)
                {
                    Mensaje = "Ocurrio un error interno no controlado, consulte a soporte";
                    string mensajeLogger = repositorioCatalogos.MensajeError;
                    //Logica para ILogger
                }
            }
            return distritos;
        }

        public List<Juzgado> RecuperaJuzgado(int idFiltro, TipoJuzgado tipoJuzgado)
        {
            List<Juzgado> juzgados = new List<Juzgado>();

            if (tipoJuzgado == TipoJuzgado.ACUSATORIO)
            juzgados = repositorioCatalogos.ObtenerJuzgadosAcusatorio(idFiltro, ref resultado);
            
            if (tipoJuzgado == TipoJuzgado.TRADICIONAL) 
            juzgados = repositorioCatalogos.ObtenerJuzgadosTradicional(idFiltro, ref resultado);
              
            if (resultado != Resultado.OK)
            {
                if (resultado == Resultado.SIN_RESULTADO)
                    Mensaje = "La consulta no genero ningun resultado";

                else if (resultado == Resultado.ERROR)
                {
                    Mensaje = "Ocurrio un error interno no controlado, consulte a soporte";
                    string mensajeLogger = repositorioCatalogos.MensajeError;
                    //Logica para ILogger
                }
            }

            return juzgados;
        }
    }
}
