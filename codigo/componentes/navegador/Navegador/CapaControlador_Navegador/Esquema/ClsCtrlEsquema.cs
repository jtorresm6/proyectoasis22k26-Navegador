using System;
using System.Collections.Generic;
using CapaModelo_Navegador;

namespace CapaControlador_Navegador
{
    // Todo lo relacionado a la estructura de una tabla (columnas, PK, FK, tipos)
    public class ClsCtrlEsquema
    {
        private ClsEsquema _Esquema = new ClsEsquema();

        public List<ClsColumnaInfo> NavegadorFuncObtenerEsquemaTabla(string NombreTabla)
        {
            try
            {
                return _Esquema.NavegadorFuncObtenerEsquemaTabla(NombreTabla);
            }
            catch (Exception Excepcion)
            {
                throw new Exception("Error al obtener el esquema de la tabla '" + NombreTabla + "': " + Excepcion.Message, Excepcion);
            }
        }

        public object NavegadorFuncObtenerSiguienteValorLlave(string NombreTabla, string ColumnaPK)
        {
            try
            {
                return _Esquema.NavegadorFuncObtenerSiguienteValorLlave(NombreTabla, ColumnaPK);
            }
            catch (Exception Excepcion)
            {
                throw new Exception("Error al calcular el siguiente valor de la llave primaria de '" + NombreTabla + "': " + Excepcion.Message, Excepcion);
            }
        }
    }
}