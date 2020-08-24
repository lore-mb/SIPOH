using PoderJudicial.SIPOH.AccesoDatos.Conexion;
using PoderJudicial.SIPOH.AccesoDatos.Enum;
using PoderJudicial.SIPOH.AccesoDatos.Interfaces;
using PoderJudicial.SIPOH.Entidades;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;


namespace PoderJudicial.SIPOH.AccesoDatos
{
    public class CatalogosRepository : ICatalogosRepository
    {
        //Atributos Publicos del Repositorio
        public string MensajeError { set; get; }

        //Atributos privados del Repositorio
        private SqlConnection Cnx;
        private bool IsValidConnection = false;

        //Metodo constructor del repositorio, se le inyecta la clase ServerConnection
        public CatalogosRepository(ServerConnection connection) 
        {
            Cnx = connection.SqlConnection;
            IsValidConnection = connection.IsValidConnection;
        }

        public List<Distrito> ObtenerDistritoPorCircuito(int idCircuito, ref Resultado resultado)
        {
            try
            {
                if (!IsValidConnection)
                throw new Exception("No se ha creado una conexion valida");

                SqlCommand comando = new SqlCommand("SP_SIPOH_DistritosPorCircuito", Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@idCircuito", SqlDbType.Int).Value = idCircuito;
                Cnx.Open();

                SqlDataReader sqlRespuesta = comando.ExecuteReader();

                DataTable tabla = new DataTable();
                tabla.Load(sqlRespuesta);

                List<Distrito> distritos = DataTableToList<Distrito>(tabla);
                
                if (distritos.Count > 0)
                resultado = Resultado.OK;
                else
                resultado = Resultado.SIN_RESULTADO;

                return distritos;
            }
            catch (Exception ex)
            {
                MensajeError = ex.Message;
                resultado = Resultado.ERROR;
                return null;
            }
            finally
            {
                if (IsValidConnection && Cnx.State == ConnectionState.Open)
                    Cnx.Close();
            }
        }

        public List<Juzgado> ObtenerJuzgadosAcusatorio(int idCircuito, ref Resultado resultado)
        {
            try
            {
                if (!IsValidConnection)
                throw new Exception("No se ha creado una conexion valida");

                SqlCommand comando = new SqlCommand("SP_SIPOH_JuzgadosPorCircuitoAcusatorio", Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@idCircuito", SqlDbType.Int).Value = idCircuito;
                Cnx.Open();

                SqlDataReader sqlRespuesta = comando.ExecuteReader();

                DataTable tabla = new DataTable();
                tabla.Load(sqlRespuesta);

                List<Juzgado> juzgados = DataTableToList<Juzgado>(tabla);

                if (juzgados.Count > 0)
                    resultado = Resultado.OK;
                else
                    resultado = Resultado.SIN_RESULTADO;

                return juzgados;
            }
            catch (Exception ex)
            {
                MensajeError = ex.Message;
                resultado = Resultado.ERROR;
                return null;
            }
            finally
            {
                if (IsValidConnection && Cnx.State == ConnectionState.Open)
                    Cnx.Close();
            }
        }

        public List<Juzgado> ObtenerJuzgadosTradicional(int idDistrito, ref Resultado resultado)
        {
            try
            {
                if (!IsValidConnection)
                    throw new Exception("No se ha creado una conexion valida");

                SqlCommand comando = new SqlCommand("SP_SIPOH_JuzgadosPorDistritoTradicional", Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@idDistrito", SqlDbType.Int).Value = idDistrito;
                Cnx.Open();

                SqlDataReader sqlRespuesta = comando.ExecuteReader();

                DataTable tabla = new DataTable();
                tabla.Load(sqlRespuesta);

                List<Juzgado> juzgados = DataTableToList<Juzgado>(tabla);

                if (juzgados.Count > 0)
                resultado = Resultado.OK;
                else
                resultado = Resultado.SIN_RESULTADO;

                return juzgados;
            }
            catch (Exception ex)
            {
                MensajeError = ex.Message;
                resultado = Resultado.ERROR;
                return null;
            }
            finally
            {
                if (IsValidConnection && Cnx.State == ConnectionState.Open)
                    Cnx.Close();
            }
        }

        #region Metodos Privados de la Clase

        private List<T> DataTableToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();
            var properties = typeof(T).GetProperties();

            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name))
                    {
                        PropertyInfo pI = objT.GetType().GetProperty(pro.Name);
                        pro.SetValue(objT, row[pro.Name] == DBNull.Value ? null : Convert.ChangeType(row[pro.Name], pI.PropertyType));
                    }
                }
                return objT;
            }).ToList();
        }
        #endregion

    }
}
