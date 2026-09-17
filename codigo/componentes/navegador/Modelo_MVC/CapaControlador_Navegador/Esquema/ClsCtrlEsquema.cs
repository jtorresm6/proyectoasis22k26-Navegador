using System;
using System.Collections.Generic;
using CapaEntidades_Navegador;
using CapaModelo_Navegador;

namespace CapaControlador_Navegador
{
    // Todo lo relacionado a la estructura de una tabla (columnas, PK, FK, tipos) 

    /* | Sofía Stella de la Rosa Juárez | 0901-16-9036 | 2026-09-16 | 
     *
     * Esta clase pertenece a la capa controlador y se encarga de solicitar a la capa modelo 
     * la información relacionada con la estructura de las tablas 
     * y con el cálculo del siguiente valor disponible para una llave primaria.
    */

    public class ClsCtrlEsquema
    {
        // Se crea una instancia de ClsEsquema para utilizar las funciones correspondientes de la capa modelo.
        private ClsEsquema _Esquema = new ClsEsquema();

        /* Obtiene el esquema de una tabla mediante la capa modelo.
         * Esta información permite conocer las característica necesarias de la estructura de la tabla.
         *
         * El bloque try-catch permite controlar cualquier error que ocurra durante la consulta 
         * y mostrar un mensaje indicando la tabla relacionada con el problema.
         */
        public List<ClsColumnaInfo> NavegadorFuncObtenerEsquemaTabla(string NombreTabla)
        {
            try
            {
                return _Esquema.NavegadorFuncObtenerEsquemaTabla(NombreTabla);
            }
            catch (Exception Excepcion)
            {
                throw new Exception("Error al obtener el esquema de la tabla '" + NombreTabla + "': " + Excepcion.Message, Excepcion);
            }
        }

        /*
         * Solicita a la capa modelo el siguiente valor disponible para la llave primaria de la tabla y columna indicadas.
         *
         * El bloque try-catch permite controlar los errores que puedan presentarse al realizar este cálculo.
         */
        public object NavegadorFuncObtenerSiguienteValorLlave(string NombreTabla, string ColumnaPK)
        {
            try
            {
                return _Esquema.NavegadorFuncObtenerSiguienteValorLlave(NombreTabla, ColumnaPK);
            }
            catch (Exception Excepcion)
            {
                throw new Exception("Error al calcular el siguiente valor de la llave primaria de '" + NombreTabla + "': " + Excepcion.Message, Excepcion);
            }
        }
    }
}

/*
 * ClsCtrlEsquema funciona como intermediario entre el sistema y la capa modelo 
 * para obtener información relacionada con la estructura de las tablas.
 *
 * Entre sus funciones se encuentra consultar el esquema de una tabla y 
 * solicitar el siguiente valor disponible para una llave primaria. 
 * También utiliza manejo de excepciones para controlar posibles errores durante estas operaciones.
 *
 * | Sofía Stella de la Rosa Juárez | 0901-16-9036 | 2026-09-16 | 
 */