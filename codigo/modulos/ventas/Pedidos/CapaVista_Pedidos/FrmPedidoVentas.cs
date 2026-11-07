using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Globalization;
using CapaControlador_Pedidos;

namespace CapaVista_Pedidos
{
    public partial class FrmPedidoVentas : Form
    {
        public FrmPedidoVentas()
        {
            InitializeComponent();
            VentasMetPrepararVista();
            Load += (Origen, Evento) => VentasMetCargarCatalogos();
        }

        private void VentasMetBtnIngresarProductoClick(object Origen, EventArgs Evento) { VentasMetAgregarProducto(); }
        private void VentasMetBtnEliminarProductoClick(object Origen, EventArgs Evento) { VentasMetQuitarProducto(); }
        private void VentasMetBtnGuardarClick(object Origen, EventArgs Evento) { VentasMetGuardar(); }
        private void VentasMetBtnCancelarClick(object Origen, EventArgs Evento) { VentasMetLimpiar(); }

        private readonly ClsPedidoControlador _Controlador = new ClsPedidoControlador();
        private readonly List<ClsLineaPedido> _Lineas = new List<ClsLineaPedido>();
        private readonly ComboBox VentasCboProductoPedido = new ComboBox();
        private readonly NumericUpDown VentasNudCantidadProducto = new NumericUpDown();
        private readonly CultureInfo _Cultura = CultureInfo.GetCultureInfo("es-GT");
        private int _MonedaId;
        private int _MonedaPublica;
        private bool _Cargando;
        private bool _CatalogosDisponibles;
        private string _NumeroCotizacionPendiente;

        private void VentasMetGuardar()
        {
            try
            {
                var Cliente = VentasCboCliente.SelectedItem as ClsCatalogoPedido;
                var Vendedor = VentasCboVendedor.SelectedItem as ClsCatalogoPedido;
                var Cotizacion = VentasCboCotizacionOrigen.SelectedItem as ClsCotizacionPedido;
                var Pedido = new ClsPedido {
                    Numero = VentasTxtNumeroPedido.Text.Trim(),
                    CotizacionId = VentasChkPedidoPorCotizacion.Checked && Cotizacion != null && Cotizacion.Id > 0 ? (int?)Cotizacion.Id : null,
                    Referencia = VentasChkPedidoIndependiente.Checked ? VentasTxtReferenciaIndependiente.Text.Trim() : null,
                    ClienteId = Cliente?.Id ?? 0, VendedorId = Vendedor?.Id ?? 0,
                    MonedaId = _MonedaId, TipoCambio = Cotizacion?.TipoCambio ?? 1m,
                    Fecha = VentasDtpPedido.Value.Date,
                    Entrega = VentasDtpEntrega.Value.Date,
                    Vencimiento = VentasDtpVencimiento.Value.Date,
                    Lineas = new List<ClsLineaPedido>(_Lineas)
                };
                int Id = _Controlador.VentasFuncGuardar(Pedido);
                MessageBox.Show(this, "Pedido " + Pedido.Numero + " guardado (ID " + Id +
                    ") con vencimiento " + Pedido.Vencimiento.ToString("dd/MM/yyyy") + ".",
                    "Pedido guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                VentasMetLimpiar();
            }
            catch (Exception Error) { MessageBox.Show(this, "No se guardó el pedido: " +
                (string.IsNullOrWhiteSpace(Error.Message) ? Error.ToString() : Error.Message),
                "Pedidos", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void VentasMetLimpiar()
        {
            VentasTxtNumeroPedido.Text = VentasFuncNuevoNumero();
            VentasTxtReferenciaIndependiente.Text = string.Empty;
            VentasDtpPedido.Value = DateTime.Today;
            VentasDtpEntrega.Value = DateTime.Today.AddDays(2);
            VentasDtpVencimiento.Value = DateTime.Today.AddDays(7);
            VentasMetCambiarOrigen(false);
            _MonedaId = _MonedaPublica;
        }
    }
}
