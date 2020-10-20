using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoderJudicial.SIPOH.WebApp.Models
{
    public class ExpedienteDTO
    {
        public int IdExpediente { set; get; }
        public string NombreJuzgado { set; get; }
        public string NumeroCausa { set; get; }
        public string NUC { set; get; }
        public string Ofendidos { set; get; }
        public string Inculpados { set; get; }
        public string Delitos { set; get; }

        public string CausaNuc 
        {
            get 
            {
                return ConcatenaCausaNuc(NumeroCausa, NUC);
            }
        }

        private string ConcatenaCausaNuc(string causa, string nuc) 
        {
            nuc = nuc != string.Empty ? "<br>" + nuc : string.Empty;
            return string.Format("{0}{1}", causa, nuc);
        }
    }
}