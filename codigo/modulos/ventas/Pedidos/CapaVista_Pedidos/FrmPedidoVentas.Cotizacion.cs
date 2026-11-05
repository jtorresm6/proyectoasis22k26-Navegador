using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CapaControlador_Pedidos;

namespace CapaVista_Pedidos
{
    public partial class FrmPedidoVentas
    {
        private void VentasMetCambiarOrigen(bool Cotizacion)
        {
            if (_Cargando) return;
            _Cargando = true;
            VentasChkPedidoPorCotizacion.Checked = Cotizacion;
            VentasChkPedidoIndependiente.Checked = !Cotizacion;
            VentasCboCotizacionOrigen.Enabled = Cotizacion;
            VentasTxtReferenciaIndependiente.Enabled = !Cotizacion;
            VentasCboCliente.Enabled = !Cotizacion;
            VentasCboVendedor.Enabled = !Cotizacion;
            VentasCboCliente.SelectedIndex = -1;
            VentasCboVendedor.SelectedIndex = -1;
            VentasLblCotizacionOrigen.Visible = Cotizacion;
            VentasCboCotizacionOrigen.Visible = Cotizacion;
            VentasLblReferenciaIndependiente.Visible = !Cotizacion;
            VentasTxtReferenciaIndependiente.Visible = !Cotizacion;
            VentasBtnIngresarProducto.Visible = !Cotizacion;
            VentasBtnEliminarProducto.Visible = !Cotizacion;
            VentasCboProductoPedido.Visible = VentasNudCantidadProducto.Visible = !Cotizacion;
            VentasLblDetalleAyuda.Text = Cotizacion
                ? "Los productos y precios se cargan de la cotización seleccionada"
                : "";
            VentasLblDetalleAyuda.Visible = Cotizacion;
            VentasChkPedidoPorCotizacion.BackColor = Cotizacion ? Color.FromArgb(59, 29, 74) : Color.FromArgb(235, 229, 238);
            VentasChkPedidoPorCotizacion.ForeColor = Cotizacion ? Color.White : Color.FromArgb(59, 29, 74);
            VentasChkPedidoIndependiente.BackColor = Cotizacion ? Color.FromArgb(235, 229, 238) : Color.FromArgb(59, 29, 74);
            VentasChkPedidoIndependiente.ForeColor = Cotizacion ? Color.FromArgb(59, 29, 74) : Color.White;
            if (Cotizacion && VentasCboCotizacionOrigen.Items.Count > 0) VentasCboCotizacionOrigen.SelectedIndex = 0;
            _Lineas.Clear();
            VentasMetActualizarDetalle();
            _Cargando = false;
            VentasMetActualizarAcciones();
        }

        private void VentasMetAplicarCotizacion()
        {
            if (_Cargando || !VentasChkPedidoPorCotizacion.Checked) return;
            var Cotizacion = VentasCboCotizacionOrigen.SelectedItem as ClsCotizacionPedido;
            _Lineas.Clear();
            if (Cotizacion != null && Cotizacion.Id > 0)
            {
                try
                {
                    VentasMetSeleccionarId(VentasCboCliente, Cotizacion.ClienteId);
                    VentasMetSeleccionarId(VentasCboVendedor, Cotizacion.VendedorId);
                    _MonedaId = Cotizacion.MonedaId;
                    foreach (var Linea in Cotizacion.Lineas)
                    {
                        ClsPedidoControlador.VentasMetCalcular(Linea);
                        _Lineas.Add(Linea);
                    }
                }
                catch (Exception Error) { MessageBox.Show(this, Error.Message, "Cotización", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }
            VentasMetActualizarDetalle();
        }

        private static void VentasMetSeleccionarId(ComboBox Combo, int Id)
        {
            Combo.SelectedItem = Combo.Items.Cast<ClsCatalogoPedido>().FirstOrDefault(Item => Item.Id == Id);
        }
    }
}
