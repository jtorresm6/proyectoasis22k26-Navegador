/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Daniella Jurado
 * Carné : 0901-23-3679
 * Fecha : 12/10/2026
 * ==================================================================
 * Propósito :
 *  Capa Modelo de la ventana 9201 - PermisosVacaciones. Guarda el nombre
 *  de las tablas que mantiene el Navegador en Permisos (9202) y
 *  Vacaciones (9203).
 * Modificado por : Byron Alexander Chiquito Paredes 0901-23-3747 (23/10/2026)
 * ===================================================================
*/

namespace CapaModelo_PermisosVacaciones.Clases
{
    public class ClsSentenciasPermisosVacaciones
    {
        private readonly string _TablaPermisos = "tblPermisoEmpleado";
        private readonly string _TablaVacaciones = "tblVacacionEmpleado";

        public string RrhhFuncTablaPermisos()
        {
            return _TablaPermisos;
        }

        public string RrhhFuncTablaVacaciones()
        {
            return _TablaVacaciones;
        }
    }
}
