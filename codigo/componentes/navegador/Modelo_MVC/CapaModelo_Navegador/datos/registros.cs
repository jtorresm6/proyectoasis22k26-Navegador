using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Text;

namespace CapaModelo_Navegador
{
    // Todo lo que consulta o modifica los datos de una tabla (no metadatos)
    public class registros
    {
        conexionBD conn = new conexionBD();

        public OdbcDataAdapter llenarTbl(string nombreTabla)
        {
            validaciones.ValidarIdentificador(nombreTabla);
            string sSQL = "SELECT * FROM " + nombreTabla;
            OdbcConnection conexion = conn.conexion();
            return new OdbcDataAdapter(sSQL, conexion);
        }

        public DataTable ConsultarTodo(string nombreTabla)
        {
            validaciones.ValidarIdentificador(nombreTabla);
            string sSQL = "SELECT * FROM " + nombreTabla;
            OdbcConnection conexion = conn.conexion();
            DataTable dt = new DataTable();

            try
            {
                using (OdbcDataAdapter da = new OdbcDataAdapter(sSQL, conexion))
                    da.Fill(dt);
            }
            finally
            {
                conn.desconexion(conexion);
            }

            return dt;
        }

        public bool ExisteLlavePrimaria(string nombreTabla, string[] camposPK, string[] valoresPK)
        {
            if (camposPK == null || camposPK.Length == 0) return false;
            if (valoresPK == null || valoresPK.Length != camposPK.Length) return false;

            validaciones.ValidarIdentificador(nombreTabla);

            string condiciones = "";

            for (int i = 0; i < camposPK.Length; i++)
            {
                validaciones.ValidarIdentificador(camposPK[i]);

                if (i > 0) condiciones += " AND ";
                condiciones += camposPK[i] + " = ?";
            }

            string sSQL = "SELECT COUNT(*) FROM " + nombreTabla + " WHERE " + condiciones;
            OdbcConnection conexion = conn.conexion();

            try
            {
                using (OdbcCommand comando = new OdbcCommand(sSQL, conexion))
                {
                    for (int i = 0; i < valoresPK.Length; i++)
                        comando.Parameters.AddWithValue("@p" + i, valoresPK[i]);

                    int cantidad = Convert.ToInt32(comando.ExecuteScalar());
                    return cantidad > 0;
                }
            }
            finally
            {
                conn.desconexion(conexion);
            }
        }

        public bool ExisteValorCampo(string nombreTabla, string nombreCampo, string valor)
        {
            validaciones.ValidarIdentificador(nombreTabla);
            validaciones.ValidarIdentificador(nombreCampo);

            string sSQL = "SELECT COUNT(*) FROM " + nombreTabla + " WHERE " + nombreCampo + " = ?";
            OdbcConnection conexion = conn.conexion();

            try
            {
                using (OdbcCommand comando = new OdbcCommand(sSQL, conexion))
                {
                    comando.Parameters.AddWithValue("@valor", valor);
                    int cantidad = Convert.ToInt32(comando.ExecuteScalar());
                    return cantidad > 0;
                }
            }
            finally
            {
                conn.desconexion(conexion);
            }
        }

        public bool InsertarRegistro(string nombreTabla, Dictionary<string, string> datos)
        {
            if (datos == null || datos.Count == 0) return false;

            validaciones.ValidarIdentificador(nombreTabla);

            string columnas = "";
            string valores = "";
            int contador = 0;

            foreach (KeyValuePair<string, string> dato in datos)
            {
                validaciones.ValidarIdentificador(dato.Key);

                if (contador > 0) { columnas += ", "; valores += ", "; }

                columnas += dato.Key;
                valores += "?";
                contador++;
            }

            string sSQL = "INSERT INTO " + nombreTabla + " (" + columnas + ") VALUES (" + valores + ")";
            OdbcConnection conexion = conn.conexion();

            try
            {
                using (OdbcCommand comando = new OdbcCommand(sSQL, conexion))
                {
                    int pos = 0;

                    foreach (KeyValuePair<string, string> dato in datos)
                    {
                        comando.Parameters.AddWithValue("@p" + pos, dato.Value);
                        pos++;
                    }

                    return comando.ExecuteNonQuery() > 0;
                }
            }
            finally
            {
                conn.desconexion(conexion);
            }
        }

        public bool ActualizarRegistro(string nombreTabla, Dictionary<string, string> valores, Dictionary<string, string> clavesPrimarias)
        {
            if (valores == null || valores.Count == 0 || clavesPrimarias == null || clavesPrimarias.Count == 0)
                return false;

            validaciones.ValidarIdentificador(nombreTabla);

            Dictionary<string, string> valoresActualizar = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (KeyValuePair<string, string> dato in valores)
            {
                validaciones.ValidarIdentificador(dato.Key);

                if (!clavesPrimarias.ContainsKey(dato.Key))
                    valoresActualizar[dato.Key] = dato.Value;
            }

            if (valoresActualizar.Count == 0) return false;

            StringBuilder sql = new StringBuilder("UPDATE " + nombreTabla + " SET ");
            int i = 0;

            foreach (KeyValuePair<string, string> dato in valoresActualizar)
            {
                if (i > 0) sql.Append(", ");
                sql.Append(dato.Key + " = ?");
                i++;
            }

            sql.Append(" WHERE ");
            i = 0;

            foreach (KeyValuePair<string, string> clave in clavesPrimarias)
            {
                validaciones.ValidarIdentificador(clave.Key);

                if (i > 0) sql.Append(" AND ");
                sql.Append(clave.Key + " = ?");
                i++;
            }

            OdbcConnection conexion = conn.conexion();

            try
            {
                using (OdbcCommand comando = new OdbcCommand(sql.ToString(), conexion))
                {
                    foreach (KeyValuePair<string, string> dato in valoresActualizar)
                        comando.Parameters.AddWithValue("@valor_" + dato.Key, dato.Value);

                    foreach (KeyValuePair<string, string> clave in clavesPrimarias)
                        comando.Parameters.AddWithValue("@pk_" + clave.Key, clave.Value);

                    return comando.ExecuteNonQuery() > 0;
                }
            }
            finally
            {
                conn.desconexion(conexion);
            }
        }

        public bool EliminarRegistro(string nombreTabla, Dictionary<string, string> clavesPrimarias)
        {
            if (clavesPrimarias == null || clavesPrimarias.Count == 0) return false;

            validaciones.ValidarIdentificador(nombreTabla);

            StringBuilder sql = new StringBuilder("DELETE FROM " + nombreTabla + " WHERE ");
            int i = 0;

            foreach (KeyValuePair<string, string> clave in clavesPrimarias)
            {
                validaciones.ValidarIdentificador(clave.Key);

                if (i > 0) sql.Append(" AND ");
                sql.Append(clave.Key + " = ?");
                i++;
            }

            OdbcConnection conexion = conn.conexion();

            try
            {
                using (OdbcCommand comando = new OdbcCommand(sql.ToString(), conexion))
                {
                    foreach (KeyValuePair<string, string> clave in clavesPrimarias)
                        comando.Parameters.AddWithValue("@pk_" + clave.Key, clave.Value);

                    return comando.ExecuteNonQuery() > 0;
                }
            }
            finally
            {
                conn.desconexion(conexion);
            }
        }

        public OdbcDataAdapter filtrarTbl(string nombreTabla, string columna, string valor)
        {
            validaciones.ValidarIdentificador(nombreTabla);
            validaciones.ValidarIdentificador(columna);

            string sSQL = "SELECT * FROM " + nombreTabla + " WHERE " + columna + " LIKE ?";
            OdbcConnection conexion = conn.conexion();

            OdbcCommand comando = new OdbcCommand(sSQL, conexion);
            comando.Parameters.AddWithValue("@valor", "%" + valor + "%");

            return new OdbcDataAdapter(comando);
        }

        public void ejecutarSql(string sql)
        {
            OdbcConnection conexion = conn.conexion();

            try
            {
                using (OdbcCommand cmd = new OdbcCommand(sql, conexion))
                    cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.desconexion(conexion);
            }
        }

        public void guardarDatos(string query)
        {
            try
            {
                using (OdbcConnection conexion = conn.conexion())
                using (OdbcCommand cmd = new OdbcCommand(query, conexion))
                    cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la sentencia en la base de datos: " + ex.Message, ex);
            }
        }
    }
}