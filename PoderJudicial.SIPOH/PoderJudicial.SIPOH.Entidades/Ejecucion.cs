using iText.Layout.Element;
using System;
using System.Collections.Generic;

namespace PoderJudicial.SIPOH.Entidades
{
    public class Ejecucion : PartesExpediente
    {
        public int IdEjecucion { set; get; }
        public string NumeroEjecucion { set; get; }
        public string FechaEjecucion { set; get; }
        public int IdSolicitante { set; get; }
        public string DescripcionSolicitante { set; get; }
        public string DetalleSolicitante { set; get; }
        public int IdSolicitud { set; get; }
        public string DescripcionSolicitud { set; get; }
        public string OtraSolicita { set; get; }
        public string NombreBeneficiario { set; get; }
        public string ApellidoPBeneficiario { set; get; }
        public string ApellidoMBeneficiario { set; get; }
        public int IdJuzgado { set; get; }
        public string NombreJuzgado { set; get; }
        public string Interno { set; get; }
        public int IdUsuario { set; get; }
        public string Tipo { set; get; }

        //Campos para Tipos de Negocio
        public List<int> IdExpedientes { set; get; }
        public List<Expediente> CausasHistoricas { set; get; } 
        public List<Toca> Tocas { set; get; }
        public List<string> Amparos { set; get; }
        public List<Anexo> Anexos { set; get; }
    }
}
