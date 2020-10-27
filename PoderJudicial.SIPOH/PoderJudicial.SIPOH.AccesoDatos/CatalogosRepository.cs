using PoderJudicial.SIPOH.AccesoDatos.Conexion;
using PoderJudicial.SIPOH.AccesoDatos.Helpers;
using PoderJudicial.SIPOH.AccesoDatos.Interfaces;
using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;
using System;
using System.Collections.Generic;
using System.Configuration;
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

                SqlCommand comando = new SqlCommand("sipoh_ConsultarDistritosPorCircuito", Cnx);
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
        /// <param name="idCircuitoDistrito">Representa el Id del Circuito o Distrito al que pertenece el Juzgado</param>
        /// <param name="tipoJuzgado">Representa el tipo de Juzgado de retorno</param>
        /// <returns></returns>
        public List<Juzgado> ObtenerJuzgadosAcusatorioTradicional(int idCircuitoDistrito, TipoJuzgado tipoJuzgado)
        {
            try
            {
                if (!IsValidConnection)
                throw new Exception("No se ha creado una conexion valida");

                string storeProcedure = tipoJuzgado == TipoJuzgado.ACUSATORIO ? "sipoh_ConsultarJuzgadosPorCircuitoAcusatorio" : "sipoh_ConsultarJuzgadosPorDistritoTradicional";

                SqlCommand comando = new SqlCommand(storeProcedure, Cnx);
                comando.CommandType = CommandType.StoredProcedure;

                if(tipoJuzgado == TipoJuzgado.ACUSATORIO)
                comando.Parameters.Add("@idCircuito", SqlDbType.Int).Value = idCircuitoDistrito;

                if (tipoJuzgado == TipoJuzgado.TRADICIONAL)
                comando.Parameters.Add("@idDistrito", SqlDbType.Int).Value = idCircuitoDistrito;

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

                SqlCommand comando = new SqlCommand("sipoh_ConsultarSalasPorTipoSistema", Cnx);
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

                SqlCommand comando = new SqlCommand("sipoh_ConsultarAnexosPorEjecucion", Cnx);
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

        public List<Juzgado> ObtenerJuzgadoEjecucionPorCircuito(int idcircuito)
        {
            try
            {
                if (!IsValidConnection) throw new Exception("No se ha creado una conexión valida.");

                SqlCommand comando = new SqlCommand("sipoh_ConsultarJuzgadosEjecucionPorCircuito", Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@idcircuito", SqlDbType.Int).Value = idcircuito;
                Cnx.Open();

                SqlDataReader sqlRespuesta = comando.ExecuteReader();

                DataTable tabla = new DataTable();
                tabla.Load(sqlRespuesta);

                List<Juzgado> JuzgadoEjec = DataHelper.DataTableToList<Juzgado>(tabla);
                if (JuzgadoEjec.Count > 0)
                    Estatus = Estatus.OK;
                else Estatus = Estatus.SIN_RESULTADO;
                return JuzgadoEjec;

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

        public List<Solicitud> ObtenerSolicitudes()
        {
            try
            {
                if (!IsValidConnection)
                    throw new Exception("No se ha creado una conexion valida");

                string query = "SELECT * FROM P_Solicitud";

                SqlCommand comando = new SqlCommand(query, Cnx);
                Cnx.Open();

                SqlDataReader sqlRespuesta = comando.ExecuteReader();

                DataTable tabla = new DataTable();
                tabla.Load(sqlRespuesta);

                List<Solicitud> solicitud = DataHelper.DataTableToList<Solicitud>(tabla);

                if (solicitud.Count > 0)
                    Estatus = Estatus.OK;
                else
                    Estatus = Estatus.SIN_RESULTADO;

                return solicitud;
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

        public List<Solicitante> ObtenerSolicitantes()
        {
            try
            {
                if (!IsValidConnection)
                    throw new Exception("No se ha creado una conexion valida");

                string query = "SELECT * FROM P_Solicitante";

                SqlCommand comando = new SqlCommand(query, Cnx);
                Cnx.Open();

                SqlDataReader sqlRespuesta = comando.ExecuteReader();

                DataTable tabla = new DataTable();
                tabla.Load(sqlRespuesta);

                List<Solicitante> solicitante = DataHelper.DataTableToList<Solicitante>(tabla);

                if (solicitante.Count > 0)
                    Estatus = Estatus.OK;
                else
                    Estatus = Estatus.SIN_RESULTADO;

                return solicitante;
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

        public List<Toca> ObtenerTocasPorEjecucion(int idEjecucion)
        {
            try
            {
                if (!IsValidConnection) 
                    throw new Exception("No se ha creado una conexión valida.");

                SqlCommand comando = new SqlCommand("sipoh_ConsultarEjecucionOriTocaPorPorFolio", Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@folio", SqlDbType.Int).Value = idEjecucion;
                Cnx.Open();

                SqlDataReader sqlRespuesta = comando.ExecuteReader();

                DataTable tabla = new DataTable();
                tabla.Load(sqlRespuesta);

                List<Toca> tocas = DataHelper.DataTableToList<Toca>(tabla);

                if (tocas.Count > 0)
                    Estatus = Estatus.OK;
                else 
                    Estatus = Estatus.SIN_RESULTADO;

                return tocas;
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

        public List<string> ObtenerAmparosPorEjecucion(int idEjecucion)
        {
            try
            {
                if (!IsValidConnection)
                    throw new Exception("No se ha creado una conexion valida");

                string query = "SELECT Amparo FROM P_EjecucionOriAmpa WHERE IdEjecucion = @folio";

                SqlCommand comando = new SqlCommand(query, Cnx);
                comando.Parameters.Add("@folio", SqlDbType.Int).Value = idEjecucion;
                Cnx.Open();

                SqlDataReader sqlRespuesta = comando.ExecuteReader();

                DataTable tabla = new DataTable();
                tabla.Load(sqlRespuesta);

                List<string> amparos = ObtenerAmparos(tabla);

                if (amparos.Count > 0)
                    Estatus = Estatus.OK;
                else
                    Estatus = Estatus.SIN_RESULTADO;

                return amparos;
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

        public List<Anexo> ObtenerAnexosPorEjecucion(int idEjecucion)
        {
            try
            {
                if (!IsValidConnection) 
                    throw new Exception("No se ha creado una conexión valida.");

                SqlCommand comando = new SqlCommand("sipoh_ConsultarAnexosPorFolio", Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@folio", SqlDbType.Int).Value = idEjecucion;
                Cnx.Open();

                SqlDataReader sqlRespuesta = comando.ExecuteReader();

                DataTable tabla = new DataTable();
                tabla.Load(sqlRespuesta);

                List<Anexo> anexos = DataHelper.DataTableToList<Anexo>(tabla);

                if (anexos.Count > 0)
                    Estatus = Estatus.OK;
                else 
                    Estatus = Estatus.SIN_RESULTADO;
                
                return anexos;
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

        public List<Juzgado> ObtenerJuzgadosPorDistrito(int idDistrito)
        {
            try
            {
                if (!IsValidConnection)
                    throw new Exception("No se ha creado una conexion valida");

                SqlCommand comando = new SqlCommand("sipoh_ConsultarJuzgadosPorDistritos", Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@idDistrito", SqlDbType.Int).Value = idDistrito;
                Cnx.Open();

                SqlDataReader sqlRespuesta = comando.ExecuteReader();

                DataTable tabla = new DataTable();
                tabla.Load(sqlRespuesta);

                List<Juzgado> jusgados = DataHelper.DataTableToList<Juzgado>(tabla);

                if (jusgados.Count > 0)
                    Estatus = Estatus.OK;
                else
                    Estatus = Estatus.SIN_RESULTADO;

                return jusgados;
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
        public List<string> ObtenerAmparos(DataTable dataTableAmparos)
        {
            List<string> amparos = new List<string>();

            for (int i = 0; i < dataTableAmparos.Rows.Count; i++)
            {
                string amparo = dataTableAmparos.Rows[i]["Amparo"].ToString();
                amparos.Add(amparo);
            }

            return amparos;
        }
        #endregion
    }
}
