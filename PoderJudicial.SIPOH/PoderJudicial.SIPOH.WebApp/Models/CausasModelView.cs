using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoderJudicial.SIPOH.WebApp.Models
{
    public class CausasModelView
    {
        public int IdCausa { set; get; }
        public string NombreJuzgado { set; get; }
        public string CausaNuc { set; get; }
        public string Ofendidos { set; get; }
        public string Inculpados { set; get; }
        public string Delitos { set; get; }
    }
}