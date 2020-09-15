using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoderJudicial.SIPOH.WebApp.Models
{
    public class DetalleEjecucionModelView
    {
        public int Folio { set; get; }
        public string JuzgadoEjecucion { set; get; }
        public string NumeroExpediente { set; get; }
        public string Fecha { set; get; }
        public string NombreBeneficiario { set; get; }
        public string ApellidoPaternoBeneficiario { set; get; }
        public string ApellidoMaternoBeneficiario { set; get; }
        public bool SentenciadoInterno { set; get; }
        public List<CausasModelView> Causas { set; get; }
        public List<TocasModelView> Tocas { set; get; }
        public List<string> Amparos { set; get; }
        public List<AnexosModelView> Anexos {set; get;}
    }
}