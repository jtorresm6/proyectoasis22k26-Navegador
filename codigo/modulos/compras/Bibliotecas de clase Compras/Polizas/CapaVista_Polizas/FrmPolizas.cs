using System.Windows.Forms;

/*
 * ==================================================================
 * Área : Compras
 * Autor : Miguel David Contreras Jacinto
 * Carné : 0901-21-3878
 * Fecha : 24/10/2026
 * ==================================================================
 * Propósito :
 *  Presenta los datos de las pólizas de compras y valida el formato de la cuenta contable seleccionada.
 * ===================================================================
*/

namespace CapaVista_Polizas
{
    public partial class FrmPolizas : Form
    {
        public FrmPolizas()
        {
            InitializeComponent();
            CapaVista_Componentes.Componentes.ClsPresentacionCompras.ComprasMetFijarTamanoFormulario(this);
            ComprasBtnIngresarCuenta.Enabled = false;
            ComprasBtnNuevo.Enabled = false;
            ComprasBtnGuardar.Enabled = false;
            ComprasBtnEnviar.Enabled = false;
            ComprasLblCuenta.MostrarAsterisco = true;
            ComprasCboCuenta.Entrada.Validating += (Remitente, Evento) =>
            {
                if (string.IsNullOrEmpty(ComprasCboCuenta.Text)) return;
                bool EsValido = CapaControlador_Polizas.ClsCuentaContable.ComprasFuncFormatoValido(ComprasCboCuenta.Text);
                Evento.Cancel = !EsValido;
                if (EsValido) ComprasCboCuenta.ComprasMetLimpiarError();
                else ComprasCboCuenta.ComprasMetMostrarError("Use el formato C.GG.CC.SSS; por ejemplo, 1.01.02.001.");
            };
        }
    }
}
