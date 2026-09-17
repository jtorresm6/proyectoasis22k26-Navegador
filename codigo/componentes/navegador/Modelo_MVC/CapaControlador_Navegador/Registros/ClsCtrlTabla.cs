/*
 * Autor: Julio Roberto Rosales Mejía.
 * Carné: 0901-23-1426
 * Creación de clase "ClsCtrlTabla.cs"
 * Documentación Interna del código.
 */


using System;
using System.Collections.Generic;
using System.Data;
using CapaModelo_Navegador;

/*Esta clase contiene la lógica de control relacionada con la consulta de tablas
 y la carga de información para el GridControl dentro del componente Navegador.
 Su función es recibir las solicitudes y delegar las operaciones correspondientes
 a las clases del Modelo.*/


namespace CapaControlador_Navegador
{
    // Todo lo relacionado a listar tablas y traer datos para el GridControl
    public class ClsCtrlTabla
    {
        // Objeto que permite acceder a los métodos del Modelo encargados de consultar
        // y filtrar los registros almacenados en las tablas de la base de datos.
        private ClsRegistros _Registros = new ClsRegistros();
        // Objeto que permite acceder a los métodos del Modelo encargados de consultar
        // la estructura de la base de datos, como las tablas y sus respectivas columnas.
        private ClsEsquema _Esquema = new ClsEsquema();
        // Obtiene todos los registros de una tabla para que puedan ser mostrados
        // posteriormente en el GridControl del componente Navegador.
        public DataTable NavegadorFuncLlenarDgv(string NombreTabla)
        {
            try
            {
                // Se solicita al Modelo que consulte todos los registros de la tabla
                // indicada y se devuelve el resultado en forma de DataTable.
                return _Registros.NavegadorFuncConsultarTodo(NombreTabla);
            }
            catch (Exception Excepcion)
            {
                /* Si ocurre un error durante la consulta, se genera una nueva excepción
                indicando el nombre de la tabla que presentó el problema y conservando
                la información de la excepción original para facilitar su identificación.*/
                throw new Exception("Error al cargar la tabla '" + NombreTabla + "': " + Excepcion.Message, Excepcion);
            }
        }
        // Obtiene los nombres de las tablas disponibles en la base de datos.
        // Esta información puede utilizarse para permitir al usuario seleccionar
        // qué tabla desea consultar desde el componente Navegador.
        public List<string> NavegadorFuncObtenerTablas()
        {
            try
            {
                // Se solicita al Modelo que consulte el esquema de la base de datos
                // y devuelva una lista con los nombres de las tablas disponibles.
                return _Esquema.NavegadorFuncObtenerTablas();
            }
            catch (Exception Excepcion)
            {
                // Si ocurre un error durante la consulta del esquema, se genera
                // una excepción indicando que no fue posible obtener la lista de tablas.
                throw new Exception("Error al obtener la lista de tablas de la base de datos: " + Excepcion.Message, Excepcion);
            }
        }

        // Obtiene los registros de una tabla aplicando un filtro sobre una columna
        // y un valor determinado para facilitar la búsqueda de información.
        public List<string> NavegadorFuncObtenerColumnas(string NombreTabla)
        {
            return _Esquema.NavegadorFuncObtenerColumnas(NombreTabla);
        }

        // Se solicita al Modelo que realice la consulta filtrada, enviando
        // el nombre de la tabla, la columna seleccionada y el valor que se desea buscar.
        // El resultado se devuelve como DataTable para su posterior presentación.
        public DataTable NavegadorFuncFiltrarDgv(string NombreTabla, string Columna, string Valor)
        {
            return _Registros.NavegadorFuncFiltrarDatos(NombreTabla, Columna, Valor);
        }
    }
}



/*
 *
 *FIN DEL PROGRAMA
 *AUTOR: Julio Roberto Rosales Mejía
 *Carné: 0901-23-1426
 *
 **/