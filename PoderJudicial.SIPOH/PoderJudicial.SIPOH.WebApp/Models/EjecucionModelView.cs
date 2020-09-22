using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoderJudicial.SIPOH.WebApp.Models
{
    public class EjecucionModelView
    {
        public int IdEjecucion { set; get; }
        public string IdJuzgado { set; get; }
        public string NumeroEjecucion { set; get; }
        public string FechaEjecucion { set; get; }
        public string NombreBeneficiario { set; get; }
        public string ApellidoPBeneficiario { set; get; }
        public string ApellidoMBeneficiario { set; get; }
        public int IdSolicitante { set; get; }
        public string Solicitante { set; get; }
        public string DetalleSolicitante { set; get; }
        public int IdSolicitud { set; get; }
        public string Solicitud { set; get; }
        public string OtraSolicita { set; get; }
        public string Interno { set; get; }
        public List<CausasModelView> Causas { set; get; }
        public List<TocasModelView> Tocas { set; get; }
        public List<string> Amparos { set; get; }
        public List<AnexosModelView> Anexos {set; get;}
    }
}