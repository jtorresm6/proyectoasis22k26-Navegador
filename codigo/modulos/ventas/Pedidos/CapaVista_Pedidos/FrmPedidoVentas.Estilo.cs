using System;
using System.Drawing;
using System.Windows.Forms;

namespace CapaVista_Pedidos
{
    public partial class FrmPedidoVentas
    {
        private void VentasMetPrepararVista()
        {
            VentasTxtNumeroPedido.Text = VentasFuncNuevoNumero();
            VentasDtpPedido.Value = DateTime.Today;
            VentasDtpEntrega.Value = DateTime.Today.AddDays(2);
            VentasDtpVencimiento.Value = DateTime.Today.AddDays(7);
            VentasPnlDatosPedido.Controls.Add(new Panel {
                Name = "VentasPnlSeparadorDatos",
                Location = new Point(26, 137), Size = new Size(1008, 1),
                BackColor = Color.FromArgb(235, 229, 238), TabStop = false });
            VentasMetPrepararDetalle();
            VentasTtpAcciones.SetToolTip(VentasBtnGuardar, "Guardar pedido");
            VentasTtpAcciones.SetToolTip(VentasBtnCancelar, "Limpiar el formulario");
            VentasDgvDetalle.SelectionChanged += (Origen, Evento) => VentasMetActualizarAcciones();
            VentasDtpPedido.ValueChanged += (Origen, Evento) => { if (_CatalogosDisponibles) VentasMetCargarProductos(); };
            VentasChkPedidoIndependiente.Checked = true;
            VentasChkPedidoPorCotizacion.CheckedChanged += (Origen, Evento) => {
                if (VentasChkPedidoPorCotizacion.Checked) VentasMetCambiarOrigen(true);
            };
            VentasChkPedidoIndependiente.CheckedChanged += (Origen, Evento) => {
                if (VentasChkPedidoIndependiente.Checked) VentasMetCambiarOrigen(false);
            };
            VentasCboCotizacionOrigen.SelectedIndexChanged += (Origen, Evento) => VentasMetAplicarCotizacion();
            VentasMetCambiarOrigen(false);
        }

        private static string VentasFuncNuevoNumero() => "PED-" + DateTime.Today.ToString("yyyyMMdd") +
            "-" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpperInvariant();

    }
}
