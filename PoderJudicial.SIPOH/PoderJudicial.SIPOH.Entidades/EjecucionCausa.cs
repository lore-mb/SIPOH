using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoderJudicial.SIPOH.Entidades
{
    public class EjecucionCausa : Ejecucion
    {
        #region Propiedades Tabla Causa
        public string NumeroCausa { set; get; }
        public string NUC { set; get; }
        #endregion

        #region Propiedades Tabla EjecucionPosterior
        public string Promovente { get; set; }
        #endregion

        public int Total { get; set; }
    }
}
