using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CapaEntidades_Navegador;
using CapaModelo_Navegador;

namespace CapaControlador_Navegador
{
    // Todo lo que modifica un registro: Insertar, Actualizar, Eliminar, y su validacion
    public class ctrlRegistro
    {
        private registros registros = new registros();
        private esquema esquema = new esquema();

        public bool ExisteLlavePrimaria(string nombreTabla, string[] camposPK, string[] valoresPK)
        {
            return registros.ExisteLlavePrimaria(nombreTabla, camposPK, valoresPK);
        }

        public bool ExisteValorCampo(string nombreTabla, string nombreCampo, string valor)
        {
            return registros.ExisteValorCampo(nombreTabla, nombreCampo, valor);
        }

        public bool InsertarRegistro(string nombreTabla, Dictionary<string, string> datos)
        {
            return registros.InsertarRegistro(nombreTabla, datos);
        }

        public bool ActualizarRegistro(string nombreTabla, Dictionary<string, string> valores, Dictionary<string, string> clavesPrimarias)
        {
            if (string.IsNullOrWhiteSpace(nombreTabla))
                throw new ArgumentException("El nombre de la tabla es obligatorio.");

            if (valores == null || valores.Count == 0)
                throw new ArgumentException("No existen datos para actualizar.");

            if (clavesPrimarias == null || clavesPrimarias.Count == 0)
                throw new ArgumentException("No se encontró la llave primaria del registro.");

            return registros.ActualizarRegistro(nombreTabla, valores, clavesPrimarias);
        }

        public bool EliminarRegistro(string nombreTabla, Dictionary<string, string> clavesPrimarias)
        {
            if (string.IsNullOrWhiteSpace(nombreTabla))
                throw new ArgumentException("El nombre de la tabla es obligatorio.");

            if (clavesPrimarias == null || clavesPrimarias.Count == 0)
                throw new ArgumentException("No se encontró la llave primaria del registro.");

            return registros.EliminarRegistro(nombreTabla, clavesPrimarias);
        }

        // Revisa que cada campo cumpla con su tipo de dato antes de guardar
        public List<string> ValidarRegistro(Dictionary<string, string> datos, string nombreTabla)
        {
            List<string> errores = new List<string>();

            try
            {
                List<columnaInfo> columnas = esquema.ObtenerEsquemaTabla(nombreTabla);

                foreach (columnaInfo col in columnas)
                {
                    if (!datos.ContainsKey(col.Nombre))
                        continue;

                    string valor = datos[col.Nombre];

                    if (col.Nullable == false && string.IsNullOrWhiteSpace(valor))
                    {
                        errores.Add("El campo '" + col.Nombre + "' es obligatorio.");
                        continue;
                    }

                    string error = ValidarCampo(valor, col);

                    if (!string.IsNullOrEmpty(error))
                        errores.Add(error);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar los datos: " + ex.Message, ex);
            }

            return errores;
        }

        // Devuelve el mensaje de error si el valor no cumple, o "" si esta bien
        private string ValidarCampo(string valor, columnaInfo col)
        {
            if (string.IsNullOrEmpty(valor))
                return "";

            string tipo = col.TipoDato.ToLower();

            switch (tipo)
            {
                case "varchar":
                case "char":
                case "text":
                case "longtext":
                case "tinytext":
                case "mediumtext":

                    if (!Regex.IsMatch(valor, @"^[\p{L}\p{N}\s\-_\.]+$"))
                        return "El campo '" + col.Nombre + "' contiene caracteres no permitidos.";

                    if (col.Longitud > 0 && valor.Length > col.Longitud)
                        return "El campo '" + col.Nombre + "' excede la longitud máxima permitida (" + col.Longitud + " caracteres).";

                    return "";

                case "int":
                case "integer":
                case "decimal":
                case "numeric":
                case "float":
                case "double":
                case "real":

                    if (!Regex.IsMatch(valor, @"^[0-9]+(\.[0-9]+)?$"))
                        return "El campo '" + col.Nombre + "' debe ser un valor numérico.";

                    return "";

                case "datetime":
                case "date":
                case "timestamp":

                    DateTime fecha;

                    if (!DateTime.TryParse(valor, out fecha))
                        return "El campo '" + col.Nombre + "' debe ser una fecha válida.";

                    return "";

                default:

                    return "";
            }
        }
    }
}