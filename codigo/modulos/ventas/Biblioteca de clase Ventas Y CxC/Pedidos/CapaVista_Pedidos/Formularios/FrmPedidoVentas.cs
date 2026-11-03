using System;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using CapaControlador_Pedidos;

namespace CapaVista_Pedidos
{
    public partial class FrmPedidoVentas : Form
    {
        private readonly ClsPedidoControlador _Controlador = new ClsPedidoControlador();
        private readonly ClsPedidoAcciones _Acciones;
        private bool _SoloLectura, _Cargando = true;
        private static readonly CultureInfo Cultura = CultureInfo.GetCultureInfo("es-GT");

        public FrmPedidoVentas() : this(0, false) { }
        public FrmPedidoVentas(int Id) : this(Id, false) { }
        public FrmPedidoVentas(int Id, bool SoloLectura)
        {
            _Acciones = new ClsPedidoAcciones(Id, _Controlador);
            _SoloLectura = SoloLectura;
            InitializeComponent();
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;

            VentasDgvDetalle.ReadOnly = true;
            VentasDgvDetalle.AllowUserToAddRows = false;
            VentasDgvDetalle.AutoGenerateColumns = false;
            string[] Columnas = { "Codigo", "Descripcion", "", "Cantidad", "Precio", "Descuento", "Total" };
            for (int Indice = 0; Indice < Columnas.Length; Indice++)
                VentasDgvDetalle.Columns[Indice].DataPropertyName = Columnas[Indice];
            VentasDgvDetalle.DataSource = _Acciones.Lineas;
            Load += (Origen, Evento) => VentasMetCargar();
            foreach (ComboBox Combo in new[] { VentasCboCliente, VentasCboListaPrecioPedido,
                VentasCboCotizacionOrigen, VentasCboProductoPedido })
                Combo.SelectedIndexChanged += VentasMetCambio;
            VentasChkPedidoIndependiente.CheckedChanged += VentasMetCambio;
            VentasChkPedidoPorCotizacion.CheckedChanged += VentasMetCambio;
            VentasDtpPedido.ValueChanged += VentasMetCambio;
        }

        private static int VentasFuncId(ComboBox Combo)
        {
            return Combo.SelectedItem is DataRowView Fila ? Convert.ToInt32(Fila[Combo.ValueMember]) : 0;
        }

        private static void VentasMetOpciones(ComboBox Combo, DataTable Datos, string Id = "Id")
        {
            Combo.DisplayMember = "Nombre";
            Combo.ValueMember = Id;
            Combo.DataSource = Datos;
        }

        private void VentasMetDetalle()
        {
            decimal[] Totales = _Acciones.VentasFuncTotales();
            string Moneda = VentasTxtMonedaPedido.Text;
            VentasLblSubtotal.Text = "Subtotal " + Moneda + " " + Totales[0].ToString("N2", Cultura);
            VentasLblTotal.Text = "TOTAL " + Moneda + " " + Totales[1].ToString("N2", Cultura);
            bool Editable = !_SoloLectura && (_Acciones.Id == 0 || _Acciones.Estado == "PENDIENTE" || _Acciones.Estado == "CONFIRMADO");
            VentasCboVendedor.Enabled = VentasDtpPedido.Enabled = VentasDtpEntrega.Enabled = VentasDtpVencimiento.Enabled = Editable;
            VentasBtnGuardar.Enabled = Editable && _Acciones.Lineas.Rows.Count > 0 && _Acciones.Moneda > 0 && VentasFuncId(VentasCboVendedor) > 0;
            VentasCboListaPrecioPedido.Enabled = Editable && !VentasChkPedidoPorCotizacion.Checked;
            VentasBtnIngresarProducto.Enabled = Editable && !VentasChkPedidoPorCotizacion.Checked && VentasFuncId(VentasCboProductoPedido) > 0;
            VentasBtnEliminarProducto.Enabled = Editable && !VentasChkPedidoPorCotizacion.Checked;
            VentasBtnModificarPedido.Enabled = _SoloLectura && _Acciones.Id > 0 && (_Acciones.Estado == "PENDIENTE" || _Acciones.Estado == "CONFIRMADO");
            VentasBtnAnularPedido.Enabled = _Acciones.Id > 0 && (_Acciones.Estado == "PENDIENTE" || _Acciones.Estado == "CONFIRMADO");
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
                    if (MessageBox.Show(this, "¿Anular el pedido?", "Pedidos", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        _Acciones.VentasMetEstado("ANULADO");
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    return;
                }
                if (Origen == VentasBtnIngresarProducto)
                {
                    var Producto = VentasCboProductoPedido.SelectedItem as DataRowView;
                    _Acciones.VentasMetProducto(Producto?.Row, VentasNudCantidadProducto.Value,
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
                if (Origen != VentasBtnGuardar) return;
                string Numero = _Acciones.VentasFuncGuardar(
                    VentasFuncId(VentasCboCliente), VentasFuncId(VentasCboVendedor),
                    VentasChkPedidoPorCotizacion.Checked,
                    VentasDtpPedido.Value.Date, VentasDtpEntrega.Value.Date, VentasDtpVencimiento.Value.Date,
                    _Acciones.Id > 0 ? VentasTxtNumeroPedido.Text : "");
                MessageBox.Show(this, "Pedido " + Numero + " guardado.", "Pedidos");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception Error)
            {
                MessageBox.Show(this, Error.Message, "Pedidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
