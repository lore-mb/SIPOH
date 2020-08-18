using System.Configuration;
using System.Data.SqlClient;

namespace PoderJudicial.SIPOH.AccesoDatos.Conexion
{
    public class ServerConnection
    {
        public SqlConnection SqlConnection = null;
        public bool IsValidConnection = false;
        private static ServerConnection Cnx = null;

        private ServerConnection()
        {
            string connectionString = GetServiceConfiguration();

            if (connectionString != null)
            {
                SqlConnection = new SqlConnection(connectionString);
                IsValidConnection = true;
            }
        }

        public static ServerConnection GetConnection()
        {
            if (Cnx == null)
                Cnx = new ServerConnection();

            return Cnx;
        }

        private string GetServiceConfiguration()
        {
            string nameConnection = ConfigurationManager.AppSettings["connectionName"];

            if (nameConnection == null)
                return null;

            if (IsValidConnectionString(nameConnection))
            {
                string connectionString = ConfigurationManager.ConnectionStrings[nameConnection].ConnectionString;
                return connectionString;
            }
            else
                return null;
        }

        private bool IsValidConnectionString(string connectionStringName)
        {
            int count = ConfigurationManager.ConnectionStrings.Count;
            bool valid = false;

            for (int a = 0; a < count; a++)
            {
                if (ConfigurationManager.ConnectionStrings[a].Name == connectionStringName)
                {
                    valid = true;
                    break;
                }
            }
            return valid;
        }
    }
}
