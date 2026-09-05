using System;
using System.Data;
using System.Data.Odbc;
using System.Text.RegularExpressions;

namespace CapaModelo_Navegador
{
    public class cls_Sentencias
    {
        private cls_ConexionBD Prv_conn = new cls_ConexionBD();

        public OdbcDataAdapter met_LlenarTbl(string nombreTabla)
        {
            if (string.IsNullOrEmpty(nombreTabla))
                throw new ArgumentException("El nombre de la tabla no puede estar vacío.");

            if (!Regex.IsMatch(nombreTabla, @"^[a-zA-Z_][a-zA-Z0-9_]*$"))
                throw new ArgumentException("El nombre de la tabla contiene caracteres no válidos.");

            try
            {
                string sSQL = "SELECT * FROM " + nombreTabla + ";";
                OdbcConnection conn = Prv_conn.met_ObtenerConexion();
                return new OdbcDataAdapter(sSQL, conn);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los datos de la tabla.", ex);
            }
        }

        public DataTable met_ObtenerEsquemaTabla(string nombreTabla)
        {
            if (string.IsNullOrEmpty(nombreTabla))
                throw new ArgumentException("El nombre de la tabla no puede estar vacío.");

            if (!Regex.IsMatch(nombreTabla, @"^[a-zA-Z_][a-zA-Z0-9_]*$"))
                throw new ArgumentException("El nombre de la tabla contiene caracteres no válidos.");

            try
            {
                string sSQL = @"
                    SELECT 
                        COLUMN_NAME, 
                        DATA_TYPE, 
                        CHARACTER_MAXIMUM_LENGTH,
                        IS_NULLABLE
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_NAME = ? 
                    ORDER BY ORDINAL_POSITION;
                ";

                OdbcConnection conn = Prv_conn.met_ObtenerConexion();
                OdbcCommand cmd = new OdbcCommand(sSQL, conn);
                cmd.Parameters.AddWithValue("?", nombreTabla);

                OdbcDataAdapter da = new OdbcDataAdapter(cmd);
                DataTable dtEsquema = new DataTable();
                da.Fill(dtEsquema);
                return dtEsquema;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el esquema de la tabla.", ex);
            }
        }
    }
}