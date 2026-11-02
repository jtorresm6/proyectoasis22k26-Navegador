/*
 * INICIO DEL CÓDIGO — Jose Torres
 * Carné: 0901-23-1091 | Fecha: 15/10/2026
 * Área: Ventas y cuentas por cobrar
 * Propósito: Abrir directamente el MDI del módulo de Ventas.
 */
using System;
using System.Windows.Forms;

namespace Ejecucion_Ventas
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            // El MDI es la ventana principal; al cerrarlo termina la aplicación.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CapaVista_Ventas.FrmMdiVentas());
        }
    }
}

// FIN DEL CÓDIGO — Jose Torres | Carné: 0901-23-1091 | Fecha: 15/10/2026
