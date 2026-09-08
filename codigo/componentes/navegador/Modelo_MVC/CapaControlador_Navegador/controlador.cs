using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaModelo_Navegador;

namespace CapaControlador_Navegador
{
    public class Controlador
    {
        Sentencias sentencias = new Sentencias();

        public DataTable llenarDgv(string nombreTabla)
        {
            DataTable dtControlador = new DataTable();
            try
            {
                using (OdbcDataAdapter daControlador = sentencias.llenarTbl(nombreTabla))
                {
                    daControlador.Fill(dtControlador);

                    // Cierra la conexión ODBC retenida por el adaptador para liberar el socket
                    if (daControlador.SelectCommand != null && daControlador.SelectCommand.Connection != null)
                    {
                        daControlador.SelectCommand.Connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al cargar la tabla '{nombreTabla}': {ex.Message}", ex);
            }

            return dtControlador;
        }

        public DataTable ConsultarEmpleados()
        {
            try
<<<<<<< HEAD
            {
                return sentencias.ConsultarEmpleados();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar la tabla de empleados: " + ex.Message, ex);
            }
=======
            {
                return sentencias.ConsultarEmpleados();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar la tabla de empleados: " + ex.Message, ex);
            }
        }

        public List<string> ObtenerColumnas(string nombreTabla)
        {
            return sentencias.ObtenerColumnas(nombreTabla);
        }

        public bool ExisteLlavePrimaria(
            string nombreTabla,
            string[] camposPK,
            string[] valoresPK)
        {
            return sentencias.ExisteLlavePrimaria(
                nombreTabla,
                camposPK,
                valoresPK
            );
        }

        public bool ExisteValorCampo(
            string nombreTabla,
            string nombreCampo,
            string valor)
        {
            return sentencias.ExisteValorCampo(
                nombreTabla,
                nombreCampo,
                valor
            );
        }

        public bool InsertarRegistro(
            string nombreTabla,
            Dictionary<string, string> datos)
        {
            return sentencias.InsertarRegistro(
                nombreTabla,
                datos
            );
        }

        public DataTable ObtenerEsquemaTabla(string nombreTabla)
        {
            return sentencias.ObtenerEsquemaTabla(nombreTabla);
        }

        private bool ValidarCampo(string valor, string tipoDato, int? longitudMaxima)
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
                    if (!System.Text.RegularExpressions.Regex.IsMatch(valor, @"^[\p{L}\p{N}\s\-_\.]+$"))
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
                    if (!System.Text.RegularExpressions.Regex.IsMatch(valor, @"^[0-9]+(\.[0-9]+)?$"))
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

        public List<string> ValidarRegistro(Dictionary<string, string> datos, string nombreTabla)
        {
            List<string> errores = new List<string>();

            try
            {
                DataTable esquema = sentencias.ObtenerEsquemaTabla(nombreTabla);

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

                        if (!ValidarCampo(valor, tipoDato, longitudMaxima))
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

        public void guardarDatos(string query)
        {
            sentencias.ejecutarSql(query);
        }

        public DataTable ObtenerEsquemaCampos(string nombreTabla)
        {
            return sentencias.ObtenerEsquemaCampos(nombreTabla);
        }

        public List<string> ObtenerLlavePrimaria(string nombreTabla)
        {
            List<string> llave = new List<string>();

            DataTable esquema = sentencias.ObtenerEsquemaCampos(nombreTabla);

            foreach (DataRow columna in esquema.Rows)
            {
                if (columna["COLUMN_KEY"].ToString().ToUpper() == "PRI")
                {
                    llave.Add(columna["COLUMN_NAME"].ToString());
                }
            }

            if (llave.Count == 0 && esquema.Rows.Count > 0)
            {
                llave.Add(esquema.Rows[0]["COLUMN_NAME"].ToString());
            }

            return llave;
>>>>>>> ffb0f2c (correcion en uso de capa en campos dinamicos)
        }

        public List<string> ObtenerColumnas(string nombreTabla)
        {
            return sentencias.ObtenerColumnas(nombreTabla);
        }

        public bool ExisteLlavePrimaria(
            string nombreTabla,
            string[] camposPK,
            string[] valoresPK)
        {
            return sentencias.ExisteLlavePrimaria(
                nombreTabla,
                camposPK,
                valoresPK
            );
        }

        public bool ExisteValorCampo(
            string nombreTabla,
            string nombreCampo,
            string valor)
        {
            return sentencias.ExisteValorCampo(
                nombreTabla,
                nombreCampo,
                valor
            );
        }

        public bool InsertarRegistro(
            string nombreTabla,
            Dictionary<string, string> datos)
        {
            return sentencias.InsertarRegistro(
                nombreTabla,
                datos
            );
        }

        public DataTable ObtenerEsquemaTabla(string nombreTabla)
        {
            return sentencias.ObtenerEsquemaTabla(nombreTabla);
        }

        private bool ValidarCampo(string valor, string tipoDato, int? longitudMaxima)
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
                    if (!System.Text.RegularExpressions.Regex.IsMatch(valor, @"^[\p{L}\p{N}\s\-_\.]+$"))
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
                    if (!System.Text.RegularExpressions.Regex.IsMatch(valor, @"^[0-9]+(\.[0-9]+)?$"))
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

        public List<string> ValidarRegistro(Dictionary<string, string> datos, string nombreTabla)
        {
            List<string> errores = new List<string>();

            try
            {
                DataTable esquema = sentencias.ObtenerEsquemaTabla(nombreTabla);

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

                        if (!ValidarCampo(valor, tipoDato, longitudMaxima))
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

<<<<<<< HEAD
        public void guardarDatos(string query)
        {
            sentencias.ejecutarSql(query);
=======
        public DataTable Consultar(string nombreTabla, IList<Criterio> criterios)
        {
            try
            {
                return sentencias.Consultar(nombreTabla, aFiltros(criterios));
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar la tabla '" + nombreTabla + "': " + ex.Message, ex);
            }
        }

        public int EliminarRegistro(string nombreTabla, IList<Criterio> llave)
        {
            try
            {
                return sentencias.EliminarRegistro(nombreTabla, aFiltros(llave));
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar en la tabla '" + nombreTabla + "': " + ex.Message, ex);
            }
>>>>>>> ffb0f2c (correcion en uso de capa en campos dinamicos)
        }
    }
}