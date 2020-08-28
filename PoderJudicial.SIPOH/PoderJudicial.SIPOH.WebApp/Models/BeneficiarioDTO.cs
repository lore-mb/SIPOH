using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace PoderJudicial.SIPOH.WebApp.Models
{
    public class BeneficiarioDTO
    {
        public int IdEjecucion { set; get; }
        public string NumeroEjecucion { set; get; }
        public string NombreJuzgado { set; get; }
        public string NombreBeneficiario { set; get; }
        public string ApellidoPBeneficiario { set; get; }
        public string ApellidoMBeneficiario { set; get; }
        public string FechaEjecucion { set; get; }
    }
}