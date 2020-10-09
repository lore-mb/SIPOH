using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoderJudicial.SIPOH.WebApp.Models
{
    public class AnexosModelView
    {
        public int IdEjecucionPosterior { get; set; }
        public int IdAnexo { set; get; }
        public int Cantidad { set; get; }
        public string Descripcion { set; get; }
    }
}