using System;

namespace PoderJudicial.SIPOH.Entidades
{
    public class Ejecucion
    {
        public int IdEjecucion { set; get; }
        public string NumeroEjecucion { set; get; }
        public DateTime FechaEjecucion { set; get; }
        public string Solicitante { set; get; }
        public string DetalleSolicitante { set; get; }
        public string Solicitud { set; get; }
        public string OtroSolicitante { set; get; }
        public string NombreBeneficiario { set; get; }
        public string ApellidoPBeneficiario { set; get; }
        public string ApellidoMBeneficiario { set; get; }
        public int IdJuzgado { set; get; }
        public string NombreJuzgado { set; get; }
        public string Interno { set; get; }
        public int IdUsuario { set; get; }
        public string Tipo { set; get; }
    }
}
