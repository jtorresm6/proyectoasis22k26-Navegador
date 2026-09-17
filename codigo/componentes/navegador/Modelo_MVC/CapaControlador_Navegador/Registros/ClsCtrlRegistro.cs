// Dylan Rene Hernandez Recinos 16/09/2026
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CapaEntidades_Navegador;
using CapaModelo_Navegador;

namespace CapaControlador_Navegador
{
    // Controlador para las operaciones que modifican registros: Insertar, Actualizar, Eliminar y validar.

    /* | Sofía Stella de la Rosa Juárez | 0901-16-9036 | 2026-09-16 | 
     *
     * Esta clase pertenece a la capa controlador y se encarga de gestionar las operaciones relacionadas con los registros, 
     * como insertar, actualizar y eliminar información. También realiza validaciones antes de enviar los datos hacia la capa modelo
     */

    public class ClsCtrlRegistro
    {
    // Sofía de la Rosa | Se crean las instancias necesarias para utilizar las funcione relacionadas con los registros y con el esquema de las tablas
        private ClsRegistros _Registros = new ClsRegistros();
        private ClsEsquema _Esquema = new ClsEsquema();

        // Verifica si ya existe un registro con esa llave primaria (evita duplicados).

        // Sofía de la Rosa | Verifica si una llave primaria ya existe en la tabla, utilizando los campos y valores recibidos
        public bool NavegadorFuncExisteLlavePrimaria(string NombreTabla, string[] CamposPK, string[] ValoresPK)
        {
            return _Registros.NavegadorFuncExisteLlavePrimaria(NombreTabla, CamposPK, ValoresPK);
        }

        // Verifica si un valor ya existe en un campo (para campos únicos).

        // Sofía de la Rosa | Comprueba si un valor específico ya se encuentra registrado dentro de un campo determinado de la tabla
        public bool NavegadorFuncExisteValorCampo(string NombreTabla, string NombreCampo, string Valor)
        {
            return _Registros.NavegadorFuncExisteValorCampo(NombreTabla, NombreCampo, Valor);
        }

        // Inserta un nuevo registro en la tabla.

        // Sofía de la Rosa | Envía a la capa modelo el nombre de la tabla y los datos necesarios para realizar la inserción de un nuevo registro.
        public bool NavegadorFuncInsertarRegistro(string NombreTabla, Dictionary<string, string> Datos)
        {
            return _Registros.NavegadorFuncInsertarRegistro(NombreTabla, Datos);
        }

        // Actualiza un registro existente según su llave primaria.

        /* Sofía de la Rosa | Antes de actualizar un registro se verifica que exista el nombre de la tabla, 
         * que existan datos para modificar y que se cuente con la llave primaria necesaria para identificar el registro.
         */
        public bool NavegadorFuncActualizarRegistro(string NombreTabla, Dictionary<string, string> Valores, Dictionary<string, string> ClavesPrimarias)
        {
            if (string.IsNullOrWhiteSpace(NombreTabla))
                throw new ArgumentException("El nombre de la tabla es obligatorio.");

            if (Valores == null || Valores.Count == 0)
                throw new ArgumentException("No existen datos para actualizar.");

            if (ClavesPrimarias == null || ClavesPrimarias.Count == 0)
                throw new ArgumentException("No se encontró la llave primaria del registro.");

            return _Registros.NavegadorFuncActualizarRegistro(NombreTabla, Valores, ClavesPrimarias);
        }

        // Elimina un registro según su llave primaria.

        // Sofía de la Rosa | Verifica que exista el nombre de la tabla y la llave primaria necesaria antes de solicitar la eliminación del registro.
        public bool NavegadorFuncEliminarRegistro(string NombreTabla, Dictionary<string, string> ClavesPrimarias)
        {
            if (string.IsNullOrWhiteSpace(NombreTabla))
                throw new ArgumentException("El nombre de la tabla es obligatorio.");

            if (ClavesPrimarias == null || ClavesPrimarias.Count == 0)
                throw new ArgumentException("No se encontró la llave primaria del registro.");

            return _Registros.NavegadorFuncEliminarRegistro(NombreTabla, ClavesPrimarias);
        }

        // Valida todos los campos del registro contra el esquema de la tabla y devuelve la lista de errores.

        /* Sofía de la Rosa | Obtiene el esquema de la tabla y recorre sus columnas para revisar los datos recibidos. 
         * También verifica los campos obligatorios y almacena en una lista los errores que se encuentren.
         *
         * El bloque try-catch permite controlar cualquier problema que pueda ocurrir durante el proceso de validación.
         */
        public List<string> NavegadorFuncValidarRegistro(Dictionary<string, string> Datos, string NombreTabla)
        {
            List<string> Errores = new List<string>();

            try
            {
                List<ClsColumnaInfo> Columnas = _Esquema.NavegadorFuncObtenerEsquemaTabla(NombreTabla);

                foreach (ClsColumnaInfo Columna in Columnas)
                {
                    if (!Datos.ContainsKey(Columna.Nombre))
                        continue;

                    string Valor = Datos[Columna.Nombre];

                    // Verifica que los campos obligatorios no estén vacíos
                    if (Columna.Nullable == false && string.IsNullOrWhiteSpace(Valor))
                    {
                        Errores.Add("El campo '" + Columna.Nombre + "' es obligatorio.");
                        continue;
                    }

                    string ErrorValidacion = NavegadorFuncValidarCampo(Valor, Columna);

                    if (!string.IsNullOrEmpty(ErrorValidacion))
                        Errores.Add(ErrorValidacion);
                }
            }
            catch (Exception Excepcion)
            {
                throw new Exception("Error al validar los datos: " + Excepcion.Message, Excepcion);
            }

            return Errores;
        }

        // Valida un campo individual según su tipo de dato. Retorna el error o "" si es válido.

        /* Sofía de la Rosa | Valida cada campo según el tipo de dato definido en el esquema.
         * Para los textos revisa caracteres permitidos y longitud máxima, para los números verifica su formato y para las fechas comprueba
         * que el valor pueda ser interpretado como una fecha válida.
        */
        private string NavegadorFuncValidarCampo(string Valor, ClsColumnaInfo Columna)
        {
            if (string.IsNullOrEmpty(Valor))
                return "";

            string Tipo = Columna.TipoDato.ToLower();

            switch (Tipo)
            {
                // Texto: solo caracteres permitidos y longitud máxima
                case "varchar":
                case "char":
                case "text":
                case "longtext":
                case "tinytext":
                case "mediumtext":

                    if (!Regex.IsMatch(Valor, @"^[\p{L}\p{N}\s\-_\.]+$"))
                        return "El campo '" + Columna.Nombre + "' contiene caracteres no permitidos.";

                    if (Columna.Longitud > 0 && Valor.Length > Columna.Longitud)
                        return "El campo '" + Columna.Nombre + "' excede la longitud máxima permitida (" + Columna.Longitud + " caracteres).";

                    return "";

                // Numérico: solo dígitos y punto decimal opcional
                case "int":
                case "integer":
                case "decimal":
                case "numeric":
                case "float":
                case "double":
                case "real":

                    if (!Regex.IsMatch(Valor, @"^[0-9]+(\.[0-9]+)?$"))
                        return "El campo '" + Columna.Nombre + "' debe ser un valor numérico.";

                    return "";

                // Fecha: debe ser una fecha válida
                case "datetime":
                case "date":
                case "timestamp":

                    DateTime Fecha;

                    if (!DateTime.TryParse(Valor, out Fecha))
                        return "El campo '" + Columna.Nombre + "' debe ser una fecha válida.";

                    return "";

                default:
                    return "";
            }
        }
    }
}

/* ClsCtrlRegistro permite controlar las operaciones relacionadas con los registros y validar la información antes de enviarla hacia capa modelo.
 *
 * Sus funciones permiten comprobar llaves primarias y valores existentes, insertar, actualizar y eliminar registros, 
 * además de validar campos obligatorios, textos, valores numéricos y fechas.
 *
 * También utiliza manejo de excepciones para controlar posibles errores durante el proceso de validación.
 *
 * | Sofía Stella de la Rosa Juárez | 0901-16-9036 | 2026-09-16 | 
 */