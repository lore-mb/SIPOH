using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoderJudicial.SIPOH.Entidades.Enum
{
    public enum Estatus
    {
        SIN_RESULTADO = 0,
        OK = 1,
        ERROR = 2,
        DUPLICADO = 3,
        NO_DUPLICADO = 4,
        ACTIVO = 5,
        INACTIVO = 6
    }

    public enum TipoSistema
    {
        TRADICIONAL = 0,
        ACUSATORIO = 1
    }

    public enum TipoNumeroExpediente 
    {
       CAUSA = 0,
       NUC = 1,
       EJECUCION = 2
    }

    public enum Relacionadas 
    {
       CAUSAS = 0,
       TOCAS = 1,
       AMPAROS = 2,
       ANEXOS = 3,
       EJECUCION = 4
    }

    public enum ParteCausaBeneficiario 
    {
        BENEFICIARIO = 0,
        PARTE = 1
    }

    public enum Distribucion 
    {
       CIRCUITO = 0,
       DISTRITO = 1
    }

    public enum TipoJuzgado 
    {
       CABEZERA = 0,
       CONTROL = 1,
       EJECUCION = 2
    }

    public enum Instancia 
    { 
       INICIAL = 0,
       PROMOCION = 2
    }

    public enum TipoReporteDia
    { 
      INICIAL = 0,
      PROMOCION = 1
    }

    public enum TipoReporteRangoFecha
    { 
    INICIAL = 0,
    PROMOCION = 1
    }

}
