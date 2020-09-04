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
    public class CatalogosRepository : ICatalogosRepository
    {
        //Atributos Publicos del Repositorio
        public string MensajeError { set; get; }
        public Estatus Estatus { get; set; }

        //Atributos privados del Repositorio
        private SqlConnection Cnx;
        private bool IsValidConnection = false;

        /// <summary>
        /// Metodo contructor del repositorio Catalgos
        /// </summary>
        /// <param name="connection">Conexion al servidor SQL</param>
        public CatalogosRepository(ServerConnection connection) 
        {
            Cnx = connection.SqlConnection;
            IsValidConnection = connection.IsValidConnection;
        }

        /// <summary>
        /// Retorna lista de distritos por medio del cicuito
        /// </summary>
        /// <param name="idCircuito">Id del circuito al que pertenece los distritos</param>
        /// <returns></returns>
        public List<Distrito> ObtenerDistritos(int idCircuito)
        {
            try
            {
                if (!IsValidConnection)
                throw new Exception("No se ha creado una conexion valida");

                SqlCommand comando = new SqlCommand("sipoh_DistritosPorCircuito", Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@idCircuito", SqlDbType.Int).Value = idCircuito;
                Cnx.Open();

                SqlDataReader sqlRespuesta = comando.ExecuteReader();

                DataTable tabla = new DataTable();
                tabla.Load(sqlRespuesta);

                List<Distrito> distritos = DataHelper.DataTableToList<Distrito>(tabla);
                
                if (distritos.Count > 0)
                Estatus = Estatus.OK;
                else
                Estatus = Estatus.SIN_RESULTADO;

                return distritos;
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

        /// <summary>
        /// Retorna lista de Juzgados filtrados por tipo y distrito o circuito
        /// </summary>
        /// <param name="id">Representa el Id del Circuito o Distrito al que pertenece el Juzgado</param>
        /// <param name="tipoJuzgado">Representa el tipo de Juzgado de retorno</param>
        /// <returns></returns>
        public List<Juzgado> ObtenerJuzgados(int id, TipoJuzgado tipoJuzgado)
        {
            try
            {
                if (!IsValidConnection)
                throw new Exception("No se ha creado una conexion valida");

                string storeProcedure = tipoJuzgado == TipoJuzgado.ACUSATORIO ? "sipoh_JuzgadosPorCircuitoAcusatorio" : "sipoh_JuzgadosPorDistritoTradicional";

                SqlCommand comando = new SqlCommand(storeProcedure, Cnx);
                comando.CommandType = CommandType.StoredProcedure;

                if(tipoJuzgado == TipoJuzgado.ACUSATORIO)
                comando.Parameters.Add("@idCircuito", SqlDbType.Int).Value = id;

                if (tipoJuzgado == TipoJuzgado.TRADICIONAL)
                comando.Parameters.Add("@idDistrito", SqlDbType.Int).Value = id;

                Cnx.Open();

                SqlDataReader sqlRespuesta = comando.ExecuteReader();

                DataTable tabla = new DataTable();
                tabla.Load(sqlRespuesta);

                List<Juzgado> juzgados = DataHelper.DataTableToList<Juzgado>(tabla);

                if (juzgados.Count > 0)
                    Estatus = Estatus.OK;
                else
                    Estatus = Estatus.SIN_RESULTADO;

                return juzgados;
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

        /// <summary>
        /// Metodo que retorna los salas(Juzgados) por tipo tradicional o acusatorio
        /// </summary>
        /// <param name="tipoJuzgado">Filtro para obtener salas de tipo tradicional o acusatorios</param>
        /// <returns></returns>
        public List<Juzgado> ObtenerSalas(TipoJuzgado tipoJuzgado)
        {
            try
            {
                if (!IsValidConnection)
                throw new Exception("No se ha creado una conexion valida");

                string tipoSistema = tipoJuzgado == TipoJuzgado.ACUSATORIO ? "SA" : "ST";

                SqlCommand comando = new SqlCommand("sipoh_SalasPorTipoSistema", Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@tipoSistema", SqlDbType.VarChar).Value = tipoSistema;

                Cnx.Open();

                SqlDataReader sqlRespuesta = comando.ExecuteReader();

                DataTable tabla = new DataTable();
                tabla.Load(sqlRespuesta);

                List<Juzgado> juzgados = DataHelper.DataTableToList<Juzgado>(tabla);

                if (juzgados.Count > 0)
                   Estatus = Estatus.OK;
                else
                   Estatus = Estatus.SIN_RESULTADO;

                return juzgados;
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

        public List<Anexo> ObtenerAnexosEjecucion(string tipo)
        {
            try
            {
                if (!IsValidConnection)
                    throw new Exception("No se ha creado una conexion valida");

                SqlCommand comando = new SqlCommand("sipoh_ConsultarAnexosEjecucion", Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@tipo", SqlDbType.VarChar).Value = tipo;
                Cnx.Open();

                SqlDataReader sqlRespuesta = comando.ExecuteReader();

                DataTable tabla = new DataTable();
                tabla.Load(sqlRespuesta);

                List<Anexo> distritos = DataHelper.DataTableToList<Anexo>(tabla);

                if (distritos.Count > 0)
                    Estatus = Estatus.OK;
                else
                    Estatus = Estatus.SIN_RESULTADO;

                return distritos;
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
        #region Metodos Privados de la Clase
        #endregion
    }
}
