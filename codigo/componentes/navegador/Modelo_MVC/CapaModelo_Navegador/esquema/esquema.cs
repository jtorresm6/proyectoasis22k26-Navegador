using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using CapaEntidades_Navegador;

namespace CapaModelo_Navegador
{
    // Todo lo relacionado a la estructura de una tabla: columnas, llaves, tipos
    public class esquema
    {
        conexionBD conn = new conexionBD();

        public List<string> ObtenerTablas()
        {
            List<string> tablas = new List<string>();
            OdbcConnection conexion = conn.conexion();

            try
            {
                DataTable dtTablas = conexion.GetSchema("Tables");

                foreach (DataRow fila in dtTablas.Rows)
                {
                    string tipo = ValorSeguro(fila, "TABLE_TYPE");

                    // Solo tablas de usuario, se descartan vistas y tablas de sistema
                    if (!string.IsNullOrWhiteSpace(tipo) && tipo.IndexOf("TABLE", StringComparison.OrdinalIgnoreCase) < 0)
                        continue;

                    if (!string.IsNullOrWhiteSpace(tipo) && tipo.IndexOf("SYSTEM", StringComparison.OrdinalIgnoreCase) >= 0)
                        continue;

                    string nombre = ValorSeguro(fila, "TABLE_NAME");

                    if (!string.IsNullOrWhiteSpace(nombre) && !tablas.Contains(nombre, StringComparer.OrdinalIgnoreCase))
                        tablas.Add(nombre);
                }
            }
            finally
            {
                conn.desconexion(conexion);
            }

            tablas.Sort(StringComparer.OrdinalIgnoreCase);
            return tablas;
        }

        public List<string> ObtenerColumnas(string nombreTabla)
        {
            List<string> columnas = new List<string>();
            OdbcConnection conexion = conn.conexion();

            try
            {
                DataTable dtColumnas = conexion.GetSchema("Columns", new string[] { null, null, nombreTabla, null });

                foreach (DataRow fila in dtColumnas.Rows)
                {
                    string nombreColumna = fila["COLUMN_NAME"].ToString();

                    if (!columnas.Contains(nombreColumna))
                        columnas.Add(nombreColumna);
                }
            }
            finally
            {
                conn.desconexion(conexion);
            }

            return columnas;
        }

        // Arma la lista completa de columnas con toda su info ya lista para usar en la Vista
        public List<columnaInfo> ObtenerEsquemaTabla(string nombreTabla)
        {
            validaciones.ValidarIdentificador(nombreTabla);

            List<columnaInfo> lista = new List<columnaInfo>();
            OdbcConnection conexion = conn.conexion();

            try
            {
                DataTable columnas = conexion.GetSchema("Columns", new string[] { null, null, nombreTabla, null });

                HashSet<string> pk = ObtenerLlavesPrimarias(conexion, nombreTabla);
                Dictionary<string, Tuple<string, string>> fk = ObtenerLlavesForaneas(conexion, nombreTabla);
                Dictionary<string, string> tiposNet = ObtenerTiposNet(conexion, nombreTabla);
                Dictionary<string, string> tiposTexto = ObtenerTiposColumnaTexto(conexion, nombreTabla);

                foreach (DataRow fila in columnas.Rows)
                {
                    string nombre = ValorSeguro(fila, "COLUMN_NAME");

                    if (string.IsNullOrWhiteSpace(nombre))
                        continue;

                    columnaInfo col = new columnaInfo();
                    col.Nombre = nombre;
                    col.TipoDato = ValorSeguro(fila, "DATA_TYPE");

                    string tipoNet;
                    tiposNet.TryGetValue(nombre, out tipoNet);
                    col.TipoNet = tipoNet ?? "";

                    string tipoTexto;
                    tiposTexto.TryGetValue(nombre, out tipoTexto);
                    col.TipoColumnaTexto = tipoTexto ?? "";

                    long longitud = 0;
                    long.TryParse(ValorSeguro(fila, "CHARACTER_MAXIMUM_LENGTH"), out longitud);
                    col.Longitud = longitud;

                    long tamano = 0;
                    long.TryParse(ValorSeguro(fila, "COLUMN_SIZE"), out tamano);
                    col.TamanoColumna = tamano;

                    col.Nullable = ValorSeguro(fila, "IS_NULLABLE").Equals("YES", StringComparison.OrdinalIgnoreCase);

                    string autoTexto = ValorSeguro(fila, "IS_AUTOINCREMENT");

                    if (string.IsNullOrWhiteSpace(autoTexto))
                        autoTexto = ValorSeguro(fila, "IS_GENERATEDCOLUMN");

                    col.EsAutoincremento = autoTexto.Equals("YES", StringComparison.OrdinalIgnoreCase)
                        || autoTexto.Equals("TRUE", StringComparison.OrdinalIgnoreCase)
                        || autoTexto == "1";

                    col.EsPK = pk.Contains(nombre);

                    Tuple<string, string> relacion;
                    col.EsFK = fk.TryGetValue(nombre, out relacion);
                    col.TablaFK = col.EsFK ? relacion.Item1 : "";
                    col.ColumnaFK = col.EsFK ? relacion.Item2 : "";

                    lista.Add(col);
                }
            }
            finally
            {
                conn.desconexion(conexion);
            }

            return lista;
        }

        // Se intenta detectar la llave primaria de 3 formas, de la mas estandar a la mas generica
        private HashSet<string> ObtenerLlavesPrimarias(OdbcConnection conexion, string nombreTabla)
        {
            HashSet<string> pk = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            try
            {
                DataTable llaves = conexion.GetSchema("Primary_Keys", new string[] { null, null, nombreTabla });

                foreach (DataRow fila in llaves.Rows)
                {
                    string col = ValorSeguro(fila, "COLUMN_NAME");

                    if (!string.IsNullOrWhiteSpace(col))
                        pk.Add(col);
                }
            }
            catch { }

            if (pk.Count > 0) return pk;

            try
            {
                DataTable indices = conexion.GetSchema("Indexes", new string[] { null, null, nombreTabla });

                foreach (DataRow fila in indices.Rows)
                {
                    string indicador = ValorSeguro(fila, "PRIMARY_KEY");

                    if (string.IsNullOrWhiteSpace(indicador))
                        indicador = ValorSeguro(fila, "INDEX_NAME");

                    bool esPrimaria = indicador.Equals("YES", StringComparison.OrdinalIgnoreCase)
                        || indicador.Equals("TRUE", StringComparison.OrdinalIgnoreCase)
                        || indicador == "1"
                        || indicador.IndexOf("PRIMARY", StringComparison.OrdinalIgnoreCase) >= 0;

                    if (esPrimaria)
                    {
                        string col = ValorSeguro(fila, "COLUMN_NAME");

                        if (!string.IsNullOrWhiteSpace(col))
                            pk.Add(col);
                    }
                }
            }
            catch { }

            if (pk.Count > 0) return pk;

            try
            {
                string sSQL = "SELECT kcu.COLUMN_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc " +
                    "JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu ON tc.CONSTRAINT_NAME = kcu.CONSTRAINT_NAME " +
                    "AND tc.TABLE_SCHEMA = kcu.TABLE_SCHEMA AND tc.TABLE_NAME = kcu.TABLE_NAME " +
                    "WHERE tc.CONSTRAINT_TYPE = 'PRIMARY KEY' AND tc.TABLE_NAME = ?";

                using (OdbcCommand comando = new OdbcCommand(sSQL, conexion))
                {
                    comando.Parameters.AddWithValue("@tabla", nombreTabla);

                    using (OdbcDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            string col = Convert.ToString(lector["COLUMN_NAME"]);

                            if (!string.IsNullOrWhiteSpace(col))
                                pk.Add(col);
                        }
                    }
                }
            }
            catch { }

            return pk;
        }

        // Mismo criterio que las llaves primarias, se intenta de varias formas
        private Dictionary<string, Tuple<string, string>> ObtenerLlavesForaneas(OdbcConnection conexion, string nombreTabla)
        {
            Dictionary<string, Tuple<string, string>> relaciones = new Dictionary<string, Tuple<string, string>>(StringComparer.OrdinalIgnoreCase);

            DataTable fks = null;

            try
            {
                fks = conexion.GetSchema("ForeignKeys", new string[] { null, null, nombreTabla, null, null, null });
            }
            catch
            {
                try { fks = conexion.GetSchema("ForeignKeys"); }
                catch { fks = null; }
            }

            if (fks != null)
            {
                foreach (DataRow fila in fks.Rows)
                {
                    string fkTabla = ValorSeguro(fila, "FK_TABLE_NAME");
                    string fkColumna = ValorSeguro(fila, "FK_COLUMN_NAME");
                    string pkTabla = ValorSeguro(fila, "PK_TABLE_NAME");
                    string pkColumna = ValorSeguro(fila, "PK_COLUMN_NAME");

                    if (string.Equals(fkTabla, nombreTabla, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(fkColumna))
                        relaciones[fkColumna] = Tuple.Create(pkTabla, pkColumna);
                }
            }

            if (relaciones.Count > 0) return relaciones;

            // Respaldo generico via INFORMATION_SCHEMA para motores que no soportan la coleccion ForeignKeys
            try
            {
                string sSQL = "SELECT kcu1.COLUMN_NAME AS FK_COLUMN, kcu2.TABLE_NAME AS PK_TABLE, kcu2.COLUMN_NAME AS PK_COLUMN " +
                    "FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc " +
                    "JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu1 ON rc.CONSTRAINT_NAME = kcu1.CONSTRAINT_NAME AND rc.CONSTRAINT_SCHEMA = kcu1.CONSTRAINT_SCHEMA " +
                    "JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu2 ON rc.UNIQUE_CONSTRAINT_NAME = kcu2.CONSTRAINT_NAME AND rc.UNIQUE_CONSTRAINT_SCHEMA = kcu2.CONSTRAINT_SCHEMA AND kcu1.ORDINAL_POSITION = kcu2.ORDINAL_POSITION " +
                    "WHERE kcu1.TABLE_NAME = ?";

                using (OdbcCommand comando = new OdbcCommand(sSQL, conexion))
                {
                    comando.Parameters.AddWithValue("@tabla", nombreTabla);

                    using (OdbcDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            string fkColumna = Convert.ToString(lector["FK_COLUMN"]);
                            string pkTabla = Convert.ToString(lector["PK_TABLE"]);
                            string pkColumna = Convert.ToString(lector["PK_COLUMN"]);

                            if (!string.IsNullOrWhiteSpace(fkColumna))
                                relaciones[fkColumna] = Tuple.Create(pkTabla, pkColumna);
                        }
                    }
                }
            }
            catch { }

            return relaciones;
        }

        // Se ejecuta una consulta sin filas para que ADO.NET diga el tipo .NET real de cada columna
        private Dictionary<string, string> ObtenerTiposNet(OdbcConnection conexion, string nombreTabla)
        {
            Dictionary<string, string> tipos = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            try
            {
                string sSQL = "SELECT * FROM " + nombreTabla + " WHERE 1 = 0";

                using (OdbcCommand comando = new OdbcCommand(sSQL, conexion))
                using (OdbcDataAdapter da = new OdbcDataAdapter(comando))
                {
                    DataTable vacio = new DataTable();
                    da.Fill(vacio);

                    foreach (DataColumn col in vacio.Columns)
                        tipos[col.ColumnName] = col.DataType.Name;
                }
            }
            catch { }

            return tipos;
        }

        // Solo funciona en MySQL/MariaDB, trae el texto real de la columna como "tinyint(1)"
        private Dictionary<string, string> ObtenerTiposColumnaTexto(OdbcConnection conexion, string nombreTabla)
        {
            Dictionary<string, string> tipos = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            try
            {
                string sSQL = "SELECT COLUMN_NAME, COLUMN_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = ?";

                using (OdbcCommand comando = new OdbcCommand(sSQL, conexion))
                {
                    comando.Parameters.AddWithValue("@tabla", nombreTabla);

                    using (OdbcDataReader lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            string nombre = Convert.ToString(lector["COLUMN_NAME"]);
                            string tipo = Convert.ToString(lector["COLUMN_TYPE"]);

                            if (!string.IsNullOrWhiteSpace(nombre))
                                tipos[nombre] = tipo ?? "";
                        }
                    }
                }
            }
            catch { }

            return tipos;
        }

        // Calcula MAX(columna) + 1 para autogenerar la llave primaria sin depender del motor de BD
        public object ObtenerSiguienteValorLlave(string nombreTabla, string columnaPK)
        {
            validaciones.ValidarIdentificador(nombreTabla);
            validaciones.ValidarIdentificador(columnaPK);

            string sSQL = "SELECT MAX(" + columnaPK + ") FROM " + nombreTabla;
            OdbcConnection conexion = conn.conexion();

            try
            {
                using (OdbcCommand comando = new OdbcCommand(sSQL, conexion))
                {
                    object resultado = comando.ExecuteScalar();

                    if (resultado == null || resultado == DBNull.Value)
                        return (long)1;

                    long maximo;

                    if (long.TryParse(Convert.ToString(resultado), out maximo))
                        return maximo + 1;

                    return null;
                }
            }
            finally
            {
                conn.desconexion(conexion);
            }
        }

        private string ValorSeguro(DataRow fila, string columna)
        {
            if (!fila.Table.Columns.Contains(columna) || fila[columna] == DBNull.Value)
                return "";

            return Convert.ToString(fila[columna]);
        }
    }
}