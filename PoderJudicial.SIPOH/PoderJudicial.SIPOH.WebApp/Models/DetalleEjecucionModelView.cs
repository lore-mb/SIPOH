using PoderJudicial.SIPOH.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoderJudicial.SIPOH.WebApp.Models
{
    public class DetalleEjecucionModelView
    {
        public int IdEjecucion { set; get; }
        public int IdJuzgado { set; get; }
        public string NombreJuzgado { set; get; }
        public string NumeroEjecucion { set; get; }
        public string FechaEjecucion { set; get; }
        public string NombreBeneficiario { set; get; }
        public string ApellidoPBeneficiario { set; get; }
        public string ApellidoMBeneficiario { set; get; }
        public int IdSolicitante { set; get; }
        public string DescripcionSolicitante { set; get; }
        public string DetalleSolicitante { set; get; }
        public int IdSolicitud { set; get; }
        public string DescripcionSolicitud { set; get; }
        public string OtraSolicita { set; get; }
        public string Interno { set; get; }

        //Entidades Relacionadas
        public List<CausasModelView> IdExpedientes { set; get; }
        public List<Toca> Tocas { set; get; }
        public List<string> Amparos { set; get; }
        public List<AnexosModelView> Anexos {set; get;}
        public DetalleEjecucionModelView() 
        {
            IdExpedientes = new List<CausasModelView>();
            Tocas = new List<Toca>();
            Amparos = new List<string>();
            Anexos = new List<AnexosModelView>();
        }
    }
}