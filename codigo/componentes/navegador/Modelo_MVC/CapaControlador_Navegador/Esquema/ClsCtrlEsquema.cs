using System;
using System.Collections.Generic;
using CapaEntidades_Navegador;
using CapaModelo_Navegador;

namespace CapaControlador_Navegador
{
    // Todo lo relacionado a la estructura de una tabla (columnas, PK, FK, tipos)

    /* 0901-16-9036 | Sofía Stella de la Rosa Juárez
     * 
     * Esta clase pertenece a la capa controlador y se encarga de solicitar a la capa modelo la información relacionada con
     * la estructura de las tablas y con el cálculo del siguiente valor disponible para una llave primaria.
    */
    public class ClsCtrlEsquema
    {
        private ClsEsquema _Esquema = new ClsEsquema();
        /* NavegadorFuncObtenerEsquemaTabla → Solicita a la capa modelo el esquema de una tabla específica.
         * Si ocurre un error durante el procso, se captura la excepción y se genera un mensaje indicando en qué tabla ocurrió el problema.
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
        /* Solicita a la capa modelo el siguiente valor disponible para la llave primaria de una tabla.
         * Si ocurre un error, se captura y se devuelve una excepción con información más clara sobre el problema. 
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
/* ClsCtrlEsquema funciona como intermediario entre otras partes del sistema y la capa modelo para obtener la información sobre la estructura de las tablas
 * También utiliza bloques try-catch para controlar errores y proporcionar mensajes más específicos cuando alguna operación no puede realizarse correctamente
 * 
 * Sofía Stella de la Rosa Juárez | 0901-16-9036
*/