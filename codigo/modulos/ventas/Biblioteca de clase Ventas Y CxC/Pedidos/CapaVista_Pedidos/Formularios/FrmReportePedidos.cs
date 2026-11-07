using System;
using System.Data;
using System.Windows.Forms;
using CapaControlador_Pedidos;

namespace CapaVista_Pedidos
{
    // Natali Sofía Montenegro Portillo | 0901-23-10017 | Recoge fechas y estado para el reporte de pedidos.
    internal partial class FrmReportePedidos : Form
    {
        private readonly ClsPedidoControlador _Controlador;
        private readonly Func<string, bool> _ErrorHasta;

        public FrmReportePedidos() : this(new ClsPedidoControlador()) { }

        internal FrmReportePedidos(ClsPedidoControlador Controlador)
        {
            _Controlador = Controlador ?? throw new ArgumentNullException(nameof(Controlador));
            InitializeComponent();
            FrmPedidoVentas.VentasFuncCrearAviso(VentasDtpDesde, VentasLblDesde);
            _ErrorHasta = FrmPedidoVentas.VentasFuncCrearAviso(VentasDtpHasta, VentasLblHasta);
            VentasDtpDesde.ValueChanged += (Origen, Evento) => _ErrorHasta("");
            VentasDtpHasta.ValueChanged += (Origen, Evento) => _ErrorHasta("");
            VentasDtpDesde.Value = DateTime.Today.AddMonths(-1);
            VentasDtpHasta.Value = DateTime.Today;
        }

        private void VentasMetMostrarReporte(object Origen, EventArgs Evento)
        {
            if (!_ErrorHasta(ClsReglasPedidos.VentasFuncErrorFecha(VentasDtpHasta.Value, VentasDtpDesde.Value)))
            {
                VentasDtpHasta.Focus();
                return;
            }
            try
            {
                UseWaitCursor = true;
                string Estado = Convert.ToString(VentasCboEstadoReporte.SelectedItem);
                DataTable Pedidos = _Controlador.VentasFuncReporte(VentasDtpDesde.Value.Date, VentasDtpHasta.Value.Date, Estado);
                using (var Visor = new FrmReporteSeguimientoPedidos(Pedidos, VentasDtpDesde.Value.Date, VentasDtpHasta.Value.Date, Estado))
                {
                    UseWaitCursor = false;
                    Visor.ShowDialog(this);
                }
            }
            catch (Exception Error) { MessageBox.Show(this, "No se pudo generar el reporte. " + Error.Message, "Reporte de pedidos", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            finally { UseWaitCursor = false; }
        }
    }
}
