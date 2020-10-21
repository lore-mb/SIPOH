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
using System.Linq.Expressions;

namespace PoderJudicial.SIPOH.AccesoDatos
{
    public class EjecucionRepository : IEjecucionRepository
    {
        //Atributos Publicos del Repositorio
        public string MensajeError { set; get; }
        public Estatus Estatus { set; get; }

        //Atributos privados del Repositorio
        private SqlConnection Cnx;
        private bool IsValidConnection = false;

        //Metodo constructor del repositorio, se le inyecta la clase ServerConnection
        public EjecucionRepository(ServerConnection connection)
        {
            Cnx = connection.SqlConnection;
            IsValidConnection = connection.IsValidConnection;
        }

        #region Metodos Publicos
        /// <summary>
        /// Metodo que retorna una lista de sentenciados beneficiarios por medio del nombre
        /// </summary>
        /// <param name="nombre">Nombre de la Persona Beneficiario</param>
        /// <param name="apellidoPaterno">Apellido Paterno de la persona beneficiaria</param>
        /// <param name="apellidoMaterno">Apeliido Materno de la personas beneficiaria</param>
        /// <returns></returns>
        public List<Ejecucion> ObtenerSentenciadoBeneficiario(string nombre, string apellidoPaterno, string apellidoMaterno, int idCircuito)
        {
            try
            {
                if (!IsValidConnection)
                    throw new Exception("No se ha creado una conexion valida");

                SqlCommand comando = new SqlCommand("sipoh_ConsultarEjecucionPorSentenciadoBeneficiario", Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@nombre", SqlDbType.VarChar).Value = nombre;
                comando.Parameters.Add("@apellidoPaterno", SqlDbType.VarChar).Value = apellidoPaterno;
                comando.Parameters.Add("@apellidoMaterno", SqlDbType.VarChar).Value = apellidoMaterno;
                comando.Parameters.Add("@idCircuito", SqlDbType.Int).Value = idCircuito;

                Cnx.Open();

                SqlDataReader sqlRespuesta = comando.ExecuteReader();

                DataTable tabla = new DataTable();
                tabla.Load(sqlRespuesta);

                List<Ejecucion> beneficiaios = DataHelper.DataTableToList<Ejecucion>(tabla);

                if (beneficiaios.Count > 0)
                    Estatus = Estatus.OK;
                else
                    Estatus = Estatus.SIN_RESULTADO;

                return beneficiaios;
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
        /// Crea un registro de ejecucion
        /// </summary>
        /// <param name="ejecucion">Parametro de tipo Ejecucion</param>
        /// <param name="circuitoPachuca">Parametro booleano para determinar si se uusara la validacion de asignacion de juzgados de ejecucion CTO PACHUCA</param>
        /// <param name="idJuzgado">Id del juzgado a asignar</param>
        /// <returns></returns>
        public int? CrearEjecucion(Ejecucion ejecucion, List<int> causas, List<Expediente> tocas, List<string> amparos, List<Anexo> anexos, int? idJuzgado, bool circuitoPachuca)
        {
            try
            {
                if (!IsValidConnection)
                    throw new Exception("No se ha creado una conexion valida");

                SqlCommand comando = new SqlCommand("sipoh_CrearEjecucion", Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@idSolicitante", SqlDbType.VarChar).Value = ejecucion.IdSolicitante;
                comando.Parameters.Add("@detalleSolicitante", SqlDbType.VarChar).Value = ejecucion.DetalleSolicitante;
                comando.Parameters.Add("@idSolicitud", SqlDbType.VarChar).Value = ejecucion.IdSolicitud;
                comando.Parameters.Add("@otraSolicita", SqlDbType.VarChar).Value = ejecucion.OtraSolicita;
                comando.Parameters.Add("@beneficiarioNombre", SqlDbType.VarChar).Value = ejecucion.NombreBeneficiario;
                comando.Parameters.Add("@beneficiarioApellidoPaterno", SqlDbType.VarChar).Value = ejecucion.ApellidoPBeneficiario;
                comando.Parameters.Add("@beneficiarioApellidoMaterno", SqlDbType.VarChar).Value = ejecucion.ApellidoMBeneficiario;
                comando.Parameters.Add("@interno", SqlDbType.Char).Value = ejecucion.Interno;
                comando.Parameters.Add("@idUser", SqlDbType.Int).Value = ejecucion.IdUsuario;
                comando.Parameters.Add("@idUnidad", SqlDbType.Int).Value = idJuzgado;
                comando.Parameters.Add("@esCircuito", SqlDbType.Bit).Value = circuitoPachuca;

                SqlParameter idEjecucion = new SqlParameter();
                idEjecucion.ParameterName = "@idEjecucion";
                idEjecucion.SqlDbType = SqlDbType.Int;
                idEjecucion.Direction = ParameterDirection.Output;
                comando.Parameters.Add(idEjecucion);

                SqlParameter causasType = new SqlParameter();
                causasType.ParameterName = "@expedientes";
                causasType.SqlDbType = SqlDbType.Structured;
                causasType.Value = CreaCausasType(causas);
                comando.Parameters.Add(causasType);

                SqlParameter tocasType = new SqlParameter();
                tocasType.ParameterName = "@tocas";
                tocasType.SqlDbType = SqlDbType.Structured;
                tocasType.Value = CreaTocasType(tocas);
                comando.Parameters.Add(tocasType);

                SqlParameter amparosType = new SqlParameter();
                amparosType.ParameterName = "@amparos";
                amparosType.SqlDbType = SqlDbType.Structured;
                amparosType.Value = CreaAmparosType(amparos);
                comando.Parameters.Add(amparosType);

                SqlParameter anexosType = new SqlParameter();
                anexosType.ParameterName = "@anexos";
                anexosType.SqlDbType = SqlDbType.Structured;
                anexosType.Value = CreaAnexoType(anexos);
                comando.Parameters.Add(anexosType);

                Cnx.Open();
                comando.ExecuteNonQuery();

                Estatus = Estatus.OK;
                return Convert.ToInt32(idEjecucion.Value);
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

        public List<Ejecucion> ObtenerEjecucionPorJuzgado(int IdJuzgado, string NumeroEjecucion)
        {
            try
            {
                if (!IsValidConnection)
                    throw new Exception("Conexión no valida");

                SqlCommand comando = new SqlCommand("sipoh_ConsultarEjecucionPorJuzgado", Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@Juzgado", SqlDbType.Int).Value = IdJuzgado;
                comando.Parameters.Add("@NoEjecucion", SqlDbType.VarChar).Value = NumeroEjecucion;
                Cnx.Open();

                SqlDataReader sqldataReader = comando.ExecuteReader();

                DataTable tabladata = new DataTable();
                tabladata.Load(sqldataReader);

                List<Ejecucion> EjecucionJuzgado = DataHelper.DataTableToList<Ejecucion>(tabladata);

                if (EjecucionJuzgado.Count > 0)
                    Estatus = Estatus.OK;
                else
                    Estatus = Estatus.SIN_RESULTADO;

                return EjecucionJuzgado;

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

        public Ejecucion ObtenerEjecucionPorFolio(int folio)
        {
            try
            {
                if (!IsValidConnection)
                    throw new Exception("No se ha creado una conexion valida");

                SqlCommand comando = new SqlCommand("sipoh_ConsultarEjecucionPorFolio", Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@folio", SqlDbType.Int).Value = folio;
                Cnx.Open();

                SqlDataReader sqldataReader = comando.ExecuteReader();

                DataTable tabladata = new DataTable();
                tabladata.Load(sqldataReader);

                Ejecucion ejecucion = DataHelper.DataTableToList<Ejecucion>(tabladata).FirstOrDefault();

                if (ejecucion != null)
                {
                    Estatus = Estatus.OK;
                    return ejecucion;
                }

                Estatus = Estatus.SIN_RESULTADO;
                return null;
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

        public Ejecucion ObtenerEjecucionPromocionPorFolio(int folio)
        {
            try
            {
                if (!IsValidConnection)
                    throw new Exception("No se ha creado una conexion valida");

                SqlCommand comando = new SqlCommand("sipoh_ConsultarEjecucionPorEjecucionPosterior", Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@IdEjecucionPosterior", SqlDbType.Int).Value = folio;
                Cnx.Open();

                SqlDataReader sqldataReader = comando.ExecuteReader();

                DataTable tabladata = new DataTable();
                tabladata.Load(sqldataReader);

                Ejecucion ejecucion = DataHelper.DataTableToList<Ejecucion>(tabladata).FirstOrDefault();

                if (ejecucion != null)
                {
                    Estatus = Estatus.OK;
                    return ejecucion;
                }

                Estatus = Estatus.SIN_RESULTADO;
                return null;
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
        /// 
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="apellidoPaterno"></param>
        /// <param name="apellidoMaterno"></param>
        /// <returns></returns>
        public List<Ejecucion> ObtenerEjecucionPorPartesCausa(string nombre, string apellidoPaterno, string apellidoMaterno, int idCircuito)
        {
            try
            {
                if (!IsValidConnection)
                    throw new Exception("No se ha creado una conexion valida");

                SqlCommand comando = new SqlCommand("sipoh_ConsultarEjecucionPorPartesPrevia", Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@nombre", SqlDbType.VarChar).Value = nombre;
                comando.Parameters.Add("@apellidoPaterno", SqlDbType.VarChar).Value = apellidoPaterno;
                comando.Parameters.Add("@apellidoMaterno", SqlDbType.VarChar).Value = apellidoMaterno;
                comando.Parameters.Add("@idCircuito", SqlDbType.Int).Value = idCircuito;

                Cnx.Open();

                SqlDataReader sqlRespuesta = comando.ExecuteReader();

                DataTable tabla = new DataTable();
                tabla.Load(sqlRespuesta);

                List<Ejecucion> partesEjecucion = DataHelper.DataTableToList<Ejecucion>(tabla);

                if (partesEjecucion.Count > 0)
                    Estatus = Estatus.OK;
                else
                    Estatus = Estatus.SIN_RESULTADO;

                return partesEjecucion;
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
        /// 
        /// </summary>
        /// <param name="numeroCausa"></param>
        /// <param name="idJuzgado"></param>
        /// <returns></returns>
        public List<Ejecucion> ObtenerEjecucionPorNumeroCausa(string numeroCausa, int idJuzgado, int idCircuito)
        {
            try
            {
                if (!IsValidConnection)
                    throw new Exception("No se ha creado una conexion válida");

                SqlCommand comando = new SqlCommand("sipoh_ConsultarEjecucionPorCausa", Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@numeroCausa", SqlDbType.VarChar).Value = numeroCausa;
                comando.Parameters.Add("@idJuzgado", SqlDbType.Int).Value = idJuzgado;
                comando.Parameters.Add("@idCircuito", SqlDbType.Int).Value = idCircuito;

                Cnx.Open();

                //se crea objeto de tipo sql ,se ejecuta y almacena valor 
                SqlDataReader sqlRespuesta = comando.ExecuteReader();

                //se crea un objeto de tipo tabla que contendra la informacion que almacenó de la ejecucion comando
                DataTable tabla = new DataTable();
                tabla.Load(sqlRespuesta);

                //convierte la informacion de la tabla en una lista de tipo ejecucion
                List<Ejecucion> numeroCausaEjecucion = DataHelper.DataTableToList<Ejecucion>(tabla);

                //condicional en caso de encontrar y no encontrar  registros 
                if (numeroCausaEjecucion.Count > 0)
                    Estatus = Estatus.OK;
                else
                    Estatus = Estatus.SIN_RESULTADO;

                return numeroCausaEjecucion;
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
        /// 
        /// </summary>
        /// <param name="detalleSolicitante"></param>
        /// <returns></returns>
        public List<Ejecucion> ObtenerEjecucionPorDetalleSolicitante(string detalleSolicitante)
        {
            try
            {
                if (!IsValidConnection)
                    throw new Exception("No se ha creado una conexion válida");
                SqlCommand comando = new SqlCommand("sipoh_ConsultarEjecucionPorDetalleSolicitante", Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@detalleSolicitante", SqlDbType.VarChar).Value = detalleSolicitante;

                Cnx.Open();

                SqlDataReader sqlRespuesta = comando.ExecuteReader();

                DataTable tabla = new DataTable();
                tabla.Load(sqlRespuesta);

                List<Ejecucion> detalleSolicitanteEjecucion = DataHelper.DataTableToList<Ejecucion>(tabla);

                if (detalleSolicitanteEjecucion.Count > 0)
                    Estatus = Estatus.OK;
                else
                    Estatus = Estatus.SIN_RESULTADO;

                return detalleSolicitanteEjecucion;
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
        /// 
        /// </summary>
        /// <param name="nuc"></param>
        /// <param name="idJuzgado"></param>
        /// <returns></returns>
        public List<Ejecucion> ObtenerEjecucionPorNUC(string nuc, int idJuzgado)
        {
            try
            {
                if (!IsValidConnection)
                    throw new Exception("No se ha creado una conexion válida");

                SqlCommand comando = new SqlCommand("sipoh_ConsultarEjecucionPorNUC", Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@nuc", SqlDbType.VarChar).Value = nuc;
                comando.Parameters.Add("@idJuzgado", SqlDbType.Int).Value = idJuzgado;

                Cnx.Open();

                SqlDataReader sqlRespuesta = comando.ExecuteReader();

                DataTable tabla = new DataTable();
                tabla.Load(sqlRespuesta);

                List<Ejecucion> nucEjecucion = DataHelper.DataTableToList<Ejecucion>(tabla);

                if (nucEjecucion.Count > 0)
                    Estatus = Estatus.OK;
                else
                    Estatus = Estatus.SIN_RESULTADO;

                return nucEjecucion;
            }
            //crea un objeto ex
            catch (Exception ex)
            {
                //accedo a la propiedad de mi objeto mensage
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
        /// 
        /// </summary>
        /// <param name="idSolicitante"></param>
        /// <returns></returns>
        public List<Ejecucion> ObtenerEjecucionPorSolicitante(int idSolicitante)
        {
            try
            {
                if (!IsValidConnection)
                    throw new Exception("No se ha creado conexion valida");

                SqlCommand comando = new SqlCommand("sipoh_ConsultarEjecucionPorSolicitante", Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("idSolicitante", SqlDbType.Int).Value = idSolicitante;

                Cnx.Open();

                SqlDataReader slqRespuesta = comando.ExecuteReader();

                DataTable tabla = new DataTable();
                tabla.Load(slqRespuesta);

                List<Ejecucion> solicitanteEjecucion = DataHelper.DataTableToList<Ejecucion>(tabla);

                if (solicitanteEjecucion.Count > 0)
                    Estatus = Estatus.OK;
                else
                    Estatus = Estatus.SIN_RESULTADO;

                return solicitanteEjecucion;

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

        public int? GuardarPostEjecucion(PostEjecucion postEjecucion, List<Anexo> anexos)
        {
            try
            {
                if (!IsValidConnection)
                    throw new Exception("No se ha creado una conexion valida");
                
                SqlCommand comandoSQL = new SqlCommand("sipoh_CrearEjecucionPosterior", Cnx);
                comandoSQL.CommandType = CommandType.StoredProcedure;
                comandoSQL.Parameters.Add("@IdEjecucion", SqlDbType.Int).Value = postEjecucion.IdEjecucion;
                comandoSQL.Parameters.Add("@Promovente", SqlDbType.VarChar).Value = postEjecucion.Promovente;
                comandoSQL.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = postEjecucion.IdUser;
                comandoSQL.Parameters.Add("@IdEjecucionPosterior", SqlDbType.Int);

                comandoSQL.Parameters["@IdEjecucionPosterior"].Direction = ParameterDirection.Output;

                SqlParameter parametroAnexos = new SqlParameter();
                parametroAnexos.ParameterName = "@AnexosPromociones";
                parametroAnexos.SqlDbType = SqlDbType.Structured;
                parametroAnexos.Value = CreaAnexoType(anexos);
                comandoSQL.Parameters.Add(parametroAnexos);


                Cnx.Open();
                comandoSQL.ExecuteNonQuery();

                Estatus = Estatus.OK;

                return (int)comandoSQL.Parameters["@IdEjecucionPosterior"].Value;

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

        #endregion

        #region Metodos Privados
        private DataTable CreaCausasType(List<int> causas)
        {
            DataTable expedientesType = new DataTable();
            expedientesType.Clear();
            expedientesType.Columns.Add("IdExpediente");

            foreach (int expediente in causas)
            {
                DataRow fila = expedientesType.NewRow();
                fila["IdExpediente"] = expediente;
                expedientesType.Rows.Add(fila);
            }
            return expedientesType;
        }

        private DataTable CreaTocasType(List<Expediente> tocas)
        {
            DataTable tocasType = new DataTable();
            tocasType.Clear();
            tocasType.Columns.Add("NumeroDeToca");
            tocasType.Columns.Add("IdUnidad");

            foreach (Expediente toca in tocas)
            {
                DataRow fila = tocasType.NewRow();
                fila["NumeroDeToca"] = toca.NumeroDeToca;
                fila["IdUnidad"] = toca.IdJuzgado;
                tocasType.Rows.Add(fila);
            }

            return tocasType;
        }

        private DataTable CreaAmparosType(List<string> amparos)
        {
            DataTable tocasType = new DataTable();
            tocasType.Clear();
            tocasType.Columns.Add("Amparo");

            foreach (string amparo in amparos)
            {
                DataRow fila = tocasType.NewRow();
                fila["Amparo"] = amparo;
                tocasType.Rows.Add(fila);
            }

            return tocasType;
        }

        private DataTable CreaAnexoType(List<Anexo> anexos)
        {
            DataTable tocasType = new DataTable();
            tocasType.Clear();
            tocasType.Columns.Add("IdCatAnexEjec");
            tocasType.Columns.Add("OtroAnexoEjecucion");
            tocasType.Columns.Add("Cantidad");

            foreach (Anexo anexo in anexos)
            {
                DataRow fila = tocasType.NewRow();
                fila["IdCatAnexEjec"] = anexo.IdAnexo;
                fila["OtroAnexoEjecucion"] = anexo.IdAnexo == 8 ? anexo.Descripcion : null;
                fila["Cantidad"] = anexo.Cantidad;
                tocasType.Rows.Add(fila);
            }

            return tocasType;
        }

    }
}

    #endregion



