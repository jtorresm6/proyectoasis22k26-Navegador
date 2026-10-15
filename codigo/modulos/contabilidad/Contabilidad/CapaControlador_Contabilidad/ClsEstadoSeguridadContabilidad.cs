/*
 * Área: Contabilidad
 * Autor: Gabriel André Guillén Pocón
 * Carné: 0901-23-1998
 * Fecha: 10/10/2026
 * Propósito: resultado de presentación de la sesión y el acceso al menú.
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
        public Dictionary<int, int> Modulos { get; } = new Dictionary<int, int>();
        public Dictionary<int, int> Aplicaciones { get; } = new Dictionary<int, int>();
        public Dictionary<int, bool> Accesos { get; } = new Dictionary<int, bool>
        {
            { 10001, false }, { 10002, false }, { 10003, false },
            { 10004, false }, { 10005, false }
        };
    }
}
