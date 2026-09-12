using System;
using System.Collections.Generic;
using CapaEntidades_Navegador;
using CapaModelo_Navegador;

namespace CapaControlador_Navegador
{
    // Todo lo relacionado a la estructura de una tabla (columnas, PK, FK, tipos)
    public class ctrlEsquema
    {
        private esquema esquema = new esquema();

        public List<columnaInfo> ObtenerEsquemaTabla(string nombreTabla)
        {
            try
            {
                return esquema.ObtenerEsquemaTabla(nombreTabla);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el esquema de la tabla '" + nombreTabla + "': " + ex.Message, ex);
            }
        }

        public object ObtenerSiguienteValorLlave(string nombreTabla, string columnaPK)
        {
            try
            {
                return esquema.ObtenerSiguienteValorLlave(nombreTabla, columnaPK);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al calcular el siguiente valor de la llave primaria de '" + nombreTabla + "': " + ex.Message, ex);
            }
        }
    }
}