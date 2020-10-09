using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoderJudicial.SIPOH.WebApp.Models
{
    public class PostEjecucionModelView
    {

        public int IdEjecucion { get; set; }
        public string Promovente { get; set; }
        public string FechaIngreso { get; set; }
        public int IdUser { get; set; }

        public List<PostEjecucionModelView> PostEjecucion { get; set; }

        public PostEjecucionModelView() {
            PostEjecucion = new List<PostEjecucionModelView>();
        }
    }
}