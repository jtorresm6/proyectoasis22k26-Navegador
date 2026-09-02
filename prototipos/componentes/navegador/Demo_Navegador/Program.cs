using System;
using System.Windows.Forms;
using Capa_Vista_Navegador; // Conecta tu biblioteca con la prueba

namespace Demo_Navegador
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize(); // Inicializador de WinForms en .NET 8

            // Lanza el formulario de tu prototipo CRUD
            Application.Run(new Frm_Crud());
        }
    }
}