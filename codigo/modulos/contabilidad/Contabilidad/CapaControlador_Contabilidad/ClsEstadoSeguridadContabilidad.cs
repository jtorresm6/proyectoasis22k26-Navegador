/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 21/10/2026
 * ==================================================================
 * Propósito :
 *  Datos de la sesión y de los permisos que el MDI muestra: usuario,
 *  roles, aviso y el permiso de cada pantalla por código de ventana.
 * ===================================================================
*/

using System.Collections.Generic;

namespace CapaControlador_Contabilidad
{
    public sealed class ClsEstadoSeguridadContabilidad
    {
        public bool HaySesion { get; internal set; }
        public string NombreUsuario { get; internal set; }
        public string Roles { get; internal set; }
        public string Mensaje { get; internal set; }
        // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        public Dictionary<int, ClsModeloPermisosPantallaContabilidad> Permisos { get; } = new Dictionary<int, ClsModeloPermisosPantallaContabilidad>
        // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        {
            // Inicio de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
            { 10001, ClsModeloPermisosPantallaContabilidad.Ninguno }, { 10002, ClsModeloPermisosPantallaContabilidad.Ninguno },
            { 10003, ClsModeloPermisosPantallaContabilidad.Ninguno }, { 10004, ClsModeloPermisosPantallaContabilidad.Ninguno },
            { 10005, ClsModeloPermisosPantallaContabilidad.Ninguno },
            { 10011, ClsModeloPermisosPantallaContabilidad.Ninguno },
            { 10012, ClsModeloPermisosPantallaContabilidad.Ninguno },
            { 10013, ClsModeloPermisosPantallaContabilidad.Ninguno },
            { 10015, ClsModeloPermisosPantallaContabilidad.Ninguno },
            { 10014, ClsModeloPermisosPantallaContabilidad.Ninguno }
            // Fin de cambio: Mario Taracena - 0901-23-9335 - 27/10/2026
        };
    }
}
