using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
// Inicio cambio - Mario Alberto Taracena Pérez - 0901-23-9335
using CapaControlador_Navegador;
// Fin cambio - Mario Alberto Taracena Pérez - 0901-23-9335
using CapaVista_Navegador;
namespace Ejecucion_Navegador
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Inicio cambio - Mario Alberto Taracena Pérez - 0901-23-9335
            // Sin login real todavía: inicializa la sesión de Seguridad con un usuario de prueba
            // sembrado (ver ClsSesionPrueba para alternar entre Administrador/Supervisor/Operativo).
            // Esto tiene que hacerse ANTES de abrir el formulario, porque el formulario ya revisa
            // los permisos del usuario en sesión apenas se crea.
            ClsSesionPrueba.NavegadorMetIniciarSesionPrueba();
            // Fin cambio - Mario Alberto Taracena Pérez - 0901-23-9335

            Application.Run(new Form1());
        }
    }
}
