/*
 * Autor: Julio Roberto Rosales Mejía.
 * Carné: 0901-23-1426
 * Creación de clase "ClsCtrlTabla.cs"
 * Documentación Interna del código.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using CapaModelo_Navegador;

namespace CapaControlador_Navegador
{
    // Esta clase contiene la lógica de control relacioanda con la consulta de tablas y la 
    // Carga de información en el GridControl. Forma parte de la Capa Controlador del componente.
    public class ClsCtrlTabla
    {
        // Objeto que permite acceder a los métodos del Modelo encargados de obtener y filtrar
        // los registros almacenados en las tablas.
        private ClsRegistros _Registros = new ClsRegistros();
        // Objeto que pemrite acceder a los métodos del Modelo encargados de consultar la 
        // Estructura de la base de datos, como sus tablas y las columnas que contiene cada una.
        private ClsEsquema _Esquema = new ClsEsquema();
        // Obtiene todos los registros de una tabla y los prepara en un DataTable para que
        // puedan ser mostrados en el GridControl.
        public DataTable NavegadorMetLlenarDgv(string NombreTabla)
        {
            // Se crea una DataTable vacío que almacenerá los registros obtenidos de la base de datos.
            DataTable TablaDatos = new DataTable();

            try
            {
                // Se solicita al Modelo que prepare el adaptador encargado de obtener los registros de la tabla indicada.
                // "Using" permite liberar automáticamente el adaptador caundo termina ese bloque de código
            
                using (OdbcDataAdapter AdaptadorDatos = _Registros.NavegadorFuncLlenarTbl(NombreTabla))
                {
                    // Ejecuta la consulta y llena el DataTable con los registros obtenidos de la base de datos.
                    
                    AdaptadorDatos.Fill(TablaDatos);
                    // Se verifica que exista una consulta y que esta tenga una conexión asociada antes de intentar cerrarla.
                    if (AdaptadorDatos.SelectCommand != null && AdaptadorDatos.SelectCommand.Connection != null)
                        AdaptadorDatos.SelectCommand.Connection.Close();
                }
            }
            catch (Exception Excepcion)
            {
                // Si ocurre algún error duante la carga de la tabla, se genera una excepción con
                // información sobre la tabla qeu estaba siendo consultada.
                
                throw new Exception("Error al cargar la tabla '" + NombreTabla + "': " + Excepcion.Message, Excepcion);
            }

            // Se devuelve el DataTable con la información obtenida para qeu la capa que solicitó
            // Los datos pueda mostrarla. 
            return TablaDatos;
        }

        // Obtiene la lista de tablas disponibles en la base de datos.
        public List<string> NavegadorFuncObtenerTablas()
        {
            try
            {
                // Se solicita al Modelo que consullte el esquema de la base de datos y 
                // devuelva los nombres de sus tablas.
                
                return _Esquema.NavegadorFuncObtenerTablas();
            }
            catch (Exception Excepcion)
            {
                // Si ocurre un error al consultar las tabals, se genera una excepción con un
                // mensaje más específico para identificar la operación que falló. 
                
                throw new Exception("Error al obtener la lista de tablas de la base de datos: " + Excepcion.Message, Excepcion);
            }
        }

        // Obtiene la lista de columans pertenecientes a una tabla específica de la BD.
        public List<string> NavegadorFuncObtenerColumnas(string NombreTabla)
        {
            // El controlador delega esta operaicón al Modelo, enviando el nombre de la tabla
            // que se desea consultar. 
            return _Esquema.NavegadorFuncObtenerColumnas(NombreTabla);
        }

        // Obtiene los registors de una tabla aplicando un filtro sobre una columna y un valor
        // determinado.
        public DataTable NavegadorMetFiltrarDgv(string NombreTabla, string Columna, string Valor)
        {
            // Se solicita al Modelo un adaptador preparado para ejectuar 
            // La consutla filtrada sobre la tabla indicada.
            OdbcDataAdapter AdaptadorDatos = _Registros.NavegadorFuncFiltrarTbl(NombreTabla, Columna, Valor);
            
            // Se crea un DataTable vacío en donde se almacenarán únicamente los registros que 
            // coinciden con el filtro.
            DataTable TablaDatos = new DataTable();

            try
            {
                // Ejecuta la consulta y carga los resultados filtrados dentro del DataTable.
                AdaptadorDatos.Fill(TablaDatos);
            }
            finally
            {
                // Se verifica que exissta una consulta y una conexión antes de intentar cerrar la 
                // conexión utilizada.
                if (AdaptadorDatos.SelectCommand != null && AdaptadorDatos.SelectCommand.Connection != null)
                    AdaptadorDatos.SelectCommand.Connection.Close();

                // Libera los recursos utilizados por el adaptador después de finalizar la opeación.
                AdaptadorDatos.Dispose();
            }

            // Devuelve los datos filtrados para que puedan ser utilizados por la intefaz del componetne navegador.
            return TablaDatos;
        }
    }
}



/*
 *FIN DEL PROGRAMA
 *AUTOR: Julio Roberto Rosales Mejía
 *Carné: 0901-23-1426
 **/