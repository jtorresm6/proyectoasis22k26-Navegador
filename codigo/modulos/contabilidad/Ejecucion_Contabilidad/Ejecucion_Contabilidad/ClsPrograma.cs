/*
 * ==================================================================
 * Área : Contabilidad
 * Autor : Gabriel André Guillén Pocón
 * Carné : 0901-23-1998
 * Fecha : 17/10/2026
 * ==================================================================
 * Propósito : abre el login de Seguridad; desde ahí se entra al MDI de
 *  Contabilidad con el usuario y los permisos reales.
 * ===================================================================
*/

using System;
using System.Windows.Forms;
// Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 03/11/2026
using CapaVista_Seguridad;
// Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 03/11/2026

namespace Ejecucion_Contabilidad
{
    internal static class ClsPrograma
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Inicio de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 03/11/2026
            Application.Run(new FrmLogin());
            // Fin de cambio: Gabriel André Guillén Pocón - 0901-23-1998 - 03/11/2026
        }
    }
}
