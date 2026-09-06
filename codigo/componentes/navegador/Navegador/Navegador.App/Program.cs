using System;
using System.Windows.Forms;
using CapaVista_Navegador;

namespace Navegador.App
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Frm_Crud());
        }    
    }
}
