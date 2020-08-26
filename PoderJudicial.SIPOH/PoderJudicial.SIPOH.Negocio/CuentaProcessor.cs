using PoderJudicial.SIPOH.AccesoDatos.Interfaces;
using PoderJudicial.SIPOH.Entidades;
using PoderJudicial.SIPOH.Entidades.Enum;

namespace PoderJudicial.SIPOH.Negocio
{
    public class CuentaProcessor : ICuentaProcessor
    {
        //Atributos publicos del Proceso
        public string Mensaje { get; set; }

        //Atributos privados del proceso
        private readonly ICuentaRepository cuentaRepositorio;

        //Metodo Contructor del proceso, se le inyecta la interfaz CuentaRepositoru
        public CuentaProcessor(ICuentaRepository repositorio)
        {
            this.cuentaRepositorio = repositorio;
        }

        #region Metodos publicos del proceso
        public Usuario ValidarLogInUsuario(string email, string password)
        {
            Usuario user = cuentaRepositorio.LogIn(email, password);

            if(cuentaRepositorio.Estatus == Estatus.INACTIVO)
                Mensaje = "Cuenta desactivada.";

            if (cuentaRepositorio.Estatus == Estatus.SIN_RESULTADO)
                Mensaje = "Usuario/Contraseña Invalidos.";

            else if (cuentaRepositorio.Estatus == Estatus.ERROR)
            {
                Mensaje = "No es posible iniciar sesion, contacte a soporte";
                string mensajeLogger = cuentaRepositorio.MensajeError;
                //Logica para ILogger
            }

            return user;
        }
        #endregion

        #region Metodos Privados del proceso
        #endregion
    }
}
