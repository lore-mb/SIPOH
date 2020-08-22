using PoderJudicial.SIPOH.AccesoDatos.Enum;
using PoderJudicial.SIPOH.AccesoDatos.Interfaces;
using PoderJudicial.SIPOH.Entidades;

namespace PoderJudicial.SIPOH.Negocio
{
    public class CuentaProcessor : ICuentaProcessor
    {
        //Atributos publicos del Proceso
        public string Mensaje { get; set; }

        //Atributos privados del proceso
        private Resultado resultado;
        private readonly ICuentaRepository repositorio;

        //Metodo Contructor del proceso, se le inyecta la interfaz CuentaRepositoru
        public CuentaProcessor(ICuentaRepository repositorio)
        {
            this.repositorio = repositorio;
        }

        #region Metodos publicos del proceso
        public Usuario ValidarLogInUsuario(string email, string password)
        {
            Usuario user = repositorio.LogIn(email, password, ref resultado);

            if (resultado != Resultado.OK)
            {
                if(resultado == Resultado.INACTIVO)
                Mensaje = "Cuenta desactivada.";

                if (resultado == Resultado.SIN_RESULTADO)
                Mensaje = "Usuario/Contraseña Invalidos.";

                else if (resultado == Resultado.ERROR)
                {
                    Mensaje = "No es posible iniciar sesion, contacte a soporte";
                    string mensajeLogger = repositorio.MensajeError;
                }
            }
            return user;
        }
        #endregion

        #region Metodos Privados del proceso
        #endregion
    }
}
