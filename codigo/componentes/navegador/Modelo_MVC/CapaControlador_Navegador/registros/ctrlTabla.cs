using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using CapaModelo_Navegador;

namespace CapaControlador_Navegador
{
    // Todo lo relacionado a listar tablas y traer datos para el grid
    public class ctrlTabla
    {
        private registros registros = new registros();
        private esquema esquema = new esquema();

        public DataTable llenarDgv(string nombreTabla)
        {
            DataTable dt = new DataTable();

            try
            {
                using (OdbcDataAdapter da = registros.llenarTbl(nombreTabla))
                {
                    da.Fill(dt);

                    if (da.SelectCommand != null && da.SelectCommand.Connection != null)
                        da.SelectCommand.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cargar la tabla '" + nombreTabla + "': " + ex.Message, ex);
            }

            return dt;
        }

        public List<string> ObtenerTablas()
        {
            try
            {
                return esquema.ObtenerTablas();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de tablas de la base de datos: " + ex.Message, ex);
            }
        }

        public List<string> ObtenerColumnas(string nombreTabla)
        {
            return esquema.ObtenerColumnas(nombreTabla);
        }

        public DataTable filtrarDgv(string nombreTabla, string columna, string valor)
        {
            OdbcDataAdapter da = registros.filtrarTbl(nombreTabla, columna, valor);
            DataTable dt = new DataTable();

            try
            {
                da.Fill(dt);
            }
            finally
            {
                if (da.SelectCommand != null && da.SelectCommand.Connection != null)
                    da.SelectCommand.Connection.Close();

                da.Dispose();
            }

            return dt;
        }
    }
}