/*
 * Nombre: Gabriel André Guillén Pocón
 * Carnet: 0901-23-420 (placeholder)
 * Fecha: 16/09/2026
 * Descripción: Clase encargada de manejar el esquema de la base de datos (tablas, columnas, llaves).
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using CapaEntidades_Navegador;

namespace CapaModelo_Navegador
{
    // Todo lo relacionado a la estructura de una tabla: columnas, llaves, tipos
    public class ClsEsquema
    {
        ClsConexionBD _ConexionBD = new ClsConexionBD();

        // Este método nos devuelve una lista con todos los nombres de las tablas de la base de datos
        public List<string> NavegadorFuncObtenerTablas()
        {
            List<string> Tablas = new List<string>();
            OdbcConnection Conexion = _ConexionBD.NavegadorFuncConexion();

            try
            {
                DataTable TablaTablas = Conexion.GetSchema("Tables");

                foreach (DataRow Fila in TablaTablas.Rows)
                {
                    string Tipo = NavegadorFuncValorSeguro(Fila, "TABLE_TYPE");

                    // Solo Tablas de Usuario, se descartan vistas y Tablas de sistema
                    if (!string.IsNullOrWhiteSpace(Tipo) && Tipo.IndexOf("TABLE", StringComparison.OrdinalIgnoreCase) < 0)
                        continue;

                    if (!string.IsNullOrWhiteSpace(Tipo) && Tipo.IndexOf("SYSTEM", StringComparison.OrdinalIgnoreCase) >= 0)
                        continue;

                    string Nombre = NavegadorFuncValorSeguro(Fila, "TABLE_NAME");

                    if (!string.IsNullOrWhiteSpace(Nombre) && !Tablas.Contains(Nombre, StringComparer.OrdinalIgnoreCase))
                        Tablas.Add(Nombre);
                }
            }
            finally
            {
                _ConexionBD.NavegadorMetDesconexion(Conexion);
            }

            Tablas.Sort(StringComparer.OrdinalIgnoreCase);
            return Tablas;
        }

        // Con este método, si le pasamos el nombre de una tabla, nos da todos los nombres de sus columnas
        public List<string> NavegadorFuncObtenerColumnas(string NombreTabla)
        {
            List<string> Columnas = new List<string>();
            OdbcConnection Conexion = _ConexionBD.NavegadorFuncConexion();

            try
            {
                DataTable TablaColumnas = Conexion.GetSchema("Columns", new string[] { null, null, NombreTabla, null });

                foreach (DataRow Fila in TablaColumnas.Rows)
                {
                    string NombreColumna = Fila["COLUMN_NAME"].ToString();

                    if (!Columnas.Contains(NombreColumna))
                        Columnas.Add(NombreColumna);
                }
            }
            finally
            {
                _ConexionBD.NavegadorMetDesconexion(Conexion);
            }

            return Columnas;
        }

        // Arma la Lista completa de columnas con toda su info ya Lista para usar en la Vista
        public List<ClsColumnaInfo> NavegadorFuncObtenerEsquemaTabla(string NombreTabla)
        {
            ClsValidaciones.NavegadorMetValidarIdentificador(NombreTabla);

            List<ClsColumnaInfo> Lista = new List<ClsColumnaInfo>();
            OdbcConnection Conexion = _ConexionBD.NavegadorFuncConexion();

            try
            {
                DataTable Columnas = Conexion.GetSchema("Columns", new string[] { null, null, NombreTabla, null });

                HashSet<string> ClavesPrimarias = NavegadorFuncObtenerLlavesPrimarias(Conexion, NombreTabla);
                Dictionary<string, Tuple<string, string>> LlavesForaneas = NavegadorFuncObtenerLlavesForaneas(Conexion, NombreTabla);
                Dictionary<string, string> TiposNet = NavegadorFuncObtenerTiposNet(Conexion, NombreTabla);
                Dictionary<string, string> TiposTexto = NavegadorFuncObtenerTiposColumnaTexto(Conexion, NombreTabla);

                foreach (DataRow Fila in Columnas.Rows)
                {
                    string Nombre = NavegadorFuncValorSeguro(Fila, "COLUMN_NAME");

                    if (string.IsNullOrWhiteSpace(Nombre))
                        continue;

                    ClsColumnaInfo Col = new ClsColumnaInfo();
                    Col.Nombre = Nombre;
                    Col.TipoDato = NavegadorFuncValorSeguro(Fila, "DATA_TYPE");

                    string TipoNet;
                    TiposNet.TryGetValue(Nombre, out TipoNet);
                    Col.TipoNet = TipoNet ?? "";

                    string TipoTexto;
                    TiposTexto.TryGetValue(Nombre, out TipoTexto);
                    Col.TipoColumnaTexto = TipoTexto ?? "";

                    long Longitud = 0;
                    long.TryParse(NavegadorFuncValorSeguro(Fila, "CHARACTER_MAXIMUM_LENGTH"), out Longitud);
                    Col.Longitud = Longitud;

                    long Tamano = 0;
                    long.TryParse(NavegadorFuncValorSeguro(Fila, "COLUMN_SIZE"), out Tamano);
                    Col.TamanoColumna = Tamano;

                    Col.Nullable = NavegadorFuncValorSeguro(Fila, "IS_NULLABLE").Equals("YES", StringComparison.OrdinalIgnoreCase);

                    string AutoTexto = NavegadorFuncValorSeguro(Fila, "IS_AUTOINCREMENT");

                    if (string.IsNullOrWhiteSpace(AutoTexto))
                        AutoTexto = NavegadorFuncValorSeguro(Fila, "IS_GENERATEDCOLUMN");

                    Col.EsAutoincremento = AutoTexto.Equals("YES", StringComparison.OrdinalIgnoreCase)
                        || AutoTexto.Equals("TRUE", StringComparison.OrdinalIgnoreCase)
                        || AutoTexto == "1";

                    Col.EsPK = ClavesPrimarias.Contains(Nombre);

                    Tuple<string, string> Relacion;
                    Col.EsFK = LlavesForaneas.TryGetValue(Nombre, out Relacion);
                    Col.TablaFK = Col.EsFK ? Relacion.Item1 : "";
                    Col.ColumnaFK = Col.EsFK ? Relacion.Item2 : "";

                    Lista.Add(Col);
                }
            }
            finally
            {
                _ConexionBD.NavegadorMetDesconexion(Conexion);
            }

            return Lista;
        }

        // Se intenta detectar la llave primaria de 3 formas, de la mas estandar a la mas generica
        private HashSet<string> NavegadorFuncObtenerLlavesPrimarias(OdbcConnection Conexion, string NombreTabla)
        {
            HashSet<string> ClavesPrimarias = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            try
            {
                DataTable Llaves = Conexion.GetSchema("Primary_Keys", new string[] { null, null, NombreTabla });

                foreach (DataRow Fila in Llaves.Rows)
                {
                    string Col = NavegadorFuncValorSeguro(Fila, "COLUMN_NAME");

                    if (!string.IsNullOrWhiteSpace(Col))
                        ClavesPrimarias.Add(Col);
                }
            }
            catch { }

            if (ClavesPrimarias.Count > 0) return ClavesPrimarias;

            try
            {
                DataTable Indices = Conexion.GetSchema("Indexes", new string[] { null, null, NombreTabla });

                foreach (DataRow Fila in Indices.Rows)
                {
                    string Indicador = NavegadorFuncValorSeguro(Fila, "PRIMARY_KEY");

                    if (string.IsNullOrWhiteSpace(Indicador))
                        Indicador = NavegadorFuncValorSeguro(Fila, "INDEX_NAME");

                    bool EsPrimaria = Indicador.Equals("YES", StringComparison.OrdinalIgnoreCase)
                        || Indicador.Equals("TRUE", StringComparison.OrdinalIgnoreCase)
                        || Indicador == "1"
                        || Indicador.IndexOf("PRIMARY", StringComparison.OrdinalIgnoreCase) >= 0;

                    if (EsPrimaria)
                    {
                        string Col = NavegadorFuncValorSeguro(Fila, "COLUMN_NAME");

                        if (!string.IsNullOrWhiteSpace(Col))
                            ClavesPrimarias.Add(Col);
                    }
                }
            }
            catch { }

            if (ClavesPrimarias.Count > 0) return ClavesPrimarias;

            try
            {
                string ConsultaSQL = "SELECT kcu.COLUMN_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc " +
                    "JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu ON tc.CONSTRAINT_NAME = kcu.CONSTRAINT_NAME " +
                    "AND tc.TABLE_SCHEMA = kcu.TABLE_SCHEMA AND tc.TABLE_NAME = kcu.TABLE_NAME " +
                    "WHERE tc.CONSTRAINT_TYPE = 'PRIMARY KEY' AND tc.TABLE_NAME = ?";

                using (OdbcCommand Comando = new OdbcCommand(ConsultaSQL, Conexion))
                {
                    Comando.Parameters.AddWithValue("@tabla", NombreTabla);

                    using (OdbcDataReader Lector = Comando.ExecuteReader())
                    {
                        while (Lector.Read())
                        {
                            string Col = Convert.ToString(Lector["COLUMN_NAME"]);

                            if (!string.IsNullOrWhiteSpace(Col))
                                ClavesPrimarias.Add(Col);
                        }
                    }
                }
            }
            catch { }

            return ClavesPrimarias;
        }

        // Mismo criterio que las llaves primarias, se intenta de varias formas
        private Dictionary<string, Tuple<string, string>> NavegadorFuncObtenerLlavesForaneas(OdbcConnection Conexion, string NombreTabla)
        {
            Dictionary<string, Tuple<string, string>> Relaciones = new Dictionary<string, Tuple<string, string>>(StringComparer.OrdinalIgnoreCase);

            DataTable Fks = null;

            try
            {
                Fks = Conexion.GetSchema("ForeignKeys", new string[] { null, null, NombreTabla, null, null, null });
            }
            catch
            {
                try { Fks = Conexion.GetSchema("ForeignKeys"); }
                catch { Fks = null; }
            }

            if (Fks != null)
            {
                foreach (DataRow Fila in Fks.Rows)
                {
                    string FkTabla = NavegadorFuncValorSeguro(Fila, "FK_TABLE_NAME");
                    string FkColumna = NavegadorFuncValorSeguro(Fila, "FK_COLUMN_NAME");
                    string PkTabla = NavegadorFuncValorSeguro(Fila, "PK_TABLE_NAME");
                    string PkColumna = NavegadorFuncValorSeguro(Fila, "PK_COLUMN_NAME");

                    if (string.Equals(FkTabla, NombreTabla, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(FkColumna))
                        Relaciones[FkColumna] = Tuple.Create(PkTabla, PkColumna);
                }
            }

            if (Relaciones.Count > 0) return Relaciones;

            // Respaldo generico via INFORMATION_SCHEMA para motores que no soportan la coleccion ForeignKeys
            try
            {
                string ConsultaSQL = "SELECT kcu1.COLUMN_NAME AS FK_COLUMN, kcu2.TABLE_NAME AS PK_TABLE, kcu2.COLUMN_NAME AS PK_COLUMN " +
                    "FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc " +
                    "JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu1 ON rc.CONSTRAINT_NAME = kcu1.CONSTRAINT_NAME AND rc.CONSTRAINT_SCHEMA = kcu1.CONSTRAINT_SCHEMA " +
                    "JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu2 ON rc.UNIQUE_CONSTRAINT_NAME = kcu2.CONSTRAINT_NAME AND rc.UNIQUE_CONSTRAINT_SCHEMA = kcu2.CONSTRAINT_SCHEMA AND kcu1.ORDINAL_POSITION = kcu2.ORDINAL_POSITION " +
                    "WHERE kcu1.TABLE_NAME = ?";

                using (OdbcCommand Comando = new OdbcCommand(ConsultaSQL, Conexion))
                {
                    Comando.Parameters.AddWithValue("@tabla", NombreTabla);

                    using (OdbcDataReader Lector = Comando.ExecuteReader())
                    {
                        while (Lector.Read())
                        {
                            string FkColumna = Convert.ToString(Lector["FK_COLUMN"]);
                            string PkTabla = Convert.ToString(Lector["PK_TABLE"]);
                            string PkColumna = Convert.ToString(Lector["PK_COLUMN"]);

                            if (!string.IsNullOrWhiteSpace(FkColumna))
                                Relaciones[FkColumna] = Tuple.Create(PkTabla, PkColumna);
                        }
                    }
                }
            }
            catch { }

            return Relaciones;
        }

        // Se ejecuta una consulta sin filas para que ADO.NET diga el tipo .NET real de cada Columna
        private Dictionary<string, string> NavegadorFuncObtenerTiposNet(OdbcConnection Conexion, string NombreTabla)
        {
            Dictionary<string, string> Tipos = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            try
            {
                string ConsultaSQL = "SELECT * FROM " + NombreTabla + " WHERE 1 = 0";

                using (OdbcCommand Comando = new OdbcCommand(ConsultaSQL, Conexion))
                using (OdbcDataAdapter AdaptadorDatos = new OdbcDataAdapter(Comando))
                {
                    DataTable Vacio = new DataTable();
                    AdaptadorDatos.Fill(Vacio);

                    foreach (DataColumn Col in Vacio.Columns)
                        Tipos[Col.ColumnName] = Col.DataType.Name;
                }
            }
            catch { }

            return Tipos;
        }

        // Solo funciona en MySQL/MariaDB, trae el texto real de la Columna como "tinyint(1)"
        private Dictionary<string, string> NavegadorFuncObtenerTiposColumnaTexto(OdbcConnection Conexion, string NombreTabla)
        {
            Dictionary<string, string> Tipos = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            try
            {
                string ConsultaSQL = "SELECT COLUMN_NAME, COLUMN_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = ?";

                using (OdbcCommand Comando = new OdbcCommand(ConsultaSQL, Conexion))
                {
                    Comando.Parameters.AddWithValue("@tabla", NombreTabla);

                    using (OdbcDataReader Lector = Comando.ExecuteReader())
                    {
                        while (Lector.Read())
                        {
                            string Nombre = Convert.ToString(Lector["COLUMN_NAME"]);
                            string Tipo = Convert.ToString(Lector["COLUMN_TYPE"]);

                            if (!string.IsNullOrWhiteSpace(Nombre))
                                Tipos[Nombre] = Tipo ?? "";
                        }
                    }
                }
            }
            catch { }

            return Tipos;
        }

        // Calcula MAX(Columna) + 1 para autogenerar la llave primaria sin depender del motor de BD
        public object NavegadorFuncObtenerSiguienteValorLlave(string NombreTabla, string ColumnaPK)
        {
            ClsValidaciones.NavegadorMetValidarIdentificador(NombreTabla);
            ClsValidaciones.NavegadorMetValidarIdentificador(ColumnaPK);

            string ConsultaSQL = "SELECT MAX(" + ColumnaPK + ") FROM " + NombreTabla;
            OdbcConnection Conexion = _ConexionBD.NavegadorFuncConexion();

            try
            {
                using (OdbcCommand Comando = new OdbcCommand(ConsultaSQL, Conexion))
                {
                    object Resultado = Comando.ExecuteScalar();

                    if (Resultado == null || Resultado == DBNull.Value)
                        return (long)1;

                    long Maximo;

                    if (long.TryParse(Convert.ToString(Resultado), out Maximo))
                        return Maximo + 1;

                    return null;
                }
            }
            finally
            {
                _ConexionBD.NavegadorMetDesconexion(Conexion);
            }
        }

        // Función para evitar errores si la columna viene nula desde la base de datos
        private string NavegadorFuncValorSeguro(DataRow Fila, string Columna)
        {
            if (!Fila.Table.Columns.Contains(Columna) || Fila[Columna] == DBNull.Value)
                return "";

            return Convert.ToString(Fila[Columna]);
        }
    }
}

/*
 * Fin de la clase ClsEsquema
 * Nombre: Gabriel André Guillén Pocón
 * Fecha: 16/09/2026
 */