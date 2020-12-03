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
        //Base de Datos
        public int IdExpediente { set; get; }
        public int IdJuzgado { set; get; }
        public string NumeroExpediente { set; get; }
        public string FechaIngreso { set; get; }

        //Datos Para Logica de Negocio
        public string Inculpados { set; get; }
        public string Ofendidos { set; get; }
        public string Delitos { set; get; }
        public string NombreJuzgado { set; get; }
        public string NumeroCausa { set; get; }
        public string NUC { set; get; }

        //Campos para Tipos de Negocio
        public List<int> IdDelitos { set; get; }
        public List<PartesExpediente> Partes { set; get; }
        
        //Campos a Revisar
        public string NumeroDeToca { set; get; }
        public int IdExpedienteNuc { set; get; }
    }
}
