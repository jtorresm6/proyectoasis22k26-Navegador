// Integrante: Matthew Juárez
// Carnet: 0901-23-4250
// Fecha: 17/09/2026
// Asignación: ClsPermisos.cs (Validación de acceso)

using System;

namespace CapaModelo_Navegador
{
    public class ClsPermisos
    {
        // Instancia para conectar a la base de datos
        private ClsConexionBD _ConexionBD = new ClsConexionBD();

        /// <summary>
        /// Evalúa si un usuario posee acceso a un módulo específico.
        /// </summary>
        /// <param name="Usuario">Identificador del usuario que consulta</param>
        /// <param name="Modulo">Módulo al que se solicita acceso</param>
        /// <returns>True si el acceso es válido, false en caso contrario</returns>
        public bool NavegadorFuncValidarAcceso(string Usuario, string Modulo)
        {
            // Rechazar si no se proveen parámetros válidos
            if (string.IsNullOrEmpty(Usuario) || string.IsNullOrEmpty(Modulo))
            {
                return false;
            }

            try
            {
                // TODO: Sustituir simulación por consulta ODBC a la base de datos
                return true;
            }
            catch (Exception Excepcion)
            {
                // Relanzar excepción para que el controlador capture la falla
                throw new Exception("Error al consultar permisos en la base de datos: " + Excepcion.Message, Excepcion);
            }
        }
    }
}