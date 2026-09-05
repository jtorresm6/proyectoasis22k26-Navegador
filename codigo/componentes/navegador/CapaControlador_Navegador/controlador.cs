using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Text.RegularExpressions;
using CapaModelo_Navegador;

namespace CapaControlador_Navegador
{
    public class cls_ControladorNavegador
    {
        private cls_Sentencias Prv_sentencias = new cls_Sentencias();

        public DataTable met_LlenarDgv(string nombreTabla)
        {
            try
            {
                OdbcDataAdapter da = Prv_sentencias.met_LlenarTbl(nombreTabla);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudieron cargar los datos: " + ex.Message, ex);
            }
        }

        private bool met_ValidarCampo(string valor, string tipoDato, int? longitudMaxima)
        {
            if (string.IsNullOrEmpty(valor))
                return true;

            switch (tipoDato.ToLower())
            {
                case "varchar":
                case "char":
                case "text":
                case "longtext":
                case "tinytext":
                case "mediumtext":
                    if (!Regex.IsMatch(valor, @"^[\p{L}\p{N}\s\-_\.]+$"))
                        return false;
                    if (longitudMaxima.HasValue && valor.Length > longitudMaxima.Value)
                        return false;
                    return true;

                case "int":
                case "integer":
                case "decimal":
                case "numeric":
                case "float":
                case "double":
                case "real":
                    if (!Regex.IsMatch(valor, @"^[0-9]+(\.[0-9]+)?$"))
                        return false;
                    return true;

                case "datetime":
                case "date":
                case "timestamp":
                    if (!DateTime.TryParse(valor, out _))
                        return false;
                    return true;

                default:
                    return true;
            }
        }

        public List<string> met_ValidarRegistro(Dictionary<string, string> datos, string nombreTabla)
        {
            List<string> errores = new List<string>();

            try
            {
                DataTable esquema = Prv_sentencias.met_ObtenerEsquemaTabla(nombreTabla);

                foreach (DataRow columna in esquema.Rows)
                {
                    string nombreCampo = columna["COLUMN_NAME"].ToString();
                    string tipoDato = columna["DATA_TYPE"].ToString();
                    int? longitudMaxima = columna["CHARACTER_MAXIMUM_LENGTH"] as int?;
                    string isNullable = columna["IS_NULLABLE"].ToString();

                    if (datos.ContainsKey(nombreCampo))
                    {
                        string valor = datos[nombreCampo];

                        if (isNullable == "NO" && string.IsNullOrWhiteSpace(valor))
                        {
                            errores.Add($"El campo '{nombreCampo}' es obligatorio.");
                            continue;
                        }

                        if (!met_ValidarCampo(valor, tipoDato, longitudMaxima))
                        {
                            switch (tipoDato.ToLower())
                            {
                                case "varchar":
                                case "char":
                                case "text":
                                    if (longitudMaxima.HasValue && valor.Length > longitudMaxima.Value)
                                        errores.Add($"El campo '{nombreCampo}' excede la longitud máxima permitida ({longitudMaxima.Value} caracteres).");
                                    else
                                        errores.Add($"El campo '{nombreCampo}' contiene caracteres no permitidos.");
                                    break;

                                case "int":
                                case "decimal":
                                case "float":
                                case "numeric":
                                    errores.Add($"El campo '{nombreCampo}' debe ser un valor numérico.");
                                    break;

                                case "datetime":
                                case "date":
                                    errores.Add($"El campo '{nombreCampo}' debe ser una fecha válida.");
                                    break;

                                default:
                                    errores.Add($"El campo '{nombreCampo}' no es válido.");
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar los datos: " + ex.Message, ex);
            }

            return errores;
        }
    }
}