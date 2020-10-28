using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoderJudicial.SIPOH.WebApp.Models
{
    public class PromocionModelView : DetalleEjecucionModelView
    {
        public string Promovente { get; set; }
        public string FechaIngreso { get; set; }
        public int IdUsuario { get; set; }
        public int IdEjecucionPosterior { get; set; }

        public PromocionModelView() {
          Anexos = new List<AnexosModelView>();
        }
    }
}