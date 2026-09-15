// Integrante: Matthew Juarez 0901-23-4250
// Asignación: ClsPermisos.cs (Validación de acceso)

using System;

namespace CapaModelo_Navegador
{
    public class ClsPermisos
    {
        // Instancia para conectar a la base de datos
        private ClsConexionBD _ConexionBD = new ClsConexionBD();

        public bool NavegadorFuncValidarAcceso(string Usuario, string Modulo)
        {
            // Retorna falso si faltan datos de entrada
            if (string.IsNullOrEmpty(Usuario) || string.IsNullOrEmpty(Modulo))
            {
                return false;
            }

            try
            {
                // TODO: Reemplazar retorno fijo por consulta SQL a la tabla de permisos
                // cuando la BD esté integrada
                return true;
            }
            catch (Exception Excepcion)
            {
                // Captura el fallo y transmite el mensaje original
                throw new Exception("Error al consultar permisos en la base de datos: " + Excepcion.Message, Excepcion);
            }
        }
    }
}