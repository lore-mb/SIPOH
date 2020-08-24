using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoderJudicial.SIPOH.WebApp.Helpers
{
    public enum EstatusRespuestaJSON { SIN_RESPUESTA, OK, ERROR}
    public class RespuestaJson
    {
        public EstatusRespuestaJSON Estatus { get; set; }
        public string Mensaje { get; set; }
        public string VistaRender { get; set; }
        public object Data { get; set; }
    }
}