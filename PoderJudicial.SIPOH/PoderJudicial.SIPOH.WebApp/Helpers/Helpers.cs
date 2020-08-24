using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace PoderJudicial.SIPOH.WebApp.Helpers
{
    public static class Helpers
    {
        public static List<Opcion> Options<T>(this List<T> list, string value, string label)
        {
            List<Opcion> opciones = new List<Opcion>();

            foreach (T objeto in list)
            {
                Opcion opcion = new Opcion();

                PropertyInfo[] properties = typeof(T).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.Name == value)
                    {
                        opcion.Value = (int)property.GetValue(objeto);
                    }
                    if (property.Name == label)
                    {
                        opcion.Text = (string)property.GetValue(objeto);
                    }
                }
                opciones.Add(opcion);
            }
            return opciones;
        }
    }
}