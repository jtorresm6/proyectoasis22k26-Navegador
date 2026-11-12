/*
 * INICIO DEL CÓDIGO — Natali Montenegro
 * Carné: 0901-23-10017 | Fecha: 09/11/2026
 * Área: Ventas y cuentas por cobrar
 * Propósito: Permitir revisar, confirmar y trasladar el cierre.
 */
// Ayuda contextual del formulario: Natali Sofía Montenegro Portillo | 0901-23-10017 | 09/11/2026
/*
 * INICIO DEL CÓDIGO — Ventas y cuentas por cobrar
 * Asignación: Todos
 * Jose Javier Torres Martínez | Carné: 0901-23-1091
 * Roger Yankhel de Jesús Herrera Alcántara | Carné: 0901-23-2429
 * Natali Sofía Montenegro Portillo | Carné: 0901-23-10017
 * Matthew Andreé Juárez Xoy | Carné: 0901-23-4250
 * Diego Alejandro Cheng Peña | Carné: 0901-22-8091
 * Encargado: Todos
 * Fecha de actualización: 17/10/2026
 * Propósito: Cierre transaccional por moneda, histórico y bloqueo de períodos.
 */
using System;
using System.Windows.Forms;

namespace CapaVista_CierreMovimientos
{
    public partial class FrmCierreMovimientosVentas : Form
    {
        public FrmCierreMovimientosVentas()
        {
            InitializeComponent();
            VentasMetConectarEventos();
            // Natali Montenegro: agregue la DLL de Pólizas al controlador de Cierre y descomente la línea para habilitar el envío.
            // VentasMetHabilitarEnlaceContable();
            VentasBtnAyuda.Click += (Origen, Evento) => VentasMetAbrirAyuda();
        }

        private void VentasMetHabilitarEnlaceContable()
        {
            VentasMetConfigurarEnlaceContable((Documento, Moneda, Fecha, Partida) =>
                _Controlador.VentasFuncRegistrarContabilidad(Documento, Moneda, Fecha, Partida, PaisContable));
        }

        private void VentasMetAbrirAyuda()
        {
            string Manual;
            try { Manual = _Controlador.VentasFuncRutaAyuda(typeof(FrmCierreMovimientosVentas).Assembly); }
            catch (System.IO.FileNotFoundException Error)
            {
                MessageBox.Show(this, Error.Message, "Ayuda de cierre", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string Tema = VentasTbcCierre.SelectedTab == VentasTbpContabilidad ? "html/02_traslado.html"
                : VentasTbcCierre.SelectedTab == VentasTbpHistorial ? "html/04_historial.html"
                : "html/01_confirmar.html";
            Help.ShowHelp(this, Manual, HelpNavigator.Topic, Tema);
        }

        protected override bool ProcessCmdKey(ref Message Mensaje, Keys Tecla)
        {
            if (Tecla == Keys.F1) { VentasMetAbrirAyuda(); return true; }
            return base.ProcessCmdKey(ref Mensaje, Tecla);
        }

        private void VentasMetPeriodoCierreChanged(object Origen, EventArgs Evento)
        {
            VentasMetPeriodoCambio();
            if (!Cargando && Moneda > 0 && VentasTbcCierre.SelectedTab == VentasTbpPreparar)
                VentasMetEjecutar(this, VentasMetRevisar);
        }
    }
}
