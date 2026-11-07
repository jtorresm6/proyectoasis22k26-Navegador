/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Daniella Jurado
 * Carné : 0901-23-3679
 * Fecha : 12/10/2026
 * ==================================================================
 * Propósito :
 *  Capa Controlador de la ventana 9201 - PermisosVacaciones. Le pasa a
 *  la Vista la tabla, el módulo y la aplicación con que se arma el
 *  Navegador de Permisos (9202) y de Vacaciones (9203).
 * Modificado por : Byron Alexander Chiquito Paredes 0901-23-3747 (23/10/2026)
 * ===================================================================
*/

using CapaModelo_PermisosVacaciones.Clases;

namespace CapaControlador_PermisosVacaciones.Clases
{
    public class ClsControladorPermisosVacaciones
    {
        private const int _IdModulo = 9000;
        private const int _IdAplicacionPermisos = 9202;
        private const int _IdAplicacionVacaciones = 9203;

        private readonly ClsSentenciasPermisosVacaciones _Sentencias = new ClsSentenciasPermisosVacaciones();

        public int RrhhFuncIdModulo()
        {
            return _IdModulo;
        }

        public string RrhhFuncTablaPermisos()
        {
            return _Sentencias.RrhhFuncTablaPermisos();
        }

        public int RrhhFuncIdAplicacionPermisos()
        {
            return _IdAplicacionPermisos;
        }

        public string RrhhFuncTablaVacaciones()
        {
            return _Sentencias.RrhhFuncTablaVacaciones();
        }

        public int RrhhFuncIdAplicacionVacaciones()
        {
            return _IdAplicacionVacaciones;
        }
    }
}
