using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoderJudicial.SIPOH.WebApp.Models
{
    public class SelloModelView
    {
        public string JuzgadoEjecucion { set; get; }
        public string NumeroExpediente { set; get; }
        public int Folio { set; get; }
        public string Fecha { set; get; }

        public SelloModelView() 
        {
            Fecha = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
        }
    }
}