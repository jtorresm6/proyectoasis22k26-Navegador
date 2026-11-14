// ------------------------------------------------------
// Autor: Gabriela Pinto García
// Carné: 9959-23-1087
// Fecha: 04/11/2026
// Módulo: Logística - Transporte Externo
// ------------------------------------------------------

using System;
using System.Data;
using System.Windows.Forms;
using CapaControlador_TransporteExterno_Logistica;

namespace CapaVista_TransporteExterno_Logistica
{
    public partial class FrmIngresarProducto : Form
    {
        private ClsControladorIngresarProducto controlador;
        private DataTable productosCatalogo;
        private DataTable productosPendientes;

        private int idServicio;
        private int idProductoEditar;
        private decimal precioUnitario;

        public FrmIngresarProducto(
            int servicio,
            DataTable productos,
            int productoEditar = 0)
        {
            InitializeComponent();

            controlador = new ClsControladorIngresarProducto();

            idServicio = servicio;
            productosPendientes = productos;
            idProductoEditar = productoEditar;

            this.Load += FrmIngresarProducto_Load;

            LogisticaCboProducto.SelectedIndexChanged +=
                LogisticaCboProducto_SelectedIndexChanged;

            LogisticaTxtCantidad.TextChanged +=
                LogisticaTxtCantidad_TextChanged;

            LogisticaTxtCantidad.KeyPress +=
                LogisticaTxtCantidad_KeyPress;

            LogisticaBtnGuardarProducto.Click +=
                LogisticaBtnGuardarProducto_Click;

            LogisticaBtnSalir.Click +=
                LogisticaBtnSalir_Click;

            LogisticaTxtCantidad.Multiline = false;
            LogisticaTxtCantidad.Height = 25;
        }

        private void FrmIngresarProducto_Load(object sender, EventArgs e)
        {
            try
            {
                productosCatalogo =
                    controlador.LogisticaFuncObtenerProductosDisponibles(idServicio);

                LogisticaCboProducto.DataSource = productosCatalogo;
                LogisticaCboProducto.DisplayMember = "producto";
                LogisticaCboProducto.ValueMember = "idProducto";

                LogisticaCboProducto.DropDownStyle =
                    ComboBoxStyle.DropDownList;

                LogisticaCboProducto.SelectedIndex = -1;

                LogisticaLblPrecioUnitario1.Text = "Q 0.00";
                LogisticaLblCantidadProducto1.Text = "0";
                LogisticaLblPrecioTotal1.Text = "Q 0.00";

                if (idProductoEditar > 0)
                {
                    LogisticaLblIngresarProducto.Text =
                        "MODIFICAR PRODUCTO";

                    LogisticaCboProducto.SelectedValue = idProductoEditar;

                    foreach (DataRow fila in productosPendientes.Rows)
                    {
                        if (Convert.ToInt32(fila["idProducto"]) ==
                            idProductoEditar)
                        {
                            LogisticaTxtCantidad.Text =
                                Convert.ToInt32(fila["cantidad"]).ToString();

                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron cargar los productos.\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private DataRow LogisticaFuncObtenerProductoSeleccionado()
        {
            if (LogisticaCboProducto.SelectedValue == null ||
                LogisticaCboProducto.SelectedValue is DataRowView)
            {
                return null;
            }

            int idProducto = Convert.ToInt32(
                LogisticaCboProducto.SelectedValue);

            foreach (DataRow fila in productosCatalogo.Rows)
            {
                if (Convert.ToInt32(fila["idProducto"]) == idProducto)
                {
                    return fila;
                }
            }

            return null;
        }

        private void LogisticaCboProducto_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            DataRow producto = LogisticaFuncObtenerProductoSeleccionado();

            if (producto == null)
            {
                precioUnitario = 0;

                LogisticaLblPrecioUnitario1.Text = "Q 0.00";
                LogisticaMetActualizarResumen();

                return;
            }

            precioUnitario = Convert.ToDecimal(producto["precioUnitario"]);

            LogisticaLblPrecioUnitario1.Text =
                "Q " + precioUnitario.ToString("N2");

            LogisticaMetActualizarResumen();
        }

        private void LogisticaTxtCantidad_TextChanged(
            object sender,
            EventArgs e)
        {
            LogisticaMetActualizarResumen();
        }

        private void LogisticaTxtCantidad_KeyPress(
            object sender,
            KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void LogisticaMetActualizarResumen()
        {
            int cantidad = 0;

            int.TryParse(LogisticaTxtCantidad.Text, out cantidad);

            LogisticaLblCantidadProducto1.Text = cantidad.ToString();

            decimal total = controlador.LogisticaFuncCalcularTotal(
                precioUnitario,
                cantidad);

            LogisticaLblPrecioTotal1.Text =
                "Q " + total.ToString("N2");
        }

        private int LogisticaFuncObtenerExistenciaDisponible(int idProducto)
        {
            DataRow producto = null;

            foreach (DataRow fila in productosCatalogo.Rows)
            {
                if (Convert.ToInt32(fila["idProducto"]) == idProducto)
                {
                    producto = fila;
                    break;
                }
            }

            if (producto == null)
            {
                return 0;
            }

            int existenciaFisica =
                Convert.ToInt32(producto["stockFisico"]);

            int cantidadGuardada =
                Convert.ToInt32(producto["cantidadGuardadaServicio"]);

            int cantidadPendiente = 0;

            foreach (DataRow fila in productosPendientes.Rows)
            {
                int idActual = Convert.ToInt32(fila["idProducto"]);

                if (idActual == idProducto &&
                    idActual != idProductoEditar)
                {
                    cantidadPendiente +=
                        Convert.ToInt32(fila["cantidad"]);
                }
            }

            return existenciaFisica +
                   cantidadGuardada -
                   cantidadPendiente;
        }

        private void LogisticaBtnGuardarProducto_Click(
            object sender,
            EventArgs e)
        {
            DataRow producto = LogisticaFuncObtenerProductoSeleccionado();

            if (producto == null)
            {
                MessageBox.Show(
                    "Seleccione un producto.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                LogisticaCboProducto.Focus();
                return;
            }

            int cantidad;

            if (!int.TryParse(LogisticaTxtCantidad.Text, out cantidad) ||
                cantidad <= 0)
            {
                MessageBox.Show(
                    "Ingrese una cantidad válida.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                LogisticaTxtCantidad.Focus();
                return;
            }

            int idProducto = Convert.ToInt32(producto["idProducto"]);

            int existencia =
                LogisticaFuncObtenerExistenciaDisponible(idProducto);

            string nombreBodega =
                productosCatalogo.Columns.Contains("nombreBodega")
                ? Convert.ToString(producto["nombreBodega"])
                : "No disponible";

            if (existencia <= 0)
            {
                MessageBox.Show(
                    "No hay inventario disponible.\n\n" +
                    "Bodega: " + nombreBodega + "\n" +
                    "Cantidad solicitada: " + cantidad + "\n" +
                    "Cantidad disponible: " + existencia,
                    "Inventario insuficiente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (cantidad > existencia)
            {
                MessageBox.Show(
                    "No hay suficiente inventario.\n\n" +
                    "Bodega: " + nombreBodega + "\n" +
                    "Cantidad solicitada: " + cantidad + "\n" +
                    "Cantidad disponible: " + existencia,
                    "Inventario insuficiente",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            // Evitar productos duplicados.
            foreach (DataRow fila in productosPendientes.Rows)
            {
                int idActual = Convert.ToInt32(fila["idProducto"]);

                if (idActual == idProducto &&
                    idActual != idProductoEditar)
                {
                    MessageBox.Show(
                        "Este producto ya se encuentra en la lista. Utilice Modificar.",
                        "Producto existente",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }
            }

            decimal total = controlador.LogisticaFuncCalcularTotal(
                precioUnitario,
                cantidad);

            if (idProductoEditar > 0)
            {
                bool encontrado = false;

                foreach (DataRow fila in productosPendientes.Rows)
                {
                    if (Convert.ToInt32(fila["idProducto"]) ==
                        idProductoEditar)
                    {
                        fila["idProducto"] = idProducto;
                        fila["producto"] = producto["producto"];
                        fila["cantidad"] = cantidad;
                        fila["unidad"] = "Unidad";
                        fila["precioUnitario"] = precioUnitario;
                        fila["precioTotal"] = total;
                        fila["stockFisico"] = producto["stockFisico"];

                        fila["cantidadGuardadaServicio"] =
                            producto["cantidadGuardadaServicio"];

                        encontrado = true;
                        break;
                    }
                }

                if (!encontrado)
                {
                    MessageBox.Show(
                        "No se encontró el producto que desea modificar.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    return;
                }

                MessageBox.Show(
                    "Producto actualizado en la lista. Pulse Guardar en la pantalla anterior para confirmar los cambios.",
                    "Producto actualizado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                DataRow nuevaFila = productosPendientes.NewRow();

                nuevaFila["idProductoTransporte"] = DBNull.Value;
                nuevaFila["idProducto"] = idProducto;
                nuevaFila["producto"] = producto["producto"];
                nuevaFila["cantidad"] = cantidad;
                nuevaFila["unidad"] = "Unidad";
                nuevaFila["precioUnitario"] = precioUnitario;
                nuevaFila["precioTotal"] = total;
                nuevaFila["stockFisico"] = producto["stockFisico"];

                nuevaFila["cantidadGuardadaServicio"] =
                    producto["cantidadGuardadaServicio"];

                productosPendientes.Rows.Add(nuevaFila);

                MessageBox.Show(
                    "Producto agregado a la lista. Puede ingresar otro producto.",
                    "Producto agregado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            // Preparar el formulario para ingresar otro producto.
            idProductoEditar = 0;

            LogisticaLblIngresarProducto.Text = "INGRESAR PRODUCTO";

            LogisticaCboProducto.SelectedIndex = -1;
            LogisticaTxtCantidad.Clear();

            precioUnitario = 0;

            LogisticaLblPrecioUnitario1.Text = "Q 0.00";
            LogisticaLblCantidadProducto1.Text = "0";
            LogisticaLblPrecioTotal1.Text = "Q 0.00";
        }

        private void LogisticaBtnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
