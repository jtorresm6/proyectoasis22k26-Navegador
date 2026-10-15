using System;
using System.Windows.Forms;

namespace Ejecucion_Contabilidad
{
    internal static class ClsPrograma
    {
        /// <summary>
        /// Inicia Contabilidad: login, pantalla de carga y MDI.
        /// </summary>
        [STAThread]
        // Main conserva su nombre porque es el punto de entrada reconocido por C#.
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CapaVista_Contabilidad.FrmLoginContabilidad());
        }
    }
}
