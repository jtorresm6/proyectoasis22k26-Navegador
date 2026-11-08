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
using CapaVista_TransporteExterno_Logistica;

namespace CapaVista_TransporteExterno_Logistica.Formularios
{
    public partial class FrmProductosATrasladar : Form
    {
        private ClsControladorIngresarProducto controlador;
        private DataTable productosPendientes;

        public FrmProductosATrasladar()
        {
            InitializeComponent();

            controlador = new ClsControladorIngresarProducto();

            this.Load += FrmProductosATrasladar_Load;

            LogisticaCboNumeroGuia.SelectedIndexChanged +=
                LogisticaCboNumeroGuia_SelectedIndexChanged;

            LogisticaBtnIngresarProducto.Click +=
                LogisticaBtnIngresarProducto_Click;

            LogisticaBtnModificarProducto.Click +=
                LogisticaBtnModificarProducto_Click;

            LogisticaBtnEliminarProducto.Click +=
                LogisticaBtnEliminarProducto_Click;

            LogisticaBtnGuardarProducto.Click +=
                LogisticaBtnGuardarProducto_Click;

            LogisticaDgvProductos.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            LogisticaDgvProductos.MultiSelect = false;
            LogisticaDgvProductos.AutoGenerateColumns = true;
        }

        private void FrmProductosATrasladar_Load(object sender, EventArgs e)
        {
            try
            {
                LogisticaCboNumeroGuia.DataSource =
                    controlador.LogisticaFuncObtenerGuias();

                LogisticaCboNumeroGuia.DisplayMember = "numeroGuia";
                LogisticaCboNumeroGuia.ValueMember = "idServicio";
                LogisticaCboNumeroGuia.SelectedIndex = -1;

                productosPendientes = null;
                LogisticaDgvProductos.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron cargar las guías.\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private int LogisticaFuncObtenerIdServicio()
        {
            if (LogisticaCboNumeroGuia.SelectedValue == null ||
                LogisticaCboNumeroGuia.SelectedValue is DataRowView)
            {
                return 0;
            }

            return Convert.ToInt32(LogisticaCboNumeroGuia.SelectedValue);
        }

        private void LogisticaCboNumeroGuia_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            int idServicio = LogisticaFuncObtenerIdServicio();

            if (idServicio <= 0)
            {
                return;
            }

            try
            {
                productosPendientes =
                    controlador.LogisticaFuncObtenerProductosDelServicio(idServicio);

                LogisticaDgvProductos.DataSource = productosPendientes;

                LogisticaMetConfigurarColumnas();
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

        private void LogisticaMetConfigurarColumnas()
        {
            if (LogisticaDgvProductos.Columns.Count == 0)
            {
                return;
            }

            LogisticaDgvProductos.Columns["idProductoTransporte"].Visible = false;
            LogisticaDgvProductos.Columns["idProducto"].Visible = false;
            LogisticaDgvProductos.Columns["stockFisico"].Visible = false;
            LogisticaDgvProductos.Columns["cantidadGuardadaServicio"].Visible = false;

            LogisticaDgvProductos.Columns["producto"].HeaderText = "Producto";
            LogisticaDgvProductos.Columns["cantidad"].HeaderText = "Cantidad";
            LogisticaDgvProductos.Columns["unidad"].HeaderText = "Unidad";
            LogisticaDgvProductos.Columns["precioUnitario"].HeaderText = "Precio unitario";
            LogisticaDgvProductos.Columns["precioTotal"].HeaderText = "Precio total";

            LogisticaDgvProductos.Columns["producto"].DisplayIndex = 0;
            LogisticaDgvProductos.Columns["cantidad"].DisplayIndex = 1;
            LogisticaDgvProductos.Columns["unidad"].DisplayIndex = 2;
            LogisticaDgvProductos.Columns["precioUnitario"].DisplayIndex = 3;
            LogisticaDgvProductos.Columns["precioTotal"].DisplayIndex = 4;

            LogisticaDgvProductos.Columns["precioUnitario"].DefaultCellStyle.Format = "Q #,##0.00";
            LogisticaDgvProductos.Columns["precioTotal"].DefaultCellStyle.Format = "Q #,##0.00";

            LogisticaDgvProductos.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void LogisticaBtnIngresarProducto_Click(
            object sender, EventArgs e)
        {
            int idServicio = LogisticaFuncObtenerIdServicio();

            if (idServicio <= 0)
            {
                MessageBox.Show(
                    "Primero debe seleccionar una guía.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (productosPendientes == null)
                {
                    productosPendientes =
                        controlador.LogisticaFuncObtenerProductosDelServicio(idServicio);
                }

                FrmIngresarProducto formulario =
                    new FrmIngresarProducto(idServicio, productosPendientes);

                formulario.ShowDialog();

                LogisticaDgvProductos.DataSource = null;
                LogisticaDgvProductos.DataSource = productosPendientes;

                LogisticaMetConfigurarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo abrir el formulario.\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LogisticaBtnModificarProducto_Click(
            object sender, EventArgs e)
        {
            int idServicio = LogisticaFuncObtenerIdServicio();

            if (idServicio <= 0 || productosPendientes == null)
            {
                MessageBox.Show("Seleccione una guía y un producto.");
                return;
            }

            if (LogisticaDgvProductos.CurrentRow == null)
            {
                MessageBox.Show(
                    "Seleccione el producto que desea modificar.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            int idProducto = Convert.ToInt32(
                LogisticaDgvProductos.CurrentRow.Cells["idProducto"].Value);

            try
            {
                FrmIngresarProducto formulario =
                    new FrmIngresarProducto(
                        idServicio,
                        productosPendientes,
                        idProducto);

                formulario.ShowDialog();

                LogisticaDgvProductos.DataSource = null;
                LogisticaDgvProductos.DataSource = productosPendientes;

                LogisticaMetConfigurarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo modificar el producto.\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LogisticaBtnEliminarProducto_Click(
            object sender, EventArgs e)
        {
            if (productosPendientes == null ||
                LogisticaDgvProductos.CurrentRow == null)
            {
                MessageBox.Show(
                    "Seleccione el producto que desea eliminar.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            DialogResult respuesta = MessageBox.Show(
                "¿Está seguro de eliminar este producto de la lista?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
            {
                int idProducto = Convert.ToInt32(
                    LogisticaDgvProductos.CurrentRow.Cells["idProducto"].Value);

                foreach (DataRow fila in productosPendientes.Rows)
                {
                    if (Convert.ToInt32(fila["idProducto"]) == idProducto)
                    {
                        productosPendientes.Rows.Remove(fila);
                        break;
                    }
                }

                LogisticaDgvProductos.DataSource = null;
                LogisticaDgvProductos.DataSource = productosPendientes;

                LogisticaMetConfigurarColumnas();
            }
        }

        private void LogisticaBtnGuardarProducto_Click(
            object sender, EventArgs e)
        {
            int idServicio = LogisticaFuncObtenerIdServicio();

            if (idServicio <= 0)
            {
                MessageBox.Show("Seleccione una guía.");
                return;
            }

            if (productosPendientes == null)
            {
                MessageBox.Show("No hay productos para guardar.");
                return;
            }

            try
            {
                controlador.LogisticaProcGuardarProductos(
                    idServicio, productosPendientes);

                MessageBox.Show(
                    "Los productos se guardaron correctamente y el inventario fue actualizado.",
                    "Proceso completado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                productosPendientes =
                    controlador.LogisticaFuncObtenerProductosDelServicio(idServicio);

                LogisticaDgvProductos.DataSource = productosPendientes;

                LogisticaMetConfigurarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron guardar los productos.\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
