using PoderJudicial.SIPOH.AccesoDatos.Conexion;
using PoderJudicial.SIPOH.AccesoDatos.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoderJudicial.SIPOH.AccesoDatos
{
    public class Prueba : IPrueba
    {
        public string MensajeError { set; get; }
        private SqlConnection cnx;
        private bool IsValidConnection = false;

        public Prueba (ServerConnection connection)
        {
            cnx = connection.SqlConnection;
            IsValidConnection = connection.IsValidConnection;
        }

        public void PruebaInterfazNinjec()
        {
            var mensaje = "Entro";
        }
    }
}
