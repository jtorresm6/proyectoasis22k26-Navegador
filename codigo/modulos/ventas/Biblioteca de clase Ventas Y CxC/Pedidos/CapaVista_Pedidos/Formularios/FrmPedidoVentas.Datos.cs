/*
 * INICIO DEL CÓDIGO — Natali Montenegro
 * Carné: 0901-23-10017 | Fecha: 22/10/2026
 * Área: Ventas y cuentas por cobrar
 * Propósito: Capturar y administrar pedidos de Ventas.
 */
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CapaControlador_Pedidos;

namespace CapaVista_Pedidos
{
    // Natali Sofía Montenegro Portillo | 0901-23-10017 | Carga clientes, listas, productos y cotizaciones en el formulario.
    public partial class FrmPedidoVentas
    {
        private Func<string, bool> _ErrorCliente, _ErrorVendedor, _ErrorLista, _ErrorCotizacion;
        private Func<string, bool> _ErrorEntrega, _ErrorVencimiento, _ErrorDetalle;
        private Func<string, bool> _ErrorProducto, _ErrorCantidad, _ErrorDescuento;

        internal static Func<string, bool> VentasFuncCrearAviso(Control Campo, Label Etiqueta = null)
        {
            Control Contenedor = Campo.Parent;
            var Aviso = new Label
            {
                AutoSize = true,
                Font = new Font("Tahoma", 8F),
                ForeColor = Color.DarkRed,
                Visible = false,
                Location = new Point(Campo.Left, Campo.Bottom + 1)
            };
            Contenedor.Controls.Add(Aviso);
            Contenedor.Paint += (Origen, Evento) =>
            {
                if (!Aviso.Visible || !Campo.Visible) return;
                Rectangle Borde = Campo.Bounds;
                Borde.Inflate(1, 1);
                Evento.Graphics.DrawRectangle(Pens.DarkRed, Borde);
            };
            if (Etiqueta != null)
            {
                var Asterisco = new Label
                {
                    AutoSize = true,
                    ForeColor = Color.DarkRed,
                    Text = "*",
                    AccessibleName = "Campo obligatorio",
                    Location = new Point(Etiqueta.Left + Etiqueta.PreferredWidth + 2, Etiqueta.Top)
                };
                Etiqueta.Parent.Controls.Add(Asterisco);
                Etiqueta.VisibleChanged += (Origen, Evento) => Asterisco.Visible = Etiqueta.Visible;
            }
            return Mensaje =>
            {
                Aviso.Text = Mensaje;
                Aviso.Visible = Mensaje.Length > 0;
                Campo.AccessibleDescription = Mensaje;
                Contenedor.Invalidate();
                return Mensaje.Length == 0;
            };
        }

        private void VentasMetPrepararValidaciones()
        {
            _ErrorCliente = VentasFuncCrearAviso(VentasCboCliente, VentasUsrCliente.VentasLblCampoCliente);
            _ErrorVendedor = VentasFuncCrearAviso(VentasCboVendedor, VentasLblVendedor);
            _ErrorLista = VentasFuncCrearAviso(VentasCboListaPrecioPedido);
            _ErrorCotizacion = VentasFuncCrearAviso(VentasCboCotizacionOrigen, VentasLblCotizacionOrigen);
            _ErrorEntrega = VentasFuncCrearAviso(VentasDtpEntrega, VentasLblEntrega);
            _ErrorVencimiento = VentasFuncCrearAviso(VentasDtpVencimiento, VentasLblVencimiento);
            _ErrorDetalle = VentasFuncCrearAviso(VentasDgvDetalle);
            _ErrorProducto = VentasFuncCrearAviso(VentasCboProductoPedido);
            _ErrorCantidad = VentasFuncCrearAviso(VentasNudCantidadProducto);
            _ErrorDescuento = VentasFuncCrearAviso(VentasNudDescuentoProducto);

            VentasCboCliente.SelectedIndexChanged += (Origen, Evento) => _ErrorCliente("");
            VentasCboVendedor.SelectedIndexChanged += (Origen, Evento) => _ErrorVendedor("");
            VentasCboListaPrecioPedido.SelectedIndexChanged += (Origen, Evento) => _ErrorLista("");
            VentasCboCotizacionOrigen.SelectedIndexChanged += (Origen, Evento) => _ErrorCotizacion("");
            VentasCboProductoPedido.SelectedIndexChanged += (Origen, Evento) => VentasMetLimpiarErrorProducto();
            VentasNudCantidadProducto.ValueChanged += (Origen, Evento) => VentasMetLimpiarErrorProducto();
            VentasNudDescuentoProducto.ValueChanged += (Origen, Evento) => _ErrorDescuento("");
            VentasDtpEntrega.ValueChanged += (Origen, Evento) => _ErrorEntrega("");
            VentasDtpVencimiento.ValueChanged += (Origen, Evento) => _ErrorVencimiento("");
            _Acciones.Lineas.RowChanged += (Origen, Evento) => _ErrorDetalle("");
        }

        private void VentasMetLimpiarErrorProducto()
        {
            _ErrorProducto("");
            _ErrorCantidad("");
            _ErrorDescuento("");
        }

        private bool VentasFuncValidarPedido()
        {
            bool Cotizado = VentasRdoPedidoPorCotizacion.Checked;
            bool Nuevo = _Acciones.Id == 0;
            string ErrorOrigen = ClsReglasPedidos.VentasFuncErrorOrigen(Cotizado, _Acciones.Cotizacion, _Acciones.Moneda);
            bool Valido = _ErrorCliente(Nuevo ? ClsReglasPedidos.VentasFuncErrorCliente(VentasFuncId(VentasCboCliente)) : "");
            Valido &= _ErrorVendedor(ClsReglasPedidos.VentasFuncErrorVendedor(VentasFuncId(VentasCboVendedor)));
            Valido &= _ErrorLista(Cotizado ? "" : ErrorOrigen);
            Valido &= _ErrorCotizacion(Cotizado ? ErrorOrigen : "");
            Valido &= _ErrorEntrega(ClsReglasPedidos.VentasFuncErrorFecha(VentasDtpEntrega.Value, VentasDtpPedido.Value));
            Valido &= _ErrorVencimiento(ClsReglasPedidos.VentasFuncErrorFecha(VentasDtpVencimiento.Value, VentasDtpPedido.Value));
            Valido &= _ErrorDetalle(ClsReglasPedidos.VentasFuncErrorDetalle(_Acciones.Lineas.Rows.Count));
            return Valido;
        }

        private bool VentasFuncValidarProducto(DataRowView Producto)
        {
            decimal Cantidad = VentasNudCantidadProducto.Value;
            decimal? Precio = Producto == null ? (decimal?)null : Convert.ToDecimal(Producto["Precio"]);
            bool Valido = _ErrorProducto(ClsReglasPedidos.VentasFuncErrorProducto(Producto != null));
            Valido &= _ErrorCantidad(ClsReglasPedidos.VentasFuncErrorCantidad(Cantidad));
            Valido &= _ErrorDescuento(ClsReglasPedidos.VentasFuncErrorDescuento(Cantidad, Precio, VentasNudDescuentoProducto.Value));
            return Valido;
        }

        private void VentasMetCargar()
        {
            try
            {
                VentasDtpPedido.Value = DateTime.Today;
                VentasDtpEntrega.Value = DateTime.Today.AddDays(2);
                VentasDtpVencimiento.Value = DateTime.Today.AddDays(7);
                VentasTxtNumeroPedido.Text = "Se asignará al guardar";
                DataTable Clientes = _Controlador.VentasFuncCatalogo("Clientes");
                VentasUsrCliente.VentasMetCargarClientes(Clientes);
                VentasMetOpciones(VentasCboVendedor, _Controlador.VentasFuncCatalogo("Vendedores"));
                VentasCboVendedor.SelectedIndex = -1;
                VentasRdoPedidoIndependiente.Checked = true;
                if (_Acciones.Id > 0) VentasMetPedido();
                _Cargando = false;
                VentasMetCambio(VentasCboCliente, EventArgs.Empty);
            }
            catch (Exception Error)
            {
                MessageBox.Show(this, Error.Message, "Pedidos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        private void VentasMetPedido()
        {
            DataTable Datos = _Controlador.VentasFuncPedido(_Acciones.Id);
            if (Datos.Rows.Count == 0) throw new InvalidOperationException("No se encontró el pedido.");
            DataRow Pedido = Datos.Rows[0];
            _Acciones.Estado = Convert.ToString(Pedido["Estado"]);
            VentasCboEstadoPedido.SelectedItem = _Acciones.Estado;
            _Acciones.Moneda = Convert.ToInt32(Pedido["MonedaId"]);
            VentasTxtNumeroPedido.Text = Convert.ToString(Pedido["Numero"]);
            VentasDtpPedido.Value = Convert.ToDateTime(Pedido["Fecha"]);
            VentasDtpEntrega.Value = Pedido["Entrega"] == DBNull.Value ? VentasDtpPedido.Value : Convert.ToDateTime(Pedido["Entrega"]);
            VentasDtpVencimiento.Value = Pedido["Vencimiento"] == DBNull.Value ? VentasDtpPedido.Value : Convert.ToDateTime(Pedido["Vencimiento"]);
            VentasCboCliente.SelectedValue = Pedido["ClienteId"];
            VentasTxtIdentificacionCliente.Text = Convert.ToString(Pedido["numeroIdentificacion"]);
            VentasTxtContactoCliente.Text = Convert.ToString(Pedido["contactoCliente"]);
            VentasTxtDireccionCliente.Text = Convert.ToString(Pedido["direccionCliente"]);
            VentasCboVendedor.SelectedValue = Pedido["VendedorId"];
            bool PorCotizacion = Pedido["CotizacionId"] != DBNull.Value;
            _Acciones.Cotizacion = PorCotizacion ? Convert.ToInt32(Pedido["CotizacionId"]) : 0;
            if (PorCotizacion) VentasRdoPedidoPorCotizacion.Checked = true;
            else VentasRdoPedidoIndependiente.Checked = true;
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
            VentasCboCliente.Enabled = VentasRdoPedidoIndependiente.Enabled = VentasRdoPedidoPorCotizacion.Enabled = false;
            VentasCboCotizacionOrigen.Enabled = false;
        }

        private void VentasMetCambio(object Origen, EventArgs Evento)
        {
            if (_Cargando || (Origen is RadioButton Tipo && !Tipo.Checked)) return;

            try
            {
                bool PorCotizacion = VentasRdoPedidoPorCotizacion.Checked;
                if (Origen is RadioButton)
                {
                    VentasMetLimpiarErrorProducto();
                    _ErrorLista("");
                    _ErrorCotizacion("");
                    Origen = VentasCboCliente;
                }
                VentasMetMostrarTipo(PorCotizacion);

                if (Origen == VentasCboCliente || Origen == VentasDtpPedido)
                {
                    if (_Acciones.Id == 0)
                        VentasMetActualizarOrigen(PorCotizacion);
                }
                else if (Origen == VentasCboCotizacionOrigen && _Acciones.Id == 0)
                    VentasMetSeleccionarOrigen(true);
                else if (Origen == VentasCboListaPrecioPedido)
                    VentasMetSeleccionarOrigen(false);

                VentasMetDetalle();
            }
            catch (Exception Error)
            {
                MessageBox.Show(this, Error.Message, "Pedidos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { _Cargando = false; }
        }

        private void VentasMetMostrarTipo(bool PorCotizacion)
        {
            VentasCboCotizacionOrigen.Visible = VentasLblCotizacionOrigen.Visible = PorCotizacion;
            VentasCboProductoPedido.Visible = VentasNudCantidadProducto.Visible = VentasNudDescuentoProducto.Visible = !PorCotizacion;
            VentasBtnIngresarProducto.Visible = VentasBtnEliminarProducto.Visible = !PorCotizacion;
            VentasTxtPrecioListaProducto.Visible = VentasLblPrecioListaProducto.Visible = !PorCotizacion;
            VentasLblCantidadProducto.Visible = VentasLblDescuentoProducto.Visible = !PorCotizacion;
            VentasLblDetalleAyuda.Visible = PorCotizacion;
        }

        private void VentasMetActualizarOrigen(bool PorCotizacion)
        {
            var Cliente = VentasCboCliente.SelectedItem as DataRowView;
            VentasTxtIdentificacionCliente.Text = Convert.ToString(Cliente?["numeroIdentificacion"]);
            VentasTxtContactoCliente.Text = Convert.ToString(Cliente?["contactoCliente"]);
            VentasTxtDireccionCliente.Text = Convert.ToString(Cliente?["direccionCliente"]);
            int IdCliente = VentasFuncId(VentasCboCliente);
            DateTime Fecha = VentasDtpPedido.Value;

            _Cargando = true;
            if (PorCotizacion)
            {
                int Seleccion = VentasFuncId(VentasCboCotizacionOrigen);
                DataTable Opciones = _Acciones.VentasFuncCotizaciones(IdCliente, Fecha);
                VentasMetOpciones(VentasCboCotizacionOrigen, Opciones, "CotizacionId");
                VentasCboCotizacionOrigen.SelectedIndex = -1;
                if (Seleccion > 0 && Opciones.Rows.Count > 0)
                    VentasCboCotizacionOrigen.SelectedValue = Seleccion;
            }
            else
            {
                VentasMetOpciones(VentasCboListaPrecioPedido, IdCliente > 0
                    ? _Controlador.VentasFuncListas(IdCliente, Fecha) : new DataTable());
                VentasCboProductoPedido.DataSource = null;
            }
            VentasMetSeleccionarOrigen(PorCotizacion);
        }

        private void VentasMetSeleccionarOrigen(bool PorCotizacion)
        {
            if (_Acciones.Id == 0)
            {
                _Acciones.Lineas.Clear();
                _Acciones.Moneda = 0;
                _Acciones.Cotizacion = 0;
            }

            if (PorCotizacion)
            {
                int Cotizacion = VentasFuncId(VentasCboCotizacionOrigen);
                if (Cotizacion > 0)
                    VentasCboVendedor.SelectedValue = _Acciones.VentasFuncCotizacion(Cotizacion);
            }
            else
                VentasMetCargarProductos();

            if (_Acciones.Id == 0)
                VentasTxtMonedaPedido.Text = _Acciones.Moneda > 0 ? _Controlador.VentasFuncMoneda(_Acciones.Moneda) : "";
        }

        private void VentasMetCargarProductos()
        {
            int IdLista = VentasFuncId(VentasCboListaPrecioPedido);
            if (IdLista <= 0) return;

            var Lista = (DataRowView)VentasCboListaPrecioPedido.SelectedItem;
            int Moneda = Convert.ToInt32(Lista["MonedaId"]);
            if (_Acciones.Id > 0)
            {
                if (Moneda != _Acciones.Moneda)
                    throw new ArgumentException("La lista debe usar la moneda del pedido.");
            }
            else
            {
                if (VentasUsrCliente.TipoClienteId != Convert.ToInt32(Lista["TipoClienteId"]))
                    throw new ArgumentException("La lista de precios no corresponde al tipo de cliente.");
                _Acciones.Moneda = Moneda;
            }

            _Cargando = true;
            VentasMetOpciones(VentasCboProductoPedido, _Controlador.VentasFuncProductos(IdLista), "ProductoId");
        }
    }
}
