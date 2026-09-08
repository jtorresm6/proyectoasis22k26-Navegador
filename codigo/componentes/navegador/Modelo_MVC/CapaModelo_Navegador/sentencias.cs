using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo_Navegador
{
    public class Sentencias
    {
        conexionBD conn = new conexionBD();

        public OdbcDataAdapter llenarTbl(string nombreTabla)
        {
            string sSQL = "SELECT * FROM " + nombreTabla;
            OdbcConnection conexion = conn.conexion();
            OdbcDataAdapter daSentencias = new OdbcDataAdapter(sSQL, conexion);
            return daSentencias;
        }

        public DataTable ConsultarEmpleados()
        {
            string sSQL = "SELECT * FROM tbl_empleados";
            OdbcConnection conexion = conn.conexion();
            DataTable dtEmpleados = new DataTable();

            try
            {
                using (OdbcDataAdapter da = new OdbcDataAdapter(sSQL, conexion))
                {
                    da.Fill(dtEmpleados);
                }
            }
            finally
            {
                conn.desconexion(conexion);
            }

            return dtEmpleados;
        }

        public List<string> ObtenerColumnas(string nombreTabla)
        {
            List<string> columnas = new List<string>();
            OdbcConnection conexion = conn.conexion();

            try
            {
                DataTable dtColumnas = conexion.GetSchema(
                    "Columns",
                    new string[] { null, null, nombreTabla, null }
                );

                foreach (DataRow fila in dtColumnas.Rows)
                {
                    string nombreColumna = fila["COLUMN_NAME"].ToString();

                    if (!columnas.Contains(nombreColumna))
                    {
                        columnas.Add(nombreColumna);
                    }
                }
            }
            finally
            {
                conn.desconexion(conexion);
            }

            return columnas;
        }

        public bool ExisteLlavePrimaria(string nombreTabla, string[] camposPK, string[] valoresPK)
        {
            if (camposPK == null || camposPK.Length == 0) return false;
            if (valoresPK == null || valoresPK.Length != camposPK.Length) return false;

            string condiciones = "";
            for (int i = 0; i < camposPK.Length; i++)
            {
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
                    {
                        comando.Parameters.AddWithValue("@p" + i, valoresPK[i]);
                    }

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

            string columnas = "";
            string valores = "";
            int contador = 0;

            foreach (KeyValuePair<string, string> dato in datos)
            {
                if (contador > 0)
                {
                    columnas += ", ";
                    valores += ", ";
                }
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
                    int posicion = 0;
                    foreach (KeyValuePair<string, string> dato in datos)
                    {
                        comando.Parameters.AddWithValue("@p" + posicion, dato.Value);
                        posicion++;
                    }

                    int resultado = comando.ExecuteNonQuery();
                    return resultado > 0;
                }
            }
            finally
            {
                conn.desconexion(conexion);
            }
        }

        public DataTable ObtenerEsquemaTabla(string nombreTabla)
        {
            string sSQL = @"
                SELECT 
                    COLUMN_NAME, 
                    DATA_TYPE, 
                    CHARACTER_MAXIMUM_LENGTH, 
                    IS_NULLABLE 
                FROM INFORMATION_SCHEMA.COLUMNS 
                WHERE TABLE_NAME = ? AND TABLE_SCHEMA = DATABASE() 
                ORDER BY ORDINAL_POSITION";

            OdbcConnection conexion = conn.conexion();
            DataTable dtEsquema = new DataTable();

            try
            {
                using (OdbcCommand comando = new OdbcCommand(sSQL, conexion))
                {
                    comando.Parameters.AddWithValue("?", nombreTabla);
                    using (OdbcDataAdapter da = new OdbcDataAdapter(comando))
                    {
                        da.Fill(dtEsquema);
                    }
                }
            }
            finally
            {
                conn.desconexion(conexion);
            }

            return dtEsquema;
        }

        public void ejecutarSql(string sql)
        {
            OdbcConnection conexion = conn.conexion();
            try
            {
                using (OdbcCommand cmd = new OdbcCommand(sql, conexion))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                conn.desconexion(conexion);
<<<<<<< HEAD
=======
            }
        }

        public DataTable ObtenerEsquemaCampos(string nombreTabla)
        {
            string sSQL = @"
                SELECT 
                    COLUMN_NAME, 
                    DATA_TYPE, 
                    COLUMN_TYPE, 
                    COLUMN_KEY, 
                    IS_NULLABLE 
                FROM INFORMATION_SCHEMA.COLUMNS 
                WHERE TABLE_NAME = ? AND TABLE_SCHEMA = DATABASE() 
                ORDER BY ORDINAL_POSITION";

            OdbcConnection conexion = conn.conexion();
            DataTable dtEsquema = new DataTable();

            try
            {
                using (OdbcCommand comando = new OdbcCommand(sSQL, conexion))
                {
                    comando.Parameters.AddWithValue("?", nombreTabla);
                    using (OdbcDataAdapter da = new OdbcDataAdapter(comando))
                    {
                        da.Fill(dtEsquema);
                    }
                }
            }
            finally
            {
                conn.desconexion(conexion);
            }

            return dtEsquema;
        }

        public DataTable Consultar(string nombreTabla, IList<Filtro> filtros)
        {
            string sSQL = "SELECT * FROM " + nombreTabla;

            OdbcConnection conexion = conn.conexion();
            DataTable dt = new DataTable(nombreTabla);

            try
            {
                using (OdbcCommand comando = new OdbcCommand())
                {
                    comando.Connection = conexion;

                    if (filtros != null && filtros.Count > 0)
                    {
                        sSQL += " WHERE ";

                        for (int i = 0; i < filtros.Count; i++)
                        {
                            if (i > 0) sSQL += " AND ";

                            sSQL += filtros[i].Columna +
                                (filtros[i].UsarLike ? " LIKE ?" : " = ?");

                            comando.Parameters.AddWithValue("@p" + i, filtros[i].Valor);
                        }
                    }

                    comando.CommandText = sSQL;

                    using (OdbcDataAdapter da = new OdbcDataAdapter(comando))
                    {
                        da.Fill(dt);
                    }
                }
            }
            finally
            {
                conn.desconexion(conexion);
            }

            return dt;
        }

        public int EliminarRegistro(string nombreTabla, IList<Filtro> llave)
        {
            if (llave == null || llave.Count == 0) return 0;

            string sSQL = "DELETE FROM " + nombreTabla + " WHERE ";

            OdbcConnection conexion = conn.conexion();

            try
            {
                using (OdbcCommand comando = new OdbcCommand())
                {
                    comando.Connection = conexion;

                    for (int i = 0; i < llave.Count; i++)
                    {
                        if (i > 0) sSQL += " AND ";

                        sSQL += llave[i].Columna + " = ?";

                        comando.Parameters.AddWithValue("@k" + i, llave[i].Valor);
                    }

                    comando.CommandText = sSQL;

                    return comando.ExecuteNonQuery();
                }
            }
            finally
            {
                conn.desconexion(conexion);
>>>>>>> ffb0f2c (correcion en uso de capa en campos dinamicos)
            }
        }
    }
}