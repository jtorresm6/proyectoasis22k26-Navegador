using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using CapaModelo_Navegador;

namespace CapaControlador_Navegador
{
    // Todo lo relacionado a listar tablas y traer datos para el GridControl
    public class ClsCtrlTabla
    {
        private ClsRegistros _Registros = new ClsRegistros();
        private ClsEsquema _Esquema = new ClsEsquema();

        public DataTable NavegadorMetLlenarDgv(string NombreTabla)
        {
            DataTable TablaDatos = new DataTable();

            try
            {
                using (OdbcDataAdapter AdaptadorDatos = _Registros.NavegadorFuncLlenarTbl(NombreTabla))
                {
                    AdaptadorDatos.Fill(TablaDatos);

                    if (AdaptadorDatos.SelectCommand != null && AdaptadorDatos.SelectCommand.Connection != null)
                        AdaptadorDatos.SelectCommand.Connection.Close();
                }
            }
            catch (Exception Excepcion)
            {
                throw new Exception("Error al cargar la tabla '" + NombreTabla + "': " + Excepcion.Message, Excepcion);
            }

            return TablaDatos;
        }

        public List<string> NavegadorFuncObtenerTablas()
        {
            try
            {
                return _Esquema.NavegadorFuncObtenerTablas();
            }
            catch (Exception Excepcion)
            {
                throw new Exception("Error al obtener la lista de tablas de la base de datos: " + Excepcion.Message, Excepcion);
            }
        }

        public List<string> NavegadorFuncObtenerColumnas(string NombreTabla)
        {
            return _Esquema.NavegadorFuncObtenerColumnas(NombreTabla);
        }

        public DataTable NavegadorMetFiltrarDgv(string NombreTabla, string Columna, string Valor)
        {
            OdbcDataAdapter AdaptadorDatos = _Registros.NavegadorFuncFiltrarTbl(NombreTabla, Columna, Valor);
            DataTable TablaDatos = new DataTable();

            try
            {
                AdaptadorDatos.Fill(TablaDatos);
            }
            finally
            {
                if (AdaptadorDatos.SelectCommand != null && AdaptadorDatos.SelectCommand.Connection != null)
                    AdaptadorDatos.SelectCommand.Connection.Close();

                AdaptadorDatos.Dispose();
            }

            return TablaDatos;
        }
    }
}