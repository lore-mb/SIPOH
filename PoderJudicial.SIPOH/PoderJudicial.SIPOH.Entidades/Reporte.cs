using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoderJudicial.SIPOH.Entidades
{
    public class Reporte : Ejecucion
    {
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public string NumeroCausa { set; get; }
        public string NUC { set; get; }
    }
}
