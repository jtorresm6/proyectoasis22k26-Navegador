/*
 * INICIO DEL CÓDIGO — Natali Montenegro
 * Carné: 0901-23-10017 | Fecha: 22/10/2026
 * Área: Ventas y cuentas por cobrar
 * Propósito: Capturar y administrar pedidos de Ventas.
 */
using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using CapaControlador_Pedidos;

namespace CapaVista_Pedidos
{
    // Natali Sofía Montenegro Portillo | 0901-23-10017 | Captura, consulta y modifica pedidos de venta.
    public partial class FrmPedidoVentas : Form
    {
        private readonly ClsPedidoControlador _Controlador = new ClsPedidoControlador();
        private readonly ClsPedidoAcciones _Acciones;
        private ComboBox VentasCboCliente { get { return VentasUsrCliente.VentasCboCliente; } }
        private bool _SoloLectura, _Cargando = true;
        private static readonly CultureInfo _Cultura = CultureInfo.GetCultureInfo("es-GT");

        public FrmPedidoVentas() : this(0, false) { }
        public FrmPedidoVentas(int Id) : this(Id, false) { }
        public FrmPedidoVentas(int Id, bool SoloLectura)
        {
            _Acciones = new ClsPedidoAcciones(Id, _Controlador);
            _SoloLectura = SoloLectura;
            InitializeComponent();
            VentasMetPrepararValidaciones();

            VentasDgvDetalle.ReadOnly = true;
            VentasDgvDetalle.AllowUserToAddRows = false;
            VentasDgvDetalle.AutoGenerateColumns = false;
            string[] Columnas = { "Codigo", "Descripcion", "Cantidad", "Precio", "Descuento", "Total" };
            for (int Indice = 0; Indice < Columnas.Length; Indice++)
                VentasDgvDetalle.Columns[Indice].DataPropertyName = Columnas[Indice];
            VentasDgvDetalleColCantidad.DefaultCellStyle.Format = "N0";
            VentasDgvDetalleColCantidad.DefaultCellStyle.FormatProvider = _Cultura;
            VentasDgvDetalle.DataSource = _Acciones.Lineas;
            Load += (Origen, Evento) => VentasMetCargar();
            foreach (ComboBox Combo in new[] { VentasCboCliente, VentasCboListaPrecioPedido,
                VentasCboCotizacionOrigen, VentasCboProductoPedido })
                Combo.SelectedIndexChanged += VentasMetCambio;
            VentasRdoPedidoIndependiente.CheckedChanged += VentasMetCambio;
            VentasRdoPedidoPorCotizacion.CheckedChanged += VentasMetCambio;
            VentasDtpPedido.ValueChanged += VentasMetCambio;
        }

        private void VentasMetAyuda()
        {
            try
            {
                Help.ShowHelp(this, _Controlador.VentasFuncRutaAyuda(typeof(FrmPedidoVentas).Assembly),
                    HelpNavigator.Topic, "html/Pedidos_Formulario.html");
            }
            catch (Exception Error)
            {
                MessageBox.Show(this, "No fue posible abrir la ayuda. " + Error.Message,
                    "Ayuda de pedidos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VentasMetAyudaClick(object Origen, EventArgs Evento) { VentasMetAyuda(); }

        protected override bool ProcessCmdKey(ref Message Mensaje, Keys Tecla)
        {
            if (Tecla == Keys.F1) { VentasMetAyuda(); return true; }
            return base.ProcessCmdKey(ref Mensaje, Tecla);
        }

        private static int VentasFuncId(ComboBox Combo)
        {
            return Combo.SelectedItem is DataRowView Fila ? Convert.ToInt32(Fila[Combo.ValueMember]) : 0;
        }

        private static void VentasMetOpciones(ComboBox Combo, DataTable Datos, string Id = "Id")
        {
            Combo.DataSource = null;
            if (Datos == null || !Datos.Columns.Contains("Nombre") || !Datos.Columns.Contains(Id))
                return;
            Combo.DisplayMember = "Nombre";
            Combo.ValueMember = Id;
            Combo.DataSource = Datos;
        }

        private string VentasFuncSimboloMoneda()
        {
            switch (VentasTxtMonedaPedido.Text)
            {
                case "GTQ": return "Q";
                case "USD": return "$";
                default: return VentasTxtMonedaPedido.Text;
            }
        }

        private string VentasFuncImporte(decimal Importe)
        {
            string Simbolo = VentasFuncSimboloMoneda();
            return (Simbolo.Length == 0 ? "" : Simbolo + " ") + Importe.ToString("N2", _Cultura);
        }

        private void VentasMetDetalle()
        {
            decimal[] Totales = _Acciones.VentasFuncTotales();
            string Simbolo = VentasFuncSimboloMoneda();
            VentasLblSubtotal.Text = "Subtotal " + VentasFuncImporte(Totales[0]);
            VentasLblDescuentoTotal.Text = "Descuentos " + VentasFuncImporte(Totales[0] - Totales[1]);
            VentasLblTotal.Text = "Total " + VentasFuncImporte(Totales[1]);
            VentasLblDescuentoProducto.Text = "Descuento" + (Simbolo.Length == 0 ? "" : " (" + Simbolo + ")");
            string Formato = Simbolo.Length == 0 ? "N2" : "'" + Simbolo + " '#,##0.00";
            foreach (var Columna in new[] { VentasDgvDetalleColPrecio, VentasDgvDetalleColDescuento, VentasDgvDetalleColTotal })
            {
                Columna.DefaultCellStyle.Format = Formato;
                Columna.DefaultCellStyle.FormatProvider = _Cultura;
                Columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            var Producto = VentasCboProductoPedido.SelectedItem as DataRowView;
            VentasTxtPrecioListaProducto.Text = Producto == null ? "" : VentasFuncImporte(Convert.ToDecimal(Producto["Precio"]));
            VentasDgvDetalle.Invalidate();
            bool Editable = !_SoloLectura && (_Acciones.Id == 0 || _Acciones.Estado == "PENDIENTE" || _Acciones.Estado == "CONFIRMADO");
            bool PorCotizacion = VentasRdoPedidoPorCotizacion.Checked;
            bool HayCotizaciones = VentasCboCotizacionOrigen.Items.Count > 0;
            VentasCboCotizacionOrigen.Enabled = Editable && _Acciones.Id == 0 && PorCotizacion && HayCotizaciones;
            if (PorCotizacion)
            {
                if (_Acciones.Id > 0 || _Acciones.Cotizacion > 0)
                    VentasLblDetalleAyuda.Text = "Productos de la cotización seleccionada.";
                else if (VentasFuncId(VentasCboCliente) <= 0)
                    VentasLblDetalleAyuda.Text = "Seleccione un cliente para buscar cotizaciones.";
                else
                    VentasLblDetalleAyuda.Text = HayCotizaciones
                        ? "Seleccione una cotización de referencia."
                        : "Sin cotizaciones disponibles para el cliente y la fecha.";
            }
            VentasCboEstadoPedido.Enabled = Editable && _Acciones.Id > 0;
            VentasCboVendedor.Enabled = VentasDtpPedido.Enabled = VentasDtpEntrega.Enabled = VentasDtpVencimiento.Enabled = Editable;
            VentasBtnGuardar.Enabled = Editable;
            VentasCboListaPrecioPedido.Enabled = Editable && !VentasRdoPedidoPorCotizacion.Checked;
            VentasBtnIngresarProducto.Enabled = Editable && !VentasRdoPedidoPorCotizacion.Checked;
            VentasBtnEliminarProducto.Enabled = Editable && !VentasRdoPedidoPorCotizacion.Checked;
            VentasBtnModificarPedido.Enabled = _SoloLectura && _Acciones.Id > 0 && (_Acciones.Estado == "PENDIENTE" || _Acciones.Estado == "CONFIRMADO");
            VentasBtnAnularPedido.Enabled = Editable && _Acciones.Id > 0;
        }

        private void VentasMetAccion(object Origen, EventArgs Evento)
        {
            try
            {
                if (Origen == VentasBtnCancelar) { Close(); return; }
                if (Origen == VentasBtnNuevoPedido) { using (var Formulario = new FrmPedidoVentas()) Formulario.ShowDialog(this); return; }
                if (Origen == VentasBtnConsultarPedido) { using (var Formulario = new FrmSeguimientoPedidos()) Formulario.ShowDialog(this); return; }
                if (Origen == VentasBtnModificarPedido) { _SoloLectura = false; VentasMetDetalle(); return; }
                if (Origen == VentasBtnAnularPedido)
                {
                    if (MessageBox.Show(this, "¿Anular el pedido?", "Pedidos", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        _Acciones.VentasMetEstado("ANULADO");
                        MessageBox.Show(this, "Pedido anulado correctamente.", "Pedidos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    return;
                }
                if (Origen == VentasBtnIngresarProducto)
                {
                    var Producto = VentasCboProductoPedido.SelectedItem as DataRowView;
                    if (!VentasFuncValidarProducto(Producto)) return;
                    _Acciones.VentasMetProducto(Producto.Row, VentasNudCantidadProducto.Value,
                        VentasNudDescuentoProducto.Value);
                    VentasMetDetalle();
                    return;
                }
                if (Origen == VentasBtnEliminarProducto)
                {
                    var Fila = VentasDgvDetalle.CurrentRow?.DataBoundItem as DataRowView;
                    if (Fila != null) _Acciones.Lineas.Rows.Remove(Fila.Row);
                    VentasMetDetalle();
                    return;
                }
                if (Origen != VentasBtnGuardar || !VentasFuncValidarPedido()) return;
                string Numero = _Acciones.VentasFuncGuardar(
                    VentasFuncId(VentasCboCliente), VentasFuncId(VentasCboVendedor),
                    VentasRdoPedidoPorCotizacion.Checked,
                    VentasDtpPedido.Value.Date, VentasDtpEntrega.Value.Date, VentasDtpVencimiento.Value.Date,
                    _Acciones.Id > 0 ? VentasTxtNumeroPedido.Text : "",
                    Convert.ToString(VentasCboEstadoPedido.SelectedItem));
                MessageBox.Show(this, "Pedido " + Numero + " guardado.", "Pedidos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception Error)
            {
                MessageBox.Show(this, "No se pudo completar la operación. " + Error.Message, "Pedidos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
