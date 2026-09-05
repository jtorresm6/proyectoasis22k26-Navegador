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
            string sSQL =
                "SELECT * FROM " + nombreTabla;

            OdbcConnection conexion =
                conn.conexion();

            OdbcDataAdapter daSentencias =
                new OdbcDataAdapter(
                    sSQL,
                    conexion
                );

            return daSentencias;
        }

        public List<string> ObtenerColumnas(
            string nombreTabla)
        {
            List<string> columnas =
                new List<string>();

            OdbcConnection conexion =
                conn.conexion();

            try
            {
                DataTable dtColumnas =
                    conexion.GetSchema(
                        "Columns",
                        new string[]
                        {
                            null,
                            null,
                            nombreTabla,
                            null
                        }
                    );

                foreach (DataRow fila
                    in dtColumnas.Rows)
                {
                    string nombreColumna =
                        fila["COLUMN_NAME"].ToString();

                    if (!columnas.Contains(
                        nombreColumna))
                    {
                        columnas.Add(
                            nombreColumna
                        );
                    }
                }
            }
            finally
            {
                conn.desconexion(conexion);
            }

            return columnas;
        }

        public bool ExisteLlavePrimaria(
            string nombreTabla,
            string[] camposPK,
            string[] valoresPK)
        {
            if (camposPK == null ||
                camposPK.Length == 0)
            {
                return false;
            }

            if (valoresPK == null ||
                valoresPK.Length != camposPK.Length)
            {
                return false;
            }

            string condiciones = "";

            for (int i = 0;
                i < camposPK.Length;
                i++)
            {
                if (i > 0)
                {
                    condiciones += " AND ";
                }

                condiciones +=
                    camposPK[i] + " = ?";
            }

            string sSQL =
                "SELECT COUNT(*) FROM " +
                nombreTabla +
                " WHERE " +
                condiciones;

            OdbcConnection conexion =
                conn.conexion();

            try
            {
                using (OdbcCommand comando =
                    new OdbcCommand(
                        sSQL,
                        conexion))
                {
                    for (int i = 0;
                        i < valoresPK.Length;
                        i++)
                    {
                        comando.Parameters.AddWithValue(
                            "@p" + i,
                            valoresPK[i]
                        );
                    }

                    int cantidad =
                        Convert.ToInt32(
                            comando.ExecuteScalar()
                        );

                    return cantidad > 0;
                }
            }
            finally
            {
                conn.desconexion(conexion);
            }
        }

        public bool ExisteValorCampo(
            string nombreTabla,
            string nombreCampo,
            string valor)
        {
            string sSQL =
                "SELECT COUNT(*) FROM " +
                nombreTabla +
                " WHERE " +
                nombreCampo +
                " = ?";

            OdbcConnection conexion =
                conn.conexion();

            try
            {
                using (OdbcCommand comando =
                    new OdbcCommand(
                        sSQL,
                        conexion))
                {
                    comando.Parameters.AddWithValue(
                        "@valor",
                        valor
                    );

                    int cantidad =
                        Convert.ToInt32(
                            comando.ExecuteScalar()
                        );

                    return cantidad > 0;
                }
            }
            finally
            {
                conn.desconexion(conexion);
            }
        }

        public bool InsertarRegistro(
            string nombreTabla,
            Dictionary<string, string> datos)
        {
            if (datos == null ||
                datos.Count == 0)
            {
                return false;
            }

            string columnas = "";
            string valores = "";

            int contador = 0;

            foreach (
                KeyValuePair<string, string> dato
                in datos)
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

            string sSQL =
                "INSERT INTO " +
                nombreTabla +
                " (" +
                columnas +
                ") VALUES (" +
                valores +
                ")";

            OdbcConnection conexion =
                conn.conexion();

            try
            {
                using (OdbcCommand comando =
                    new OdbcCommand(
                        sSQL,
                        conexion))
                {
                    int posicion = 0;

                    foreach (
                        KeyValuePair<string, string> dato
                        in datos)
                    {
                        comando.Parameters.AddWithValue(
                            "@p" + posicion,
                            dato.Value
                        );

                        posicion++;
                    }

                    int resultado =
                        comando.ExecuteNonQuery();

                    return resultado > 0;
                }
            }
            finally
            {
                conn.desconexion(conexion);
            }
        }
    }
}