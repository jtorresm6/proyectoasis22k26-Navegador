using System;
using System.Collections.Generic;
using System.Data;
using CapaModelo_Navegador;

namespace CapaControlador_Navegador
{
    // Todo lo relacionado a listar tablas y traer datos para el GridControl
    public class ClsCtrlTabla
    {
        private ClsRegistros _Registros = new ClsRegistros();
        private ClsEsquema _Esquema = new ClsEsquema();

        public DataTable NavegadorFuncLlenarDgv(string NombreTabla)
        {
            try
            {
                return _Registros.NavegadorFuncConsultarTodo(NombreTabla);
            }
            catch (Exception Excepcion)
            {
                throw new Exception("Error al cargar la tabla '" + NombreTabla + "': " + Excepcion.Message, Excepcion);
            }
        }
        public bool NavegadorFuncValidarLlavePrimariaUnica(string tabla, string campoLlave, string valorLlave)
        {
            if (string.IsNullOrWhiteSpace(valorLlave))
            {
                return false;
            }

            // Usa el método real de ClsRegistros
            return !_Registros.NavegadorFuncExisteValorCampo(tabla, campoLlave, valorLlave);
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

        public DataTable NavegadorFuncFiltrarDgv(string NombreTabla, string Columna, string Valor)
        {
            return _Registros.NavegadorFuncFiltrarDatos(NombreTabla, Columna, Valor);
        }
    }
}