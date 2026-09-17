using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CapaEntidades_Navegador;
using CapaModelo_Navegador;

namespace CapaControlador_Navegador
{
    // Todo lo que modifica un registro: Insertar, Actualizar, Eliminar, y su validacion
    public class ClsCtrlRegistro
    {
        private ClsRegistros _Registros = new ClsRegistros();
        private ClsEsquema _Esquema = new ClsEsquema();

        public bool NavegadorFuncExisteLlavePrimaria(string NombreTabla, string[] CamposPK, string[] ValoresPK)
        {
            return _Registros.NavegadorFuncExisteLlavePrimaria(NombreTabla, CamposPK, ValoresPK);
        }

        public bool NavegadorFuncExisteValorCampo(string NombreTabla, string NombreCampo, string Valor)
        {
            return _Registros.NavegadorFuncExisteValorCampo(NombreTabla, NombreCampo, Valor);
        }

        public bool NavegadorFuncInsertarRegistro(string NombreTabla, Dictionary<string, string> Datos)
        {
            return _Registros.NavegadorFuncInsertarRegistro(NombreTabla, Datos);
        }

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

        public bool NavegadorFuncEliminarRegistro(string NombreTabla, Dictionary<string, string> ClavesPrimarias)
        {
            if (string.IsNullOrWhiteSpace(NombreTabla))
                throw new ArgumentException("El nombre de la tabla es obligatorio.");

            if (ClavesPrimarias == null || ClavesPrimarias.Count == 0)
                throw new ArgumentException("No se encontró la llave primaria del registro.");

            return _Registros.NavegadorFuncEliminarRegistro(NombreTabla, ClavesPrimarias);
        }

        // Revisa que cada campo cumpla con su tipo de dato antes de guardar
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

        // Devuelve el mensaje de error si el valor no cumple, o "" si esta bien
        private string NavegadorFuncValidarCampo(string Valor, ClsColumnaInfo Columna)
        {
            if (string.IsNullOrEmpty(Valor))
                return "";

            string Tipo = Columna.TipoDato.ToLower();

            switch (Tipo)
            {
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