/*
 * ==================================================================
 * Área : Recursos Humanos
 * Autor : Byron Alexander
 * Carné : 0901-23-3747
 * Fecha : 11/10/2026
 * ==================================================================
 * Propósito :
 *  Punto de entrada del ejecutable de Recursos Humanos, abre el
 *  formulario MDI del módulo.
 * ===================================================================
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ejecucion_RecursosHumanos
{
    internal static class Program
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        [STAThread]
        static void Main()
        {
            if (Environment.OSVersion.Version.Major >= 6) SetProcessDPIAware();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CapaVista_Rrhh.Formularios.FrmMDIRrhh());
        }
    }
}
