using PoderJudicial.SIPOH.Entidades.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoderJudicial.SIPOH.Entidades
{
    public class Expediente
    {
        public int IdExpediente { set; get; }
        public int IdJuzgado { set; get; }
        public string NombreJuzgado { set; get; }
        public string NumeroCausa { set; get; }
        public string NumeroDeToca { set; get; }
        public string NUC { set; get; }
        public string Inculpados { set; get; }
        public string Ofendidos { set; get; }
        public string Delitos { set; get; }
        public Estatus Estatus { get; set; }
    }
}
