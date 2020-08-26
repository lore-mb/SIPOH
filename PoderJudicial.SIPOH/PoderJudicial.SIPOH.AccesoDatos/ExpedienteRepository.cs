using PoderJudicial.SIPOH.AccesoDatos.Conexion;
using PoderJudicial.SIPOH.AccesoDatos.Helpers;
using PoderJudicial.SIPOH.AccesoDatos.Interfaces;
using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PoderJudicial.SIPOH.AccesoDatos
{
    public class ExpedienteRepository : IExpedienteRepository
    {
        //Atributos Publicos del Repositorio
        public string MensajeError { set; get; }
        public Estatus Estatus { set; get; }

        //Atributos privados del Repositorio
        private SqlConnection Cnx;
        private bool IsValidConnection = false;

        //Metodo constructor del repositorio, se le inyecta la clase ServerConnection
        public ExpedienteRepository(ServerConnection connection)
        {
            Cnx = connection.SqlConnection;
            IsValidConnection = connection.IsValidConnection;
        }

        public List<Expediente> ObtenerExpedientes(int idJuzgado, string numeroExpediente, TipoExpediente expediente)
        {
            try
            {
                if (!IsValidConnection)
                    throw new Exception("No se ha creado una conexion valida");

                string storeProcedure = expediente == TipoExpediente.CAUSA ? "sipoh_ExpedientePorJuzgadoCausa" : "sipoh_ExpedientePorJuzgadoNuc";

                SqlCommand comando = new SqlCommand(storeProcedure, Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@idJuzgado", SqlDbType.Int).Value = idJuzgado;

                if (expediente == TipoExpediente.CAUSA)
                    comando.Parameters.Add("@numeroCausa", SqlDbType.VarChar).Value = numeroExpediente;

                if (expediente == TipoExpediente.NUC)
                    comando.Parameters.Add("@nuc", SqlDbType.VarChar).Value = numeroExpediente;

                Cnx.Open();

                SqlDataReader sqlRespuesta = comando.ExecuteReader();

                DataTable tabla = new DataTable();
                tabla.Load(sqlRespuesta);

                List<Expediente> expedientes = DataHelper.DataTableToList<Expediente>(tabla);

                if (expedientes.Count > 0)
                    Estatus = Estatus.OK;

                else
                    Estatus = Estatus.SIN_RESULTADO;

                return expedientes;
            }
            catch (Exception ex)
            {
                MensajeError = ex.Message;
                Estatus = Estatus.ERROR;
                return null;
            }
            finally
            {
                if (IsValidConnection && Cnx.State == ConnectionState.Open)
                    Cnx.Close();
            }
        }
    }
}
