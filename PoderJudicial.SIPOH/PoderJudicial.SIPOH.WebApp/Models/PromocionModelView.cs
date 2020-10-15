using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoderJudicial.SIPOH.WebApp.Models
{
    public class PromocionModelView
    {
        public int IdEjecucion { get; set; }
        public string Promovente { get; set; }
        public int IdUsuario { get; set; }
        public int IdEjecucionPosterior { get; set; }

        public List<AnexosModelView> Anexos { set; get; }

        public PromocionModelView() {
          Anexos = new List<AnexosModelView>();
        }
    }
}