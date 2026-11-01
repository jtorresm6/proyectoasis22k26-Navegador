/*
 * INICIO DEL CÓDIGO — ComisionVendedores
 * Fecha de actualización: 18/10/2026
 * Propósito: Ejecutar el formulario principal de comisiones de vendedores.
 */
using System;
using System.Windows.Forms;

namespace ComisionVendedores
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CapaVista_ComisionVendedores.FrmMdiComisionVendedores());
        }
    }
}
