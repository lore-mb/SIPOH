using PoderJudicial.SIPOH.Entidades.Enum;

namespace PoderJudicial.SIPOH.AccesoDatos.Interfaces
{
    public interface IRepository<T>
    {
        string MensajeError { get; set; }
        Estatus Estatus { set; get; }
        T Listar();
        T Buscar(string valor);
        void Existe(string nombre);
        void Insertar(T categoria);
        void Actualizar(T categoria);
        void Eliminar(int id);
    }
}
