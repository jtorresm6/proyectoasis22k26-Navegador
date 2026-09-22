using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;

namespace CapaModelo_Navegador
{
    // Todo lo relacionado a la estructura de una tabla: columnas, llaves, tipos
    public class ClsEsquema
    {
        private ClsConexionBD _ConexionBD = new ClsConexionBD();

        public List<string> NavegadorFuncObtenerTablas()
        {
            List<string> Tablas = new List<string>();
            using (OdbcConnection Conexion = _ConexionBD.NavegadorFuncConexion())
            {
                foreach (DataRow Fila in Conexion.GetSchema("Tables").Rows)
                {
                    string Tipo = NavegadorFuncValorSeguro(Fila, "TABLE_TYPE");
                    string Nombre = NavegadorFuncValorSeguro(Fila, "TABLE_NAME");

                    // Solo Tablas de Usuario, se descartan vistas y Tablas de sistema
                    if ((string.IsNullOrWhiteSpace(Tipo) || Tipo.IndexOf("TABLE", StringComparison.OrdinalIgnoreCase) >= 0) &&
                        Tipo.IndexOf("SYSTEM", StringComparison.OrdinalIgnoreCase) < 0 &&
                        !string.IsNullOrWhiteSpace(Nombre) &&
                        !Tablas.Contains(Nombre, StringComparer.OrdinalIgnoreCase))
                        Tablas.Add(Nombre);
                }
            }

            Tablas.Sort(StringComparer.OrdinalIgnoreCase);
            return Tablas;
        }

        public List<string> NavegadorFuncObtenerColumnas(string NombreTabla)
        {
            List<string> Columnas = new List<string>();

            using (OdbcConnection Conexion = _ConexionBD.NavegadorFuncConexion())
                foreach (DataRow Fila in Conexion.GetSchema("Columns",
                    new string[] { null, null, NombreTabla, null }).Rows)
                    Columnas.Add(Convert.ToString(Fila["COLUMN_NAME"]));

            return Columnas;
        }

        // Arma la Lista completa de columnas con toda su info ya Lista para usar en la Vista
        public List<ClsColumnaInfo> NavegadorFuncObtenerEsquemaTabla(string NombreTabla)
        {
            ClsValidaciones.NavegadorMetValidarIdentificador(NombreTabla);
            List<ClsColumnaInfo> Lista = new List<ClsColumnaInfo>();

            using (OdbcConnection Conexion = _ConexionBD.NavegadorFuncConexion())
            {
                DataTable Columnas = Conexion.GetSchema("Columns",
                    new string[] { null, null, NombreTabla, null });

                HashSet<string> ClavesPrimarias = NavegadorFuncObtenerLlavesPrimarias(Conexion, NombreTabla);
                Dictionary<string, Tuple<string, string>> LlavesForaneas = NavegadorFuncObtenerLlavesForaneas(Conexion, NombreTabla);
                Dictionary<string, string> TiposNet = NavegadorFuncObtenerTiposNet(Conexion, NombreTabla);
                Dictionary<string, string> TiposTexto = NavegadorFuncObtenerTiposColumnaTexto(Conexion, NombreTabla);

                foreach (DataRow Fila in Columnas.Rows)
                {
                    string Nombre = NavegadorFuncValorSeguro(Fila, "COLUMN_NAME");
                    if (string.IsNullOrWhiteSpace(Nombre)) continue;

                    string TipoNet, TipoTexto;
                    long Longitud, TamanoColumna;
                    Tuple<string, string> Relacion;

                    TiposNet.TryGetValue(Nombre, out TipoNet);
                    TiposTexto.TryGetValue(Nombre, out TipoTexto);
                    long.TryParse(NavegadorFuncValorSeguro(Fila, "CHARACTER_MAXIMUM_LENGTH"), out Longitud);
                    long.TryParse(NavegadorFuncValorSeguro(Fila, "COLUMN_SIZE"), out TamanoColumna);

                    string AutoTexto = NavegadorFuncValorSeguro(Fila, "IS_AUTOINCREMENT");
                    if (string.IsNullOrWhiteSpace(AutoTexto))
                        AutoTexto = NavegadorFuncValorSeguro(Fila, "IS_GENERATEDCOLUMN");

                    bool EsFK = LlavesForaneas.TryGetValue(Nombre, out Relacion);

                    Lista.Add(new ClsColumnaInfo
                    {
                        Nombre = Nombre,
                        TipoDato = NavegadorFuncValorSeguro(Fila, "DATA_TYPE"),
                        TipoNet = TipoNet ?? "",
                        TipoColumnaTexto = TipoTexto ?? "",
                        Longitud = Longitud,
                        TamanoColumna = TamanoColumna,
                        Nullable = NavegadorFuncValorSeguro(Fila, "IS_NULLABLE").Equals("YES", StringComparison.OrdinalIgnoreCase),
                        EsAutoincremento = AutoTexto.Equals("YES", StringComparison.OrdinalIgnoreCase) ||
                                          AutoTexto.Equals("TRUE", StringComparison.OrdinalIgnoreCase) || AutoTexto == "1",
                        EsPK = ClavesPrimarias.Contains(Nombre),
                        EsFK = EsFK,
                        TablaFK = EsFK ? Relacion.Item1 : "",
                        ColumnaFK = EsFK ? Relacion.Item2 : ""
                    });
                }
            }

            return Lista;
        }

        // Se intenta detectar la llave primaria de 3 formas, de la mas estandar a la mas generica
        private HashSet<string> NavegadorFuncObtenerLlavesPrimarias(OdbcConnection Conexion, string NombreTabla)
        {
            HashSet<string> ClavesPrimarias = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            try
            {
                foreach (DataRow Fila in Conexion.GetSchema("Primary_Keys",
                    new string[] { null, null, NombreTabla }).Rows)
                {
                    string Columna = NavegadorFuncValorSeguro(Fila, "COLUMN_NAME");
                    if (!string.IsNullOrWhiteSpace(Columna)) ClavesPrimarias.Add(Columna);
                }
            }
            catch { }

            if (ClavesPrimarias.Count > 0) return ClavesPrimarias;

            try
            {
                foreach (DataRow Fila in Conexion.GetSchema("Indexes",
                    new string[] { null, null, NombreTabla }).Rows)
                {
                    string Indicador = NavegadorFuncValorSeguro(Fila, "PRIMARY_KEY");
                    if (string.IsNullOrWhiteSpace(Indicador))
                        Indicador = NavegadorFuncValorSeguro(Fila, "INDEX_NAME");

                    if (Indicador.Equals("YES", StringComparison.OrdinalIgnoreCase) ||
                        Indicador.Equals("TRUE", StringComparison.OrdinalIgnoreCase) ||
                        Indicador == "1" ||
                        Indicador.IndexOf("PRIMARY", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        string Columna = NavegadorFuncValorSeguro(Fila, "COLUMN_NAME");
                        if (!string.IsNullOrWhiteSpace(Columna)) ClavesPrimarias.Add(Columna);
                    }
                }
            }
            catch { }

            if (ClavesPrimarias.Count > 0) return ClavesPrimarias;

            try
            {
                string ConsultaSQL =
                    "SELECT kcu.COLUMN_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc " +
                    "JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu ON tc.CONSTRAINT_NAME=kcu.CONSTRAINT_NAME " +
                    "AND tc.TABLE_SCHEMA=kcu.TABLE_SCHEMA AND tc.TABLE_NAME=kcu.TABLE_NAME " +
                    "WHERE tc.CONSTRAINT_TYPE='PRIMARY KEY' AND tc.TABLE_SCHEMA=DATABASE() AND tc.TABLE_NAME=?";

                using (OdbcCommand Comando = new OdbcCommand(ConsultaSQL, Conexion))
                {
                    Comando.Parameters.AddWithValue("@tabla", NombreTabla);
                    using (OdbcDataReader Lector = Comando.ExecuteReader())
                        while (Lector.Read())
                            ClavesPrimarias.Add(Convert.ToString(Lector["COLUMN_NAME"]));
                }
            }
            catch { }

            return ClavesPrimarias;
        }


        private Dictionary<string, Tuple<string, string>> NavegadorFuncObtenerLlavesForaneas(OdbcConnection Conexion, string NombreTabla)
        {
            Dictionary<string, Tuple<string, string>> Relaciones =
                new Dictionary<string, Tuple<string, string>>(StringComparer.OrdinalIgnoreCase);

            try
            {
                string ConsultaSQL =
                    "SELECT COLUMN_NAME, REFERENCED_TABLE_NAME, REFERENCED_COLUMN_NAME " +
                    "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE " +
                    "WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME=? AND REFERENCED_TABLE_NAME IS NOT NULL";

                using (OdbcCommand Comando = new OdbcCommand(ConsultaSQL, Conexion))
                {
                    Comando.Parameters.AddWithValue("@tabla", NombreTabla);
                    using (OdbcDataReader Lector = Comando.ExecuteReader())
                        while (Lector.Read())
                            Relaciones[Convert.ToString(Lector["COLUMN_NAME"])] = Tuple.Create(
                                Convert.ToString(Lector["REFERENCED_TABLE_NAME"]),
                                Convert.ToString(Lector["REFERENCED_COLUMN_NAME"]));
                }
            }
            catch { }

            if (Relaciones.Count > 0) return Relaciones;

            // Respaldo generico via REFERENTIAL_CONSTRAINTS, por si el motor no expone
            // REFERENCED_TABLE_NAME directamente en KEY_COLUMN_USAGE (motores distintos a MySQL/MariaDB)
            try
            {
                string ConsultaSQL =
                    "SELECT kcu1.COLUMN_NAME AS FK_COLUMN,kcu2.TABLE_NAME AS PK_TABLE,kcu2.COLUMN_NAME AS PK_COLUMN " +
                    "FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc " +
                    "JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu1 ON rc.CONSTRAINT_NAME=kcu1.CONSTRAINT_NAME AND rc.CONSTRAINT_SCHEMA=kcu1.CONSTRAINT_SCHEMA " +
                    "JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE kcu2 ON rc.UNIQUE_CONSTRAINT_NAME=kcu2.CONSTRAINT_NAME AND rc.UNIQUE_CONSTRAINT_SCHEMA=kcu2.CONSTRAINT_SCHEMA AND kcu1.ORDINAL_POSITION=kcu2.ORDINAL_POSITION " +
                    "WHERE kcu1.TABLE_SCHEMA=DATABASE() AND kcu1.TABLE_NAME=?";

                using (OdbcCommand Comando = new OdbcCommand(ConsultaSQL, Conexion))
                {
                    Comando.Parameters.AddWithValue("@tabla", NombreTabla);
                    using (OdbcDataReader Lector = Comando.ExecuteReader())
                        while (Lector.Read())
                            Relaciones[Convert.ToString(Lector["FK_COLUMN"])] = Tuple.Create(
                                Convert.ToString(Lector["PK_TABLE"]),
                                Convert.ToString(Lector["PK_COLUMN"]));
                }
            }
            catch { }

            if (Relaciones.Count > 0) return Relaciones;

            // Ultimo recurso: coleccion ODBC "ForeignKeys". Se valida cada fila contra NombreTabla
            // por si el driver no respeta el orden esperado de restricciones.
            try
            {
                foreach (DataRow Fila in Conexion.GetSchema("ForeignKeys").Rows)
                {
                    string TablaFK = NavegadorFuncValorSeguro(Fila, "FK_TABLE_NAME");
                    string ColumnaFK = NavegadorFuncValorSeguro(Fila, "FK_COLUMN_NAME");

                    if (string.Equals(TablaFK, NombreTabla, StringComparison.OrdinalIgnoreCase) &&
                        !string.IsNullOrWhiteSpace(ColumnaFK))
                        Relaciones[ColumnaFK] = Tuple.Create(
                            NavegadorFuncValorSeguro(Fila, "PK_TABLE_NAME"),
                            NavegadorFuncValorSeguro(Fila, "PK_COLUMN_NAME"));
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
                using (OdbcDataAdapter AdaptadorDatos =
                    new OdbcDataAdapter("SELECT * FROM " + NombreTabla + " WHERE 1=0", Conexion))
                {
                    DataTable Tabla = new DataTable();
                    AdaptadorDatos.Fill(Tabla);

                    foreach (DataColumn Columna in Tabla.Columns)
                        Tipos[Columna.ColumnName] = Columna.DataType.Name;
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
                using (OdbcCommand Comando = new OdbcCommand(
                    "SELECT COLUMN_NAME,COLUMN_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA=DATABASE() AND TABLE_NAME=?", Conexion))
                {
                    Comando.Parameters.AddWithValue("@tabla", NombreTabla);

                    using (OdbcDataReader Lector = Comando.ExecuteReader())
                        while (Lector.Read())
                            Tipos[Convert.ToString(Lector["COLUMN_NAME"])] =
                                Convert.ToString(Lector["COLUMN_TYPE"]);
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

            using (OdbcConnection Conexion = _ConexionBD.NavegadorFuncConexion())
            using (OdbcCommand Comando = new OdbcCommand(
                "SELECT MAX(" + ColumnaPK + ") FROM " + NombreTabla, Conexion))
            {
                object Resultado = Comando.ExecuteScalar();
                long Maximo;

                if (Resultado == null || Resultado == DBNull.Value) return (long)1;
                return long.TryParse(Convert.ToString(Resultado), out Maximo) ? (object)(Maximo + 1) : null;
            }
        }

        private string NavegadorFuncValorSeguro(DataRow Fila, string Columna)
        {
            return !Fila.Table.Columns.Contains(Columna) || Fila[Columna] == DBNull.Value
                ? "" : Convert.ToString(Fila[Columna]);
        }
    }
}