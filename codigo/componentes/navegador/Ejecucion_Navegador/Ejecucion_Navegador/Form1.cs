using System.Windows.Forms;

namespace CapaVista_Navegador
{
    // Formulario que usa el navegador. Hereda de FrmNavegadorCrud (que ya trae el control Navegador
    // insertado) y en una sola línea le pasa los 3 datos que necesita: tabla, código de aplicación
    // (con el que Seguridad valida al usuario) y la ruta del archivo de ayuda.
    public partial class Form1 : FrmNavegadorCrud
    {
        public Form1()
        {
            InitializeComponent();

            NavegadorMetConfigurar("tblempleado", 4, @"Ayudas\tblaplicacion.chm");
        }
    }
}