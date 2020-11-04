using PoderJudicial.SIPOH.AccesoDatos.Conexion;
using PoderJudicial.SIPOH.AccesoDatos.Helpers;
using PoderJudicial.SIPOH.AccesoDatos.Interfaces;
using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

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

        public Expediente ConsultaExpediente(int idJuzgado, string numeroExpediente, TipoNumeroExpediente expediente)
        {
            try
            {
                if (!IsValidConnection)
                    throw new Exception("No se ha creado una conexion valida");

                string storeProcedure = expediente == TipoNumeroExpediente.CAUSA ? "sipoh_ConsultarExpedientePorJuzgadoCausa" : "sipoh_ConsultarExpedientePorJuzgadoNuc";

                SqlCommand comando = new SqlCommand(storeProcedure, Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@idJuzgado", SqlDbType.Int).Value = idJuzgado;

                if (expediente == TipoNumeroExpediente.CAUSA)
                    comando.Parameters.Add("@numeroCausa", SqlDbType.VarChar).Value = numeroExpediente;

                if (expediente == TipoNumeroExpediente.NUC)
                    comando.Parameters.Add("@nuc", SqlDbType.VarChar).Value = numeroExpediente;

                Cnx.Open();

                SqlDataReader sqlRespuesta = comando.ExecuteReader();

                DataTable tabla = new DataTable();
                tabla.Load(sqlRespuesta);

                List<Expediente> expedientes = DataHelper.DataTableToList<Expediente>(tabla);

                if (expedientes.Count > 0)
                { 
                    Expediente causa = expedientes.FirstOrDefault();
                    Estatus = Estatus.OK;
                    return causa;
                }   
                else
                    Estatus = Estatus.SIN_RESULTADO;
                    
                return new Expediente();
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
      
        public List<Expediente> ConsultaExpedientes(int idEjecucion)
        {
            try
            {
                if (!IsValidConnection)
                    throw new Exception("No se ha creado una conexion valida");

                SqlCommand comando = new SqlCommand("sipoh_ConsultarExpedientePorFolio", Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@folio", SqlDbType.Int).Value = idEjecucion;
                Cnx.Open();

                SqlDataReader sqldataReader = comando.ExecuteReader();

                DataTable tabladata = new DataTable();
                tabladata.Load(sqldataReader);

                List<Expediente> expedientes = DataHelper.DataTableToList<Expediente>(tabladata);

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

        public void ConsultaTotalExpedientes(int idJuzgado, string numeroDeCausa)
        {
            try
            {
                if (!IsValidConnection)
                    throw new Exception("No se ha creado una conexion valida");

                SqlCommand comando = new SqlCommand("sipoh_ConsultarTotalExpedientesPorJuzgadoNumeroCausa", Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@idJuzgado", SqlDbType.Int).Value = idJuzgado;
                comando.Parameters.Add("@numeroCausa", SqlDbType.VarChar).Value = numeroDeCausa;

                //Parametro de Salida
                SqlParameter totalExpediente = new SqlParameter();
                totalExpediente.ParameterName = "@total";
                totalExpediente.SqlDbType = SqlDbType.Int;
                totalExpediente.Direction = ParameterDirection.Output;
                comando.Parameters.Add(totalExpediente);

                Cnx.Open();

                if (Convert.ToInt32(totalExpediente.Value) > 0)
                    Estatus = Estatus.OK;
                else
                    Estatus = Estatus.SIN_RESULTADO;
            }
            catch (Exception ex)
            {
                MensajeError = ex.Message;
                Estatus = Estatus.ERROR;
            }
            finally
            {
                if (IsValidConnection && Cnx.State == ConnectionState.Open)
                    Cnx.Close();
            }
        }

        public void ConsultaTotalExpedientes(int idJuzgado, string numeroDeCausa, string nuc)
        {
            try
            {
                if (!IsValidConnection)
                    throw new Exception("No se ha creado una conexion valida");

                SqlCommand comando = new SqlCommand("sipoh_ConsultarTotalExpedientesPorJuzgadoNUC", Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@idJuzgado", SqlDbType.Int).Value = idJuzgado;
                comando.Parameters.Add("@numeroCausa", SqlDbType.VarChar).Value = numeroDeCausa;
                comando.Parameters.Add("@nuc", SqlDbType.VarChar).Value = nuc;

                //Parametro de Salida
                SqlParameter totalExpediente = new SqlParameter();
                totalExpediente.ParameterName = "@total";
                totalExpediente.SqlDbType = SqlDbType.Int;
                totalExpediente.Direction = ParameterDirection.Output;
                comando.Parameters.Add(totalExpediente);

                Cnx.Open();

                if (Convert.ToInt32(totalExpediente.Value) > 0)
                    Estatus = Estatus.OK;
                else
                    Estatus = Estatus.SIN_RESULTADO;
            }
            catch (Exception ex)
            {
                MensajeError = ex.Message;
                Estatus = Estatus.ERROR;
            }
            finally
            {
                if (IsValidConnection && Cnx.State == ConnectionState.Open)
                    Cnx.Close();
            }
        }
    }
}
