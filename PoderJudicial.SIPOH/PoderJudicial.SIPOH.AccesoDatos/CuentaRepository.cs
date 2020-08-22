using PoderJudicial.SIPOH.AccesoDatos.Conexion;
using PoderJudicial.SIPOH.AccesoDatos.Enum;
using PoderJudicial.SIPOH.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace PoderJudicial.SIPOH.AccesoDatos.Interfaces
{
    public class CuentaRepository : ICuentaRepository
    {
        //Atributos Publicos del Repositorio
        public string MensajeError { set; get; }

        //Atributos privados del Repositorio
        private SqlConnection Cnx;
        private bool IsValidConnection = false;

        //Metodo constructor del repositorio, se le inyecta la clase ServerConnection
        public CuentaRepository(ServerConnection connection)
        {
            Cnx = connection.SqlConnection;
            IsValidConnection = connection.IsValidConnection;
        }

        #region Metodos Publicos del Repositorio
        public Usuario LogIn(string email, string password, ref Resultado resultado)
        {
            try
            {
                if (!IsValidConnection)
                throw new Exception("No se ha creado una conexion valida");

                SqlCommand comando = new SqlCommand("SP_SIPOH_LogIn", Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@usuario", SqlDbType.VarChar).Value = email;
                comando.Parameters.Add("@contrasenia", SqlDbType.VarChar).Value = password;

                Cnx.Open();

                SqlDataReader sqlRespuesta = comando.ExecuteReader();

                DataTable tabla = new DataTable();
                tabla.Load(sqlRespuesta);

                List<Usuario> usuarios = DataTableToList<Usuario>(tabla);
                if (usuarios.Count > 0)
                {
                    Usuario usuario = usuarios.FirstOrDefault();
                    usuario.Activo = true;
                    resultado = Resultado.OK;
                    return usuario;
                }

                return null;
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
        #endregion

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
