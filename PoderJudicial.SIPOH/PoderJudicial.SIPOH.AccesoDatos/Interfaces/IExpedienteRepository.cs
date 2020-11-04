using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoderJudicial.SIPOH.AccesoDatos.Interfaces
{
    public interface IExpedienteRepository
    {
        string MensajeError { get; set; }
        Estatus Estatus { get; set; }
        Expediente ConsultaExpediente(int idJuzgado, string causaNuc, TipoNumeroExpediente expediente);
        List<Expediente> ConsultaExpedientes(int idEjecucion);

        //Convencion para Totales
        void ConsultaTotalExpedientes(int idJuzgado, string numeroDeCausa);
        void ConsultaTotalExpedientes(int idJuzgado, string numeroDeCausa, string nuc);
    }
}
