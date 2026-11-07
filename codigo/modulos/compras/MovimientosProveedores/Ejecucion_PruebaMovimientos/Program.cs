using System;



using System.Windows.Forms;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Carlos Andres Arriaza Lara
 * Carné : 0901-23-13862
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Inicializa la aplicación de prueba y abre el formulario utilizado para revisar Movimientos de Proveedores.
 * ===================================================================
*/

namespace Ejecucion_PruebaMovimientos
{
    internal static class ClsPrograma
    {
        
        
        
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmPruebaMovimientos());
        }
    }
}
