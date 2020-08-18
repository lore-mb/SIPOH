using PoderJudicial.SIPOH.AccesoDatos.Interfaces;
using PoderJudicial.SIPOH.Negocio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoderJudicial.SIPOH.Negocio
{
    public class PruebaProcessor : IPruebaProcessor
    {
        private readonly IPrueba prueba;
        public PruebaProcessor(IPrueba prueba) 
        {
            this.prueba = prueba;
        }

        public void PruebaProcessorInterface()
        {
            var p = "esto es una prueba";
            prueba.PruebaInterfazNinjec();
        }
    }
}
