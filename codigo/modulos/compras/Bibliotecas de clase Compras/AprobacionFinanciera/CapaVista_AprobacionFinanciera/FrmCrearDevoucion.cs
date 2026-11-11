using System;
using System.Data;
using System.Windows.Forms;
using CapaControlador_AprobacionFinanciera;

namespace CapaVista_AprobacionFinanciera
{
    public partial class FrmCrearDevolucion : Form
    {
        private ClsControladorDevoluciones _Controlador = new ClsControladorDevoluciones();
        private int _IdCompraBase;
        private DataTable _DetallesCompraOriginal; 
        private DataTable _DetallesDevolucion;     
        private decimal _CantidadMaxPermitida = 0; 

        public FrmCrearDevolucion(int IdCompra)
        {
            InitializeComponent();
            _IdCompraBase = IdCompra;

            this.Text = "6008 - Crear Devolución de Compra";
            this.MaximizeBox = false;

            ComprasMetConfigurarTablaDevolucion();
            ComprasMetBloquearCampos();
            ComprasMetCargarDatosBase();

            // Eventos
            ComprasCboBodega.SelectedIndexChanged += ComprasCboBodega_SelectedIndexChanged;
            ComprasCboProductos.SelectedIndexChanged += ComprasCboProductos_SelectedIndexChanged;
            ComprasBtnAgregarProducto.Click += ComprasBtnAgregarProducto_Click;
            ComprasBtnGuardar.Click += ComprasBtnGuardar_Click;
            ComprasBtnDescartar.Click += (s, e) => this.Close();
        }

        private void ComprasMetConfigurarTablaDevolucion()
        {
            // estructura de la tabla temporal para la cuadrícula
            _DetallesDevolucion = new DataTable();
            _DetallesDevolucion.Columns.Add("IdProducto", typeof(int));
            _DetallesDevolucion.Columns.Add("Producto", typeof(string));
            _DetallesDevolucion.Columns.Add("IdBodega", typeof(int));
            _DetallesDevolucion.Columns.Add("Precio", typeof(decimal));
            _DetallesDevolucion.Columns.Add("Cantidad", typeof(decimal));
            _DetallesDevolucion.Columns.Add("Subtotal", typeof(decimal));

            ComprasDgvComprasrecibido.DataSource = _DetallesDevolucion;
            ComprasDgvComprasrecibido.AllowUserToAddRows = false;
            ComprasDgvComprasrecibido.ReadOnly = true;
            ComprasDgvComprasrecibido.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            ComprasDgvComprasrecibido.Columns["IdProducto"].Visible = false;
            ComprasDgvComprasrecibido.Columns["IdBodega"].Visible = false;
            ComprasDgvComprasrecibido.Columns["Precio"].DefaultCellStyle.Format = "N2";
            ComprasDgvComprasrecibido.Columns["Subtotal"].DefaultCellStyle.Format = "N2";
        }

        private void ComprasMetBloquearCampos()
        {
            // Bloqueos de seguridad
            ComprasTxtNumFactura.Enabled = false;
            ComprasTxtSerieFactura.Enabled = false;
            ComprasTxtNumOrden.Enabled = false;
            ComprasTxtMontoNota.Enabled = false; 
            ComprasTxtPrecio.Enabled = false;    
            ComprasTxtMontoNota.Text = "0.00";
        }

        private void ComprasMetCargarDatosBase()
        {
            try
            {
                // Cargar Encabezado
                DataTable Encabezado = _Controlador.ComprasFuncObtenerEncabezadoCompra(_IdCompraBase);
                if (Encabezado != null && Encabezado.Rows.Count > 0)
                {
                    DataRow Fila = Encabezado.Rows[0];
                    ComprasTxtSerieFactura.Text = Fila["serieFacturaCompra"].ToString();
                    ComprasTxtNumFactura.Text = Fila["numeroFacturaCompra"].ToString();
                    if (Fila["idOrdenCompra"] != DBNull.Value)
                        ComprasTxtNumOrden.Text = Fila["idOrdenCompra"].ToString();
                }

                // Cargar Detalles Originales para el ComboBox
                _DetallesCompraOriginal = _Controlador.ComprasFuncObtenerDetallesCompra(_IdCompraBase);

                if (_DetallesCompraOriginal != null && _DetallesCompraOriginal.Rows.Count > 0)
                {
                    DataTable bodegasDistintas = _DetallesCompraOriginal.DefaultView.ToTable(true, "idBodega");
                    ComprasCboBodega.DataSource = bodegasDistintas;
                    ComprasCboBodega.DisplayMember = "idBodega";
                    ComprasCboBodega.ValueMember = "idBodega";
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error al cargar la compra: " + Ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ComprasCboBodega_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComprasCboBodega.SelectedValue != null && _DetallesCompraOriginal != null)
            {
                // Cuando cambia la bodega filtramos los productos
                if (int.TryParse(ComprasCboBodega.SelectedValue.ToString(), out int idBodegaSeleccionada))
                {
                    DataView vistaProductos = new DataView(_DetallesCompraOriginal);
                    vistaProductos.RowFilter = $"idBodega = {idBodegaSeleccionada}";

                    ComprasCboProductos.DataSource = vistaProductos;
                    ComprasCboProductos.DisplayMember = "Producto";
                    ComprasCboProductos.ValueMember = "IdProducto";
                }
            }
        }

        private void ComprasCboProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComprasCboProductos.SelectedValue != null && ComprasCboProductos.DataSource != null)
            {
                DataRowView filaProducto = (DataRowView)ComprasCboProductos.SelectedItem;

                // Extraemos el precio original y la cantidad máxima comprada
                decimal precioOriginal = Convert.ToDecimal(filaProducto["Precio"]);
                _CantidadMaxPermitida = Convert.ToDecimal(filaProducto["CantidadRecibida"]);

                ComprasTxtPrecio.Text = precioOriginal.ToString("N2");
                ComprasTxtCantidad.Text = "0"; 
            }
        }

        private void ComprasBtnAgregarProducto_Click(object sender, EventArgs e)
        {
            if (ComprasCboProductos.SelectedValue == null) return;

            // Validar cantidad ingresada
            if (!decimal.TryParse(ComprasTxtCantidad.Text, out decimal cantidadIngresada) || cantidadIngresada <= 0)
            {
                MessageBox.Show("Ingrese una cantidad válida mayor a 0.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validar que no exceda lo que se compró originalmente
            if (cantidadIngresada > _CantidadMaxPermitida)
            {
                MessageBox.Show($"No puede devolver más de lo comprado. Máximo permitido: {_CantidadMaxPermitida:0.##}", "Restricción", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idProducto = Convert.ToInt32(ComprasCboProductos.SelectedValue);
            int idBodega = Convert.ToInt32(ComprasCboBodega.SelectedValue);
            string nombreProducto = ComprasCboProductos.Text;
            decimal precio = Convert.ToDecimal(ComprasTxtPrecio.Text);

            // Validar que el producto de esa bodega no esté ya en la cuadrícula
            foreach (DataRow filaExistente in _DetallesDevolucion.Rows)
            {
                if (Convert.ToInt32(filaExistente["IdProducto"]) == idProducto && Convert.ToInt32(filaExistente["IdBodega"]) == idBodega)
                {
                    MessageBox.Show("Este producto ya está agregado en la devolución. Si desea cambiar la cantidad, elimine la fila y agréguelo de nuevo.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            // Agregar a la tabla de devoluciones
            DataRow nuevaFila = _DetallesDevolucion.NewRow();
            nuevaFila["IdProducto"] = idProducto;
            nuevaFila["Producto"] = nombreProducto;
            nuevaFila["IdBodega"] = idBodega;
            nuevaFila["Precio"] = precio;
            nuevaFila["Cantidad"] = cantidadIngresada;
            nuevaFila["Subtotal"] = cantidadIngresada * precio;
            _DetallesDevolucion.Rows.Add(nuevaFila);

            ComprasMetActualizarMontoNota();
            ComprasTxtCantidad.Text = "";
        }

        private void ComprasMetActualizarMontoNota()
        {
            // Sumar el total a devolver
            object sumaTotal = _DetallesDevolucion.Compute("SUM(Subtotal)", "");
            decimal totalDevolucion = sumaTotal != DBNull.Value ? Convert.ToDecimal(sumaTotal) : 0;
            ComprasTxtMontoNota.Text = totalDevolucion.ToString("N2");
        }

        private void ComprasBtnGuardar_Click(object sender, EventArgs e)
        {
            if (_DetallesDevolucion.Rows.Count == 0)
            {
                MessageBox.Show("Debe agregar al menos un producto a la devolución.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal totalDevolucion = Convert.ToDecimal(ComprasTxtMontoNota.Text);
            DateTime fechaDevolucion = ComprasDtpFechaemision.Value;

            // 1. Extraemos los textos de los nuevos TextBox de la Nota de Crédito
            string serieNota = ComprasTxtSerieNota.Text;
            string numeroNota = ComprasTxtNumeroNota.Text;

            try
            {
                // 2. Enviamos los 7 parámetros exactos al Controlador
                _Controlador.ComprasFuncRegistrarDevolucion(
                    _IdCompraBase,
                    fechaDevolucion,
                    "Devolución por garantía/defecto",
                    serieNota,
                    numeroNota,
                    totalDevolucion,
                    _DetallesDevolucion
                );

                MessageBox.Show("Devolución registrada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error al guardar la devolución: " + Ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}