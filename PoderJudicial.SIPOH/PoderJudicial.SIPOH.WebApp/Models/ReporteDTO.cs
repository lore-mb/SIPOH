using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoderJudicial.SIPOH.WebApp.Models
{
    public class ReporteDTO
    {
        public string NumeroEjecucion { set; get; }
        public string DetalleSolicitante { set; get; }
        public string FechaEjecucion { set; get; }
        public string DescripcionSolicitud { set; get; }
        public string NombreBeneficiario { set; get; }
        public string ApellidoPBeneficiario { set; get; }
        public string ApellidoMBeneficiario { set; get; }
        public string NumeroCausa { set; get; }
        public string NUC { set; get; }
        public int Total { get; set; }
        public string Promovente { get; set; }

    }
}