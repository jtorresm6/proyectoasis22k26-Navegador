/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 17/10/2026
 * ==================================================================
 * Propósito : abre el MDI de Contabilidad con una sesión de Administrador.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CapaControlador_Seguridad.Objetos_de_valor;
using CapaVista_Contabilidad;

namespace Ejecucion_Contabilidad
{
    internal static class ClsPrograma
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            List<ClsRolInfo> Roles = new List<ClsRolInfo> { new ClsRolInfo { IdRol = 1, NombreRol = "Administrador" } };
            ClsSesionSeguridad.SeguridadMetIniciarSesion(1, "admin", "Administrador", Roles);
            FrmMDIContabilidad MDIContabilidad = new FrmMDIContabilidad();
            MDIContabilidad.ContabilidadMetActualizarInfoUsuario(ClsSesionSeguridad.NombreEmpleado, ClsSesionSeguridad.SeguridadMetRolesComoTexto());
            Application.Run(MDIContabilidad);
        }
    }
}
