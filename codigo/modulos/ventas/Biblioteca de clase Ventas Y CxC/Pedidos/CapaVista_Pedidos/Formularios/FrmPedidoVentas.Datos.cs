using System;
using System.Data;
using System.Windows.Forms;
using CapaControlador_Pedidos;

namespace CapaVista_Pedidos
{
    public partial class FrmPedidoVentas
    {
        private void VentasMetCargar()
        {
            try
            {
                VentasDtpPedido.Value = DateTime.Today;
                VentasDtpEntrega.Value = DateTime.Today.AddDays(2);
                VentasDtpVencimiento.Value = DateTime.Today.AddDays(7);
                VentasTxtNumeroPedido.Text = "Se asignará al guardar";
                DataTable Clientes = _Controlador.VentasFuncCatalogo("Clientes");
                VentasMetOpciones(VentasCboCliente, Clientes);
                VentasCboCliente.SelectedIndex = -1;
                VentasMetOpciones(VentasCboVendedor, _Controlador.VentasFuncCatalogo("Vendedores"));
                VentasCboVendedor.SelectedIndex = -1;
                VentasChkPedidoIndependiente.Checked = true;
                if (_Acciones.Id > 0) VentasMetPedido();
                _Cargando = false;
                VentasMetCambio(VentasCboCliente, EventArgs.Empty);
            }
            catch (Exception Error)
            {
                MessageBox.Show(this, Error.Message, "Pedidos");
                Close();
            }
        }

        private void VentasMetPedido()
        {
            DataTable Datos = _Controlador.VentasFuncPedido(_Acciones.Id);
            if (Datos.Rows.Count == 0) throw new InvalidOperationException("No se encontró el pedido.");
            DataRow Pedido = Datos.Rows[0];
            _Acciones.Estado = Convert.ToString(Pedido["Estado"]);
            _Acciones.Moneda = Convert.ToInt32(Pedido["MonedaId"]);
            VentasTxtNumeroPedido.Text = Convert.ToString(Pedido["Numero"]);
            VentasTxtEstadoPedido.Text = _Acciones.Estado;
            VentasDtpPedido.Value = Convert.ToDateTime(Pedido["Fecha"]);
            VentasDtpEntrega.Value = Pedido["Entrega"] == DBNull.Value ? VentasDtpPedido.Value : Convert.ToDateTime(Pedido["Entrega"]);
            VentasDtpVencimiento.Value = Pedido["Vencimiento"] == DBNull.Value ? VentasDtpPedido.Value : Convert.ToDateTime(Pedido["Vencimiento"]);
            VentasCboCliente.SelectedValue = Pedido["ClienteId"];
            VentasCboVendedor.SelectedValue = Pedido["VendedorId"];
            bool PorCotizacion = Pedido["CotizacionId"] != DBNull.Value;
            _Acciones.Cotizacion = PorCotizacion ? Convert.ToInt32(Pedido["CotizacionId"]) : 0;
            VentasChkPedidoPorCotizacion.Checked = PorCotizacion;
            VentasChkPedidoIndependiente.Checked = !PorCotizacion;
            VentasTxtMonedaPedido.Text = _Controlador.VentasFuncMoneda(_Acciones.Moneda);

            if (PorCotizacion)
            {
                var Opciones = new DataTable();
                Opciones.Columns.Add("Id", typeof(int));
                Opciones.Columns.Add("Nombre");
                Opciones.Rows.Add(Pedido["CotizacionId"], Pedido["CotizacionNumero"]);
                VentasMetOpciones(VentasCboCotizacionOrigen, Opciones);
            }
            else
            {
                VentasMetOpciones(VentasCboListaPrecioPedido,
                    _Controlador.VentasFuncListas(Convert.ToInt32(Pedido["ClienteId"]), VentasDtpPedido.Value));
                VentasCboListaPrecioPedido.SelectedIndex = -1;
            }
            foreach (DataRow Fila in Datos.Rows)
                if (Fila["ProductoId"] != DBNull.Value)
                    _Acciones.VentasMetLinea(Fila);
            VentasCboCliente.Enabled = VentasChkPedidoIndependiente.Enabled = VentasChkPedidoPorCotizacion.Enabled = false;
            VentasCboCotizacionOrigen.Enabled = false;
        }

        private void VentasMetCambio(object Origen, EventArgs Evento)
        {
            if (_Cargando) return;
            try
            {
                bool PorCotizacion = VentasChkPedidoPorCotizacion.Checked;
                if (Origen == VentasChkPedidoPorCotizacion || Origen == VentasChkPedidoIndependiente)
                {
                    _Cargando = true;
                    PorCotizacion = Origen == VentasChkPedidoPorCotizacion
                        ? VentasChkPedidoPorCotizacion.Checked : !VentasChkPedidoIndependiente.Checked;
                    VentasChkPedidoPorCotizacion.Checked = PorCotizacion;
                    VentasChkPedidoIndependiente.Checked = !PorCotizacion;
                    _Cargando = false;
                    Origen = VentasCboCliente;
                }
                VentasCboCotizacionOrigen.Visible = VentasLblCotizacionOrigen.Visible = PorCotizacion;
                VentasCboProductoPedido.Visible = VentasNudCantidadProducto.Visible = VentasNudDescuentoProducto.Visible = !PorCotizacion;
                VentasBtnIngresarProducto.Visible = VentasBtnEliminarProducto.Visible = !PorCotizacion;
                VentasTxtPrecioListaProducto.Visible = VentasLblPrecioListaProducto.Visible = !PorCotizacion;
                VentasLblCantidadProducto.Visible = VentasLblDescuentoProducto.Visible = !PorCotizacion;

                if (Origen == VentasCboCliente || Origen == VentasDtpPedido)
                {
                    var Cliente = VentasCboCliente.SelectedItem as DataRowView;
                    VentasTxtIdentificacionCliente.Text = Cliente == null ? "" : Convert.ToString(Cliente["numeroIdentificacion"]);
                    VentasTxtContactoCliente.Text = Cliente == null ? "" : Convert.ToString(Cliente["contactoCliente"]);
                    VentasTxtDireccionCliente.Text = Cliente == null ? "" : Convert.ToString(Cliente["direccionCliente"]);
                    if (_Acciones.Id > 0) { VentasMetDetalle(); return; }

                    int IdCliente = VentasFuncId(VentasCboCliente);
                    _Cargando = true;
                    if (PorCotizacion)
                    {
                        DataTable Opciones = _Acciones.VentasFuncCotizaciones(IdCliente, VentasDtpPedido.Value);
                        if (Opciones.Rows.Count == 0)
                            VentasCboCotizacionOrigen.DataSource = null;
                        else
                            VentasMetOpciones(VentasCboCotizacionOrigen, Opciones, "CotizacionId");
                        VentasCboCotizacionOrigen.SelectedIndex = -1;
                    }
                    else
                        VentasMetOpciones(VentasCboListaPrecioPedido, IdCliente > 0
                            ? _Controlador.VentasFuncListas(IdCliente, VentasDtpPedido.Value) : new DataTable());
                    _Cargando = false;
                    Origen = PorCotizacion ? (object)VentasCboCotizacionOrigen : VentasCboListaPrecioPedido;
                }
                if (Origen == VentasCboProductoPedido)
                {
                    var Producto = VentasCboProductoPedido.SelectedItem as DataRowView;
                    VentasTxtPrecioListaProducto.Text = Producto == null ? "" : Convert.ToDecimal(Producto["Precio"]).ToString("N4", Cultura);
                    return;
                }
                if (_Acciones.Id > 0)
                {
                    if (Origen == VentasCboListaPrecioPedido && VentasFuncId(VentasCboListaPrecioPedido) > 0)
                    {
                        var Lista = VentasCboListaPrecioPedido.SelectedItem as DataRowView;
                        if (Convert.ToInt32(Lista["MonedaId"]) != _Acciones.Moneda)
                            throw new ArgumentException("La lista debe usar la moneda del pedido.");
                        VentasMetOpciones(VentasCboProductoPedido,
                            _Controlador.VentasFuncProductos(VentasFuncId(VentasCboListaPrecioPedido)), "ProductoId");
                    }
                    VentasMetDetalle();
                    return;
                }
                _Acciones.Lineas.Clear();
                _Acciones.Moneda = 0;
                _Acciones.Cotizacion = 0;
                if (Origen == VentasCboCotizacionOrigen && VentasFuncId(VentasCboCotizacionOrigen) > 0)
                {
                    VentasCboVendedor.SelectedValue = _Acciones.VentasFuncCotizacion(
                        VentasFuncId(VentasCboCotizacionOrigen));
                }
                else if (Origen == VentasCboListaPrecioPedido && VentasFuncId(VentasCboListaPrecioPedido) > 0)
                {
                    var Lista = VentasCboListaPrecioPedido.SelectedItem as DataRowView;
                    _Acciones.Moneda = Convert.ToInt32(Lista["MonedaId"]);
                    _Cargando = true;
                    VentasMetOpciones(VentasCboProductoPedido,
                        _Controlador.VentasFuncProductos(VentasFuncId(VentasCboListaPrecioPedido)), "ProductoId");
                    _Cargando = false;
                }
                VentasTxtMonedaPedido.Text = _Acciones.Moneda > 0 ? _Controlador.VentasFuncMoneda(_Acciones.Moneda) : "";
                VentasMetDetalle();
            }
            catch (Exception Error)
            {
                _Cargando = false;
                MessageBox.Show(this, Error.Message, "Pedidos");
            }
        }

    }
}
