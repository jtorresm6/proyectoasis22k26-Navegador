using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Se importa la Capa Modelo para que el controlador use ClsPermisos
using CapaModelo_Navegador;


//Roger Yankhel de Jesús Herrera Alcántara 0901-23-2429
namespace CapaControlador_Navegador
{
    //Recibe la solicitud de la capa Vista
    public class ClsCtrlPermiso
    {
        //Se llama las funciones del modelo
        private ClsPermisos _Permisos = new ClsPermisos();

        //Este metodo recibe el usuario y devuelve si tiene acceso o no de manera booleana 
        public bool NavegadorFuncValidarAcceso(string Usuario, string Modulo)
        {
            //Intenta que el modelo realice la validacion 
            try
            {
                return _Permisos.NavegadorFuncValidarAcceso(Usuario, Modulo);
            }
            //Si existe algun tipo de error en el modelo muestra el error y lo muestra en pantalla 
            catch (Exception Excepcion)
            {
                throw new Exception(
                    "Error al validar los permisos del Usuario '" +
                    Usuario +
                    "' sobre el módulo '" +
                    Modulo +
                    "': " +
                    Excepcion.Message,
                    Excepcion);
            }
        }
    }
}
//Roger Yankhel de Jesús Herrera Alcántara 0901-23-2429