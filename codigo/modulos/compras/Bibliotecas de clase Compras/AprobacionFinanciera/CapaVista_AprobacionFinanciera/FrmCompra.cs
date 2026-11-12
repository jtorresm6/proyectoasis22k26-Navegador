using System;
using System.Data;
using System.Windows.Forms;
using CapaControlador_AprobacionFinanciera;

namespace CapaVista_AprobacionFinanciera
{
    public partial class FrmCompra : Form
    {
        private ClsAprobacionFinanciera _Controlador = new ClsAprobacionFinanciera();
        private int _IdOrdenCompraBase;
        private decimal _MontoEsperadoBase;
        private DataTable _DetallesOrdenBase;

        // 1. Nueva variable para guardar el método de valuación (PEPS, UEPS, Promedio)
        private string _MetodoValuacionBase;

        // 2. Se actualiza el constructor para recibir el método de valuación
        public FrmCompra(int IdOrdenCompra, decimal MontoEsperado, string MetodoValuacion)
        {
            InitializeComponent();

            _IdOrdenCompraBase = IdOrdenCompra;
            _MontoEsperadoBase = MontoEsperado;
            _MetodoValuacionBase = MetodoValuacion;

            this.Text = "6006 - ComprobacionCompra";
            this.MaximizeBox = false;

            ComprasMetCargarDatosBase();
        }

        private void ComprasMetCargarDatosBase()
        {
            // Llenar Encabezado
            ComprasTxtNumOrden.Text = _IdOrdenCompraBase.ToString();
            ComprasTxtNumOrden.Enabled = false;

            ComprasTxtMontoEsperado.Text = _MontoEsperadoBase.ToString("N2");
            ComprasTxtMontoEsperado.Enabled = false;

            // Descargar datos y llenar DataGridView / ComboBox
            try
            {
                // llenado _DetallesOrdenBase
                _DetallesOrdenBase = _Controlador.ComprasFuncObtenerDetalleOrden(_IdOrdenCompraBase);

                // bodegas para el ComboBox
                DataTable bodegasDistintas = _DetallesOrdenBase.DefaultView.ToTable(true, "idBodega");
                ComprasCboBodega.DataSource = bodegasDistintas;
                ComprasCboBodega.DisplayMember = "idBodega";
                ComprasCboBodega.ValueMember = "idBodega";

                // Configuramos la cuadrícula visual
                ComprasDgvComprasrecibido.AutoGenerateColumns = true;
                ComprasDgvComprasrecibido.DataSource = _DetallesOrdenBase;
                ComprasDgvComprasrecibido.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                ComprasDgvComprasrecibido.ReadOnly = true;

                // Formato de columnas
                ComprasDgvComprasrecibido.Columns["Precio"].DefaultCellStyle.Format = "N2";
                ComprasDgvComprasrecibido.Columns["Subtotal"].DefaultCellStyle.Format = "N2";
                ComprasDgvComprasrecibido.Columns["CantidadPedida"].DefaultCellStyle.Format = "0.##";
                ComprasDgvComprasrecibido.Columns["CantidadRecibida"].DefaultCellStyle.Format = "0.##";

                // Ocultar columnas técnicas
                ComprasDgvComprasrecibido.Columns["Linea"].Visible = false;
                ComprasDgvComprasrecibido.Columns["IdBodega"].Visible = false;
                ComprasDgvComprasrecibido.Columns["Descuento"].Visible = false;
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error al cargar los detalles: " + Ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Clic en el DataGridView
        private void ComprasDgvComprasrecibido_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && ComprasDgvComprasrecibido.Rows.Count > 0)
            {
                DataGridViewRow filaSelec = ComprasDgvComprasrecibido.Rows[e.RowIndex];

                // Producto 
                ComprasTxtProductos.Text = filaSelec.Cells["Producto"].Value.ToString();
                ComprasTxtProductos.Enabled = false;

                // Precio
                decimal precioFila = Convert.ToDecimal(filaSelec.Cells["Precio"].Value);
                ComprasTxtPrecio.Text = precioFila.ToString("N2");
                ComprasTxtPrecio.Enabled = false;

                // Cantidad
                decimal cantidadFila = Convert.ToDecimal(filaSelec.Cells["CantidadRecibida"].Value);
                ComprasTxtCantidad.Text = cantidadFila.ToString("0.##");

                ComprasDgvComprasrecibido.Tag = e.RowIndex;
            }
        }

        // Botón Modificar
        private void ComprasBtnModificarProducto_Click(object sender, EventArgs e)
        {
            if (ComprasDgvComprasrecibido.Tag != null)
            {
                int rowIndex = (int)ComprasDgvComprasrecibido.Tag;

                decimal nuevaCantidad;
                if (!decimal.TryParse(ComprasTxtCantidad.Text, out nuevaCantidad) || nuevaCantidad < 0)
                {
                    MessageBox.Show("Ingrese una cantidad válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataRowView vistaFila = (DataRowView)ComprasDgvComprasrecibido.Rows[rowIndex].DataBoundItem;
                DataRow filaDatos = vistaFila.Row;

                decimal cantidadPedida = Convert.ToDecimal(filaDatos["CantidadPedida"]);

                if (nuevaCantidad > cantidadPedida)
                {
                    MessageBox.Show($"La cantidad no puede superar lo pedido ({cantidadPedida}).", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Iniciar edición, modificar y CERRAR la edición
                vistaFila.BeginEdit();
                filaDatos["CantidadRecibida"] = nuevaCantidad;
                decimal precio = Convert.ToDecimal(filaDatos["Precio"]);
                filaDatos["Subtotal"] = nuevaCantidad * precio;
                vistaFila.EndEdit();

                ComprasDgvComprasrecibido.Refresh();

                // Limpiar campos
                ComprasTxtProductos.Text = "";
                ComprasTxtPrecio.Text = "";
                ComprasTxtCantidad.Text = "";
                ComprasDgvComprasrecibido.Tag = null;
            }
            else
            {
                MessageBox.Show("Seleccione un producto de la tabla.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Botón Guardar
        private void ComprasBtnGuardar_Click(object sender, EventArgs e)
        {
            // Forzar a la tabla visual a soltar cualquier edición a medias
            ComprasDgvComprasrecibido.EndEdit();

            string NumeroFactura = ComprasTxtNumFactura.Text.Trim();
            string SerieFactura = ComprasTxtSerieFactura.Text.Trim();
            string MontoFacturaFisicaStr = ComprasTxtMontoFisica.Text.Trim();

            if (string.IsNullOrEmpty(NumeroFactura) || string.IsNullOrEmpty(SerieFactura) || string.IsNullOrEmpty(MontoFacturaFisicaStr))
            {
                MessageBox.Show("Debe llenar todos los campos de la factura.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(MontoFacturaFisicaStr, out decimal MontoFacturaFisicaValor))
            {
                MessageBox.Show("El monto físico debe ser numérico.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // CORRECCIÓN: Sumamos exactamente lo que se ve en la cuadrícula visual (DataGridView)
            decimal TotalProductosRecibidos = 0;
            foreach (DataGridViewRow fila in ComprasDgvComprasrecibido.Rows)
            {
                if (!fila.IsNewRow && fila.Cells["Subtotal"].Value != null)
                {
                    TotalProductosRecibidos += Convert.ToDecimal(fila.Cells["Subtotal"].Value);
                }
            }

            // Validar restricción dura usando redondeo para evitar fallos por decimales invisibles
            if (Math.Round(TotalProductosRecibidos, 2) != Math.Round(MontoFacturaFisicaValor, 2))
            {
                MessageBox.Show(
                    $"¡ERROR DE CUADRE!\n\nLa suma de los productos en pantalla (Q {TotalProductosRecibidos:N2}) NO coincide con la Factura Física (Q {MontoFacturaFisicaValor:N2}).",
                    "Restricción", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string EstadoCompra = "Pendiente";
            if (Math.Round(MontoFacturaFisicaValor, 2) == Math.Round(_MontoEsperadoBase, 2))
            {
                EstadoCompra = "Registrada";
            }
            else
            {
                DialogResult Respuesta = MessageBox.Show(
                    "El monto físico no coincide con la Orden Original. ¿Desea registrar como 'Anulada'?",
                    "Descuadre", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (Respuesta == DialogResult.No) return;
                EstadoCompra = "Anulada";
            }

            // Guardar en BD 
            try
            {
                DateTime FechaEmisionFactura = ComprasDtpFechaemision.Value;
                DateTime FechaRecepcionFisica = ComprasDtpRecibido.Value;

                _Controlador.ComprasFuncRegistrarComprobacion(
                    _IdOrdenCompraBase,
                    SerieFactura,
                    NumeroFactura,
                    FechaRecepcionFisica,
                    FechaEmisionFactura,
                    _MontoEsperadoBase,
                    MontoFacturaFisicaValor,
                    EstadoCompra,
                    _MetodoValuacionBase,
                    _DetallesOrdenBase
                );

                MessageBox.Show("Compra guardada en el inventario correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error al guardar en BD: " + Ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Botón Descartar/Regresar
        private void ComprasBtnDescartar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ComprasLblBodega_Click(object sender, EventArgs e)
        {

        }

        private void ComprasCboBodega_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            // Forzamos a guardar cualquier celda que se esté editando
            ComprasDgvComprasrecibido.EndEdit();

            if (ComprasCboBodega.SelectedValue != null && _DetallesOrdenBase != null)
            {
                string valorSeleccionado = ComprasCboBodega.SelectedValue.ToString();

                // Solo intentamos filtrar si el valor es un número entero real
                if (int.TryParse(valorSeleccionado, out int idBodegaSeleccionada))
                {
                    _DetallesOrdenBase.DefaultView.RowFilter = $"idBodega = {idBodegaSeleccionada}";
                }
            }
        }
    }
}