using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CapaControlador_Pedidos;

namespace CapaVista_Pedidos
{
    public partial class FrmPedidoVentas
    {
        private void VentasMetPrepararDetalle()
        {
            VentasCboProductoPedido.Name = "VentasCboProductoPedido";
            VentasCboProductoPedido.DropDownStyle = ComboBoxStyle.DropDownList;
            VentasCboProductoPedido.DisplayMember = "Descripcion";
            VentasCboProductoPedido.Location = new Point(255, 18);
            VentasCboProductoPedido.Size = new Size(390, 25);
            VentasNudCantidadProducto.Name = "VentasNudCantidadProducto";
            VentasNudCantidadProducto.Location = new Point(655, 18);
            VentasNudCantidadProducto.Size = new Size(95, 25);
            VentasNudCantidadProducto.DecimalPlaces = 4;
            VentasNudCantidadProducto.Minimum = 1m;
            VentasNudCantidadProducto.Maximum = 1000000m;
            VentasNudCantidadProducto.Value = 1m;
            VentasPnlEncabezadoDetalle.Controls.Add(VentasCboProductoPedido);
            VentasPnlEncabezadoDetalle.Controls.Add(VentasNudCantidadProducto);
            VentasCboProductoPedido.BringToFront();
            VentasNudCantidadProducto.BringToFront();
        }

        private void VentasMetAgregarProducto()
        {
            var Producto = VentasCboProductoPedido.SelectedItem as ClsLineaPedido;
            if (Producto == null) return;
            var Linea = new ClsLineaPedido {
                ProductoId = Producto.ProductoId, Codigo = Producto.Codigo,
                Descripcion = Producto.Descripcion, Tipo = Producto.Tipo,
                Cantidad = VentasNudCantidadProducto.Value, Precio = Producto.Precio,
                PorcentajeImpuesto = Producto.PorcentajeImpuesto
            };
            ClsPedidoControlador.VentasMetCalcular(Linea);
            _Lineas.Add(Linea);
            VentasMetActualizarDetalle();
        }

        private void VentasMetQuitarProducto()
        {
            if (VentasDgvDetalle.CurrentRow == null) return;
            int Indice = VentasDgvDetalle.CurrentRow.Index;
            if (Indice >= 0 && Indice < _Lineas.Count) { _Lineas.RemoveAt(Indice); VentasMetActualizarDetalle(); }
        }

        private void VentasMetActualizarDetalle()
        {
            VentasDgvDetalle.Rows.Clear();
            foreach (var Linea in _Lineas)
                VentasDgvDetalle.Rows.Add(Linea.Codigo, Linea.Descripcion, string.Empty, Linea.Cantidad.ToString("N4", _Cultura),
                    Linea.Precio.ToString("N4", _Cultura), Linea.Descuento.ToString("N2", _Cultura), Linea.Total.ToString("N2", _Cultura));
            decimal Subtotal = _Lineas.Sum(Linea => Math.Round(Linea.Cantidad * Linea.Precio, 2, MidpointRounding.AwayFromZero));
            VentasLblSubtotal.Text = "Subtotal    Q " + Subtotal.ToString("N2", _Cultura);
            VentasLblImpuesto.Text = "Impuestos    Q " + _Lineas.Sum(Linea => Linea.Impuesto).ToString("N2", _Cultura);
            VentasLblTotal.Text = "TOTAL    Q " + _Lineas.Sum(Linea => Linea.Total).ToString("N2", _Cultura);
            VentasMetActualizarAcciones();
        }

        private void VentasMetActualizarAcciones()
        {
            bool Independiente = VentasChkPedidoIndependiente.Checked;
            bool PuedeEditar = Independiente && !_Cargando && _CatalogosDisponibles;
            VentasCboProductoPedido.Enabled = VentasNudCantidadProducto.Enabled = PuedeEditar;
            VentasBtnIngresarProducto.Enabled = PuedeEditar && VentasCboProductoPedido.SelectedItem is ClsLineaPedido;
            VentasBtnEliminarProducto.Enabled = PuedeEditar &&
                VentasDgvDetalle.CurrentRow != null && VentasDgvDetalle.CurrentRow.Index < _Lineas.Count;
            VentasBtnGuardar.Enabled = !_Cargando && _CatalogosDisponibles;
        }
    }
}
