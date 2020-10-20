using Microsoft.Owin.Security.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoderJudicial.SIPOH.WebApp.Models
{
    public class EjecucionDTO
    {
        public int IdEjecucion { set; get; }
        public string NumeroEjecucion { set; get; }
        public string NombreJuzgado { set; get; }
        public string DetalleSolicitante { set; get; }
        public string FechaEjecucion { set; get; }
        public string DescripcionSolicitud { set; get; }
        public string NombreBeneficiario { set; get; }
        public string ApellidoPBeneficiario { set; get; }
        public string ApellidoMBeneficiario { set; get; }
        public string Beneficiario
        {
            get
            {
                return ConcatenaNombre(NombreBeneficiario, ApellidoPBeneficiario, ApellidoMBeneficiario);
            }
        }

        public string Tipo { set; get; }

        //Partes relacionadas a la Causa de Ejecucion
        public string NombreParte { get; set; }
        public string ApellidoPParte { get; set; }
        public string ApellidoMParte { get; set; }
        public string ParteRelacioanada 
        {
            get
            {
                return ConcatenaNombre(NombreParte, ApellidoPParte, ApellidoMParte);
            }
        }
        public string TipoParte { get; set; }

        private string ConcatenaNombre(string nombre, string paterno, string materno)
        {
            paterno = paterno != string.Empty ? " " + paterno : string.Empty;
            materno = materno != string.Empty ? " " + materno : string.Empty;
            return string.Format("{0}{1}{2}", nombre, paterno, materno);
        }
    }
}