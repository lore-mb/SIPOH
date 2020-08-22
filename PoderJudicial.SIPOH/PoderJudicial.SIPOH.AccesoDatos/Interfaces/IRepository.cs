using PoderJudicial.SIPOH.AccesoDatos.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoderJudicial.SIPOH.AccesoDatos.Interfaces
{
    public interface IRepository<T>
    {
        string MensajeError { get; set; }
        T Listar(ref Resultado status);
        T Buscar(string valor, ref Resultado status);
        void Existe(string nombre, ref Resultado status);
        void Insertar(T categoria, ref Resultado status);
        void Actualizar(T categoria, ref Resultado status);
        void Eliminar(int id, ref Resultado status);
    }
}
