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

        #region Metodos Publicos del Repositorio Ejecucion

        /// <summary>
        /// Consume SPSQL y retorna una lista de tipo Ejecución por medio del nombre del Sentenciado Beneficiario o una Parte de causa
        /// </summary>
        /// <param name="nombre">Nombre de la Persona Beneficiario o Parte de la Causa</param>
        /// <param name="apellidoPaterno">Apellido Paterno de la persona beneficiaria o Parte de la Causa</param>
        /// <param name="apellidoMaterno">Apeliido Materno de la personas beneficiaria o Parte de la Causa</param>
        /// <returns>Lista tipo Ejecucion</returns>
        public List<Ejecucion> ConsultaEjecuciones(ParteCausaBeneficiario parteBeneficiario, string nombre, string apellidoPaterno, string apellidoMaterno, int idCircuito)
        {
            try
            {
                if (!IsValidConnection)
                {
                    throw new Exception("No se ha creado una conexion valida");
                }

                string storeProcedure = parteBeneficiario == ParteCausaBeneficiario.BENEFICIARIO ? "sipoh_ConsultarEjecucionPorSentenciadoBeneficiario" : "sipoh_ConsultarEjecucionPorPartesPrevia";

                SqlCommand comando = new SqlCommand(storeProcedure, Cnx);
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
        /// Consume SPSQL y retorna una lista de tipo Ejecución por medio del numero de expediente y el juzgado asignado al expediente
        /// </summary>
        /// <param name="tipoNumeroExpediente">Especifica el tipo de numero de Expediente</param>
        /// <param name="numeroExpediente">Numero del expediente, puede ser un numero de causa, nuc o numero de ejecucion</param>
        /// <param name="idJuzgado">Id del juzgado asignado al expediente</param>
        /// <returns>Lista tipo Ejecucion</returns>
        public List<Ejecucion> ConsultaEjecuciones(TipoNumeroExpediente tipoNumeroExpediente, string numeroExpediente, int idJuzgado)
        {
            try
            {
                if (!IsValidConnection)
                   throw new Exception("No se ha creado una conexion válida");
                

                var storeProcedure = tipoNumeroExpediente == TipoNumeroExpediente.CAUSA ? "sipoh_ConsultarEjecucionPorCausa" : tipoNumeroExpediente == TipoNumeroExpediente.NUC ? "sipoh_ConsultarEjecucionPorNUC" : "sipoh_ConsultarEjecucionPorJuzgado";

                SqlCommand comando = new SqlCommand(storeProcedure, Cnx);
                comando.CommandType = CommandType.StoredProcedure;

                if (tipoNumeroExpediente == TipoNumeroExpediente.CAUSA)
                comando.Parameters.Add("@numeroCausa", SqlDbType.VarChar).Value = numeroExpediente;

                if (tipoNumeroExpediente == TipoNumeroExpediente.NUC)
                comando.Parameters.Add("@nuc", SqlDbType.VarChar).Value = numeroExpediente;

                if (tipoNumeroExpediente == TipoNumeroExpediente.EJECUCION)
                comando.Parameters.Add("@numeroEjecucion", SqlDbType.VarChar).Value = numeroExpediente;

                comando.Parameters.Add("@idJuzgado", SqlDbType.Int).Value = idJuzgado;

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
        /// Consume SPSQL y retorna el Id de la Ejecucion creada.
        /// </summary>
        /// <param name="ejecucion">Objeto de tipo ejecucion</param>
        /// <param name="causas">Es una lista de tipo entero que contiene los id de las causas relacionadas a la ejecucion </param>
        /// <param name="tocas">Es una lista de tipo toca que contiene la realcion de salas y numeros de tocas relacionadas a la ejecucion</param>
        /// <param name="amparos">Es una lista de tipo string que contiene la relacion de numeros de amparos relacionados a la ejecucion</param>
        /// <param name="anexos">Es una lista de tipo anexo que contiene la relacion de anexos relacionados a la ejecucion</param>
        /// <param name="idJuzgado">Contiene el id del juzgado de ejecución asignado al registro de ejecucion creado es de tipo null</param>
        /// <param name="circuitoPachuca">Contiene el id del circuito al que se ara la asignacion</param>
        /// <returns>Id de la Ejecucion</returns>
        public int ? CreaEjecucion(Ejecucion ejecucion, List<int> causas, List<Toca> tocas, List<string> amparos, List<Anexo> anexos, int ? idJuzgado, bool circuitoPachuca)
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
                
                //Tipos Data Table
                comando.Parameters.Add("@expedientes", SqlDbType.Structured).Value = CreaCausasType(causas);
                comando.Parameters.Add("@tocas", SqlDbType.Structured).Value = CreaTocasType(tocas);
                comando.Parameters.Add("@amparos", SqlDbType.Structured).Value = CreaAmparosType(amparos);
                comando.Parameters.Add("@anexos", SqlDbType.Structured).Value = CreaAnexoType(anexos);

                //Parametro de Salida
                SqlParameter idEjecucion = new SqlParameter();
                idEjecucion.ParameterName = "@idEjecucion";
                idEjecucion.SqlDbType = SqlDbType.Int;
                idEjecucion.Direction = ParameterDirection.Output;
                comando.Parameters.Add(idEjecucion);

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

        /// <summary>
        /// Consume SPSQL y retorna un objeto de tipo ejecucion por medio del idEjecucion
        /// </summary>
        /// <param name="idEjecucion"></param>
        /// <returns></returns>
        public Ejecucion ConsultaEjecucion(int idEjecucion)
        {
            try
            {
                if (!IsValidConnection)
                    throw new Exception("No se ha creado una conexion valida");

                SqlCommand comando = new SqlCommand("sipoh_ConsultarEjecucionPorFolio", Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@folio", SqlDbType.Int).Value = idEjecucion;
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
        /// Consume SPSQL y retorna una lista de tipo Ejecución por medio del detalle Solicitante
        /// </summary>
        /// <param name="detalleSolicitante">Contiene el detalle del solicitante de una ejecucion</param>
        /// <returns>Lista tipo Ejecucion</returns>
        public List<Ejecucion> ConsultaEjecuciones(string detalleSolicitante, int idCircuito)
        {
            try
            {
                if (!IsValidConnection)
                    throw new Exception("No se ha creado una conexion válida");

                SqlCommand comando = new SqlCommand("sipoh_ConsultarEjecucionPorDetalleSolicitante", Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@detalleSolicitante", SqlDbType.VarChar).Value = detalleSolicitante;
                comando.Parameters.Add("@idCircuito", SqlDbType.Int).Value = idCircuito;
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
        /// Consume SPSQL y retorna una lista de tipo Ejecución por medio del idSolicitante
        /// </summary>
        /// <param name="idSolicitante">Id del solicitante relacioando a la ejecucion</param>
        /// <returns>Lista tipo ejecucion</returns>
        public List<Ejecucion> ConsultaEjecuciones(int idSolicitante, int idCircuito)
        {
            try
            {
                if (!IsValidConnection)
                    throw new Exception("No se ha creado conexion valida");

                SqlCommand comando = new SqlCommand("sipoh_ConsultarEjecucionPorSolicitante", Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@idSolicitante", SqlDbType.Int).Value = idSolicitante;
                comando.Parameters.Add("@idCircuito", SqlDbType.Int).Value = idCircuito;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folio"></param>
        /// <returns></returns>
        public EjecucionPosterior ConsultaEjecucionPosterior(int IdEjecucionPosterior)
        {
            try
            {
                if (!IsValidConnection)
                    throw new Exception("No se ha creado una conexion valida");

                SqlCommand comando = new SqlCommand("sipoh_ConsultarEjecucionPorEjecucionPosterior", Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@IdEjecucionPosterior", SqlDbType.Int).Value = IdEjecucionPosterior;
                Cnx.Open();

                SqlDataReader sqldataReader = comando.ExecuteReader();

                DataTable tabladata = new DataTable();
                tabladata.Load(sqldataReader);

                EjecucionPosterior ejecucion = DataHelper.DataTableToList<EjecucionPosterior>(tabladata).FirstOrDefault();

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
        /// <param name="postEjecucion"></param>
        /// <param name="anexos"></param>
        /// <returns></returns>
        public int? CreaEjecucionPosterior(EjecucionPosterior ejecucionPosterior, List<Anexo> anexos)
        {
            try
            {
                if (!IsValidConnection)
                    throw new Exception("No se ha creado una conexion valida");
                
                SqlCommand comandoSQL = new SqlCommand("sipoh_CrearEjecucionPosterior", Cnx);
                comandoSQL.CommandType = CommandType.StoredProcedure;
                comandoSQL.Parameters.Add("@IdEjecucion", SqlDbType.Int).Value = ejecucionPosterior.IdEjecucion;
                comandoSQL.Parameters.Add("@Promovente", SqlDbType.VarChar).Value = ejecucionPosterior.Promovente;
                comandoSQL.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = ejecucionPosterior.IdUser;
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

        /// <summary>
        /// Valida la existencia de un numero de ejecucion por medio del numero de ejecucion y el IdJuzgado de ejecucion asignado
        /// </summary>
        /// <param name="idJuzgadoEjecucion">Id del Juzgado de Ejecucion asignado el registro de ejecucion</param>
        /// <param name="numeroEjecucion">Numero de Ejecucion</param>
        public void ValidaNumeroEjecucion(int idJuzgadoEjecucion, string numeroEjecucion)
        {
            try
            {
                if (!IsValidConnection)
                    throw new Exception("No se ha creado una conexion valida");

                SqlCommand comando = new SqlCommand("sipoh_ConsultarTotalEjecucionPorJuzgadoNumeroEjecucion", Cnx);
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.Add("@idJuzgado", SqlDbType.Int).Value = idJuzgadoEjecucion;
                comando.Parameters.Add("@numeroEjecucion", SqlDbType.VarChar).Value = numeroEjecucion;

                //Parametro de Salida
                SqlParameter totalExpediente = new SqlParameter();
                totalExpediente.ParameterName = "@total";
                totalExpediente.SqlDbType = SqlDbType.Int;
                totalExpediente.Direction = ParameterDirection.Output;
                comando.Parameters.Add(totalExpediente);

                Cnx.Open();
                comando.ExecuteNonQuery();

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

        /// <summary>
        /// Consulta el ultimo numero de ejecucion generado por Juzgado de Ejecucion
        /// </summary>
        /// <param name="idJuzgadoEjecucion">Id del juzgado de ejecucion asignado al numero de ejecucion</param>
        /// <returns></returns>
        public void ConsultaRengoDeNumeroEjecucion(int idJuzgadoEjecucion, string anio, out string numeroEjecucionMin, out string numeroEjecucionMax)
        {
            try
            {
                if (!IsValidConnection)
                    throw new Exception("No se ha creado una conexion valida");
              
                SqlCommand comandoSQL = new SqlCommand("sipoh_ConsultarRangoDeNumerosEjecucion", Cnx);
                comandoSQL.CommandType = CommandType.StoredProcedure;
                comandoSQL.Parameters.Add("@idJuzgadoEjecucion", SqlDbType.Int).Value = idJuzgadoEjecucion;
                comandoSQL.Parameters.Add("@numeroEjecucionAnio", SqlDbType.VarChar).Value = anio;

                //Parametro de Salida
                SqlParameter minimo = new SqlParameter();
                minimo.ParameterName = "@minimo";
                minimo.Size = 255;
                minimo.SqlDbType = SqlDbType.VarChar;
                minimo.Direction = ParameterDirection.Output;
                comandoSQL.Parameters.Add(minimo);

                //Parametro de Salida
                SqlParameter maximo = new SqlParameter();
                maximo.ParameterName = "@maximo";
                maximo.Size = 255;
                maximo.SqlDbType = SqlDbType.VarChar;
                maximo.Direction = ParameterDirection.Output;
                comandoSQL.Parameters.Add(maximo);

                Cnx.Open();
                comandoSQL.ExecuteNonQuery();

                numeroEjecucionMin = Convert.ToString(minimo.Value);
                numeroEjecucionMax = Convert.ToString(maximo.Value);

                if (numeroEjecucionMin == string.Empty && numeroEjecucionMax == string.Empty)
                Estatus = Estatus.SIN_RESULTADO;
                else
                Estatus = Estatus.OK;         
            }
            catch (Exception ex)
            {
                numeroEjecucionMin = null;
                numeroEjecucionMax = null;

                MensajeError = ex.Message;
                Estatus = Estatus.ERROR;
            }
            finally
            {
                if (IsValidConnection && Cnx.State == ConnectionState.Open)
                    Cnx.Close();
            }
        }

        #endregion
        /// <summary>
        /// Metodo de consulta a SIAGA_2020 mediante SP-SQL para obtener los registros pertenecientes al rango de fechas introducidos 
        /// </summary>
        /// <param name="tipoReporte"> Variable tipo ENUM </param>
        /// <param name="FechaInicial"></param>
        /// <param name="FechaFinal"></param>
        /// <returns> Lista tipo Reportes</returns>
        public List<Reporte> GenerarReporteRangoFecha(TipoReporteRangoFecha TipoReporte, string FechaInicial, string FechaFinal, int IdJuzgado)
        {

            try
            {
                if (!IsValidConnection)
                {
                    throw new Exception("No se ha creado una conexion valida");
                }

                // Anexar SP Promociones
                string objetoStoreProcedure = TipoReporte == TipoReporteRangoFecha.INICIAL ? "sipoh_GenerarReporteInicialPorRangoFecha" : "sipoh_GenerarReporteInicialPorRangoFecha";

                SqlCommand Comando = new SqlCommand(objetoStoreProcedure, Cnx);
                Comando.CommandType = CommandType.StoredProcedure;
                Comando.Parameters.Add("@FechaInicial", SqlDbType.Date).Value = FechaInicial;
                Comando.Parameters.Add("@FechaFinal", SqlDbType.Date).Value = FechaFinal;
                Comando.Parameters.Add("@IdJuzgado", SqlDbType.Int).Value = IdJuzgado;
                Cnx.Open();

                SqlDataReader sqlRespuesta = Comando.ExecuteReader();

                DataTable Tabla = new DataTable();
                Tabla.Load(sqlRespuesta);

                List<Reporte> Registros = DataHelper.DataTableToList<Reporte>(Tabla);

                if (Registros.Count > 0)
                    Estatus = Estatus.OK;
                else
                    Estatus = Estatus.SIN_RESULTADO;

                return Registros;
            }
            catch (Exception Ex)
            {
                MensajeError = Ex.Message;
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
        /// Metodo de consulta a SIAGA_2020 mediante SP-SQL para obtener los registros pertenecientes al día en que se ejecuta la consulta
        /// </summary>
        /// <param name="TipoReporte">
        /// Variable tipo ENUM
        /// </param>
        /// <param name="FechaHoy"></param>
        /// <returns>Lista tipo Reportes</returns>

        public List<Reporte> GenerarReportePorDia(TipoReporteDia TipoReporte, string FechaHoy, int IdJuzgado)
        {  
            try
            {
                if (!IsValidConnection)
                {
                    throw new Exception("No se ha creado una conexion valida");
                }
                // Agregar SP para Promociones
                string StoreProcedure = TipoReporte == TipoReporteDia.INICIAL ? "sipoh_GenerarReporteInicialPorDia" : "sipoh_GenerarReporteInicialPorDia";

                SqlCommand Comando = new SqlCommand(StoreProcedure, Cnx);
                Comando.CommandType = CommandType.StoredProcedure;
                Comando.Parameters.Add("@FechaHoy", SqlDbType.Date).Value = FechaHoy;
                Comando.Parameters.Add("@IdJuzgado", SqlDbType.Int).Value = IdJuzgado;
                Cnx.Open();

                SqlDataReader sqlRespuesta = Comando.ExecuteReader();

                DataTable Tabla = new DataTable();
                Tabla.Load(sqlRespuesta);

                List<Reporte> Registros = DataHelper.DataTableToList<Reporte>(Tabla);

                if (Registros.Count > 0)
                    Estatus = Estatus.OK;
                else
                    Estatus = Estatus.SIN_RESULTADO;

                return Registros;
            }
            catch (Exception Ex)
            {
                MensajeError = Ex.Message;
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

        private DataTable CreaTocasType(List<Toca> tocas)
        {
            DataTable tocasType = new DataTable();
            tocasType.Clear();
            tocasType.Columns.Add("NumeroDeToca");
            tocasType.Columns.Add("IdUnidad");

            foreach (Toca toca in tocas)
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



